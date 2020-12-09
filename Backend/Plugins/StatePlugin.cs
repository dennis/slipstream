using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class StatePlugin : BasePlugin
    {
        private const string ALLOWED_KEY_CHARACTER_REGEX = @"^([\-\.a-zA-Z 0-9]+)$";
        private readonly IDictionary<string, string> KeyValues = new Dictionary<string, string>();
        private readonly IEventBus EventBus;
        private readonly string FilePath;

        public StatePlugin(string id, IEventBus eventBus) : base(id, "StatePlugin", "StatePlugin", "Core")
        {
            EventBus = eventBus;
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\state.txt";

            // Read existing values
            if(File.Exists(FilePath))
            {
                using StreamReader r = new StreamReader(FilePath);

                string line;
                while ((line = r.ReadLine()) != null)
                {
                    var f = line.Split(new string[] { "=" }, 2, StringSplitOptions.None);
                    if (f.Length != 2)
                        return;

                    if (KeyValues.ContainsKey(f[0]))
                    {
                        KeyValues.Remove(f[0]);
                    }

                    if(f[1].Length > 0)
                    {
                        KeyValues.Add(f[0], f[1]);
                    }
                }
            }

            // Write cleaned up version
            using StreamWriter Writer = new StreamWriter(FilePath);
            foreach (var pair in KeyValues)
            {
                Writer.WriteLine($"{pair.Key}={pair.Value}");
            }

            EventHandler.OnStateGetValue += EventHandler_OnStateGetValue;
            EventHandler.OnStateSetValue += EventHandler_OnStateSetValue;
        }

        private void EventHandler_OnStateSetValue(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.State.StateSetValue> e)
        {
            using StreamWriter Writer = File.AppendText(FilePath);

            if(!IsValidKey(e.Event.Key))
            {
                EventBus.PublishEvent(new Shared.Events.Utility.WriteToConsole { Message = $"'{e.Event.Key}' is not a valid state key. Ignoring" });
                return;
            }

            if (KeyValues.ContainsKey(e.Event.Key))
            {
                KeyValues.Remove(e.Event.Key);
            }

            if (e.Event.Value != null && e.Event.Value.Length > 0)
            {
                KeyValues.Add(e.Event.Key, e.Event.Value);
            }

            Writer.WriteLine($"{e.Event.Key}={e.Event.Value}");

            ReturnKey(e.Event.Key);
        }

        private void EventHandler_OnStateGetValue(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.State.StateGetValue> e)
        {
            ReturnKey(e.Event.Key);
        }

        private void ReturnKey(string key)
        {
            if (KeyValues.ContainsKey(key))
            {
                EventBus.PublishEvent(new Shared.Events.State.StateValue { Key = key, Value = KeyValues[key] });
            }
            else
            {
                EventBus.PublishEvent(new Shared.Events.State.StateValue { Key = key });
            }
        }

        private bool IsValidKey(string value)
        {
            return Regex.IsMatch(value, ALLOWED_KEY_CHARACTER_REGEX);
        }
    }
}
