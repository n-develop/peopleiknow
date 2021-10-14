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