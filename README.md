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

However, I will be targeting x64 Windows.

## Original
The `00 original` folder is about getting the game going with no attempt to think about efficiency. It's left as is, warts and all. There are some inefficencies, useless assignments, etc. But that's all part of this game to shrink the file - we need a baseline *somewhere*.

```
dotnet publish -r win-x64 -c Release

Total binary size: 62,091 KB
```

