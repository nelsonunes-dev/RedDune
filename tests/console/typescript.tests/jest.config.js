/** @type {import('jest').Config} */
const path = require('path');

module.exports = {
  testEnvironment: "node",
  rootDir: path.resolve(__dirname, '../..'),
  moduleFileExtensions: ["ts", "js"],
  testMatch: ["**/tests/console/typescript.tests/**/*.test.ts"],
  transform: {
    '^.+\\.tsx?$': [path.resolve(__dirname, 'node_modules/ts-jest/dist/index.js'), {
      tsconfig: path.resolve(__dirname, 'tsconfig.json'),
      diagnostics: {
        ignoreCodes: ['TS151002'],
      },
    }],
  },
};