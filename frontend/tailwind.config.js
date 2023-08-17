/** @type {import('tailwindcss').Config} */
module.exports = {
   content: ["./src/**/*.{js,jsx,ts,tsx}"],
   theme: {
      extend: {},
   },
   plugins: [require("@tailwindcss/forms")],
   safelist: [
      {
         // Tailwind does not find some class names that are
         // generated with string interpolation (as used in BoardColumn.tsx),
         // which results in them not being included in the bundled css.
         // For more details see:
         // https://tailwindcss.com/docs/content-configuration#class-detection-in-depth
         pattern: /grid-cols-[0-9]+/,
      },
   ],
};
