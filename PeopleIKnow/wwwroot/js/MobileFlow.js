const MobileFlow = {
    showPane: function () {
        const pane = document.getElementById("people-pane");
        MobileFlow.showElementOnMobile(pane);
        const feed = document.getElementById("people-feed");
        MobileFlow.hideElementOnMobile(feed);
        const backButton = document.getElementById("back-button");
        backButton.classList.remove("is-hidden-mobile");
    },
    showFeed: function () {
        const feed = document.getElementById("people-feed");
        MobileFlow.showElementOnMobile(feed);
        const pane = document.getElementById("people-pane");
        MobileFlow.hideElementOnMobile(pane);
        const backButton = document.getElementById("back-button");
        backButton.classList.add("is-hidden-mobile");
    },
    showElementOnMobile: function (el) {
        el.classList.remove("is-hidden-mobile");
        el.classList.add("is-full-mobile");
    },
    hideElementOnMobile: function (el) {
        el.classList.remove("is-full-mobile");
        el.classList.add("is-hidden-mobile");
    }
};