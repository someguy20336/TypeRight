# TypeRight
TypeRight is a simple tool that generates TypeScript files from your C# objects and controller actions. This project stemmed from some annoying things I found while doing web development using C# and MVC:

1. Making requests required entering a free text URL and parameter names.  If anything changed, the request broke.
2. If you wanted to strongly type a result from a web request, you would need to make the same C# object in TypeScript.  This led to maintenance nightmares trying to keep those in sync.
3. Using enums on the server and client required keeping them in sync

This tool solves those problems by autogenerating those TypeScript files every time you build your project.

**NOTE**: I am temporarily unlisting version 0.11.0 until I can take a look at [this issue](https://github.com/someguy20336/TypeRight/issues/29).  It is possible that some unique project configurations (and or VS version) are causing some problems during build.

**RECENT BREAKING CHANGES**

***Breaking changes between the tool and the VS extention (Tools: 0.11.0, VS Extension: 0.8.0)***
- Tools Version 0.11.0 is only compatible with the most recent version of the extension (>= 0.8)
- The extension (0.8.0) is not compatible with anything before tools version 0.11.0 

This was done in an effort to make the contract between the two a little more solid and to refactor some of the solution architecture to be a little more solid.  You should update your version of the Nuget package and extension for best results.

See [the wiki](https://github.com/someguy20336/TypeRight/wiki/Quick-Start) for a quick start guide






