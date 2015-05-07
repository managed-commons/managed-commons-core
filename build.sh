#!/bin/bash
mkdir -p ~/bin && PATH="~/bin;$PATH" && echo === Adjusted PATH: $PATH && \
curl -sSL https://nuget.org/nuget.exe -o ~/bin/nuget.exe && echo === Downloaded nuget.exe && \
echo '#!/bin/bash' > nuget && echo "mono `which nuget.exe`" >> nuget && chmod +x nuget && \
mv -f nuget ~/bin/ && echo === Wrapping script for nuget.exe 'nuget' created: && cat ~/bin/nuget && \
nuget | head -n 2 && echo === Starting build... && \
xbuild Commons.Core.csproj
