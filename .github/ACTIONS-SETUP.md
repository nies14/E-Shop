# GitHub Actions Setup Guide for Gemini CLI

This guide helps you set up GitHub Actions workflows for automated AI analysis and documentation generation.

## 🔧 Setup Steps

### 1. Add GitHub Secret

1. Go to your GitHub repository: `https://github.com/nies14/E-Shop`
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Add the following secret:
   - **Name**: `GEMINI_API_KEY`
   - **Value**: Your Gemini API key (starts with `AIzaSy...`)

### 2. Enable Workflows

The workflows are already committed to your repository. They will automatically activate when you push the `.github/workflows/` folder.

### 3. Workflow Overview

#### 🔄 **Main Workflow**: `gemini-analysis.yml`
- **Triggers**: Push to main/develop/feature branches, Pull Requests
- **Actions**: 
  - Project analysis
  - Documentation generation for all services
  - Test suggestions
  - Docker optimization
  - Artifact upload
  - PR comments with results

#### ⏰ **Scheduled Workflow**: `auto-docs.yml`
- **Triggers**: Every Monday at 2 AM UTC, Manual dispatch
- **Actions**:
  - Updates all service documentation
  - Commits changes back to repository

#### 🎛️ **Custom Workflow**: `custom-analysis.yml`
- **Triggers**: Manual dispatch only
- **Actions**:
  - Targeted analysis for specific services
  - Customizable analysis types
  - On-demand documentation generation

## 🚀 How to Use

### Automatic Triggers
```bash
# Push code to trigger analysis
git add .
git commit -m "feat: Add new feature"
git push origin feature/your-feature

# Create PR to get AI analysis comments
gh pr create --title "Add new feature" --body "Description"
```

### Manual Triggers

#### Run Custom Analysis
1. Go to **Actions** tab in GitHub
2. Select **Custom AI Analysis**
3. Click **Run workflow**
4. Choose:
   - **Service**: Catalog, Basket, Ordering, Discount, or All
   - **Analysis Type**: docs, tests, docker, or analyze

#### Force Documentation Update
1. Go to **Actions** tab
2. Select **Auto Documentation Update**
3. Click **Run workflow**

## 📊 Workflow Outputs

### Artifacts Generated
- **API Documentation**: Complete API docs for each service
- **Test Suggestions**: AI-generated test cases and scenarios
- **Docker Optimization**: Dockerfile improvement recommendations
- **Project Analysis**: Overall architecture analysis

### PR Comments
When you create a pull request, the workflow will automatically:
- Analyze your changes
- Generate documentation
- Comment on the PR with results
- Upload artifacts for download

## 🔍 Monitoring Workflows

### Check Workflow Status
```bash
# View workflow runs
gh run list

# View specific run details
gh run view <run-id>

# Download artifacts
gh run download <run-id>
```

### Workflow Logs
1. Go to **Actions** tab in GitHub
2. Click on any workflow run
3. Expand job steps to view detailed logs

## 🛠️ Troubleshooting

### Common Issues

#### 1. API Key Invalid
- Verify your Gemini API key in GitHub Secrets
- Ensure the key starts with `AIzaSy...`
- Check if the key is active in Google AI Studio

#### 2. Workflow Fails
- Check the workflow logs in GitHub Actions
- Verify all required files exist in the repository
- Ensure Node.js dependencies are properly specified

#### 3. No Documentation Generated
- Check if the service exists in the repository
- Verify the CLI script paths are correct
- Review error messages in workflow logs

### Local Testing
```bash
# Test workflows locally before pushing
npm run ci:install
npm run ci:docs
npm run ci:analysis
```

## 📈 Optimization Tips

1. **Reduce API Calls**: Use conditional triggers to avoid unnecessary runs
2. **Cache Dependencies**: Workflows use npm caching for faster builds
3. **Parallel Execution**: Services are analyzed in parallel when possible
4. **Error Handling**: Workflows continue on errors to maximize output

## 🔄 Updating Workflows

To modify workflows:
1. Edit files in `.github/workflows/`
2. Commit and push changes
3. Workflows will automatically use the new configuration

## 📞 Support

For issues with:
- **GitHub Actions**: Check GitHub Actions documentation
- **Gemini API**: Verify API key and quotas
- **Workflow Configuration**: Review YAML syntax and job steps
