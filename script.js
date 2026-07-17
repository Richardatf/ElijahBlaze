const navToggle = document.querySelector('.nav-toggle');
const mainNav = document.querySelector('.main-nav');

navToggle?.addEventListener('click', () => {
  const isOpen = mainNav.classList.toggle('open');
  navToggle.setAttribute('aria-expanded', String(isOpen));
  navToggle.textContent = isOpen ? '×' : '☰';
});

mainNav?.querySelectorAll('a').forEach((link) => {
  link.addEventListener('click', () => {
    mainNav.classList.remove('open');
    navToggle?.setAttribute('aria-expanded', 'false');
    if (navToggle) navToggle.textContent = '☰';
  });
});

document.getElementById('year').textContent = new Date().getFullYear();

const observer = new IntersectionObserver((entries) => {
  entries.forEach((entry) => {
    if (entry.isIntersecting) entry.target.classList.add('revealed');
  });
}, { threshold: 0.12 });

document.querySelectorAll('.status-card, .codex-grid article, .principles div').forEach((element) => {
  observer.observe(element);
});
