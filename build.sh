#!/bin/bash
echo === Downloading nuget.exe && curl -sSL https://nuget.org/nuget.exe -o nuget.exe && \
echo === Building the library && xbuild /property:DoNotPack=true Commons.Core.csproj && \
echo === Packing the nuget && mono nuget.exe pack Commons.Core.csproj
