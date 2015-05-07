#!/bin/bash
echo === Verifying nuget existence: && nuget && echo === Starting build... && \
xbuild Commons.Core.csproj
