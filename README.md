# Symbol Storage manager
This repository contains tools for mantaining the company or private symbol storage in accordance [Simple Symbol Query Protocol (SSQP)](https://github.com/dotnet/symstore/blob/master/docs/specs/Simple_Symbol_Query_Protocol.md) and [SSQP Key Conventions](https://github.com/dotnet/symstore/blob/master/docs/specs/SSQP_Key_Conventions.md).

##### Main features:
- [x] Add metadata for each set of files
- [x] Storage validation with fix some inconsistency with reference counting and file checking
- [x] Validate and fix access rights on Amazon S3
- [x] Delete unnecessary files from storage with using some kinds of filtering
- [x] Creating new storage
- [x] Casing support for data files for working in cooperation with Amazon CloudFront
- [ ] Support refreshing Amazon CloudFront
- [x] Uploading one storage to another
- [x] Gather files on user directories and generate storage for them
- [ ] Working with archives
- [ ] Generate .symref files to ability to download symbols with scripts

##### Supported storages
- Local filesystem
- Amazon Simple Storage Service (Amazon S3)

##### Supported formats
- Portable PDB
- Windows PDB
- Linux debug symbols
- macOS DWARF symbols
- PE binaries
- ELF binaries
- Mach-O binaries

##### Supported platforms (same as .NET 5.0)
- Windows x64/x86
- Linux x64/arm64
- MacOS x64

##### Tested on:
- Windows 10 Pro x64 20H2 Build 19042.804
- Ubuntu 20.10 LTS 5.8.0-1011-raspi aarch64