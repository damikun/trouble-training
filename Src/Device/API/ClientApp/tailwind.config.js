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
      },
      width: {
        sm: "600px",
        md: "728px",
        lg: "984px",
      },
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
    gradientColorStops: [
      "responsive",
      "hover",
      "focus",
      "active",
      "group-hover",
    ],

    scrollSnapType: ["responsive"],
    hover: ["responsive", "hover", "focus"],
    boxShadow: ["responsive", "hover", "focus"],
    outline: ["responsive", "focus"],
    borderRadius: ["responsive", "hover", "focus", "last", "first"],
    backgroundColor: ["responsive", "hover", "focus", `group-hover`],
    borderColor: [
      "responsive",
      "hover",
      "focus",
      "focus-within",
      "active",
      "group-hover",
    ],
    scale: ["active", "group-hover"],
    animation: ["hover", `group-hover`, "group-focus", "focus-within"],
    visibility: ["responsive", `group-hover`, `group-focus`],
    display: ["responsive", "group-hover", "group-focus"],
    gridTemplateRows: ["responsive"],
    borderWidth: [
      "responsive",
      "last",
      "first",
      "hover",
      "focus",
      "focus-within",
    ],
    margin: ["responsive", "last"],
    alignContent: ["responsive", "hover", "focus"],
  },
  plugins:[require('tailwindcss-truncate-multiline')],
}
