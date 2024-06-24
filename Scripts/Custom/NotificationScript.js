document.addEventListener('DOMContentLoaded', function () {
    var notificationElement = document.getElementById('liveToast');
    setTimeout(function () {
        notificationElement.classList.remove('fade');
        notificationElement.classList.add('show');
    }, 0);
    setTimeout(function () {
        notificationElement.style.display = 'none';
    }, 5000);
});