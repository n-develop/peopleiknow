const Notification = {
    show: function (modalId) {
        const $target = document.getElementById(modalId);
        rootEl.classList.add('is-clipped');
        $target.classList.add('is-active');
        setTimeout(() => {
            rootEl.classList.remove('is-clipped');
            $target.classList.remove('is-active');
        }, 1800);
    }
};