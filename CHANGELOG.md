# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0]

#### Added

- added a new SMS delivery channel: D7Networks.

#### Changed

- Updated the packages.
- Some code refactoring.
- Updated the packages to target .NET 6.

#### Removed

- removed all DI packages. now the DI registration exists on each package directly.
- removed Moq, and replaced it with NSubstitute.

## [1.1.0]

#### Added

- Added RavenSMS delivery channel.

## [1.0.0]
 
#### Added

- the initial beta release.
