FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
COPY ./StreamServer /tmp/StreamServer
COPY ./StreamClient /tmp/StreamClient
COPY ./LoopLibrary /tmp/LoopLibrary
COPY ./StreamServerCommonLibrary /tmp/StreamServerCommonLibrary
RUN dotnet publish -c Release -o /tmp/StreamServer/build /tmp/StreamServer/StreamServer.csproj
RUN rm /tmp/StreamServer/build/StreamServer.pdb

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
COPY --from=builder /tmp/StreamServer/build /usr/local/StreamServer
ENV PATH $PATH:/usr/local/StreamServer
CMD ["/bin/bash", "-c", "dotnet /usr/local/StreamServer/StreamServer.dll"]
