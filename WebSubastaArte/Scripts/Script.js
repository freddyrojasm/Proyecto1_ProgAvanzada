// script.js
function mostrarImagen(imagen) {
    // Obtener la imagen original
    var imagenOriginal = new Image();
    imagenOriginal.src = imagen.src;

    var ventanaEmergente = window.open("", "Imagen", "width=" + imagenOriginal.width + ", height=" + imagenOriginal.height);
    ventanaEmergente.document.write("<img src='" + imagen.src + "' />");
}
