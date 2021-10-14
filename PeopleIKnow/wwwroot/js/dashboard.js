const rootEl = document.documentElement;

function addOnChangeEventToImageInput() {
    const file = document.getElementById("image-input");
    file.onchange = function () {
        if (file.files.length > 0) {
            document.getElementById('file-name').innerHTML = file.files[0].name;
        }
    };
}

/* Helper stuff */
function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}