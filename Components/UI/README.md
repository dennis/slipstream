# UI

Very crude UI functionality. Adds/Removes buttons in the UI. Show text in
console

## Lua

<details><summary>ui:print(message)</summary><br />
Writes a string to the log shown in the UI.

| Parameter | Type   | Description     |
|:----------|:------:|:----------------|
| message   | string | string to write |

```lua
ui:print("Hello world")
```

This function publishes `UICommandWriteToConsole` event, that is handled by
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

This function publishes `UICommandCreateButton` event, that is handled by
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

This function publishes `UICommandDeleteButton` event, that is handled by MainWindow.

This function is aliased as ``delete_button`` (deprecated)
</details>

## Events

<details><summary>UIButtonTriggered</summary><br />
Is sent every time a button is pressed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `UIButtonTriggered` (constant)                                    |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"UIButtonTriggered","Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandCreateButton</summary><br />
Create a new button, unless it exists

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `UICommandCreateButton` (constant)                                |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"UICommandCreateButton","Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandDeleteButton</summary><br />
Removes a button again, if it exists

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `UICommandDeleteButton` (constant)                                |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Text            | string  | Text of the button                                                |

**JSON Example:**
`{"EventType":"UICommandDeleteButton","Uptime":1742,"Text":"World"}`
</details>

<details><summary>UICommandWriteToConsole</summary><br />
Output something to the console.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `UICommandWriteToConsole` (constant)                              |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Message         | string  | Message                                                           |

**JSON Example:**
`{"EventType":"UICommandWriteToConsole","Uptime":1742,"Message":"Hello World"}`
</details>
