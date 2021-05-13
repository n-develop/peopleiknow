const rootEl = document.documentElement;

/* loading indicator */
const $loadingIndicator = document.getElementById('loading-indicator');
const LoadingIndicator = {
    show: function () {
        rootEl.classList.add('is-clipped');
        $loadingIndicator.classList.add('is-active');
    },
    hide: function () {
        rootEl.classList.remove('is-clipped');
        $loadingIndicator.classList.remove('is-active');
    }
};

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

const Contact = {
    add: async function () {
        LoadingIndicator.show();
        const response = await fetch("/contact/add");
        if (!response.ok) {
            console.log("Something went wrong while creating a contact. " + response.statusText);
        } else {
            await PeoplePane.update(response);
        }
        LoadingIndicator.hide();
        showPane();
    },

    create: async function () {
        const form = new FormData(document.getElementById('contact-form'));
        LoadingIndicator.show();
        const response = await fetch("/contact/add", {
            method: "POST",
            body: form
        });
        if (!response.ok) {
            // TODO maybe make sure there is a nice error message in the data?
            console.log("Something went wrong while creating a contact. " + response.statusText);
        } else {
            await PeoplePane.update(response);
            await ContactList.reload();
        }
        LoadingIndicator.hide();
    },

    delete: async function () {
        const id = document.querySelector(".contact-preview").getAttribute("data-contact-id");
        LoadingIndicator.show();
        const response = await fetch("/contact/delete/" + id, {
            method: "DELETE"
        });

        const responseObj = await response.json();
        if (responseObj.success) {
            Notification.show("successfully-deleted-modal");
            PeoplePane.clear();
            document.getElementById("contact-teaser-" + id).remove();
        } else {
            alert(responseObj.message);
        }
        LoadingIndicator.hide();
    },

    save: async function () {
        const form = new FormData(document.getElementById('contact-form'));
        LoadingIndicator.show();
        const response = await fetch("/dashboard/details", {
            method: "POST",
            body: form
        });
        if (!response.ok) {
            console.log('Something went wrong saving a contact');
            return;
        }
        await PeoplePane.update(response);
        Notification.show("successfully-saved-modal");

        await updateTeaser();

        LoadingIndicator.hide();
    },

    toggleFavorite: async function (event) {
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
        await ContactList.update(response);
    }
};

/* Handle Teaser clicks */

function addContactTeaserClickEvent() {
    const contactCards = document.querySelectorAll("#people-feed > .card");
    contactCards.forEach(function (currentValue) {
        currentValue.onclick = handleTeaserClick;
    });

    const favs = document.querySelectorAll(".favorite");
    favs.forEach(function (currentValue) {
        currentValue.onclick = Contact.toggleFavorite;
    });

    const backButton = document.getElementById("back-button");
    backButton.onclick = showFeed;
}

addContactTeaserClickEvent();

async function updateTeaser() {
    const preview = document.querySelector(".contact-preview");
    const id = preview.getAttribute("data-contact-id");
    const teaser = document.getElementById("contact-teaser-" + id);
    const teaserResponse = await fetch("/contact/teaser?id=" + id);
    if (!teaserResponse.ok) {
        console.log(`Failed to update teaser for contact with id ${id}`);
        return;
    }
    teaser.outerHTML = await teaserResponse.text();
    addContactTeaserClickEvent();
}

async function handleTeaserClick(element) {
    const id = element.currentTarget.getAttribute("data-contact-id");
    const response = await fetch("/Dashboard/Details/" + id);
    if (!response.ok) {
        console.log("Something went wrong while loading a contact. " + response.statusText);
        return;
    }
    await PeoplePane.showDetails(response);
    showPane();
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

function addOnChangeEventToImageInput() {
    const file = document.getElementById("image-input");
    file.onchange = function () {
        if (file.files.length > 0) {
            document.getElementById('file-name').innerHTML = file.files[0].name;
        }
    };
}

const Navigation = {
    init: function () {
        document.addEventListener('DOMContentLoaded', () => {
            const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

            if ($navbarBurgers.length > 0) {
                $navbarBurgers.forEach(el => {
                    el.addEventListener('click', () => {
                        const target = el.dataset.target;
                        const $target = document.getElementById(target);

                        el.classList.toggle('is-active');
                        $target.classList.toggle('is-active');
                    });
                });
            }
        });
    },
    backToDetails: async function (id) {
        const response = await fetch("/Dashboard/Details/" + id);
        if (!response.ok) {
            console.log('Something went wrong while going back to the contact details');
            return;
        }
        await PeoplePane.update(response);
    }
};

Navigation.init();

const ContactList = {
    update: async function (response) {
        const feed = document.getElementById("people-feed");
        feed.innerHTML = await response.text();
        addContactTeaserClickEvent();
    },

    reload: async function () {
        const response = await fetch("/dashboard/contactlist");
        if (!response.ok) {
            console.log('Something went wrong while reloading the contact list');
            return;
        }
        await this.update(response);
    }
};

const Search = {
    execute: async function () {
        const searchInput = document.getElementById("search-term");
        const term = searchInput.value;
        LoadingIndicator.show();
        const response = await fetch("/Search?term=" + term);
        if (!response.ok) {
            console.log('Something went wrong while searching for contacts');
            return;
        }
        await ContactList.update(response);
        LoadingIndicator.hide();
        showFeed();
    },
    init: function () {
        let timeout = null;

        const searchButton = document.getElementById("search-button");
        searchButton.onclick = this.search;

        const searchTerm = document.getElementById("search-term");
        searchTerm.addEventListener("keyup", () => {
            clearTimeout(timeout);
            timeout = setTimeout(async () => {
                await this.execute()
            }, 600);
        });
    },
    searchRelationship: async function (contactName) {
        const searchInput = document.getElementById("search-term");
        searchInput.value = contactName;
        await this.execute();
    }
};

Search.init();

const Entities = {
    add: async function (entityName) {
        const preview = document.querySelector(".contact-preview");
        const id = preview.getAttribute("data-contact-id");
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Add?contactId=${id}`);
        if (!response.ok) {
            console.log(`Something went wrong while adding a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    save: async function (entityName, formId) {
        const form = new FormData(document.getElementById(formId));
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Add`, {
            method: "POST",
            body: form
        });
        if (!response.ok) {
            console.log(`Something went wrong while saving a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    edit: async function (id, entityName) {
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Edit/${id}`);
        if (!response.ok) {
            console.log(`Something went wrong while editing a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    update: async function (entityName, formId) {
        const form = new FormData(document.getElementById(formId));
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Edit`, {
            method: "POST",
            body: form
        });
        if (!response.ok) {
            console.log(`Something went wrong while updating a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    delete: async function (id, entityName) {
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Delete/${id}`);
        if (!response.ok) {
            console.log(`Something went wrong while deleting a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    }
};

const PeoplePane = {
    update: async function (response) {
        const detailsPane = document.getElementById("people-pane");
        detailsPane.innerHTML = await response.text();
    },

    showDetails: async function (response) {
        const detailsPane = document.getElementById("people-pane");
        detailsPane.innerHTML = await response.text();
        addOnChangeEventToImageInput();
    },

    clear: function () {
        const empty = '<div class="columns is-desktop is-vcentered" style="height: 100%;">\n' +
            '        <div class="column">\n' +
            '            <h2 class="has-text-centered">Choose a Contact from the list!</h2>\n' +
            '        </div>\n' +
            '    </div>';
        const detailsPane = document.getElementById("people-pane");
        detailsPane.innerHTML = empty;
    }
};

const Notification = {
    show: function (modalId) {
        const $target = document.getElementById(modalId);
        rootEl.classList.add('is-clipped');
        $target.classList.add('is-active');
        setTimeout(() => {
            rootEl.classList.remove('is-clipped');
            $target.classList.remove('is-active');
        }, 1800);
    }
};


/* Helper stuff */

function getAll(selector) {
    return Array.prototype.slice.call(document.querySelectorAll(selector), 0);
}