# TypeRight
TypeRight is a simple tool that generates TypeScript files from your C# objects and controller actions. This project stemmed from two annoying things I found while doing web development using C# and MVC:

1. Making requests required entering a free text URL and parameter names.  If anything changed, the request broke.
2. If you wanted to strongly type a result from a web request, you would need to make the same C# object in TypeScript.  This led to maintenance nightmares trying to keep those in sync.
3. Using enums on the server and client required keeping them in sync as well

This tool solves both of those problems by autogenerating those TypeScript files every time you build your project.

# Getting started
For the best experience, first install the TypeRight extension from the Visual Studio marketplace.  This extension is not required for the whole thing to work, but it does add some benefits:

1. Better script generating performance since it integrates directly with the Visual Studio data model
2. Some helper commands to install the package, generate scripts on demand, and add the config file
3. Down the road, this extension may become more useful... but that is TBD

After you install the extension (or don't, it doesn't matter) install the TypeRight package from Nuget to any project you are interested in extracting objects from.  Generally, this is the web project at the very least.

# Configuration
In order to properly function, a config file must exist in the web csproj root directory named "typeRightConfig.json". The config file consists of the following options:

- **enabled**
  - a boolean flag for whether script generation is enabled
- **templateType**
  - The type of template to use.  The options are "namespace" or "module".  More about these templates below.
- **classNamespace**
  - When using the namespace template, this is the namespace to use for class/interface types
- **enumNamespace**
  - When using the namespace template, this is the namespace to use for enums
- **webMethodNamespace**
  - When using the namespace template, this is the namespace to use for controller actions (more about that below)
- **serverObjectsResultFilepath**
  - The relative path to which the classes, interfaces, and enums should be printed out to.  By default, "./Scripts/ServerObjects.ts"
- **ajaxFunctionName**
  - Allows you to define the function that makes Ajax calls.  More later

# Extracting classes, interfaces, and enums
Extracting a class or interface is as simple as adding an the `ScriptObject` attribute to the class.  From there, all properties and documentation are automatically extracted to the script.  So, the following C# object:

```C#

/// <summary>
/// This is my class
/// </summary>
[ScriptObject]
public class MyClass
{
  /// <summary>
  /// Gets the first property
  /// </summary>
  public string PropertyOne { get; }
  
  public int PropertyTwo { get; }
  
  public List<string> PropertyThree { get; }
}
```

will be extracted to the following TypeScript object

```TypeScript
/*
 * This is my class
 */
export interface MyClass {
  /** Gets the first property */
  PropertyOne: string;
  
  PropertyTwo: number;
  
  PropertyThree: string[];
}
```

Enums are extracted by adding the `ScriptEnum` attribute.  This will create an enumeration type in TypeScript.  As another example:

```C#
[ScriptEnum]
public enum MyEnum 
{
  One = 1
  Two = 2,
  ThreeHundred = 300
}

```

will give you 

```TypeScript
export enum MyEnum {
  One = 1,
  Two = 2,
  ThreeHundred = 300
}
```

The `ScriptEnum` attribute has one property named `UseExtendedSyntax` that can be used to export the enum with some additional properties that are sometimes useful if your enum might have a display name or abbreviation.  Note, to add a display name or abbreviation, you can use any attribute that inherits from `IEnumDisplayNameProvider`.  A default implementation is provided as `DefaultEnumDisplayNameProvider`

```C#
[ScriptEnum(UseExtendedSyntax = true)]
public enum MyEnum 
{
  One = 1
  Two = 2,
  [DefaultEnumDisplayNameProvider(DisplayName = "Three Hundred", Abbreviation = "3 Hundo")]
  ThreeHundred = 300
}

```

will give you 

```TypeScript
export let MyEnum = {
  One = {
    id: 1,
    name: "One",
    abbrev: "One"
  },
  Two = {
    id: 2,
    name: "Two",
    abbrev: "Two"
  },
  ThreeHundred = {
    id: 300,
    name: "Three Hundred",
    abbrev: "3 Hundo"
  }
}
```

# Extracting Controller Actions
Coming soon....

# Templates
Coming soon....

# Translating types
Coming soon....
