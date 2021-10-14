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
        MobileFlow.showPane();
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

        await Teaser.update();

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