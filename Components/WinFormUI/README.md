# WinFormUI

Very crude UI functionality. Adds/Removes buttons in the UI. Show text in
console

## Lua


<details><summary>Construction</summary><br />

```lua
local ui = require("api/winformui"):instance(config)
```

This will construct an instance of `api/winformui` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                             |
| :---------- | :-----------: | :--------: | :-------------------------------------- |
| id          | string        |            | Mandatory: Id of this instance          |
| deepview    | bool          |            | Show more details in UI (for debugging) |
</details>

<details><summary>ui:print(message)</summary><br />
Writes a string to the log shown in the UI.

| Parameter | Type   | Description     |
|:----------|:------:|:----------------|
| message   | string | string to write |

```lua
ui:print("Hello world")
```

This function publishes `WinFormUICommandWriteToConsole` event, that is handled by
MainWindow.

This function is aliased as ``print`` (deprecated)
</details>

<details><summary>ui:create_button(label)</summary><br />
Creates a button if it doesn't exist. If it does exist, it is ignored.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| label     | string |             |

```lua
ui:create_button("Hello UI")
```

This function publishes `WinFormUICommandCreateButton` event, that is handled by
MainWindow. `UIButtonTriggered` events will be sent, if the button is pressed.

This function is aliased as ``create_button`` (deprecated)
</details>

<details><summary>ui:delete_button(label)</summary><br />
Deletes a button if it exists.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| label     | string |             |

```lua
ui:delete_button("Hello UI")
```

This function publishes `WinFormUICommandDeleteButton` event, that is handled by MainWindow.

This function is aliased as ``delete_button`` (deprecated)
</details>

## Events

<details><summary>WinFormUIButtonTriggered</summary><br />
Is sent every time a button is pressed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `WinFormUIButtonTriggered` (constant)                             |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"WinFormUIButtonTriggered","Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>WinFormUICommandCreateButton</summary><br />
Create a new button, unless it exists

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `WinFormUICommandCreateButton` (constant)                         |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"WinFormUICommandCreateButton","Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>WinFormUICommandDeleteButton</summary><br />
Removes a button again, if it exists

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `WinFormUICommandDeleteButton` (constant)                         |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"WinFormUICommandDeleteButton","Uptime":1742,"Text":"World"}`
</details>

<details><summary>WinFormUICommandWriteToConsole</summary><br />
Output something to the console.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `WinFormUICommandWriteToConsole` (constant)                       |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Message         | string  | Message                                                           |

**JSON Example:**
`{"EventType":"WinFormUICommandWriteToConsole","Uptime":1742,"Message":"Hello World"}`
</details>