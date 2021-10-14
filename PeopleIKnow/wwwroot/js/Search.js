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
        MobileFlow.showFeed();
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