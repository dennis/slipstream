# Components

A component is a collection of plugins with its events, event-factory,
event-handler, Lua functionality and services.

The component itself is merely a class that registers what is provided by it.
All functionality is located in plugins with a number of support classes.

Each component is located in a subdirectory within this directory and as they
inherit `IComponent` they will be picked up by PluginManager.
