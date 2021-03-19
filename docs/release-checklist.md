# Release checklist

- Update `Properties/AssemblyInfo.cs`, changing the value of
  `AssemblyVersion` and `AssemblyFileVersion` to current version number
- Tag code with v**MAJOR**.**MINOR**.**PATCH**
- Update [CHANGELOG.md](../CHANGELOG.md): Change "next version" to the version
  number, create a new "next version". Update the diff-links
- Build Slipstream with the release target to create the next release and use
  the files from the `Releases` folder to create the new release. Make sure to
  copy all the *.nupkg files from the previous release so the delta updates from
  Squirrel.Windows works as intended, otherwise it will do a full install for 
  every update.
- Github release: Create a new version using the v**MAJOR**.**MINOR**.**PATCH**
  tag. In the description, link to the [CHANGELOG.md](../CHANGELOG.md) to the
  description of the version.

