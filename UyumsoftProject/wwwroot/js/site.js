// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openCart() {
    document.getElementById("cartPanel").style.display = "block";
    loadCartItems();
}
function closeCart() {
    document.getElementById("cartPanel").style.display = "none";
}
async function addToCart(form) {
    event.preventDefault();

    const data = new FormData(form);
    const response = await fetch(form.action, {
        method: 'POST',
        body: data,
    });

    if (response.ok) {
        loadCartItems(); // Ürün eklendikten sonra sepeti günceller
        openCart();
    }
}

async function loadCartItems() {
    const response = await fetch('/Cart/CartPartial'); // CartPartial action'ını çağırır
    const html = await response.text();
    document.getElementById("cartItems").innerHTML = html; // Sepet içeriğini yükler
}

async function removeFromCart(form) {
    event.preventDefault();

    const data = new FormData(form);
    const response = await fetch(form.action, {
        method: 'POST',
        body: data,
    });

    if (response.ok) {
        loadCartItems(); // Ürün çıkarıldıktan sonra sepeti günceller
    }
}