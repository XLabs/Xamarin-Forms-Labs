#!/usr/bin/env bash
export EnableNuGetPackageRestore="true"
xbuild build.proj /t:Build
