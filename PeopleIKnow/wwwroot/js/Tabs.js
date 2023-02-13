const Tabs = {
    show: function (category, key) {
        const categoryElements = document.querySelectorAll("[data-category='" + category + "']")
        categoryElements.forEach((currentValue) => {
            currentValue.style.setProperty('display', 'none', 'important');
        });
        const keyElements = document.querySelectorAll("[data-key='" + key + "']")
        keyElements.forEach((currentValue) => {
            currentValue.style.removeProperty("display")
        });

        const tabs = document.querySelectorAll("[data-tab='" + category + "']")
        tabs.forEach((tab) => {
            tab.classList.remove("is-active")
        });
        document.getElementById("tab-" + key).classList.add("is-active")
    }
}