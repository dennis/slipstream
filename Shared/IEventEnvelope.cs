namespace Slipstream.Shared
{
    public interface IEventEnvelope
    {
        string Sender { get; }
        string[]? Recipients { get; set; }

        IEventEnvelope Reply(string instanceId);
        IEventEnvelope Add(string instanceId);
        IEventEnvelope Remove(string instanceId);
        bool ContainsRecipient(string instanceId);
    }
}
