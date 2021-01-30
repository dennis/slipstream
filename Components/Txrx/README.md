# Txrx

This component provides two different Plugins. ReceiverPlugin 
and TransmitterPlugin. They are mutual exclusive.

TransmitterPlugin will send any events it receieves to the configured
endpoint, if the event's `ExcludeFromTxrx` is `false`.

ReceiverPlugin will receive any event and inject it into the local Eventbus.
