// Swagger UI Theme Switcher
// Local themes under /theme keep styling stable in offline/dev environments.

const THEMES = {
  light: {
    name: 'Default Light',
    css: '/theme/default-light.css',
    ui: {
      containerBg: 'rgba(255, 255, 255, 0.92)',
      containerBorder: 'rgba(26, 26, 26, 0.15)',
      labelColor: '#2d3748',
      selectBg: '#ffffff',
      selectBorder: '#cbd5e1',
      selectColor: '#2d3748',
      focusRing: 'rgba(79, 140, 255, .35)'
    }
  },
  dark: {
    name: 'Default Dark',
    css: '/theme/default-dark.css',
    ui: {
      containerBg: 'rgba(19, 23, 34, 0.72)',
      containerBorder: 'rgba(255, 255, 255, 0.15)',
      labelColor: '#e8ecf3',
      selectBg: '#202430',
      selectBorder: '#3a4254',
      selectColor: '#e8ecf3',
      focusRing: 'rgba(148, 163, 184, .35)'
    }
  },
  emilia: {
    name: 'Emilia (Dark)',
    css: '/theme/emilia-theme.css',
    ui: {
      containerBg: 'rgba(55, 50, 55, 0.9)',
      containerBorder: 'rgba(229, 148, 191, 0.45)',
      labelColor: '#e594bf',
      selectBg: '#342c34',
      selectBorder: '#b895c9',
      selectColor: '#f2d5e6',
      focusRing: 'rgba(229, 148, 191, .4)'
    }
  }
};

const STORAGE_KEY = 'swagger-theme';

function injectSwitcherStyles() {
  if (document.getElementById('theme-switcher-style')) {
    return;
  }

  const style = document.createElement('style');
  style.id = 'theme-switcher-style';
  style.textContent = `
    #theme-switcher-container {
      --ts-container-bg: rgba(255, 255, 255, 0.92);
      --ts-container-border: rgba(26, 26, 26, 0.15);
      --ts-label-color: #2d3748;
      --ts-select-bg: #ffffff;
      --ts-select-border: #cbd5e1;
      --ts-select-color: #2d3748;
      --ts-focus-ring: rgba(79, 140, 255, .35);
      display: flex;
      align-items: center;
      gap: 8px;
      margin-left: 10px;
      padding: 4px 6px;
      border-radius: 6px;
      border: 1px solid var(--ts-container-border);
      background: var(--ts-container-bg);
      font-family: "Segoe UI", Arial, sans-serif;
      flex-shrink: 0;
    }

    #theme-switcher-container label {
      margin: 0;
      font-size: 12px;
      font-weight: 600;
      line-height: 1;
      letter-spacing: .2px;
      color: var(--ts-label-color);
    }

    #theme-selector {
      min-width: 140px;
      height: 30px;
      padding: 4px 10px;
      border-radius: 6px;
      border: 1px solid var(--ts-select-border);
      background: var(--ts-select-bg);
      color: var(--ts-select-color);
      font-size: 12px;
      font-weight: 600;
      cursor: pointer;
      outline: none;
      transition: all .2s ease;
    }

    #theme-selector:focus {
      box-shadow: 0 0 0 2px var(--ts-focus-ring);
    }

    @media (max-width: 820px) {
      #theme-switcher-container {
        margin-left: 8px;
        margin-top: 8px;
      }

      #theme-selector {
        min-width: 0;
        width: 140px;
      }
    }
  `;

  document.head.appendChild(style);
}

function updateSwitcherMode(mode) {
  const theme = THEMES[mode];
  const container = document.getElementById('theme-switcher-container');
  if (!container || !theme || !theme.ui) return;

  container.style.setProperty('--ts-container-bg', theme.ui.containerBg);
  container.style.setProperty('--ts-container-border', theme.ui.containerBorder);
  container.style.setProperty('--ts-label-color', theme.ui.labelColor);
  container.style.setProperty('--ts-select-bg', theme.ui.selectBg);
  container.style.setProperty('--ts-select-border', theme.ui.selectBorder);
  container.style.setProperty('--ts-select-color', theme.ui.selectColor);
  container.style.setProperty('--ts-focus-ring', theme.ui.focusRing);
}

function initThemeSwitcher() {
  if (document.getElementById('theme-switcher-container')) {
    return;
  }

  injectSwitcherStyles();

  let attempts = 0;
  const maxAttempts = 100;
  const attachToTopbar = setInterval(() => {
    attempts += 1;
    const topbarWrapper = document.querySelector('.swagger-ui .topbar .wrapper');
    const topbarControls = document.querySelector('.swagger-ui .topbar .download-url-wrapper');
    if (!topbarWrapper && !topbarControls && attempts < maxAttempts) {
      return;
    }

    clearInterval(attachToTopbar);

    const savedTheme = localStorage.getItem(STORAGE_KEY) || 'light';

    const themeSelector = document.createElement('div');
    themeSelector.id = 'theme-switcher-container';

    const label = document.createElement('label');
    label.setAttribute('for', 'theme-selector');
    label.textContent = 'Theme';

    const select = document.createElement('select');
    select.id = 'theme-selector';

    Object.entries(THEMES).forEach(([key, theme]) => {
      const option = document.createElement('option');
      option.value = key;
      option.textContent = theme.name;
      select.appendChild(option);
    });

    select.value = THEMES[savedTheme] ? savedTheme : 'light';
    select.addEventListener('change', (event) => {
      applyTheme(event.target.value);
    });

    themeSelector.appendChild(label);
    themeSelector.appendChild(select);
    const mountTarget = topbarControls || topbarWrapper || document.body;
    mountTarget.appendChild(themeSelector);

    applyTheme(select.value);
  }, 120);
}

function applyTheme(themeName) {
  const theme = THEMES[themeName];
  if (!theme) {
    return;
  }

  document.querySelectorAll('link[data-theme-switcher]').forEach((link) => {
    link.remove();
  });

  const link = document.createElement('link');
  link.rel = 'stylesheet';
  link.href = theme.css;
  link.setAttribute('data-theme-switcher', 'true');
  document.head.appendChild(link);

  updateSwitcherMode(themeName);
  localStorage.setItem(STORAGE_KEY, themeName);
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initThemeSwitcher);
} else {
  initThemeSwitcher();
}
