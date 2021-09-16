module.exports = {
  purge: {
    enabled: false,
    content: ['./src/**/*.{js,jsx,ts,tsx}', './public/index.html', "./public/**/*.{html}"]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
