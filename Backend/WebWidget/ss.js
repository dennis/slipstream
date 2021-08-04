var INSTANCE_ID
var WEB_WIDGET_TYPE
var ASSETS

function connect() {  
    let socket = a = new WebSocket("ws://" + document.location.host + "/events/" + INSTANCE_ID)

    socket.onopen = function (e) {
        console.log("[slipstream ws] [open] Connection established")

        if (typeof(onConnect) === "function") {
            onConnect()
        }
    }

    socket.onmessage = function (event) {
        if (typeof (onData) !== "function") {
            console.log("[slipstream ws] [onmessage] got data, but no onData() function defined. Ignored", event.data)
        }
        else {
            console.log("[slipstream ws] [onmessage] got data", event.data)
            onData(JSON.parse(event.data))
        }
    }

    socket.onclose = function (event) {
        if (event.wasClean) {
            console.log("[slipstream ws] [close] Connection closed cleanly, code=" + event.code + " reason=" + event.reason)
        } else {
            console.log('[slipstream ws] [close] Connection died')
        }

        if (typeof (onDisconnect) === "function") {
            onDisconnect()
        }

        setTimeout(connect, 1000);
    }

    socket.onerror = function (error) {
        console.log("[slipstream ws] [error] " + error.message)
        socket.close()
    }
}

window.addEventListener('load', function () {
    INSTANCE_ID = document.body.getAttribute("data-instance-id")
    WEB_WIDGET_TYPE = document.body.getAttribute("data-web-widget-type")
    ASSETS = document.body.getAttribute("data-assets")

    connect()
})