let wrapper;

if (!String.prototype.replaceAll) {
    String.prototype.replaceAll = function (search, replace) {
        return this.split(search).join(replace);
    }
}

function onConnect() {
    document.body.classList.remove("offline")
}

function onDisconnect() {
    document.body.classList.add("offline")
}

function onData(data) {
    if (wrapper === undefined) {
        wrapper = document.getElementsByClassName("wrapper")[0]
    }
    
    let visible = wrapper.classList.contains("show")

    if (data.mode) {
        if (!visible) {
            wrapper.classList.remove("transition")
        }

        wrapper.classList.remove("from-right")
        wrapper.classList.remove("from-left")
        wrapper.classList.remove("from-top")
        wrapper.classList.remove("from-bottom")

        if (!wrapper.classList.contains("from-" + data.mode)) {
            wrapper.classList.add("from-" + data.mode)
        }
        data = ""
    }
    else {
        if (!visible) {
            wrapper.classList.add("transition")
        }
    }

    let html = undefined;

    if (data.text) {
        html = "<p>" + data.text.trim() + "</p>"
    }
    if (data.html) {
        // We need to split the "needle" up as to strings, to avoid having it replaced with 
        // the assets path
        html = data.html.replace("{{ASSETS}}", ASSETS)
    }

    if (html === undefined || html === "") {
        wrapper.classList.remove("show")
    }
    else {
        wrapper.children[0].innerHTML = html
        wrapper.classList.add("show")
    }
}


let socket = null

function connect() {
    var url = "ws://" + document.location.host + document.location.pathname + "ws";
    console.log("[slipstream ws] [open] Connecting to " + url)
    socket = new WebSocket(url)
    socket.onopen = function (e) {
        console.log("[slipstream ws] [open] Connection established")

        if (typeof (onConnect) === "function") {
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

function sendData(data) {
    if (socket != null)
        socket.send(data)
}

window.addEventListener('load', function () {
    connect()
})