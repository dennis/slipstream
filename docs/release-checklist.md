# Release checklist

- Update `Properties/AssemblyInfo.cs`, changing the value of
  `AssemblyInformationalVersionAttribute` to current version number
- Tag code with v**MAJOR**.**MINOR**.**PATCH**
- Update [CHANGELOG.md](../CHANGELOG.md): Change "next version" to the version
  number, create a new "next version". Update the diff-links
- Build `setup.msi` (requires [Microsoft Visual Studio 2018 Installer
  Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects)),
  rename it to `slipstream-vMAJOR.MINOR.PATCH.msi`
- Github release: Create a new version using the v**MAJOR**.**MINOR**.**PATCH**
  tag. In the description, link to the [CHANGELOG.md](../CHANGELOG.md) to the
  description of the version.

