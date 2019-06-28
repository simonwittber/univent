# univent
Univent is like UnityEngine.Events.UnityEvent, but better.

It uses no runtime reflection, and has no runtime allocations.

It requires surrogates. https://github.com/simonwittber/surrogates

It allows any combination of primitive types and Unity objects as function parameters.

To use in your project, clone it as a submodule somewhere in your Assets folder.

```git subtree add --prefix {YOUR FOLDER} https://github.com/simonwittber/univent master```

