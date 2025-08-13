# GitHub Actions Workflows

This directory contains automated workflows for the E-Shop project that integrate with the Gemini CLI for AI-powered analysis and documentation.

## 📁 Workflows

### 🔄 `gemini-analysis.yml` - Main Analysis Workflow
**Triggers**: Push to main/develop/feature branches, Pull Requests, Manual

**What it does**:
- 🔍 Analyzes project architecture
- 📝 Generates API documentation for all services
- 🧪 Creates test suggestions
- 🐳 Provides Docker optimization recommendations
- 💬 Comments on Pull Requests with results
- 📤 Uploads artifacts for download

### ⏰ `auto-docs.yml` - Scheduled Documentation Updates
**Triggers**: Every Monday at 2 AM UTC, Manual

**What it does**:
- 📝 Updates all service documentation
- 📊 Generates fresh project analysis
- 🔄 Commits changes back to repository

### 🎛️ `custom-analysis.yml` - On-Demand Analysis
**Triggers**: Manual only

**What it does**:
- 🎯 Targeted analysis for specific services
- 🔧 Customizable analysis types (docs, tests, docker, analyze)
- 📤 Uploads results as artifacts

## 🚀 Quick Start

1. **Add API Key**: Go to Settings → Secrets → Add `GEMINI_API_KEY`
2. **Push Code**: Workflows activate automatically
3. **Monitor**: Check Actions tab for results

## 🔧 Local Testing

```bash
# Validate setup
npm run validate-setup

# Test CI scripts locally
npm run ci:install
npm run ci:docs
npm run ci:analysis
```

## 📖 Documentation

See [ACTIONS-SETUP.md](ACTIONS-SETUP.md) for detailed setup instructions and troubleshooting.
