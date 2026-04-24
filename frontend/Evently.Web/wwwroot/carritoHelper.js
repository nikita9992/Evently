//Funciones para gestionar el carrito en localStorage

//Obtener carrito del localStorage
window.obtenerCarrito = () => {
    return localStorage.getItem("evently_carrito") || "[]";
};

//Guardar carrito en localStorage
window.guardarCarrito = (carritoJson) => {
    localStorage.setItem("evently_carrito", carritoJson);
};

//Limpiar carrito del localStorage
window.limpiarCarrito = () => {
    localStorage.removeItem("evently_carrito");
};

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