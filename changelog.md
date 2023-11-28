# Changelog

### 1.0.0 - Custom Render Tree

1.0.0 constitutes the first major and biggest update to the framework. It adds a custom render tree, selective rerendering, editor components, as well as major changes and improvements to StateVar, such as lower-hierarchy persistence. This severely changed the development paradigm, but makes it more streamlined with increased clarity.

**Changes**

- ...
- ...

**Breaking Changes**

- ...

---

### 0.3.1 - Changelog & Version

0.3.1 moves the changelog to the project root and fixes a missing version increment in package.json

**Changes**

- Moved changelog.md to project root.
- Corrected version in package.json.

**Breaking Changes**

- /

---

### 0.3.0 - StateVar modifications & fixes

0.3.0 attempts to fix some issues with StateVar, mainly fixing implicit comparison issues and encapsulating the value setter.

**Changes**

- Fixed implicit value comparison using == and != in StateVar, when right-hand argument is `null`.

**Breaking Changes**

- Encapsulated Value Setter for StateVar
  - Enforced the usage of `StateVar.Set()` to assign to `Value`.
  - Previously, there was potential for misinterpretation when setting properties of object-values, which could mistakenly be considered as triggering a state change. The updated implementation ensures clarity by requiring the explicit use of `StateVar.Set()` to initiate state changes.
