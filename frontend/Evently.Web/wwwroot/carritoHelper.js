// ── Carrito por usuario con expiración de 12 horas ────────────

// Obtener clave del carrito para un usuario específico
function obtenerClaveCarrito(idUsuario) {
    return idUsuario && idUsuario > 0
        ? `evently_carrito_${idUsuario}`
        : "evently_carrito_anonimo";
}

// Guardar carrito con timestamp de expiración
window.guardarCarrito = (carritoJson, idUsuario) => {
    const clave = obtenerClaveCarrito(idUsuario);
    const datos = {
        items: JSON.parse(carritoJson),
        expiracion: Date.now() + (12 * 60 * 60 * 1000) 
    };
    localStorage.setItem(clave, JSON.stringify(datos));
};

// Obtener carrito verificando expiración
window.obtenerCarrito = (idUsuario) => {
    const clave = obtenerClaveCarrito(idUsuario);
    const raw = localStorage.getItem(clave);

    if (!raw) return "[]";

    try {
        const datos = JSON.parse(raw);

        // Verificar si ha expirado
        if (Date.now() > datos.expiracion) {
            localStorage.removeItem(clave);
            return "[]";
        }

        return JSON.stringify(datos.items);
    } catch {
        localStorage.removeItem(clave);
        return "[]";
    }
};

// Limpiar carrito del usuario
window.limpiarCarrito = (idUsuario) => {
    const clave = obtenerClaveCarrito(idUsuario);
    localStorage.removeItem(clave);
};

// ── Token y datos del usuario ──────────────────────────────────

// Guardar token de autenticación
window.guardarToken = (token) => {
    localStorage.setItem("evently_token", token);
};

// Obtener token de autenticación
window.obtenerToken = () => {
    return localStorage.getItem("evently_token") || "";
};

// Eliminar token (cerrar sesión)
window.eliminarToken = () => {
    localStorage.removeItem("evently_token");
};

// Guardar datos del usuario
window.guardarUsuario = (email, rol, idUsuario, expiracion) => {
    localStorage.setItem("evently_email", email);
    localStorage.setItem("evently_rol", rol);
    localStorage.setItem("evently_idUsuario", idUsuario);
    localStorage.setItem("evently_expiracion", expiracion);
};

// Obtener datos del usuario
window.obtenerUsuario = () => {
    return {
        email: localStorage.getItem("evently_email") || "",
        rol: localStorage.getItem("evently_rol") || "",
        idUsuario: parseInt(localStorage.getItem("evently_idUsuario") || "0"),
        expiracion: localStorage.getItem("evently_expiracion") || ""
    };
};