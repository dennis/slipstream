# WebWidget

Enables you to create http endpoints hosted by Slipstream. These endpoints called WebWidgets 
instances can serve html and optionally assets. You can send updates to them, if this is needed.

This can be used with OBS as a browswer source, allowing Slipstream to visually
display data on stream.

## Lua

<details><summary>Construction</summary><br />

```lua
local webwidget = require("api/webwidget"):instance(config)
```

This will return a WebWidget instance and create it if it does not exists.

`config` is the initial configuration of the instance if one needs to be created. 
It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| type        | string        |            | Mandatory: WebWidget type      |
| data        | lua table-    |  nil       | Optional: Initial data         |

When creating an instance, Slipstream will start up an embedded HTTP server, serving
that endpoint. URL is shown in the console, but will be http://127.0.0.1:1919/instances/<id>,
where <id> is replaced with the id provided to the instance. The endpoint is removed
when no longer referenced from Lua. 

In Slipstream data directory, you'll find a WebWidgets directory. The content of this directory 
is served via http://127.0.0.1:1919/webwidgets/ allowing you to have you javascript,
css, images and other assets located here. 

The WebWidget directory, will also require an index.html exists in a subdirectory with the same
name as provided as the WebWidgetType.

For example: 
```lua
local webwidget = require("api/webwidget"):instance({ id = "mywidget", type = "test", data = "Hello")
```

Will create the endpoint http://127.0.0.1:1919/instances/wmywidget. If you go to this URL with
you browser, it will serve `WebWidget\test\index.html`. If this file does not exist, Slipstream
will not create the endpoint, but show a message in the console saying it is ignoring it.

The `index.html` file should be based on the following template
```html
<!DOCTYPE html>
<html>
<head>
    {{SLIPSTREAM_HEADERS}}
    <link type="text/css" rel="stylesheet" href="{{ASSETS}}/style.css"/>
</head>
<body class="flagborder-none" {{SLIPSTREAM_BODY_ATTRS}}>
    <script type="text/javascript">
        function onData(data) {
            document.body.className = "flagborder-" + data
        }
    </script>
</body>
</html>
```

For most part, this is regular HTML, except there are a few special strings:

| Name   | Required | Description                    |
| :-------- | :-----------:| :----------------------------- |
| {{SLIPSTREAM_HEADERS}} | Yes  | Needs to be in the <head>-tags for your HTML document. This will include javascript needed for the WebWidget to work |
| {{SLIPSTREAM_BODY_ATTRS}} | Yes | Needs to be a part of your <body>-tag. e.g: `<body {{SLIPSTREAM_BODY_ATTRS}}>` |
| {{ASSETS}} | No | Will be replaced with the path to the WebWidgets assets directory (a subdirectory under `WebWidget` in your Slipstream data Directory) | 

To get access to the data provided, you need to define a javascript function named `onData`, 
taking one argument, `data`, which is whatever data sent to the WebWidget. Additional two other javascript functions can
be implemented. `onConnect` (takes no arguments) and `onDisconnect()`, these are triggered when connecting/disconnecting to
Slipstream backend. It's optinal to implement these, but can be used to implement a "ghosted" view of the widget, 
not connected to Slipstream.

There are also three global variables available: `ASSETS` (for the assets path), `INSTANCE_ID` and finally `WEB_WIDGET_TYPE`

See `widget:send(data)` for how to send data to your widget.
</details>

<details><summary>widget:send(data)</summary><br />
Delivers data to the WebWidget. Data itself is unparsed so it can be formatted in whatever
way that is appropiate. The callback in the `index.html` will receive it as-is. So you can
provide a string, json, numbers and so on. You will need to make the callback parse it, and 
handle it as needed.

```lua
local webwidget = require("api/webwidget"):instance(config)
webwidget:send("green")
```
