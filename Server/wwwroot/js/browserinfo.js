window.browserInfo = {
    getUserAgent: function () {
        return navigator.userAgent;
    }
};

window.saveFile = (fileName, contentType, base64Data) => {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = `data:${contentType};base64,${base64Data}`;
    link.click();
};

window.getPublicIp = async function () {
    let res = await fetch("/api/client-ip");
    let data = await res.json();
    return data.ip;
}