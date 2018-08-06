///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDirectory = Directory("./artifacts/");
var buildDirectory = Directory("./src/Wyam.ReadingTime");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
    CleanDirectory(artifactsDirectory);
    DotNetCoreClean(buildDirectory);
});

Task("Restore")
.IsDependentOn("Clean")
.Does(() => {
    DotNetCoreRestore("./Wyam.ReadingTime.sln");
});

Task("Build")
.IsDependentOn("Restore")
.Does(() =>
{
    DotNetCoreBuild(buildDirectory, new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoRestore = true
    });
});

Task("Pack")
.IsDependentOn("Build")
.Does(() =>
{
    DotNetCorePack(buildDirectory, new DotNetCorePackSettings
    {
        OutputDirectory = artifactsDirectory,
        NoBuild = true,
        Configuration = configuration,
    });
});

Task("Default")
.IsDependentOn("Pack");

RunTarget(target);