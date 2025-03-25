function addClickOutsideListener(dotnetReference) {
        document.addEventListener('click', function (event) {
            const sidebar = document.querySelector('.sidebar.open');
            const hamburger = document.querySelector('.hamburger-icon');
            if (sidebar && !sidebar.contains(event.target) && !hamburger.contains(event.target)) {
                dotnetReference.invokeMethodAsync('CloseMenuFromOutside');
            }
        });
    }