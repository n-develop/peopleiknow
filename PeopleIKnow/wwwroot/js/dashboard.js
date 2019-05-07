var rootEl = document.documentElement;
var $modals = getAll('.modal');
var $modalCloses = getAll('.modal-close, .delete');
if ($modalCloses.length > 0) {
    $modalCloses.forEach(function ($el) {
        $el.addEventListener('click', function () {
            closeModals();
        });
    });
}

function closeModals() {
    rootEl.classList.remove('is-clipped');
    $modals.forEach(function ($el) {
        $el.classList.remove('is-active');
    });
}

var contactCards = document.querySelectorAll("#people-feed > .card");
contactCards.forEach(function (currentValue, currentIndex, listObj) {
    currentValue.onclick = handleTeaserClick;
});

var deleteEmailLinks = document.querySelectorAll(".remove-email");
deleteEmailLinks.forEach(function (currentValue, currentIndex, listObj) {
    currentValue.onclick = handleDeleteEmailClick;
});

function handleDeleteEmailClick(element) {
    var id = element.currentTarget.getAttribute("data-email-id");
    fetch("/Email/Delete/" + id)
        .then(function (response) {
            //TODO Handle the response

        });
}

function handleTeaserClick(element) {
    var id = element.currentTarget.getAttribute("data-contact-id");
    fetch("/Dashboard/Details/" + id)
        .then(function (response) {
            var detailsPane = document.getElementById("people-pane");
            response.text().then(function (value) {
                detailsPane.innerHTML = value;
            });

        });
}

function saveContact() {
    var form = new FormData(document.getElementById('contact-form'));
    fetch("/dashboard/details", {
        method: "POST",
        body: form
    }).then(function (response) {
        var detailsPane = document.getElementById("people-pane");
        response.text().then(function (value) {
            detailsPane.innerHTML = value;
        });
    }).then(function () {
        var $target = document.getElementById("successfully-saved-modal");
        rootEl.classList.add('is-clipped');
        $target.classList.add('is-active');
    }).then(function () {
        var preview = document.querySelector(".contact-preview");
        var id = preview.getAttribute("data-contact-id");
        var teaser = document.getElementById("contact-teaser-" + id);
        fetch("/contact/teaser?id=" + id).then(function (response) {
            response.text().then(function (value) {
                teaser.outerHTML = value;
            })
        });

    });
}

function addEmail() {
    var preview = document.querySelector(".contact-preview");
    var id = preview.getAttribute("data-contact-id");
    fetch("/Email/Add?contactId=" + id)
        .then(function (response) {
            var detailsPane = document.getElementById("people-pane");
            response.text().then(function (value) {
                detailsPane.innerHTML = value;
            });
        });
}

function saveEmail() {
    var form = new FormData(document.getElementById('email-form'));
    fetch("/Email/Add", {
        method: "POST",
        body: form
    }).then(function (response) {
        var detailsPane = document.getElementById("people-pane");
        response.text().then(function (value) {
            detailsPane.innerHTML = value;
        });
    })
}

function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}