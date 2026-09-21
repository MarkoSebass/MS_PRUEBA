// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.querySelectorAll('.task-toast').forEach((toast) => {
	const hideToast = () => {
		toast.classList.add('is-hiding');
		window.setTimeout(() => toast.remove(), 200);
	};

	toast.querySelector('.task-toast-close')?.addEventListener('click', hideToast);
	window.setTimeout(hideToast, 1500);
});
