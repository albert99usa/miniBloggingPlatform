
window.setTimeout(function () {
        var errorMessage = document.getElementById("error-message");
    if (errorMessage) {
        errorMessage.style.transition = "opacity 1s ease";
    errorMessage.style.opacity = "0";
    setTimeout(function () {
        errorMessage.remove();
            }, 1000); 
        }
}, 3000);

// Theme Changing function
document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById("theme-toggle");
    const currentTheme = localStorage.getItem("theme") || "light";
    if (currentTheme === "dark") {
        document.body.classList.add("dark-theme");
        themeToggleBtn.textContent = "Switch to Light Mode";
    }

    window.toggleTheme = function () {
        document.body.classList.toggle("dark-theme");
        const theme = document.body.classList.contains("dark-theme") ? "dark" : "light";
        localStorage.setItem("theme", theme);
        themeToggleBtn.textContent = theme === "dark" ? "Switch to Light Mode" : "Switch to Dark Mode";
    };
});