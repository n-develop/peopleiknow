const rootEl = document.documentElement;

/* modal  stuff */

const $modals = getAll('.modal');
const $modalCloses = getAll('.modal-close, .delete');
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

/* Handle add button click */

function addContact() {
    fetch("/contact/add")
        .then(updatePane);
}

function createContact() {
    const form = new FormData(document.getElementById('contact-form'));
    fetch("/contact/add", {
        method: "POST",
        body: form
    })
        .then(updatePane)
        .then(updateContactList);
}

/* Handle Teaser clicks */

function addContactTeaserClickEvent() {
    const contactCards = document.querySelectorAll("#people-feed > .card");
    contactCards.forEach(function (currentValue, currentIndex, listObj) {
        currentValue.onclick = handleTeaserClick;
    });
}

function handleTeaserClick(element) {
    const id = element.currentTarget.getAttribute("data-contact-id");
    fetch("/Dashboard/Details/" + id)
        .then(updatePane);
}

addContactTeaserClickEvent();

/* Contact List Stuff */

function updateContactList() {
    fetch("/dashboard/contactlist")
        .then((response) => {
            const feed = document.getElementById("people-feed");
            response.text().then(function (value) {
                feed.innerHTML = value;
            })
                .then(addContactTeaserClickEvent);
        });
}

/* Actions on Entities */

function deleteContact() {
    const id = document.querySelector(".contact-preview").getAttribute("data-contact-id");
    fetch("/contact/delete/" + id, {
        method: "DELETE"
    })
        .then((response) => {
            return response.json();
        })
        .then((responseObj) => {
            if (responseObj.success) {
                alert(responseObj.message);

                const empty = '<div class="columns is-desktop is-vcentered" style="height: 100%;">\n' +
                    '        <div class="column">\n' +
                    '            <h2 class="has-text-centered">Choose a Contact from the list!</h2>\n' +
                    '        </div>\n' +
                    '    </div>';
                const detailsPane = document.getElementById("people-pane");
                detailsPane.innerHTML = empty;

                document.getElementById("contact-teaser-" + id).remove();
            } else {
                alert(responseObj.message);
            }
        });
}

function saveContact() {
    const form = new FormData(document.getElementById('contact-form'));
    fetch("/dashboard/details", {
        method: "POST",
        body: form
    })
        .then(updatePane)
        .then(function () {
            const $target = document.getElementById("successfully-saved-modal");
            rootEl.classList.add('is-clipped');
            $target.classList.add('is-active');
        }).then(function () {
        const preview = document.querySelector(".contact-preview");
        const id = preview.getAttribute("data-contact-id");
        const teaser = document.getElementById("contact-teaser-" + id);
        fetch("/contact/teaser?id=" + id).then(function (response) {
            response.text().then(function (value) {
                teaser.outerHTML = value;
            })
        });

    });
}

function addTelephone() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    fetch("/Telephone/Add?contactId=" + id)
        .then(updatePane);
}

function saveTelephone() {
    const form = new FormData(document.getElementById('telephone-form'));
    fetch("/Telephone/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function deleteTelephone(id) {
    fetch("/Telephone/Delete/" + id)
        .then(updatePane);
}

function editTelephone(id) {
    fetch("/Telephone/Edit/" + id)
        .then(updatePane);
}

function updateTelephone() {
    const form = new FormData(document.getElementById('telephone-form'));
    fetch("/Telephone/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function addRelationship() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    fetch("/Relationship/Add?contactId=" + id)
        .then(updatePane);
}

function saveRelationship() {
    const form = new FormData(document.getElementById('relationship-form'));
    fetch("/Relationship/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function deleteRelationship(id) {
    fetch("/Relationship/Delete/" + id)
        .then(updatePane);
}

function editRelationship(id) {
    fetch("/Relationship/Edit/" + id)
        .then(updatePane);
}

function updateRelationship() {
    const form = new FormData(document.getElementById('relationship-form'));
    fetch("/Relationship/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function saveEmail() {
    const form = new FormData(document.getElementById('email-form'));
    fetch("/Email/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function addEmail() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    fetch("/Email/Add?contactId=" + id)
        .then(updatePane);
}

function deleteEmail(id) {
    fetch("/Email/Delete/" + id)
        .then(updatePane);
}

function editEmail(id) {
    fetch("/Email/Edit/" + id)
        .then(updatePane);
}

function updateEmail() {
    const form = new FormData(document.getElementById('email-form'));
    fetch("/Email/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function addStatusUpdate() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    fetch("/StatusUpdate/Add?contactId=" + id)
        .then(updatePane);
}

function saveStatusUpdate() {
    const form = new FormData(document.getElementById('status-update-form'));
    fetch("/StatusUpdate/Add", {
        method: "POST",
        body: form
    }).then(updatePane);
}

/* Update Pane */

function updatePane(response) {
    const detailsPane = document.getElementById("people-pane");
    response.text().then(function (value) {
        detailsPane.innerHTML = value;
    });
}

/* Helper stuff */

function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}