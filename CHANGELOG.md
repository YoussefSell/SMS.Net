# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.1.0]

- Updated the packages to use netstandard2.1.
- Updated project dependencies for compatibility.
- Replaced MariaDB with in-memory storage for Hangfire.
- Upgraded target framework from .NET 6.0 to .NET 9.0 for some packages.
- Modified Twilio integration to use account SID and auth token.
- Renamed methods and parameters to reflect SMS terminology.
- Adjusted exception handling for the new SMS service structure.
- Updated test cases to align with SMS service changes.
- Updated handling of custom channel data for Twilio authentication.
- Ensured documentation and comments are current with changes.

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
