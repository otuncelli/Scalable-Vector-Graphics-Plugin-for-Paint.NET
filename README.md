# ![W3C SVG Logo](https://www.w3.org/Icons/SVG/svg-logo-v.png) Scalable Vector Graphics (SVG) Plugin for Paint.NET 

[![](https://img.shields.io/github/release-pre/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET.svg?style=flat)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)
[![](https://img.shields.io/github/downloads/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/total.svg?style=flat)](https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases)

This is a Paint.NET filetype plugin for loading SVG (Scalable Vector Graphics) and its compressed variant SVGZ files. 
SVG elements can be rendered as a flat image file or each on a separate layer.

The plugin is a tiny wrapper around the [SVG.NET Library](https://github.com/vvvv/SVG) which does the actual SVG reading.

Tested on Paint.NET 4.2.16 & 4.3 Beta Build 7929.

### Download links

Here are the download links for latest release:

<table>
  <tr>
    <th>Installer</th>
    <th>Manual Installation</th>
  </tr>
  <tr>
    <td><a href="https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases/latest/download/SvgFileType_setup.exe">SvgFileTypePlugin_setup.exe</a> (399 KiB)</td>
    <td><a href="https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET/releases/latest/download/SvgFileType.zip">SvgFileTypePlugin.zip</a> (274 KiB)</td>
  </tr>
</table>

### How to install

&#x1F534; **Note: Before install make sure you don't have any other file type plugin installed handling the same file types as this plugin.**

To auto install (recommended) the plugin perform the following steps:
  * Download and run `SvgFileTypePlugin_setup.exe`
  * Follow the steps of the setup wizard.
	
To manually install the plugin perform the following steps:
  * Download and extract `SvgFileTypePlugin.zip`
  * If you're using Paint.NET 4.3 or later:
    * If you're using Classic version of Paint.NET:
	    * Create a new folder named `SvgFileTypePlugin` in the `<Paint.NET>\FileTypes` directory (default location is `C:\Program Files\paint.net\FileTypes`).
    * If you're using [Microsoft Store version of Paint.NET](https://www.microsoft.com/store/apps/9NBHCS1LX4R0):
	    * Create a new folder named `SvgFileTypePlugin` in the `<Documents>\paint.net App Files\FileTypes` directory.
    * Put the extracted files in this newly created folder.
  * If you're using Paint.NET 4.2:
	  * If you're using Classic version of Paint.NET:
	    * Put the extracted files in the `<Paint.NET>\FileTypes` directory (default location is `C:\Program Files\paint.net\FileTypes`).
	  * If you're using [Microsoft Store version of Paint.NET](https://www.microsoft.com/store/apps/9NBHCS1LX4R0):
	    * Put the extracted files in the `<Documents>\paint.net App Files\FileTypes` directory.
  * Restart Paint.NET.