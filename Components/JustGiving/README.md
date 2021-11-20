# JustGiving

Allows you to track a fundraising donations.

## Lua

<details><summary>Construction</summary><br />

```lua
local jg = require("api/justgiving"):instance(config)

```

This will construct an instance of `api/justgiving` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                     |
| :---------- | :-----------: | :--------: | :------------------------------ |
| id          | string        |            | Mandatory: Id of this instance  |
| appid       | string        |            | Mandatory: JustGiving.com AppId |
| page        | string        |            | Mandatory: JustGiving.com Page (shortname) |


On JustGiving.com you can create an account and use that to create a `AppId`. If your fundraising page is:
`https://www.justgiving.com/fundraising/annierabbets` - and the final part of the URL (`annierabbets`) is
your `page` parameter. 

</details>

<details><summary>jg:send_all_donations()</summary><br />
Send all existing donations found. Normally only new donations are sent as events.

No arguments

</details>
