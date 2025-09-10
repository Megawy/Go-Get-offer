/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./app/**/*.{js,ts,jsx,tsx}",
        "./components/**/*.{js,ts,jsx,tsx}",
        "./src/**/*.{js,ts,jsx,tsx}",
    ],
    theme: {
        extend: {
            fontFamily: {
                figtree: ['var(--font-figtree)'],
                lemands: ['var(--font-lemands)'],
                hacen: ['var(--font-hacen)'],
                honor: ['var(--font-honor)'],
                bavistage: ['var(--font-bavistage)'],
                ibmPlex : ['var(--font-ibm)'],
                roboto : ['var(--font-roboto)']
            },
            container: {
                center: true,
                padding: '2rem',
            },
            translate: {
                '101': '101%',
            },
            keyframes: {
                marquee: {
                    'from': { transform: 'translateX(0%)' },
                    'to': { transform: 'translateX(-50%)' }
                }
            },
            animation: {
                marquee: 'marquee 15s linear infinite',
                bounceSlow: 'bounce 2s infinite',
            }
        },
    },
    plugins: [],
}
