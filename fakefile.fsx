#r @"packages/FAKE.Core/tools/FakeLib.dll"
open System
open Fake

let rootDir = __SOURCE_DIRECTORY__
let tempDir = rootDir @@ "temp/net40"

let buildConfig = environVarOrDefault "BUILD_CONFIGURATION" "Release"

let packages = [
  "Coypu"
  "Coypu.NUnit"
  "Coypu.NUnit262"
  "Coypu.Watin"
]

let versions =
  let versionEntry a =
    let first:string = Array.get a 0
    let second:string = Array.get a 1
    let name = first.Trim()
    let version = second.Trim()
    (name, version)

  ReadCSVFile "version"
  |> Seq.map versionEntry

let versionOf name =
  versions
  |> Seq.find (fun (n,v) -> String.Compare(n, name, StringComparison.OrdinalIgnoreCase) = 0)
  |> snd

let nuspecCommonParams = (fun p ->
  { p with
      NuGetParams.OutputPath = rootDir
  }
)

let coypuNuspecParams = (fun p ->
  { nuspecCommonParams p with
      Project = "Coypu"
      Version = versionOf "Coypu"
      WorkingDir = tempDir
      Files = [("*.*", Some "lib/net40", None)]
  }
)

let coypuNunitNuspecParams = (fun p ->
  { nuspecCommonParams p with
      Project = "Coypu.NUnit"
      Version = versionOf "Coypu.NUnit"
      WorkingDir = rootDir
      Files = [("src/Coypu.NUnit/bin/Release/Coypu.NUnit.dll", Some "lib/net40", None)]
  }
)

let coypuNunit262NuspecParams = (fun p ->
  { nuspecCommonParams p with
      Project = "Coypu.NUnit262"
      Version = versionOf "Coypu.NUnit262"
      WorkingDir = rootDir
      Files = [("src/Coypu.NUnit262/bin/Release/Coypu.NUnit262.dll", Some "lib/net40", None)]
  }
)

let coypuWatinDriverNuspecParams = (fun p ->
  { nuspecCommonParams p with
      Project = "Coypu.Watin"
      Version = versionOf "Coypu.Watin"
      WorkingDir = rootDir
      Files = [("src/Coypu.Drivers.Watin/bin/Release/Coypu.Drivers.Watin.dll", Some "lib/net40", None)]
  }
)

let nuspecParams = [
  coypuNuspecParams
  coypuNunitNuspecParams
  coypuNunit262NuspecParams
  coypuWatinDriverNuspecParams
]

let nuspecFile name = sprintf "%s.nuspec" name
let nuspecFiles = Seq.map nuspecFile packages

let nupkgFilePattern name = sprintf "%s.*.nupkg" name
let nupkgFilePatterns = Seq.map nupkgFilePattern packages

let nuspecs = Seq.zip nuspecFiles nuspecParams
let nupkgs = Seq.zip nupkgFilePatterns nuspecParams

Target "Clean" (fun _ ->
  CleanDir tempDir
)

Target "Restore" RestorePackages

Target "Compile" (fun _ ->
  !! "**/Coypu.sln"
  |> MSBuild "" "Build" ["Configuration", buildConfig]
  |> ignore
)

Target "TestDrivers" (fun _ ->
  !! ("src/**/bin/" @@ buildConfig @@ "/Coypu.Drivers.Tests.dll")
  |> NUnit (fun p ->
      { p with
          ExcludeCategory = if isMono then "NoMonoChrome" else ""
      }
    )
)

Target "TestUnit" (fun _ ->
  !! ("src/**/bin/" @@ buildConfig @@ "/Coypu.Tests.dll")
  |> NUnit (fun _ -> NUnitDefaults)
)

Target "TestAcceptance" (fun _ ->
  !! ("src/**/bin/" @@ buildConfig @@ "/Coypu.AcceptanceTests.dll")
  |> NUnit (fun _ -> NUnitDefaults)
)

Target "TestAll" DoNothing

Target "Package" (fun _ ->
  !! "Coypu*.nupkg"
  |> DeleteFiles

  CreateDir tempDir

  !! "src/Coypu/bin/Release/Coypu*.dll"
  ++ "src/Coypu/bin/Release/Coypu*.xml"
  |> CopyTo tempDir

  nuspecs |> Seq.iter (fun (nuspec, parameters) ->
    NuGetPack parameters nuspec
  )
)

Target "Publish" (fun _ ->
  NuGetPublish coypuNuspecParams
)

Target "PublishNUnit" (fun _ ->
  NuGetPublish coypuNunitNuspecParams
)

Target "PublishNUnit262" (fun _ ->
  NuGetPublish coypuNunit262NuspecParams
)

Target "PublishWatin" (fun _ ->
  NuGetPublish coypuWatinDriverNuspecParams
)

"Clean"
  ==> "Restore"
  ==> "Compile"

"Compile" ==> "TestDrivers"
"Compile" ==> "TestUnit"
"Compile" ==> "TestAcceptance"

"Compile"
  ==> "TestUnit"
  ==> "TestDrivers"
  ==> "TestAcceptance"
  ==> "TestAll"

"Compile" ==> "Package"
"Package" ==> "Publish"
"Package" ==> "PublishNUnit"
"Package" ==> "PublishNUnit262"
"Package" ==> "PublishWatin"

RunTargetOrDefault "Compile"
