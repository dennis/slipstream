# init.lua

The init.lua file is the only required Lua script to run the slipstream application.
The file will be autmatically created if it does not exist in the scripts folder.

IMPORTANT: init.lua is application version specific and a new default will be generated
on startup of a different version. This is done to prioritise slipstream startup instead
of Lua script compatability during the high churn phases of development and will be looked
at again once plugin development slows down.

Located in %AppData%/Slipstream/init-{app-version}.lua

## Usage
