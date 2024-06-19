function checkToken() {
    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = '/login.html'; // URL da página de autenticação
    }
}

function logout() {
    localStorage.removeItem('token');
    window.location.reload();
}

function autorizacao() {
    const rotas_autorizadas = {
        'Admin': 'all',
        'worker': ['/', '/produto.html', '/cadastros/produto.html', '/atualizar/produto.html', '/unautorized.html', '/index.html' ],
        'client': ['/', '/retiradas.html', '/cadastros/retiradas.html', '/unautorized.html', '/index.html']
    }
    const role = localStorage.getItem('role');
    const rotas = rotas_autorizadas[role];
    if (rotas === 'all') {
        return
    } else if (!rotas.includes(window.location.pathname)) {
        window.location.href = '/unautorized.html';
    }
}

autorizacao();
checkToken();

