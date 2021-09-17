module.exports = {
  purge: {
    enabled: true,
    content: ['./src/**/*.{js,jsx,ts,tsx}', './public/index.html', "./public/**/*.{html}"]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      truncate: {
        lines: {
          1: "1",
          2: "2",
          3: "3",
          5: "5",
          8: "8",
        },
      }
    },
  },
  variants: {
    extend: {
      display: ["responsive", "group-hover", "group-focus"],
      width: ["hover", "focus", "focus-within", `group-hover`, "responsive"],
      backgroundColor: ["focus-within"],
      borderStyle: ["first", "last"],
      dropShadow: ["hover", "focus"],
    },
  },
  plugins:[require('tailwindcss-truncate-multiline')],
}
