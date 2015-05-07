#!/bin/bash
xbuild /property:DoNotPack=true Commons.Core.csproj && nuget pack Commons.Core.csproj
