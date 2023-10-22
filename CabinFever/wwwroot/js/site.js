// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// CREATE AND UPDATE
// Character counter
const description = document.getElementById('description');
const descriptionCounter = document.getElementById('descriptionCounter');

function charCount() {
    const count = description.value.length;
    descriptionCounter.innerText = "(Max characters: " + count + " / 5000)";
}
