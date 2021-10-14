const ContactList = {
    update: async function (response) {
        const feed = document.getElementById("people-feed");
        feed.innerHTML = await response.text();
        Teaser.addClickEvent();
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