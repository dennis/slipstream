using System;
using System.Collections.Generic;
using System.IO;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class StateService : IStateService
    {
        private readonly IDictionary<string, string> KeyValues = new Dictionary<string, string>();
        private readonly IEventBus EventBus;
        private readonly string FilePath;

        public StateService(IEventBus eventBus, string filePath)
        {
            EventBus = eventBus;
            FilePath = filePath;

            ReadStateFromFile();
            WriteStateToFile();
        }

        private void ReadStateFromFile()
        {
            if (File.Exists(FilePath))
            {
                using StreamReader r = new StreamReader(FilePath);

                string line;
                while ((line = r.ReadLine()) != null)
                {
                    var f = line.Split(new string[] { "\t" }, 2, StringSplitOptions.None);
                    if (f.Length != 2)
                        return;

                    if (KeyValues.ContainsKey(f[0]))
                    {
                        KeyValues.Remove(f[0]);

                    }

                    if (f[1].Length > 0)
                    {
                        KeyValues.Add(f[0], f[1]);
                    }
                }
            }
        }

        private void WriteStateToFile()
        {
            using StreamWriter Writer = new StreamWriter(FilePath);
            foreach (var pair in KeyValues)
            {
                Writer.WriteLine($"{pair.Key}\t{pair.Value}");
            }
        }

        public void SetState(string key, string value)
        {
            using StreamWriter Writer = File.AppendText(FilePath);

            if(!IsValidKey(key))
            {
                EventBus.PublishEvent(new Shared.Events.Utility.WriteToConsole { Message = $"'{key}' is not a valid state key. Ignoring" });
                return;
            }

            if (KeyValues.ContainsKey(key))
            {
                KeyValues.Remove(key);
            }

            if (value != null && value.Length > 0)
            {
                KeyValues.Add(key, value);
            }

            Writer.WriteLine($"{key}\t{value}");
        }

        public string GetState(string key)
        {
            if (KeyValues.TryGetValue(key, out string value))
            {
                return value;
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
