# ![W3C SVG Logo](https://www.w3.org/Icons/SVG/svg-logo-v.png) Scalable Vector Graphics (SVG) Plugin for Paint.NET 

[![](https://img.shields.io/github/release-pre/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET.svg?style=flat)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)
[![](https://img.shields.io/github/downloads/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/latest/total)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)

This is a [paint.net](https://getpaint.net) filetype plugin for loading SVG (Scalable Vector Graphics) and its compressed variant SVGZ files. SVG files can be rendered as a flat image or each element/group on a separate layer. This plugin also provides basic image tracing (raster to vector conversion) functionality which works best with black and white drawings. You may export the tracing result as SVG or Paint.NET Shape.

### Prerequisites
paint.net 5.1 or later

### Download links

Here are the download links for latest release:

Installer[^1] (Recommended) | Manual Installation
--- | ---
:floppy_disk: [SvgFileTypePlugin_setup.exe](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases/latest/download/SvgFileTypePlugin_setup.exe) | :floppy_disk: [SvgFileTypePlugin.zip](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases/latest/download/SvgFileTypePlugin.zip)

[^1]: The portable version of paint.net is not supported by the installer.

### How to install

:warning: **Note: Before install make sure you don't have any other file type plugin installed handling the same file extensions: .svg, .svgz**

:point_right: To automatically install the plugin perform the following steps:
  * Make sure paint.net is not running.
  * Download and run `SvgFileTypePlugin_setup.exe`
  * Follow the steps of the setup wizard.
	
:point_right: To manually install the plugin perform the following steps:
  * Make sure paint.net is not running.
  * Download and extract `SvgFileTypePlugin.zip`
  * If you're using Classic version of paint.net:
    * Copy the `SvgFileTypePlugin` folder into the `<paint.net>\FileTypes` directory (Default location is `C:\Program Files\paint.net\FileTypes`).
  * If you're using [Microsoft Store version of paint.net](https://www.microsoft.com/store/apps/9NBHCS1LX4R0):
    * Copy the `SvgFileTypePlugin` folder into the `<Documents>\paint.net App Files\FileTypes` directory.
  * Done. You may start paint.net.