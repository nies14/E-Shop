#!/usr/bin/env node

const fs = require('fs');
const path = require('path');

console.log('🔍 Validating GitHub Actions setup for Gemini CLI...\n');

const checks = [
    {
        name: 'Workflow files exist',
        check: () => {
            const workflows = [
                '.github/workflows/gemini-analysis.yml',
                '.github/workflows/auto-docs.yml', 
                '.github/workflows/custom-analysis.yml'
            ];
            return workflows.every(file => fs.existsSync(file));
        }
    },
    {
        name: 'Package.json has CI scripts',
        check: () => {
            const pkg = JSON.parse(fs.readFileSync('package.json', 'utf8'));
            return pkg.scripts['ci:install'] && pkg.scripts['ci:docs'] && pkg.scripts['ci:analysis'];
        }
    },
    {
        name: 'Gemini CLI exists',
        check: () => fs.existsSync('tools/gemini-cli/gemini-cli.js')
    },
    {
        name: 'Gemini CLI package.json exists', 
        check: () => fs.existsSync('tools/gemini-cli/package.json')
    },
    {
        name: 'Environment template exists',
        check: () => fs.existsSync('tools/gemini-cli/.env.example')
    },
    {
        name: 'Gitignore includes generated files',
        check: () => {
            const gitignore = fs.readFileSync('.gitignore', 'utf8');
            return gitignore.includes('**/API-Documentation.md') && 
                   gitignore.includes('test-suggestions-*.md');
        }
    }
];

let passed = 0;
let failed = 0;

checks.forEach(check => {
    try {
        if (check.check()) {
            console.log(`✅ ${check.name}`);
            passed++;
        } else {
            console.log(`❌ ${check.name}`);
            failed++;
        }
    } catch (error) {
        console.log(`❌ ${check.name} - Error: ${error.message}`);
        failed++;
    }
});

console.log(`\n📊 Results: ${passed} passed, ${failed} failed\n`);

if (failed === 0) {
    console.log('🎉 All checks passed! Your GitHub Actions setup is ready.');
    console.log('\n📋 Next steps:');
    console.log('1. Add GEMINI_API_KEY to GitHub Secrets');
    console.log('2. Push changes to trigger workflows');
    console.log('3. Check Actions tab for workflow results');
} else {
    console.log('🚨 Some checks failed. Please review the setup.');
    process.exit(1);
}
