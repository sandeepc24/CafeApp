// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build"
let testDir = "./tests"
let nunitRunnerPath = "packages/NUnit.Runners/tools"

// Filesets
let appReferences  =
    !! "/**/*.csproj"
      ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ -> CleanDirs [buildDir; testDir])

Target "BuildApp" (fun _ ->
    !! "/**/*.fsproj"
    -- "/**/*.Tests.fproj"
    |> MSBuildRelease buildDir "Build"
    |> Log "BuildApp-Output: "
)

Target "BuildTests" (fun _ ->
  !! "/**/*.fsproj"
  |> MSBuildDebug testDir "Build"
  |> Log "BuildTests-Output"
)

Target "RunUnitTests" (fun _ ->
  !!(testDir + "/*.Tests.dll")
  |> NUnit(fun p -> { p with ToolPath = nunitRunnerPath })
)

// Build order
"Clean"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunUnitTests"

// start build
RunTargetOrDefault "RunUnitTests"
