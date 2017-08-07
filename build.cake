#tool nuget:?package=xunit.runner.console&version=2.2.0

// ARGS
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// PREP

// Define directories.
var buildDir = Directory("./bin") + Directory(configuration);

// TASKS

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // NuGetRestore("./src/Example.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./build.proj", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./build.proj", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    // NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
    //    NoResults = true
    //    });
});

// TASK TARGETS

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

// EXECUTION
RunTarget(target);
