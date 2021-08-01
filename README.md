﻿SvgFileType
===========
[![](https://img.shields.io/github/release-pre/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET.svg?style=flat)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)
[![](https://img.shields.io/github/downloads/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/total.svg?style=flat)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)

**Please check the [Releases](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases) section for binary download.**

**Please note:** The binary forms that you can get from the releases section are under the LGPL terms since they contain code from 
[Fizzler: .NET CSS Selector Engine](https://github.com/atifaziz/Fizzler) project.

This is a Paint.NET filetype plugin for loading SVG (Scalable Vector Graphics) and its compressed variant SVGZ files. 
SVG elements can be rendered as a flat image file or each on a separate layer.

The plugin is a tiny wrapper around the [SVG.NET Library](https://github.com/vvvv/SVG) which does the actual SVG reading.

Tested on paint.net 4.2.16 stable & 4.3 alpha (4.300.7881.3082) releases.

To install the plugin perform the following steps:
 * Put the DLL in the `<Paint.NET>\FileTypes` directory (default location is `C:\Program Files\paint.net\FileTypes`)
   * For the [Windows Store version of Paint.NET](https://www.microsoft.com/store/apps/9NBHCS1LX4R0), put the DLL in the `<Documents>\paint.net App Files\FileTypes` directory
 * Restart Paint.NET if you have it open
