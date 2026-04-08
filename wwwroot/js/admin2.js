
document.addEventListener('DOMContentLoaded', () => {
    
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.sidebar ul.nav-list li a');

 
    navLinks.forEach(link => {
        
        const linkPath = link.getAttribute('href');

        
        if (linkPath === currentPath) {
            
            link.classList.add('active-link');

        }
    });

    let sidebar = document.querySelector(".sidebar");
    let menuBtn = document.querySelector("#btn");
    let overlay = document.querySelector(".overlay");
    let isSidebarOpen = false;

    menuBtn.onclick = function () {
        toggleSidebar();
    };

    if (overlay) {
        overlay.onclick = function () {
            if (isSidebarOpen) {
                toggleSidebar();
            }
        };
    }

    function toggleSidebar() {
        if (isSidebarOpen) {
            sidebar.classList.remove("active");
            if (overlay) overlay.style.display = 'none';
            isSidebarOpen = false;
        } else {
            sidebar.classList.add("active");
            if (overlay) overlay.style.display = 'block';
            isSidebarOpen = true;
        }
    }
});