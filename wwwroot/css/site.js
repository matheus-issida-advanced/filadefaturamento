// Fecha qualquer menu "..." (details.menu) aberto quando o usuário
document.addEventListener('click', function (event) {
    document.querySelectorAll('details.menu[open]').forEach(function (menu) {
        if (!menu.contains(event.target)) {
            menu.removeAttribute('open');
        }
    });
});