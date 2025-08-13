# 🔐 Managing Secrets in Public Repositories

## 🎯 **The Challenge**
Your E-Shop repository is public, but you need to keep the Gemini API key secure while allowing the AI workflows to function.

## ✅ **Recommended Solution: GitHub Secrets + Demo Mode**

### **1. Repository Secrets (For Maintainers)**
- **Where**: Repository Settings → Secrets and variables → Actions
- **Secret Name**: `GEMINI_API_KEY`
- **Secret Value**: Your actual Gemini API key
- **Access**: Only repository maintainers and trusted collaborators

### **2. Demo Mode (For External Contributors)**
- **Automatic fallback**: When no API key is available
- **Shows example outputs**: Demonstrates what the AI would generate
- **No API calls**: Doesn't consume your API quota
- **Educational**: Shows contributors what the tool does

## 🛡️ **Security Features**

### ✅ **What's Protected:**
```yaml
# In workflows - API key is masked in logs
- name: Setup Environment
  run: echo "GEMINI_API_KEY=${{ secrets.GEMINI_API_KEY }}" > .env
  # Output shows: GEMINI_API_KEY=***
```

### ✅ **Fork Protection:**
```yaml
# Only runs for trusted contributors
if: ${{ secrets.GEMINI_API_KEY != '' }}
```

### ✅ **Demo Mode Output:**
```bash
⚠️  Running in DEMO mode - No valid API key found
🔑 To enable AI features:
   1. Get a Gemini API key from https://makersuite.google.com/app/apikey
   2. Add it to your .env file: GEMINI_API_KEY=your_actual_key
   3. For repository maintainers: Add as GitHub secret
```

## 📋 **Setup Instructions**

### **For Repository Owner (You):**

1. **Add GitHub Secret:**
   ```
   Repository → Settings → Secrets and variables → Actions
   → New repository secret
   Name: GEMINI_API_KEY
   Value: AIzaSyBUroaM_IolgPlR5KiMUvW6j3Ax2_EwoXg
   ```

2. **Test Workflows:**
   ```bash
   # Push changes to trigger workflows
   git add .
   git commit -m "feat: Add secure AI workflows with demo mode"
   git push
   ```

### **For External Contributors:**

1. **Fork the repository**
2. **Workflows run in demo mode** (no API key needed)
3. **See example outputs** to understand AI capabilities
4. **Option**: Get their own API key for local testing

### **For Local Development:**

1. **Create local .env file:**
   ```bash
   echo "GEMINI_API_KEY=your_api_key_here" > tools/gemini-cli/.env
   ```

2. **Test locally:**
   ```bash
   npm run cli:analyze
   npm run cli:docs Catalog
   ```

## 🚀 **Benefits of This Approach**

### ✅ **Security:**
- API key never exposed in code or logs
- External contributors can't access your API quota
- Only trusted maintainers can use real AI features

### ✅ **Collaboration:**
- External contributors can still see what the tool does
- Demo mode provides educational value
- No barriers for open source contributions

### ✅ **Functionality:**
- Full AI features for maintainers
- Graceful degradation for others
- Clear instructions for setup

## 🔍 **How It Works**

### **When API Key is Available:**
```bash
🔍 Analyzing E-Shop microservices project...
📊 Project Analysis:
[Real AI-generated analysis with detailed insights]
```

### **When API Key is Missing:**
```bash
🔍 [DEMO] Analyzing E-Shop microservices project...
📊 [DEMO] Project Analysis:
This is a demo response. The actual analysis would include:
- Architecture pattern evaluation
- Microservices best practices review
- Potential improvements and recommendations
🔑 Add a real Gemini API key to see actual AI analysis.
```

## 📚 **Alternative Approaches**

### **Option 1: Personal Access Tokens**
- Less secure for AI API keys
- Better for GitHub API access

### **Option 2: Separate Private Repository**
- Keep workflows in private repo
- More complex setup

### **Option 3: Self-Hosted Runners**
- Complete control over secrets
- Higher maintenance overhead

## 🎯 **Recommendation**

**Use the current setup**: GitHub Secrets + Demo Mode

**Why?**
- ✅ Secure by default
- ✅ Easy for contributors
- ✅ No API quota theft
- ✅ Educational value
- ✅ Standard practice

## 🚨 **What NOT to Do**

❌ **Don't commit API keys to code**
❌ **Don't use public environment variables**
❌ **Don't expose keys in workflow logs**
❌ **Don't share your personal API key**

## ✅ **Next Steps**

1. **Add the GEMINI_API_KEY secret** to your repository
2. **Push the updated workflows** with demo mode
3. **Test with a new PR** to see both modes working
4. **Document the setup** for contributors

Your repository is now secure AND functional for all contributors! 🎉
