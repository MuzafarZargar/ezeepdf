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

window.getImageDimensions = async (base64Data) => {
    return new Promise((resolve, reject) => {
        const img = new Image();

        img.onload = () => {
            resolve({
                width: img.naturalWidth,
                height: img.naturalHeight
            });
        };

        img.onerror = (e) => reject(e);

        img.src = base64Data;
    });
};

window.disableImageEditorDrop = function () {
    document.addEventListener('dragover', function (e) {
        e.preventDefault();
    });

    document.addEventListener('drop', function (e) {
        e.preventDefault();
    });
}