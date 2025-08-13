#!/usr/bin/env node

const { GoogleGenerativeAI } = require('@google/generative-ai');
const fs = require('fs');
const path = require('path');
require('dotenv').config({ path: path.resolve(__dirname, '.env') });

class EShopGeminiCLI {
    constructor() {
        const apiKey = process.env.GEMINI_API_KEY;
        
        if (!apiKey || apiKey === 'demo_key_replace_with_real_key' || apiKey === 'your_gemini_api_key_here') {
            console.log('⚠️  Running in DEMO mode - No valid API key found');
            console.log('🔑 To enable AI features:');
            console.log('   1. Get a Gemini API key from https://makersuite.google.com/app/apikey');
            console.log('   2. Add it to your .env file: GEMINI_API_KEY=your_actual_key');
            console.log('   3. For repository maintainers: Add as GitHub secret');
            this.demoMode = true;
            this.genAI = null;
            this.model = null;
        } else {
            this.genAI = new GoogleGenerativeAI(apiKey);
            this.model = this.genAI.getGenerativeModel({ model: "gemini-1.5-flash" });
            this.demoMode = false;
        }
        
        this.projectRoot = path.resolve(__dirname, '../..');
    }

    async analyzeProject() {
        if (this.demoMode) {
            console.log('🔍 [DEMO] Analyzing E-Shop microservices project...');
            console.log('\n📊 [DEMO] Project Analysis:\n');
            console.log('This is a demo response. The actual analysis would include:');
            console.log('- Architecture pattern evaluation');
            console.log('- Microservices best practices review');
            console.log('- Potential improvements and recommendations');
            console.log('- Missing components identification');
            console.log('\n🔑 Add a real Gemini API key to see actual AI analysis.');
            return;
        }
        
        console.log('🔍 Analyzing E-Shop microservices project...');
        
        const projectStructure = this.getProjectStructure();
        const prompt = `
        Analyze this .NET microservices e-commerce project structure:
        ${projectStructure}
        
        Provide insights on:
        1. Architecture patterns used
        2. Potential improvements
        3. Missing components
        4. Best practices recommendations
        `;

        try {
            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            console.log('\n📊 Project Analysis:\n');
            console.log(response.text());
        } catch (error) {
            console.error('❌ Error analyzing project:', error.message);
        }
    }

    async generateDocumentation(service) {
        if (this.demoMode) {
            console.log(`📝 [DEMO] Generating documentation for ${service} service...`);
            console.log(`\n📚 [DEMO] ${service} Service Documentation:\n`);
            console.log('This is a demo response. The actual documentation would include:');
            console.log('- Complete API endpoint documentation');
            console.log('- Request/response models');
            console.log('- Authentication requirements');
            console.log('- Error codes and handling');
            console.log('\n🔑 Add a real Gemini API key to see actual AI-generated documentation.');
            return;
        }
        
        console.log(`📝 Generating documentation for ${service} service...`);
        
        const servicePath = path.join(this.projectRoot, service);
        if (!fs.existsSync(servicePath)) {
            console.error(`❌ Service ${service} not found`);
            return;
        }

        const controllers = this.getControllers(servicePath);
        const prompt = `
        Generate comprehensive API documentation for this ${service} microservice:
        Controllers: ${controllers}
        
        Include:
        1. API endpoints with HTTP methods
        2. Request/response models
        3. Status codes
        4. Usage examples
        `;

        try {
            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            
            const docPath = path.join(servicePath, 'API-Documentation.md');
            fs.writeFileSync(docPath, response.text());
            console.log(`✅ Documentation generated: ${docPath}`);
        } catch (error) {
            console.error('❌ Error generating documentation:', error.message);
        }
    }

    async suggestTests(service) {
        console.log(`🧪 Generating test suggestions for ${service} service...`);
        
        const servicePath = path.join(this.projectRoot, service);
        const controllers = this.getControllers(servicePath);
        
        const prompt = `
        Suggest comprehensive unit and integration tests for this ${service} microservice:
        Controllers: ${controllers}
        
        Include:
        1. Unit test cases for controllers
        2. Integration test scenarios
        3. Test data setup
        4. Mock configurations
        5. xUnit test framework examples
        `;

        try {
            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            console.log('\n🧪 Test Suggestions:\n');
            console.log(response.text());
        } catch (error) {
            console.error('❌ Error generating test suggestions:', error.message);
        }
    }

    async optimizeDockerfiles() {
        console.log('🐳 Analyzing and optimizing Dockerfiles...');
        
        const dockerfiles = this.findDockerfiles();
        if (dockerfiles.length === 0) {
            console.log('❌ No Dockerfiles found');
            return;
        }

        for (const dockerfile of dockerfiles) {
            const content = fs.readFileSync(dockerfile, 'utf8');
            const prompt = `
            Optimize this Dockerfile for a .NET microservice:
            ${content}
            
            Suggestions for:
            1. Multi-stage builds
            2. Layer caching
            3. Security improvements
            4. Size reduction
            5. Performance optimizations
            `;

            try {
                const result = await this.model.generateContent(prompt);
                const response = await result.response;
                console.log(`\n🐳 Optimization for ${dockerfile}:\n`);
                console.log(response.text());
            } catch (error) {
                console.error(`❌ Error optimizing ${dockerfile}:`, error.message);
            }
        }
    }

    getProjectStructure() {
        const structure = [];
        const services = ['Catalog', 'Basket', 'Ordering', 'Discount'];
        
        services.forEach(service => {
            const servicePath = path.join(this.projectRoot, service);
            if (fs.existsSync(servicePath)) {
                structure.push(`${service}/`);
                const subDirs = fs.readdirSync(servicePath, { withFileTypes: true })
                    .filter(dirent => dirent.isDirectory())
                    .map(dirent => `  ${dirent.name}/`);
                structure.push(...subDirs);
            }
        });
        
        return structure.join('\n');
    }

    getControllers(servicePath) {
        const controllersPath = path.join(servicePath, service + '.API', 'Controllers');
        if (!fs.existsSync(controllersPath)) return 'No controllers found';
        
        return fs.readdirSync(controllersPath)
            .filter(file => file.endsWith('.cs'))
            .map(file => {
                const content = fs.readFileSync(path.join(controllersPath, file), 'utf8');
                return `File: ${file}\n${content.substring(0, 500)}...`;
            })
            .join('\n\n');
    }

    findDockerfiles() {
        const dockerfiles = [];
        
        function searchDirectory(dir) {
            const items = fs.readdirSync(dir, { withFileTypes: true });
            
            items.forEach(item => {
                const fullPath = path.join(dir, item.name);
                
                if (item.isFile() && item.name === 'Dockerfile') {
                    dockerfiles.push(fullPath);
                } else if (item.isDirectory() && !item.name.startsWith('.') && !item.name.includes('node_modules')) {
                    searchDirectory(fullPath);
                }
            });
        }
        
        searchDirectory(this.projectRoot);
        return dockerfiles;
    }

    async reviewPullRequest(prNumber, reviewType = 'full') {
        if (this.demoMode) {
            console.log(`🕵️ [DEMO] Reviewing Pull Request #${prNumber} (${reviewType} review)...`);
            console.log('\n🕵️ [DEMO] Code Review Complete:\n');
            console.log('This is a demo response. The actual review would include:');
            console.log('- Code quality analysis');
            console.log('- Security vulnerability detection');
            console.log('- Performance optimization suggestions');
            console.log('- Best practices recommendations');
            console.log('\n🔑 Add a real Gemini API key to see actual AI code review.');
            return;
        }
        
        console.log(`🕵️ Reviewing Pull Request #${prNumber} (${reviewType} review)...`);
        
        try {
            // Get PR diff and files
            const changedFiles = this.getChangedFiles();
            const diffContent = this.getPRDiff(prNumber);
            
            const prompt = `
            Review this Pull Request with focus on ${reviewType} analysis:
            
            PR #${prNumber}
            Changed Files: ${changedFiles.join(', ')}
            
            Diff Content:
            ${diffContent}
            
            Provide detailed review covering:
            1. Code quality and best practices
            2. Potential bugs and issues
            3. Security vulnerabilities
            4. Performance implications
            5. Architecture compliance
            6. Specific improvement suggestions
            7. Auto-implementable fixes
            
            Format as actionable feedback with severity levels (High/Medium/Low).
            `;

            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            const reviewContent = response.text();
            
            // Save review to file
            fs.writeFileSync('code-review-summary.md', reviewContent);
            console.log('\n🕵️ Code Review Complete:\n');
            console.log(reviewContent);
            
            return reviewContent;
        } catch (error) {
            console.error('❌ Error reviewing PR:', error.message);
        }
    }

    async generateImplementationSuggestions(prNumber) {
        console.log(`🔧 Generating implementation suggestions for PR #${prNumber}...`);
        
        try {
            const changedFiles = this.getChangedFiles();
            const codebaseContext = this.getCodebaseContext(changedFiles);
            
            const prompt = `
            Based on this codebase and recent changes, suggest specific code implementations:
            
            PR #${prNumber}
            Changed Files: ${changedFiles.join(', ')}
            
            Codebase Context:
            ${codebaseContext}
            
            Provide:
            1. Missing error handling implementations
            2. Suggested unit tests with actual code
            3. Performance optimizations with code examples
            4. Security improvements with implementations
            5. Best practice implementations
            6. Missing validation logic
            7. Logging and monitoring improvements
            
            For each suggestion, provide:
            - Exact file path
            - Exact code to add/modify
            - Explanation of the improvement
            - Risk level (Safe/Medium/High)
            `;

            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            const suggestions = response.text();
            
            fs.writeFileSync('implementation-suggestions.md', suggestions);
            console.log('\n� Implementation Suggestions:\n');
            console.log(suggestions);
            
            return suggestions;
        } catch (error) {
            console.error('❌ Error generating suggestions:', error.message);
        }
    }

    async performSecurityScan() {
        console.log('🔒 Performing security analysis...');
        
        try {
            const codeFiles = this.getAllCodeFiles();
            let securityIssues = [];
            
            for (const file of codeFiles.slice(0, 10)) { // Limit to avoid token limits
                const content = fs.readFileSync(file, 'utf8');
                
                const prompt = `
                Analyze this code file for security vulnerabilities:
                
                File: ${file}
                Content:
                ${content.substring(0, 2000)}...
                
                Check for:
                1. SQL injection vulnerabilities
                2. XSS vulnerabilities
                3. Authentication bypasses
                4. Authorization issues
                5. Input validation problems
                6. Sensitive data exposure
                7. Insecure dependencies
                8. Configuration issues
                
                Rate severity: Critical/High/Medium/Low
                Provide specific line numbers and fix suggestions.
                `;

                const result = await this.model.generateContent(prompt);
                const response = await result.response;
                securityIssues.push(`\n## ${file}\n${response.text()}`);
            }
            
            const securityReport = securityIssues.join('\n');
            fs.writeFileSync('security-report.md', securityReport);
            console.log('\n🔒 Security Report:\n');
            console.log(securityReport);
            
            return securityReport;
        } catch (error) {
            console.error('❌ Error in security scan:', error.message);
        }
    }

    async performanceReview() {
        console.log('⚡ Analyzing performance characteristics...');
        
        try {
            const controllers = this.getAllControllers();
            const prompt = `
            Analyze these API controllers for performance issues:
            
            ${controllers}
            
            Review for:
            1. N+1 query problems
            2. Missing async/await patterns
            3. Inefficient database queries
            4. Missing caching opportunities
            5. Memory leaks potential
            6. Blocking operations
            7. Missing pagination
            8. Inefficient data structures
            
            Provide specific performance optimizations with code examples.
            `;

            const result = await this.model.generateContent(prompt);
            const response = await result.response;
            
            fs.writeFileSync('performance-report.md', response.text());
            console.log('\n⚡ Performance Review:\n');
            console.log(response.text());
            
            return response.text();
        } catch (error) {
            console.error('❌ Error in performance review:', error.message);
        }
    }

    getChangedFiles() {
        try {
            if (fs.existsSync('changed-files.txt')) {
                return fs.readFileSync('changed-files.txt', 'utf8').split('\n').filter(f => f.trim());
            }
            return [];
        } catch (error) {
            return [];
        }
    }

    getPRDiff(prNumber) {
        try {
            // In a real implementation, this would use GitHub API
            // For now, return a placeholder
            return 'PR diff would be retrieved via GitHub API';
        } catch (error) {
            return 'Unable to retrieve PR diff';
        }
    }

    getCodebaseContext(files) {
        let context = '';
        files.slice(0, 5).forEach(file => {
            if (fs.existsSync(file) && file.endsWith('.cs')) {
                const content = fs.readFileSync(file, 'utf8');
                context += `\n## ${file}\n${content.substring(0, 1000)}...\n`;
            }
        });
        return context;
    }

    getAllCodeFiles() {
        const codeFiles = [];
        const services = ['Catalog', 'Basket', 'Ordering', 'Discount'];
        
        services.forEach(service => {
            const servicePath = path.join(this.projectRoot, service);
            if (fs.existsSync(servicePath)) {
                this.findCodeFiles(servicePath, codeFiles);
            }
        });
        
        return codeFiles;
    }

    getAllControllers() {
        let controllers = '';
        const services = ['Catalog', 'Basket', 'Ordering', 'Discount'];
        
        services.forEach(service => {
            const controllersPath = path.join(this.projectRoot, service, service + '.API', 'Controllers');
            if (fs.existsSync(controllersPath)) {
                const files = fs.readdirSync(controllersPath).filter(f => f.endsWith('.cs'));
                files.forEach(file => {
                    const content = fs.readFileSync(path.join(controllersPath, file), 'utf8');
                    controllers += `\n## ${service}/${file}\n${content.substring(0, 1500)}...\n`;
                });
            }
        });
        
        return controllers;
    }

    findCodeFiles(dir, codeFiles) {
        const items = fs.readdirSync(dir, { withFileTypes: true });
        
        items.forEach(item => {
            const fullPath = path.join(dir, item.name);
            
            if (item.isFile() && (item.name.endsWith('.cs') || item.name.endsWith('.js'))) {
                codeFiles.push(fullPath);
            } else if (item.isDirectory() && !item.name.startsWith('.') && !item.name.includes('node_modules')) {
                this.findCodeFiles(fullPath, codeFiles);
            }
        });
    }

    showHelp() {
        console.log(`
�🚀 E-Shop Gemini CLI

Usage: node gemini-cli.js <command> [options]

Commands:
  analyze                    Analyze the entire project structure
  docs <service>            Generate API documentation for a service
  tests <service>           Suggest test cases for a service
  optimize-docker           Optimize Dockerfiles in the project
  review-pr <number> [type] Review a Pull Request (types: full, security, performance, architecture)
  implement-suggestions <pr> Generate specific implementation suggestions
  security-scan             Perform comprehensive security analysis
  performance-review        Analyze performance characteristics
  help                      Show this help message

Examples:
  node gemini-cli.js analyze
  node gemini-cli.js docs Catalog
  node gemini-cli.js tests Basket
  node gemini-cli.js optimize-docker
  node gemini-cli.js review-pr 123 security
  node gemini-cli.js implement-suggestions 123
  node gemini-cli.js security-scan
  node gemini-cli.js performance-review

Services: Catalog, Basket, Ordering, Discount

Note: Make sure to set your GEMINI_API_KEY in the .env file
        `);
    }
}

// CLI Entry Point
async function main() {
    const cli = new EShopGeminiCLI();
    const args = process.argv.slice(2);
    
    if (args.length === 0 || args[0] === 'help') {
        cli.showHelp();
        return;
    }

    const command = args[0];
    
    switch (command) {
        case 'analyze':
            await cli.analyzeProject();
            break;
        case 'docs':
            if (args[1]) {
                await cli.generateDocumentation(args[1]);
            } else {
                console.error('❌ Please specify a service name');
            }
            break;
        case 'tests':
            if (args[1]) {
                await cli.suggestTests(args[1]);
            } else {
                console.error('❌ Please specify a service name');
            }
            break;
        case 'optimize-docker':
            await cli.optimizeDockerfiles();
            break;
        case 'review-pr':
            if (args[1]) {
                await cli.reviewPullRequest(args[1], args[2] || 'full');
            } else {
                console.error('❌ Please specify PR number');
            }
            break;
        case 'implement-suggestions':
            if (args[1]) {
                await cli.generateImplementationSuggestions(args[1]);
            } else {
                console.error('❌ Please specify PR number');
            }
            break;
        case 'security-scan':
            await cli.performSecurityScan();
            break;
        case 'performance-review':
            await cli.performanceReview();
            break;
        case 'help':
            cli.showHelp();
            break;
        default:
            console.error(`❌ Unknown command: ${command}`);
            cli.showHelp();
    }
}

if (require.main === module) {
    main().catch(console.error);
}

module.exports = EShopGeminiCLI;
