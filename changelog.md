# Changelog

### 0.4.0-beta3 - Remove StateVar Rerender Restriction

0.4.0-beta3 removes a useless restriction on StateVar, where it would not cause a rerender if assigned a value equal to its current one.

**Changes**

- Remove early exit on `StateVar.Set()` if the set-value is equal to the current one.

**Hotfixes**

- Removed hardcoded placeholder UXML path for `uxmlSource`
- Fixed blank editor window NullRef due to missing VisualTreeAsset when not created through wizard.

**Breaking Changes**

- /

---

### 0.4.0-beta2 - [Hotfix] StateVar Host

0.4.0-beta2 is a hotfix allowing StateVar to be initialized with any IStateHost to allow for StatefulPopupWindow to be StateVar host.

**Changes**

- StateVar ctor state host type changed from StatefulEditorWindow to IStateHost.

**Breaking Changes**

- /

---

### 0.4.0-beta1 - Stateful Popup Window

0.4.0-beta1 adds a popup window abstraction akin to the StatefulEditorWindow, able to render and update using StateVar.

**Changes**

- Added `StatefulPopupWindow `and `StatefulPopupWindow<T>`
  - The generic window comes with a default callback implementation, to send data from the popup back to the caller.
- Abstracted away some of the core popup logic

**Breaking Changes**

- /

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
