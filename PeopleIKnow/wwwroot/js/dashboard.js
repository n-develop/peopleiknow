const rootEl = document.documentElement;

/* modal  stuff */
const $loadingIndicator = document.getElementById('loading-indicator');
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

function showLoadingIndicator() {
    rootEl.classList.add('is-clipped');
    $loadingIndicator.classList.add('is-active');
}

function hideLoadingIndicator() {
    rootEl.classList.remove('is-clipped');
    $loadingIndicator.classList.remove('is-active');
}

/* Handle add button click */

function addContact() {
    showLoadingIndicator();
    fetch("/contact/add")
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
    showPane();
}

function createContact() {
    const form = new FormData(document.getElementById('contact-form'));
    showLoadingIndicator();
    fetch("/contact/add", {
        method: "POST",
        body: form
    })
        .then(updatePane)
        .then(reloadContactList)
        .finally(() => hideLoadingIndicator());

}

/* Handle Teaser clicks */

function addContactTeaserClickEvent() {
    const contactCards = document.querySelectorAll("#people-feed > .card");
    contactCards.forEach(function (currentValue, currentIndex, listObj) {
        currentValue.onclick = handleTeaserClick;
    });

    const favs = document.querySelectorAll(".favorite");
    favs.forEach(function (currentValue, currentIndex, listObj) {
        currentValue.onclick = favClick;
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
        .then(showDetailsInPane);

    showPane();
}

function addOnChangeEventToImageInput() {
    const file = document.getElementById("image-input");
    file.onchange = function () {
        if (file.files.length > 0) {
            document.getElementById('file-name').innerHTML = file.files[0].name;
        }
    };
}

function backToDetails(id) {
    fetch("/Dashboard/Details/" + id)
        .then(updatePane);
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

function favClick(event) {
    event.stopPropagation();
    const i = event.currentTarget;
    const id = i.getAttribute("data-contact-id");
    fetch("/Contact/Favorite/" + id, {
        method: "POST"
    })
        .then(updateContactList);
}
/* Search */

function search() {
    const searchInput = document.getElementById("search-term");
    const term = searchInput.value;
    showLoadingIndicator();
    fetch("/Search?term=" + term)
        .then(updateContactList)
        .finally(() => hideLoadingIndicator());
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
    showLoadingIndicator();
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
        })
        .finally(() => hideLoadingIndicator());
}

function saveContact() {
    const form = new FormData(document.getElementById('contact-form'));
    showLoadingIndicator();
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

    })
        .finally(() => hideLoadingIndicator());
}

function addTelephone() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/Telephone/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveTelephone() {
    const form = new FormData(document.getElementById('telephone-form'));
    showLoadingIndicator();
    fetch("/Telephone/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteTelephone(id) {
    showLoadingIndicator();
    fetch("/Telephone/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editTelephone(id) {
    showLoadingIndicator();
    fetch("/Telephone/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateTelephone() {
    const form = new FormData(document.getElementById('telephone-form'));
    showLoadingIndicator();
    fetch("/Telephone/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function addRelationship() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/Relationship/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveRelationship() {
    const form = new FormData(document.getElementById('relationship-form'));
    showLoadingIndicator();
    fetch("/Relationship/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteRelationship(id) {
    showLoadingIndicator();
    fetch("/Relationship/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editRelationship(id) {
    showLoadingIndicator();
    fetch("/Relationship/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateRelationship() {
    const form = new FormData(document.getElementById('relationship-form'));
    showLoadingIndicator();
    fetch("/Relationship/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveEmail() {
    const form = new FormData(document.getElementById('email-form'));
    showLoadingIndicator();
    fetch("/Email/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function addEmail() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/Email/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteEmail(id) {
    showLoadingIndicator();
    fetch("/Email/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editEmail(id) {
    showLoadingIndicator();
    fetch("/Email/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateEmail() {
    const form = new FormData(document.getElementById('email-form'));
    showLoadingIndicator();
    fetch("/Email/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function addStatusUpdate() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/StatusUpdate/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveStatusUpdate() {
    const form = new FormData(document.getElementById('status-update-form'));
    showLoadingIndicator();
    fetch("/StatusUpdate/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editStatusUpdate(id) {
    showLoadingIndicator();
    fetch("/StatusUpdate/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateStatusUpdate() {
    const form = new FormData(document.getElementById('status-update-form'));
    showLoadingIndicator();
    fetch("/StatusUpdate/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteStatusUpdate(id) {
    showLoadingIndicator();
    fetch("/StatusUpdate/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

/* Common Activities */

function addActivity() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/CommonActivity/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveActivity() {
    const form = new FormData(document.getElementById('common-activity-form'));
    showLoadingIndicator();
    fetch("/CommonActivity/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editActivity(id) {
    showLoadingIndicator();
    fetch("/CommonActivity/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateActivity() {
    const form = new FormData(document.getElementById('common-activity-form'));
    showLoadingIndicator();
    fetch("/CommonActivity/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteActivity(id) {
    showLoadingIndicator();
    fetch("/CommonActivity/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

/* Gifts */

function addGift() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/Gift/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveGift() {
    const form = new FormData(document.getElementById('gift-form'));
    showLoadingIndicator();
    fetch("/Gift/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editGift(id) {
    showLoadingIndicator();
    fetch("/Gift/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateGift() {
    const form = new FormData(document.getElementById('gift-form'));
    showLoadingIndicator();
    fetch("/Gift/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteGift(id) {
    showLoadingIndicator();
    fetch("/Gift/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

/* Reminder */

function addReminder() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    fetch("/Reminder/Add?contactId=" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function saveReminder() {
    const form = new FormData(document.getElementById('reminder-form'));
    showLoadingIndicator();
    fetch("/Reminder/Add", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function editReminder(id) {
    showLoadingIndicator();
    fetch("/Reminder/Edit/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function updateReminder() {
    const form = new FormData(document.getElementById('reminder-form'));
    showLoadingIndicator();
    fetch("/Reminder/Edit", {
        method: "POST",
        body: form
    }).then(updatePane)
        .finally(() => hideLoadingIndicator());
}

function deleteReminder(id) {
    showLoadingIndicator();
    fetch("/Reminder/Delete/" + id)
        .then(updatePane)
        .finally(() => hideLoadingIndicator());
}

/* Update Pane */

function updatePane(response) {
    const detailsPane = document.getElementById("people-pane");
    response.text().then(function (value) {
        detailsPane.innerHTML = value;
    });
}

function showDetailsInPane(response) {
    const detailsPane = document.getElementById("people-pane");
    response.text().then(function (value) {
        detailsPane.innerHTML = value;
    })
        .then(addOnChangeEventToImageInput);
}

/* Helper stuff */

function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}