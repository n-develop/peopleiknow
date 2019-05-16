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

function handleTeaserClick(element) {
    var id = element.currentTarget.getAttribute("data-contact-id");
    fetch("/Dashboard/Details/" + id)
        .then(updatePane);
}

function deleteEmail(id) {
    fetch("/Email/Delete/" + id)
        .then(updatePane);
}

function saveContact() {
    var form = new FormData(document.getElementById('contact-form'));
    fetch("/dashboard/details", {
        method: "POST",
        body: form
    })
        .then(updatePane)
        .then(function () {
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

function addTelephone() {
    var preview = document.querySelector(".contact-preview");
    var id = preview.getAttribute("data-contact-id");
    fetch("/Telephone/Add?contactId=" + id)
        .then(updatePane);
}

function saveTelephone() {
    var form = new FormData(document.getElementById('telephone-form'));
    fetch("/Telephone/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function deleteTelephone(id) {
    fetch("/Telephone/Delete/" + id)
        .then(updatePane);
}

function addRelationship() {
    var preview = document.querySelector(".contact-preview");
    var id = preview.getAttribute("data-contact-id");
    fetch("/Relationship/Add?contactId=" + id)
        .then(updatePane);
}

function saveRelationship() {
    var form = new FormData(document.getElementById('relationship-form'));
    fetch("/Relationship/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function deleteRelationship(id) {
    fetch("/Relationship/Delete/" + id)
        .then(updatePane);
}

function saveEmail() {
    var form = new FormData(document.getElementById('email-form'));
    fetch("/Email/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function addEmail() {
    var preview = document.querySelector(".contact-preview");
    var id = preview.getAttribute("data-contact-id");
    fetch("/Email/Add?contactId=" + id)
        .then(updatePane);
}

function editEmail(id) {
    fetch("/Email/Edit/" + id)
        .then(updatePane);
}

function updateEmail() {
    var form = new FormData(document.getElementById('email-form'));
    fetch("/Email/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function editRelationship(id) {
    fetch("/Relationship/Edit/" + id)
        .then(updatePane);
}

function updateRelationship() {
    var form = new FormData(document.getElementById('relationship-form'));
    fetch("/Relationship/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function addStatusUpdate() {
    var preview = document.querySelector(".contact-preview");
    var id = preview.getAttribute("data-contact-id");
    fetch("/StatusUpdate/Add?contactId=" + id)
        .then(updatePane);
}

function saveStatusUpdate() {
    var form = new FormData(document.getElementById('status-update-form'));
    fetch("/StatusUpdate/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function updatePane(response) {
    var detailsPane = document.getElementById("people-pane");
    response.text().then(function (value) {
        detailsPane.innerHTML = value;
    });
}

function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}