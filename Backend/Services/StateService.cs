using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class StateService : IStateService
    {
        private readonly IDictionary<string, StateValue> KeyValues = new Dictionary<string, StateValue>();
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly string FilePath;
        private DateTime? NextKeyExpiresAt;

        class StateValue
        {
            public string Value = "";
            public DateTime? ExpiresAt;

            public StateValue(string value, DateTime? expiresAt)
            {
                Value = value;
                ExpiresAt = expiresAt;
            }
        }

        public StateService(IEventFactory eventFactory, IEventBus eventBus, string filePath)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            FilePath = filePath;

            ReadStateFromFile();
            WriteStateToFile();
        }

        private void ReadStateFromFile()
        {
            var now = DateTime.Now;

            if (File.Exists(FilePath))
            {
                using StreamReader r = new StreamReader(FilePath);

                string line;
                while ((line = r.ReadLine()) != null)
                {
                    var f = line.Split(new string[] { "\t" }, 3, StringSplitOptions.None);
                    if (f.Length != 3)
                        return;

                    var key = f[0];
                    var expire = f[1];
                    var value = f[2];

                    if (KeyValues.ContainsKey(key))
                    {
                        KeyValues.Remove(key);

                    }

                    if (value.Length > 0)
                    {
                        DateTime? expiresAt = null;
                        if (expire.Length > 0)
                            expiresAt = DateTime.Parse(expire);

                        if (expiresAt == null || expiresAt > now)
                        {
                            AddKeyValue(key, value, expiresAt);
                        }
                    }
                }
            }
        }

        private void AddKeyValue(string key, string value, DateTime? expiresAt)
        {
            if (NextKeyExpiresAt == null || NextKeyExpiresAt > expiresAt)
                NextKeyExpiresAt = expiresAt;

            KeyValues.Add(key, new StateValue(value, expiresAt));
        }

        private void WriteStateToFile()
        {
            using StreamWriter Writer = new StreamWriter(FilePath);
            foreach (var pair in KeyValues)
            {
                Writer.WriteLine($"{pair.Key}\t{pair.Value.ExpiresAt}\t{pair.Value.Value}");
            }
        }

        public void SetState(string key, string value, int lifetimSeconds = 0)
        {
            using StreamWriter Writer = File.AppendText(FilePath);

            DateTime? expiresAt = null;

            if (!IsValidKey(key))
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"'{key}' is not a valid state key. Ignoring"));
                return;
            }

            if (KeyValues.ContainsKey(key))
            {
                KeyValues.Remove(key);
            }

            if (value != null && value.Length > 0)
            {
                if (lifetimSeconds > 0)
                    expiresAt = DateTime.Now.AddSeconds(lifetimSeconds);

                AddKeyValue(key, value, expiresAt);
                Writer.WriteLine($"{key}\t{expiresAt}\t{value}");
            }
        }

        public string GetState(string key)
        {
            var now = DateTime.Now;

            if (NextKeyExpiresAt != null && now > NextKeyExpiresAt)
            {
                NextKeyExpiresAt = null;

                // Something expired
                foreach (var k in KeyValues.Keys.ToList())
                {
                    var kval = KeyValues[k];

                    if (kval.ExpiresAt < now)
                    {
                        EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"'{k}' expired"));
                        SetState(k, "");
                    }
                    else
                    {
                        if (NextKeyExpiresAt == null || kval.ExpiresAt < NextKeyExpiresAt)
                            NextKeyExpiresAt = kval.ExpiresAt;
                    }
                }
            }

            if (KeyValues.TryGetValue(key, out StateValue value))
            {
                return value.Value;
            }
            else
            {
                return "";
            }
        }

        private bool IsValidKey(string value)
        {
            return value.IndexOf('\t') == -1;
        }
    }
}
