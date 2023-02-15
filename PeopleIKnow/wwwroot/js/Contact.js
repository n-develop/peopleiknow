const Contact = {
    add: async function () {
        LoadingIndicator.show();
        const response = await fetch("/contact/add");
        if (!response.ok) {
            Notification.showError('Something went wrong while creating a contact');
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
            Notification.showError('Something went wrong while creating a contact');
        } else {
            await PeoplePane.update(response);
            await ContactList.reload();
        }
        LoadingIndicator.hide();
    },

    delete: async function () {
        let shouldDeleteContact = confirm("Do you really want to delete this contact?")
        if (!shouldDeleteContact) {
            return
        }
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
            Notification.showError(responseObj.message);
        }
        LoadingIndicator.hide();
    },

    save: async function () {
        const form = new FormData(document.getElementById('contact-form'));
        LoadingIndicator.show();
        const response = await fetch("/Contact/Details", {
            method: "POST",
            body: form
        });
        if (!response.ok) {
            Notification.showError('Something went wrong saving the contact');
            return;
        }
        await PeoplePane.update(response);
        Notification.show("successfully-saved-modal");

        await Teaser.update();

        LoadingIndicator.hide();
    },

    toggleFavorite: async function (event) {
        event.stopPropagation();
        const target = event.currentTarget;
        const id = target.getAttribute("data-contact-id");
        const response = await fetch("/Contact/Favorite/" + id, {
            method: "POST"
        });
        if (!response.ok) {
            Notification.showError('Something went wrong while triggering the favorite state');
            return;
        }
        await ContactList.update(response);
    }
};