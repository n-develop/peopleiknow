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

async function addContact() {
    showLoadingIndicator();
    const response = await fetch("/contact/add");
    if (!response.ok) {
        console.log("Something went wrong while creating a contact. " + response.statusText);
    } else {
        updatePane(response);
    }
    hideLoadingIndicator();
    showPane();
}

async function createContact() {
    const form = new FormData(document.getElementById('contact-form'));
    showLoadingIndicator();
    const response = await fetch("/contact/add", {
        method: "POST",
        body: form
    });
    if (!response.ok) {
        // TODO maybe make sure there is a nice error message in the data?
        console.log("Something went wrong while creating a contact. " + response.statusText);
    } else {
        updatePane(response);
        await reloadContactList();
    }
    hideLoadingIndicator()
}

/* Handle Teaser clicks */

function addContactTeaserClickEvent() {
    const contactCards = document.querySelectorAll("#people-feed > .card");
    contactCards.forEach(function (currentValue) {
        currentValue.onclick = handleTeaserClick;
    });

    const favs = document.querySelectorAll(".favorite");
    favs.forEach(function (currentValue) {
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

async function handleTeaserClick(element) {
    const id = element.currentTarget.getAttribute("data-contact-id");
    const response = await fetch("/Dashboard/Details/" + id);
    if (!response.ok) {
        console.log("Something went wrong while loading a contact. " + response.statusText);
        return;
    }
    showDetailsInPane(response);
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

async function backToDetails(id) {
    const response = await fetch("/Dashboard/Details/" + id);
    if (!response.ok) {
        console.log('Something went wrong while going back to the contact details');
        return;
    }
    updatePane(response);
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

async function reloadContactList() {
    const response = await fetch("/dashboard/contactlist");
    if (!response.ok) {
        console.log('Something went wrong while reloading the contact list');
        return;
    }
    updateContactList(response);
}

async function favClick(event) {
    event.stopPropagation();
    const i = event.currentTarget;
    const id = i.getAttribute("data-contact-id");
    const response = await fetch("/Contact/Favorite/" + id, {
        method: "POST"
    });
    if (!response.ok) {
        console.log('Something went wrong while triggering the favorite state');
        return;
    }
    updateContactList(response);
}

/* Search */

async function search() {
    const searchInput = document.getElementById("search-term");
    const term = searchInput.value;
    showLoadingIndicator();
    const response = await fetch("/Search?term=" + term);
    if (!response.ok) {
        console.log('Something went wrong while searching for contacts');
        return;
    }
    await updateContactList(response);
    hideLoadingIndicator();
    showFeed();
}

let timeout = null;

const searchButton = document.getElementById("search-button");
searchButton.onclick = search;

const searchTerm = document.getElementById("search-term");
searchTerm.addEventListener("keyup", function onEvent() {
    clearTimeout(timeout);
    timeout = setTimeout(async function () {
        await search()
    }, 600);
});

async function searchRelationship(contactName) {
    const searchInput = document.getElementById("search-term");
    searchInput.value = contactName;
    await search();
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
                const $target = document.getElementById("successfully-deleted-modal");
                showToastNotification($target);

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

function showToastNotification($target) {
    rootEl.classList.add('is-clipped');
    $target.classList.add('is-active');
    setTimeout(() => {
        rootEl.classList.remove('is-clipped');
        $target.classList.remove('is-active');
    }, 1800);
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
            showToastNotification($target);
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

/* CRUD entities */

async function addEntity(entityName) {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    showLoadingIndicator();
    const response = await fetch(`/${entityName}/Add?contactId=${id}`);
    if (!response.ok) {
        console.log(`Something went wrong while adding a ${entityName}`);
        return
    }
    updatePane(response);
    hideLoadingIndicator();
}

async function saveEntity(entityName, formId) {
    const form = new FormData(document.getElementById(formId));
    showLoadingIndicator();
    const response = await fetch(`/${entityName}/Add`, {
        method: "POST",
        body: form
    });
    if (!response.ok) {
        console.log(`Something went wrong while saving a ${entityName}`);
        return
    }
    updatePane(response);
    hideLoadingIndicator();
}

async function editEntity(id, entityName) {
    showLoadingIndicator();
    const response = await fetch(`/${entityName}/Edit/${id}`);
    if (!response.ok) {
        console.log(`Something went wrong while editing a ${entityName}`);
        return
    }
    updatePane(response);
    hideLoadingIndicator();
}

async function updateEntity(entityName, formId) {
    const form = new FormData(document.getElementById(formId));
    showLoadingIndicator();
    const response = await fetch(`/${entityName}/Edit`, {
        method: "POST",
        body: form
    });
    if (!response.ok) {
        console.log(`Something went wrong while updating a ${entityName}`);
        return
    }
    updatePane(response);
    hideLoadingIndicator();
}

async function deleteEntity(id, entityName) {
    showLoadingIndicator();
    const response = await fetch(`/${entityName}/Delete/${id}`);
    if (!response.ok) {
        console.log(`Something went wrong while deleting a ${entityName}`);
        return
    }
    updatePane(response);
    hideLoadingIndicator();
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