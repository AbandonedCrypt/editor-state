# Changelog
### 0.3.1 - Changelog & Version

0.3.1 moves the changelog to the project root and fixes a missing version increment in package.json

**Changes**
- Moved changelog.md to project root.
- Corrected version in package.json.

**Breaking Changes**
- /

______________________
### 0.3.0 - StateVar modifications & fixes

0.3.0 attempts to fix some issues with StateVar, mainly fixing implicit comparison issues and encapsulating the value setter.

**Changes**
- Fixed implicit value comparison using == and != in StateVar, when right-hand argument is `null`.

**Breaking Changes**
- Encapsulated Value Setter for StateVar
  - Enforced the usage of `StateVar.Set()` to assign to `Value`.
  - Previously, there was potential for misinterpretation when setting properties of object-values, which could mistakenly be considered as triggering a state change. The updated implementation ensures clarity by requiring the explicit use of `StateVar.Set()` to initiate state changes.
