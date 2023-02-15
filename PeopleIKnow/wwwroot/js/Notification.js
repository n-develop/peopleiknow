const Notification = {
    show: function (modalId) {
        const $target = document.getElementById(modalId);
        rootEl.classList.add('is-clipped');
        $target.classList.add('is-active');
        setTimeout(() => {
            rootEl.classList.remove('is-clipped');
            $target.classList.remove('is-active');
        }, 1800);
    },
    showError: function(message) {
        const $target = document.getElementById('action-failed-modal');
        const $message = document.getElementById('action-failed-message');
        $message.innerHTML = '😔' + message;
        rootEl.classList.add('is-clipped');
        $target.classList.add('is-active');
        setTimeout(() => {
            rootEl.classList.remove('is-clipped');
            $target.classList.remove('is-active');
        }, 2000);
    },
};