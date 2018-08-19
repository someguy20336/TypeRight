# TypeRight
TypeRight is a simple tool that generates TypeScript files from your C# objects and controller actions. This project stemmed from some annoying things I found while doing web development using C# and MVC:

1. Making requests required entering a free text URL and parameter names.  If anything changed, the request broke.
2. If you wanted to strongly type a result from a web request, you would need to make the same C# object in TypeScript.  This led to maintenance nightmares trying to keep those in sync.
3. Using enums on the server and client required keeping them in sync

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
  - The relative path (from the perspective of the csproj root) to which the classes, interfaces, and enums should be printed out to.  By default, this value is "./Scripts/ServerObjects.ts"
- **ajaxFunctionName**
  - Allows you to define the function that makes Ajax calls.  More later in the Extracting Controller Actions section
- **ajaxFunctionModulePath**
  - The module file location for the ajax function defined above.  Used only in the module template.  Similiar to the server object result filepath, it should be relative from the perspective of the csproj root.

# Extracting classes, interfaces, and enums
Extracting a class or interface is as simple as adding an the `ScriptObject` attribute to the class.  From there, all properties and documentation are automatically extracted to a TypeScript file.  This output file location is configured by the `serverObjectsResultFilepath` configuration option.

As a example, the following C# object:

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

The `ScriptEnum` attribute has one property named `UseExtendedSyntax` that can be used to export the enum with some additional properties that are sometimes useful if your enum might have a display name or abbreviation.  Note, to add a display name or abbreviation, you can use any attribute that inherits from `IEnumDisplayNameProvider`.  A default implementation is provided in `EnumDisplayName`.  Due to the nature of parsing the code, **it is important that all properties be defined in the constructor as Named Arguments**.

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
Since all of the `ScriptObject`s and `ScriptEnum`s will be objects that you are passing back and forth between web service calls, it would make sense to strongly type those web service, too.  Currently, TypeRight only works with MVC actions.  To create a script for MVC Actions, add the `ScriptAction` attribute to the method call.  The output for these types of scripts will be:

`<project Root>\Scripts\<Controller Name>\<Controller Name>Actions.ts`

Let's take a look at an example.  Say we have this controller:

```C#

public class MyDefaultController : Controller 
{
  public ActionResult Index() 
  {
    // Do things
  }
  
  [ScriptAction]
  public JsonResult GetSomeData(int param1, MyClass someOtherData)
  {
    // Do things
    
    return Json(new MySecondClass())
  }
}

```

Assuming `MyClass` and `MySecondClass` have been attributed with `ScriptObject`, the resulting TypeScript will be created (some template specific details have been omitted for clarity):

```TypeScript

export function GetSomeData(param1: number, 
                            someOtherData: MyClass, 
                            success?: (result: MySecondClass) => void, 
                            fail?: () => void): void {
      // Makes a POST webservce call to /MyDefault/GetSomeData      
      // How this is done is configurable 
}

```

Now here is where the configuration comes into play.  

- By default, each controller action will call an auto generated method that uses $.ajax (yes... JQuery.  Maybe I will change this some day)
- You can change this to your own custom function located somewhere in your project by using the **ajaxFunctionName** configuration setting.  The function must have the following signature: (url: string, data: any, success: (result: any) => void, fail: () => any): any
  - The url you get will be in the form: "/<Controller Name>/<Action Name>".  If you use Areas, it will be "/<Area Name>/<Controller Name>/<Action>"
  - The data you get will be a dictionary of keys (parameter names) and values
- If using the module template, note that you will need to specify the file that the module is located in the **ajaxFunctionModulePath** configuration setting

You will notice that the `success` function will automatically pull the return type of the webservice call.  It does this by:
1. Finding the first return statement
2. If the return statement is the function "Json(...)", it will pull the type from the first parameter
3. Otherwise, it is whatever the return type of the method is


# Templates
There are two types of templates for no other reason than they are what I have used in the past.  If you are interested in some other type of template, let me know!  I think sometime in the future it would be cool to give users more flexibility on the template, but that is some ways off.

## Module Template
This is likely the recommended template with todays standards and module loading.  For this template, the result files will just be a collection of classes/methods that can be imported into other files.

## Namespace Template
This template is probably not the recommended one to use anymore, but I used it in the past so maybe someone might find it useful.  For this template, all classes/service calls are wrapped in a "namespace".  For example:

```TypeScript
namespace MyNamespace.Classes {
  export interface SomeType {
      // ... things
  }
}
```

```TypeScript
namespace MyNamespace.WebMethods {
  export function getStuff(param:string, success, fail): void {
    // Do stuff
  }
}
```

