async function loadNavbar() {
    const role = localStorage.getItem('role');
    const nav_role = {
        'Admin': '/partials/navbar.html',
        'worker': '/partials/navBarWorker.html',
        'client': '/partials/navBarCliente.html'
    }
    try {
        const response = await fetch(nav_role[role]);
        const html = await response.text();
        document.getElementById('navbar-container').innerHTML = html;
    } catch (error) {
        console.error('Erro ao carregar a navbar:', error);
    }
}

document.addEventListener('DOMContentLoaded', loadNavbar);