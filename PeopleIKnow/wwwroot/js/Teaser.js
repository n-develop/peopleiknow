const Teaser = {
    addClickEvent: function () {
        const contactCards = document.querySelectorAll("#people-feed > .card");
        contactCards.forEach((currentValue) => {
            currentValue.onclick = this.handleClick;
        });

        const favs = document.querySelectorAll(".favorite");
        favs.forEach(function (currentValue) {
            currentValue.onclick = Contact.toggleFavorite;
        });

        const backButton = document.getElementById("back-button");
        backButton.onclick = MobileFlow.showFeed;
    },
    update: async function () {
        const preview = document.querySelector(".contact-preview");
        const id = preview.getAttribute("data-contact-id");
        const teaser = document.getElementById("contact-teaser-" + id);
        const teaserResponse = await fetch("/contact/teaser?id=" + id);
        if (!teaserResponse.ok) {
            console.log(`Failed to update teaser for contact with id ${id}`);
            return;
        }
        teaser.outerHTML = await teaserResponse.text();
        this.addClickEvent();
    },
    handleClick: async function (element) {
        const id = element.currentTarget.getAttribute("data-contact-id");
        const response = await fetch("/Contact/Details/" + id);
        if (!response.ok) {
            console.log("Something went wrong while loading a contact. " + response.statusText);
            return;
        }
        await PeoplePane.showDetails(response);
        MobileFlow.showPane();
    }
};

Teaser.addClickEvent();