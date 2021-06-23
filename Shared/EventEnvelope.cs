#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class EventEnvelope : IEventEnvelope
    {
        public string Sender { get; }
        public string[]? Recipients { get; set; }

        public EventEnvelope()
        {
            Sender = "UNKNOWN";
        }

        public EventEnvelope(string sender)
        {
            Sender = sender;
        }

        public IEventEnvelope Reply(string sender)
        {
            return new EventEnvelope(sender)
            {
                Recipients = new string[] { Sender }
            };
        }

        public IEventEnvelope Add(string instanceId)
        {
            var newRecipients = new List<string>();

            if(Recipients != null)
            {
                foreach (var r in Recipients)
                { 
                    newRecipients.Add(r);
                }
            }

            newRecipients.Add(instanceId);

            return new EventEnvelope(Sender)
            {
                Recipients = newRecipients.ToArray()
            };
        }

        public IEventEnvelope Remove(string removeInstanceId)
        {
            var newRecipients = new List<string>();

            if(Recipients != null)
            {
                foreach (var r in Recipients)
                {
                    if (r != removeInstanceId)
                        newRecipients.Add(r);
                }
            }

            return new EventEnvelope(Sender)
            {
                Recipients = newRecipients.ToArray()
            };
        }

        public bool ContainsRecipient(string instanceId)
        {
            // No recipients, means all
            if (Recipients != null && Recipients.Length > 0)
            {
                foreach (var r in Recipients)
                {
                    if (r == instanceId)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
