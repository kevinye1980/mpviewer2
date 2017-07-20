# MPViewer 2012 R2

## Description

This repository contains my own fork of the original MPviewer source ever so generously provided by [Boris and Daniele](https://blogs.msdn.microsoft.com/dmuscett/2012/02/19/boriss-opsmgr-tools-updated/)

My goal is to continue to maintain this awesome tool and add features to further enhance it.

Download


## Release 1

### *New Features*
* Compiled with the 2012 R2 SDK
* Added the possibility to open multiple management packs at once
** An additional table 'Management Packs' is added to the overview that shows all MP's currently loaded
** Each MP item now has an additional column that shows the Management Packs where it resides in
* Added the possibility to load MP's from a management group instead of files
** Use the file menu to connect to a management group
** both single sign-on and credentials-based connections supported
** Multiple MP selection also available in this mode!
* Management Pack Module Types are now also shown and reported
* OpenWith support: MPViewer can be made the default app for MP's. This allows you to open a MP-file by just double clicking on it!
### *Known Issues*
* CommandLine mode not supported due to the addition of OpenWith support, will be re-added later
* GUI quirky in some areas

## Past Releases
