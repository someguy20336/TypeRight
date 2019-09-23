# TypeRight
TypeRight is a simple tool that generates TypeScript files from your C# objects and controller actions. This project stemmed from some annoying things I found while doing web development using C# and MVC:

1. Making requests required entering a free text URL and parameter names.  If anything changed, the request broke.
2. If you wanted to strongly type a result from a web request, you would need to make the same C# object in TypeScript.  This led to maintenance nightmares trying to keep those in sync.
3. Using enums on the server and client required keeping them in sync

This tool solves those problems by autogenerating those TypeScript files every time you build your project.

**NOTE**: Version 0.9.0 has some breaking changes with configuration. Some options have been removed and action configuration has changed. See the configuration section for more.

# Quick Start

1. Find ["TypeRight" in Nuget](https://www.nuget.org/packages/TypeRight) and install it (generally to the web project, but it could be for any)
2. (Optional) If you haven't already, install the [VS Extension](https://marketplace.visualstudio.com/items?itemName=someguy20336.TypeRight) for a better experience.  This extension is not required for the whole thing to work, but it does add some benefits.
   - Better script generating performance since it integrates directly with the Visual Studio data model
   - Some helper commands to install the package, generate scripts on demand, and add the config file
   - Down the road, this extension may become more useful... but that is TBD
3. Add or update the typeRightConfig.json file.  As of 0.5.2, a default config file is included in the nuget content.  You can also add the config file via a right click menu option on the project node if you have extension installed.  Config options are located below.
   - **NOTE**: if you install TypeRight in multiple projects for your solution, the config file will be pulled into each one.  You should disable it through the config option for all projects that aren't web projects (i.e. your core/business class library projects).  It was a battle between making it easy to include the config and incorrectly having config files for projects that shouldn't.  Maybe I'll fix it someday...
4. Add the `ScriptObject` attribute to any classes or interfaces you want to extract to a TypeScript file
   - If you aren't a fan of polluting your business classes with attributes, you can use the assembly attribute `ScriptObjectsAttribute` to provide a list of types to extract.  Example: `[assembly: ScriptObjects(typeof(Class1), typeof(Class2), ...)]`
5. Add the `ScriptEnum` attribute to any enums you want to extract to a TypeScript file
6. Add the `ScriptAction` attribute to any Controller Actions (methods) that you want to extract to TypeScript files
7. Build the project.  The following TypeScript files will now be created
   - One containing all the classes, interfaces, and enums output to the location specified in the **serverObjectsResultFilepath** config option (default "./Scripts/ServerObjects.ts")
   - One for each controller with extracted actions. The output path follows the pattern `<project Root>\Scripts\<Controller Name>\<Controller Name>Actions.ts`


# Configuration
In order to properly function, a config file must exist in the web csproj root directory named "typeRightConfig.json". The config file consists of the following options:


| Option   | Type | Description|
|----------|------|------------|
|enabled   |boolean | a boolean flag for whether script generation is enabled |
|templateType|string| The type of template to use.  The options are "namespace" or "module".  More about these templates below, but "module" is recommended |
|classNamespace|string|When using the namespace template, this is the namespace to use for class/interface types |
|enumNamespace|string |When using the namespace template, this is the namespace to use for enums|
|webMethodNamespace | string | When using the namespace template, this is the namespace to use for controller actions (more about that below) |
|serverObjectsResultFilepath | string | The relative path (from the perspective of the csproj root) to which the classes, interfaces, and enums should be printed out to.  By default, this value is "./Scripts/ServerObjects.ts" |
| ~~ajaxFunctionName~~ | string | Allows you to define the function that makes Ajax calls.  More later in the Extracting Controller Actions section.  **Note:** This option is being deprecated and may be removed in favor of the `actionConfig` options below.|
| ~~ajaxFunctionModulePath~~ | string | The module file location for the ajax function defined above.  Used only in the module template.  Similiar to the server object result filepath, it should be relative from the perspective of the csproj root.  **Note:** This option is being deprecated and may be removed in favor of the `actionConfig` options below. |
|modelBindingType | string | The type of model binding to use.  This affects how the MVC method is invoked in the autogenerated script.  The options are described below |
|actionConfigurations| Array of ActionConfiguration | (Note: this describes version 0.9.0) This is an array of "Action Configuration" object (below).  You can add multiple configurations, but you should have (at most) one for each request method (Get, Post) and you should also specify the default |

Action Configuration

| Name | Type | Description |
|----- | ------- | ------- |
|fetchFunctionName | string | The name of the function that will be fetching data for you.  The first two parameters must be a string url and your data (as an object), but the rest of the parameters can be configured via the `parameters` property.  |
|fetchFilePath| string | The relative path to the file that contains your `fetchFunctionName` |
|parameters | Array of objects| This is an array of optional additional parameters you want to pass to your fetch function.  The objects have 3 properties: `name`, `type` (the string type of the parameter), and `optional` (boolean) |
|returnType| string | The optional return type of the auto-generated routine.  Default is `void`.  It has a replaceable token of `$returnType$` to sub in the return type of the MVC action  This should probably match the return type of your fetch function.  For example, you might return a Promise from your fetch function, so you could put a value of `Promise<$returnType$>` in this property |
|imports | Array of objects | This optional property allows you to add some more imports that might be required for your parameters or return types.  Each object has the following properties: `items` (string array of the names of the items ot import), `useAlias` (set to true to have an alias assigned for you - i.e. `import * as alias from X`), and `path` (relative path to the file).|
|method | "get", "post", "default" | This is the request method for this action configuration. |

Model Binding Types:
- singleParam - Use this if your POST web methods will consist of a single parameter marked with `FromBody`.  This is the recommended approach and is the method that is officially supported by default in ASP.NET Core MVC ([see documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-2.1))
  - Example: `public IActionResult DoSomething([FromBody] DoSomethingModel model)`
- multiParam - Use this if your POST web methods consist of several parameters that need to be deserialized from JSON.  Apparently this worked before (possibly old ASP.NET), but isn't natively supported by .NET Core.
    - Example: `public ActionResult DoSomething(int a, SomeClass b, bool flag)` 


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

The other alternative for classes is to use `ScriptObjectsAttribute`.  For this assembly level attribute, you can specify types that are in another project or DLL.  You might use this if you don't want to install the nuget package in your core project or referenced code from another DLL.  Here is how you might use it:

```C#
[assembly: ScriptObjects(typeof(Class1), typeof(Class2), ...)]
[assembly: ScriptObjects("./Scripts/NonDefaultLocation.ts", typeof(Class3), typeof(Class3), ...)]
```

As demonstrated above, you can optionally group objects and save them to a different typescript file by specifying a relative path from the project root as the first parameter of the attribute.  The above settings will add `Class1` and `Class2` to the default output and `Class3` and `Class4` to the "NonDefaultLocation.ts" file.

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

Let's take a look at an example.  First, say I have this configuration for "actionConfig"

```JSON
...
"actionConfig": {
	"fetchFunctionName": "callPost",
	"fetchFilePath": "./Scripts/CallServiceStuff.ts",
	"parameters": [
		{
			"name": "abort",
			"type": "AbortSignal",
			"optional": true
		}
	],
	"returnType": "Promise<$returnType$>",
	"imports": null
}
...
```


And say we have this controller:

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

Assuming `MyClass` and `MySecondClass` have been attributed with `ScriptObject`, the resulting TypeScript will look something like this:

```TypeScript

export function GetSomeData(param1: number, someOtherData: MyClass, abort?: AbortSignal): Promise<MySecondClass> {
      return callPost("/MyDefault/GetSomeData", { param1: param1, someOtherData: someOtherData }, abort);
}

```

Now here is where the configuration comes into play.  

- The parameters of the method are configurable.  Here, I added the AbortSignal parameter.  At a minimum, your ```fetchFunctionName``` must begin with  (url: string, data: any, ...)
  - The url you get will be in the form: `/<Controller Name>/<Action Name>`.  If you use Areas, it will be `/<Area Name>/<Controller Name>/<Action>`
  - The data you get will be a dictionary of keys (parameter names) and values
- The return type is configurable.  Here, my ```callPost``` method returns a promise object for the result object.
- By default, if no fetch function is defined (not recommended and I may not support this in the future) each controller action will call an auto generated method that uses JQuery ```$.ajax``` method
- If using the module template, note that you will need to specify the file that the module is located in the **fetchFilePath** configuration setting

You will notice that the return type of the function will automatically pull the return type of the webservice call in C# code.  It does this by:
1. Finding the first return statement
2. If the return statement is the function "Json(...)", it will pull the type from the first parameter
3. Otherwise, it is whatever the return type of the method is

## Routes
The Route attribute is only (currently) supported on the Controller level.  For example, if you specify this route:

```
[Route("api/[controller]")]
public class TestController {
```
The resulting actions will be generated with a URL similar to this:

`fetch("/api/Test/Action",...)`

## Parameter binding
Currently, only `FromBodyAttribute` and `FromQueryAttribute` are supported.  This example action (for the controller above in the Routes section):

`public string TestAction([FromQuery] string id, [FromBody] MyModel model)`

Will create a script that looks like this:
```TypeScript
export function TestAction(id: string, model: MyModel): void {
	fetch(`/api/Test/TestAction?id=${id}`, model);
}

```

# Templates
There are two types of templates for no other reason than they are what I have used in the past.  If you are interested in some other type of template, let me know!  I think sometime in the future it would be cool to give users more flexibility on the template, but that is some ways off.

## Module Template
This is likely the recommended template with todays standards and module loading.  For this template, the result files will just be a collection of classes/methods that can be imported into other files.

## Namespace Template
This template is probably not the recommended one to use anymore, but I used it in the past so maybe someone might find it useful.  Note: I may, an probably will, remove support for this in a future version!

For this template, all classes/service calls are wrapped in a "namespace".  For example:

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

