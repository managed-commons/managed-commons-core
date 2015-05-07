#!/bin/bash
curl -sSL https://nuget.org/nuget.exe -o nuget.exe && echo Downloaded nuget.exe && \
mkdir -p ~/bin && PATH="~/bin;$PATH" && echo $PATH && \
chmod +x nuget.sh && \
ln -T -s nuget.sh ~/bin/nuget && \
xbuild Commons.Core.csproj
