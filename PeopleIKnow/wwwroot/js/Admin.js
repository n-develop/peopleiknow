const rootEl = document.documentElement;
const Users = {
    toggleRole: async function (userId, roleName) {
        LoadingIndicator.show();
        const response = await fetch(`/Admin/Toggle/${roleName}/${userId}`, {
            method: "PUT"
        });
        LoadingIndicator.hide();
        if (!response.ok) {
            console.log(`Something went wrong while revoke access for user with id ${userId}`);
            return
        }
        const state = await response.text();
        this.updateButton(userId, roleName, state);
    },
    updateButton: function (userId, roleName, state) {
        const button = document.getElementById(roleName + "-" + userId);
        if (button) {
            if (state === "GRANTED") {
                button.classList.remove("is-danger");
                button.classList.add("is-success");
                button.firstElementChild.classList.remove("fa-minus-circle")
                button.firstElementChild.classList.add("fa-check-circle")
            } else if (state === "REVOKED") {
                button.classList.remove("is-success");
                button.classList.add("is-danger");
                button.firstElementChild.classList.remove("fa-check-circle")
                button.firstElementChild.classList.add("fa-minus-circle")
            }
        }
    },
};