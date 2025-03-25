function addClickOutsideListenerForBottomMenu(dotnetReference) {
    document.addEventListener('click', function (event) {
        const bottomMenu = document.querySelector('.sliding-menu.open');
        const chevronButton = document.querySelector('.chevron-up-btn');
        if (bottomMenu && !bottomMenu.contains(event.target) && !chevronButton.contains(event.target)) {
            dotnetReference.invokeMethodAsync('CloseMenuFromOutside');
        }
    });
}
