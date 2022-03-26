# TinyWordle
A C# console clone of Wordle, but with an attempt to make the binary really tiny.

Based off the popular [Wordle game](https://www.nytimes.com/games/wordle/index.html) where the rule are:

![image](images/rules.png)

Each attempt at shinking the final binary will have it's own project to easily compare and understand the steps taken. 

Console work helped out by [Console Games - Snake](https://dev.to/pcmichaels/console-games-snake-part-1-3jfg)

The project was kicked off via `dotnet new console`.

## Basic Game Overview
![image](images/Gameplay1.gif)

Pretty similar to the original given the constraints.

## Shrinking
Each shrinking attempt will be around looking at a published `.exe` in Release mode. The executable will be able to run stand alone without needing .NET on the target machine. We can achieve this by publishing as a [Single File Application](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file/overview) by modifying the `.csproj` file with:
```xml
<PropertyGroup>
	<PublishSingleFile>true</PublishSingleFile>
</PropertyGroup>
```

However, I will be targeting x64 Windows. And to make sure it works, I'll be playing one game after each attempt to see if it still works.

## Original
The `00 original` folder is about getting the game going with no attempt to think about efficiency. It's left as is, warts and all. There are some inefficencies, useless assignments, etc. But that's all part of this game to shrink the file - we need a baseline *somewhere*.

```
dotnet publish -r win-x64 -c Release

Total binary size: 62,091 KB
```

## Attempt 1 (-50,902 KB)
I can probably get the biggest gain by setting the [Trimming options](https://docs.microsoft.com/en-us/dotnet/core/deploying/trimming/trimming-options). 

We can trim either by adding it as a publish argument or via the `.csproj` file. I've opted to add `PublishTrimmed` to the `.csproj` file:
```xml
<PropertyGroup>
	<PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 11,189 KB
```

## Attempt 2 (-0 KB)
We can also [tune the trimming options](https://docs.microsoft.com/en-us/dotnet/core/deploying/trimming/trimming-options#trimming-granularity), and the easiest switch to flick is `TrimMode`. However, out of the two options:
1. `link`
1. `copyused` 

.NET6 by default uses the more aggressive one, `link`. Meaning we won't see any size change even if explicitly stating it in the `.csproj` file.
```xml
<PropertyGroup>
	<TrimMode>link</TrimMode>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 11,189 KB
```

## Attempt 3 (- 16 KB)
Onward with trimming, we can [trim our assembly](https://docs.microsoft.com/en-us/dotnet/core/deploying/trimming/trimming-options#trim-additional-assemblies) to skim off a little more space.
```xml
<ItemGroup>
	<TrimmableAssembly Include="TinyWordle" />
</ItemGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 11,173 KB
```

## Attempt 4 (-6,825 KB)
Time for some [experimental native AOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT-LLVM). If you know about CoreRT, I believe this is the next successor. This attempt will simply be adding it to the project via [this document](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/compiling.md).

Note: that you will have to install the C++ Development module of Visual Studio for this to work.

Note: I did have to remove the `PublishSingleFile` element from the `.csproj` file otherwise the publish fails when using AOT.

```
dotnet publish -r win-x64 -c Release

Total binary size: 4,348 KB
```

## Attempt 5 (-221 KB)
Looking at the [root documentation](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT/docs/using-nativeaot) it looks like we can [optimise](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md)!
```xml
<PropertyGroup>
	<InvariantGlobalization>true</InvariantGlobalization>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 4,127 KB
```

## Attempt 6 (-69 KB, nice)
On the same [optimisation page](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md) it also describes a `IlcOptimizationPreference` option and we can set this as `size`.
```xml
<PropertyGroup>
	<IlcOptimizationPreference>Size</IlcOptimizationPreference>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 4,058 KB
```

## Attempt 7 (-199 KB)
Next up is setting `IlcFoldIdenticalMethodBodies` which according to the [docs](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md#options-related-to-code-generation) can get a bit weird with stack traces. 
```xml
<PropertyGroup>
	<IlcFoldIdenticalMethodBodies>true</IlcFoldIdenticalMethodBodies>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 3,859 KB
```

## Attempt 8 (-2,692 KB)
Now for some [metadata options](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md#options-related-to-metadata-generation). First up is setting `IlcDisableReflection`, which disables all the reflection based metadata generation. Things can get a bit funky when we begin picking off relfection parts of .NET though. 

```xml
<PropertyGroup>
	<IlcDisableReflection>true</IlcDisableReflection>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 1,167 KB
```

## Attempt 9 (-0 KB)
Metadata? Who needs it. Time for [`IlcTrimMetadata` to shine](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md#options-related-to-metadata-generation). Though it turns out, I might already have a whole bunch removed as for this attempt, it didn't do much for me.

```xml
<PropertyGroup>
	<IlcTrimMetadata>true</IlcTrimMetadata>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 1,167 KB
```

## Attempt 10 (-129 KB)
[`IlcGenerateStackTraceData`](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/optimizing.md#options-related-to-metadata-generation) is next up to attempt to cut bytes.

```xml
<PropertyGroup>
	<IlcGenerateStackTraceData>false</IlcGenerateStackTraceData>
</PropertyGroup>
```
```
dotnet publish -r win-x64 -c Release

Total binary size: 1,038 KB
```

## Attempt 11
Now that we've exhausted the switches I can find, it's time to go back look at the code.

### Smallest Limit
Without tinkering with the linker and the build toolchain, it seems the smallest value I can get an `.exe` is around **975 KB**. I tested this by removing all references to the actual TinyWordle code and just did a simple `Console.Writeline("");` in `program.cs`. 

This means I think a good goal is to get the codebase to be sub 1 MB, or 1,024 KB - and according to the attempt 10 amount, only 14 bytes need to be shaved off. So let's see where we can lose them.

### Record Structs (-0 KB)
Specifically in reducing the amount of MSIL we create. First up, we're going to look at the `GuessedWord` and `GuessedLetter` `struct`s. Looking at [this example from SharpLap.io](https://sharplab.io/#v2:EYLgZgpghgLgrgJwgZwLTIgWygOxgSwGN0d8AHMiGZAGgBMQBqAHwAEAmARgFgAoVgMwACJFDoB7HABsAniIiFxCOkOQwEcQjCEBxOCgx0A6kroAKVpwAMQk8oCUAbj59Bq9Zu16DEY6fZ8AN58QqFCbpY2diqBQgDmVI5CAL4hYW7eyIbR7BbWQgDupvZCwbxhFbamQgC8habO5WGpvMlAA), a record produces a lot more code. 

However, while this might impact the final size, for my two `struct` use case, converted them away from records did not change the resulting file size.

### Random (-0 KB)
Originally I was selecting the word via creating a new `Random` object. However, you can also get a random number via a static call, `Random.Shared.Next()`. Replacing the object with the static call did not change the resulting file size.

### Random v2 (-2 KB)
Using the [random `struct`](https://github.com/MichalStrehovsky/SeeSharpSnake/blob/master/Game/Random.cs) from the [whole inspriation from this little project](https://medium.com/@MStrehovsky/building-a-self-contained-game-in-c-under-8-kilobytes-74c3cf60ea04) shaved off 2 KB.
```
dotnet publish -r win-x64 -c Release

Total binary size: 1,036 KB
```

### No more `.ToLower()` (-5 KB)
As long as our uses don't use any upper case they'll be fine. Shaves off 5 KB.
```
dotnet publish -r win-x64 -c Release

Total binary size: 1,031 KB
```

### No more `.WriteLine()` (-1 KB)
Since I need to use `Console.Write()` to colour specific characters, I can remove the `WriteLine()` calls and just append `/r/n` to the necessary calls.

```
dotnet publish -r win-x64 -c Release

Total binary size: 1,030 KB
```

### No more `.Contains()` (- 512 B)
A really small one, but just creating a basic method to replace the built in `string.Contains()` method. While it didn't save enough to make a difference on the rounded binary size. At least this time the size changed - unlike other attempts. 

```
dotnet publish -r win-x64 -c Release

Total binary size: 1,030 KB
```

### No more `Console.ReadKey()` (-2 KB)
To continue or quit the game after winning/losing, I opted to use a `ReadKey()` call so the user could go straight back into the game. That's now been replaced with a `ReadLine()` meaning the user has to hit enter to continue. But that's worth the saving.

```
dotnet publish -r win-x64 -c Release

Total binary size: 1,028 KB
```

### Swapping out `while` (-0 KB)
I was thinking since I'm using `for` anyway, I could use the `for(;;)` infinite loop syntax instead of `while(true)`. Turns out, they both end up as the same thing for IL.

## Attempt 12
Quick list of things that failed
- Changing things from/to class/structs/records/etc
- Removing the new `Random` and hard coding a value
- Many others

After spend awhile looking through the code myself, I figured it was time to see the actual output of the publish. Previously I had used the Visual Studio Performance Profiler to help me out to see what calls are being made where. But I'm now ready to see what I can see from the AOT output.

I will be using the following:
```xml
<PropertyGroup>
	<IlcGenerateDgmlFile>true</IlcGenerateDgmlFile>
	<IlcGenerateMapFile>true</IlcGenerateMapFile>
	<IlcDumpGeneratedIL>true</IlcDumpGeneratedIL>
</PropertyGroup>
```

Looking at the dumps of what is linked, it's all the core library things like `System.Threading`, `System.Collections`, and things like the various primitive types. 