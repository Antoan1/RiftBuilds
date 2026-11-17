document.addEventListener("click", function (event) {
    if (!(event.target instanceof Element)) {
        return;
    }

    const button = event.target.closest("[data-password-toggle]");

    if (!button) {
        return;
    }

    const input = document.getElementById(
        button.dataset.passwordToggle
    );

    if (!(input instanceof HTMLInputElement)) {
        return;
    }

    const showPassword = input.type === "password";

    const fieldName = input.id === "ConfirmPassword"
        ? "password confirmation"
        : "password";

    input.type = showPassword ? "text" : "password";

    button.textContent = showPassword ? "Hide" : "Show";

    button.setAttribute(
        "aria-pressed",
        String(showPassword)
    );

    button.setAttribute(
        "aria-label",
        `${showPassword ? "Hide" : "Show"} ${fieldName}`
    );
});