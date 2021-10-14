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
