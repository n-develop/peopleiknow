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
        .then(reloadContactList);
}

/* Handle Teaser clicks */

function addContactTeaserClickEvent() {
    const contactCards = document.querySelectorAll("#people-feed > .card");
    contactCards.forEach(function (currentValue, currentIndex, listObj) {
        currentValue.onclick = handleTeaserClick;
    });

    const backButton = document.getElementById("back-button");
    backButton.onclick = showFeed;
}

function showPane() {
    const pane = document.getElementById("people-pane");
    showElementOnMobile(pane);
    const feed = document.getElementById("people-feed");
    hideElementOnMobile(feed);
    const backButton = document.getElementById("back-button");
    backButton.classList.remove("is-hidden-mobile");
}

function showFeed() {
    const feed = document.getElementById("people-feed");
    showElementOnMobile(feed);
    const pane = document.getElementById("people-pane");
    hideElementOnMobile(pane);
    const backButton = document.getElementById("back-button");
    backButton.classList.add("is-hidden-mobile");
}

function showElementOnMobile(el) {
    el.classList.remove("is-hidden-mobile");
    el.classList.add("is-full-mobile");
}

function hideElementOnMobile(el) {
    el.classList.remove("is-full-mobile");
    el.classList.add("is-hidden-mobile");
}

function handleTeaserClick(element) {
    const id = element.currentTarget.getAttribute("data-contact-id");
    fetch("/Dashboard/Details/" + id)
        .then(updatePane);

    showPane();
}

document.addEventListener('DOMContentLoaded', () => {

    // Get all "navbar-burger" elements
    const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Check if there are any navbar burgers
    if ($navbarBurgers.length > 0) {

        // Add a click event on each of them
        $navbarBurgers.forEach(el => {
            el.addEventListener('click', () => {

                // Get the target from the "data-target" attribute
                const target = el.dataset.target;
                const $target = document.getElementById(target);

                // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
                el.classList.toggle('is-active');
                $target.classList.toggle('is-active');

            });
        });
    }

});

addContactTeaserClickEvent();

/* Contact List Stuff */

function updateContactList(response) {
    const feed = document.getElementById("people-feed");
    response.text().then(function (value) {
        feed.innerHTML = value;
    })
        .then(addContactTeaserClickEvent);
}

function reloadContactList() {
    fetch("/dashboard/contactlist")
        .then(updateContactList);
}

/* Search */

function search() {
    const searchInput = document.getElementById("search-term");
    const term = searchInput.value;
    fetch("/Search?term=" + term)
        .then(updateContactList);
    showFeed();
}

const searchButton = document.getElementById("search-button");
searchButton.onclick = search;
const searchTerm = document.getElementById("search-term");
searchTerm.addEventListener("keyup", function onEvent(e) {
    if (e.keyCode === 13) {
        search();
    }
});

function searchRelationship(contactName) {
    const searchInput = document.getElementById("search-term");
    searchInput.value = contactName;
    search();
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
                .then(addContactTeaserClickEvent);
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

function editStatusUpdate(id) {
    fetch("/StatusUpdate/Edit/" + id)
        .then(updatePane);
}

function updateStatusUpdate() {
    const form = new FormData(document.getElementById('status-update-form'));
    fetch("/StatusUpdate/Edit", {
        method: "POST",
        body: form
    }).then(updatePane);
}

function deleteStatusUpdate(id) {
    fetch("/StatusUpdate/Delete/" + id)
        .then(updatePane);
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