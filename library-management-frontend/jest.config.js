module.exports = {
    collectCoverage: true,
    collectCoverageFrom: ["src/**/*.{js,jsx}"],
    coverageDirectory: "coverage",
    testEnvironment: "jsdom",
    transform: {
        '^.+\\.jsx?$': 'babel-jest', // Add this line to transform ES6 modules
    },
    setupFilesAfterEnv: ["<rootDir>/jest.setup.js"],
};