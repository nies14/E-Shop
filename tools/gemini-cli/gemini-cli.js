#!/usr/bin/env node

const { GoogleGenerativeAI } = require('@google/generative-ai');
const fs = require('fs');
const path = require('path');
require('dotenv').config({ path: path.resolve(__dirname, '.env') });

class EShopGeminiCLI {
    constructor() {
        this.genAI = new GoogleGenerativeAI(process.env.GEMINI_API_KEY);
        this.model = this.genAI.getGenerativeModel({ model: "gemini-1.5-flash" });
        this.projectRoot = path.resolve(__dirname, '../..');
    }

    async analyzeProject() {
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

    showHelp() {
        console.log(`
🚀 E-Shop Gemini CLI

Usage: node gemini-cli.js <command> [options]

Commands:
  analyze                    Analyze the entire project structure
  docs <service>            Generate API documentation for a service
  tests <service>           Suggest test cases for a service
  optimize-docker           Optimize Dockerfiles in the project
  help                      Show this help message

Examples:
  node gemini-cli.js analyze
  node gemini-cli.js docs Catalog
  node gemini-cli.js tests Basket
  node gemini-cli.js optimize-docker

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
        default:
            console.error(`❌ Unknown command: ${command}`);
            cli.showHelp();
    }
}

if (require.main === module) {
    main().catch(console.error);
}

module.exports = EShopGeminiCLI;
