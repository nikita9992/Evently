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