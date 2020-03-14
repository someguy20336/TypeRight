# TypeRight
TypeRight is a simple tool that generates TypeScript files from your C# objects and controller actions. This project stemmed from some annoying things I found while doing web development using C# and MVC:

1. Making requests required entering a free text URL and parameter names.  If anything changed, the request broke.
2. If you wanted to strongly type a result from a web request, you would need to make the same C# object in TypeScript.  This led to maintenance nightmares trying to keep those in sync.
3. Using enums on the server and client required keeping them in sync

This tool solves those problems by autogenerating those TypeScript files every time you build your project.

See [the wiki](https://github.com/someguy20336/TypeRight/wiki/Quick-Start) for a quick start guide






