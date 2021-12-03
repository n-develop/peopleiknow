const Navigation = {
    init: function () {
        document.addEventListener('DOMContentLoaded', () => {
            const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

            if ($navbarBurgers.length > 0) {
                $navbarBurgers.forEach(el => {
                    el.addEventListener('click', () => {
                        const target = el.dataset.target;
                        const $target = document.getElementById(target);

                        el.classList.toggle('is-active');
                        $target.classList.toggle('is-active');
                    });
                });
            }
        });
    },
    backToDetails: async function (id) {
        const response = await fetch("/Contact/Details/" + id);
        if (!response.ok) {
            console.log('Something went wrong while going back to the contact details');
            return;
        }
        await PeoplePane.update(response);
    }
};

Navigation.init();