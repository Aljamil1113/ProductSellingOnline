// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//******************* POPUP MODAL *********************************// 
document.querySelectorAll('.modal-button').forEach(function (el) {
    el.addEventListener('click', function () {
        var target = document.querySelector(el.getAttribute('data-target'));

        target.classList.add('is-active');

        target.querySelector('.modal-close').addEventListener('click', function () {
            target.classList.remove('is-active');
        });
    });
});
//====================================================================================