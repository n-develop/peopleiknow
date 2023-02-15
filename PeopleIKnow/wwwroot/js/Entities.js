const Entities = {
    add: async function (entityName) {
        const preview = document.querySelector(".contact-preview");
        const id = preview.getAttribute("data-contact-id");
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Add?contactId=${id}`);
        if (!response.ok) {
            Notification.showError(`Something went wrong while adding a ${entityName}`);
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
            Notification.showError(`Something went wrong while saving a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    edit: async function (id, entityName) {
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Edit/${id}`);
        if (!response.ok) {
            Notification.showError(`Something went wrong while editing a ${entityName}`);
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
            Notification.showError(`Something went wrong while updating a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    },
    delete: async function (id, entityName) {
        LoadingIndicator.show();
        const response = await fetch(`/${entityName}/Delete/${id}`);
        if (!response.ok) {
            Notification.showError(`Something went wrong while deleting a ${entityName}`);
            return
        }
        await PeoplePane.update(response);
        LoadingIndicator.hide();
    }
};