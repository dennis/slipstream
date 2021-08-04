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