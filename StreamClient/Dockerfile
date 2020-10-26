FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
COPY ./StreamServer /tmp/StreamServer
COPY ./StreamClient /tmp/StreamClient
COPY ./LoopLibrary /tmp/LoopLibrary
COPY ./DebugPrintLibrary /tmp/DebugPrintLibrary
COPY ./StreamServerCommonLibrary /tmp/StreamServerCommonLibrary
RUN dotnet publish -c Release -o /tmp/StreamClient/build /tmp/StreamClient/StreamClient.csproj
RUN rm /tmp/StreamClient/build/StreamClient.pdb

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
COPY --from=builder /tmp/StreamClient/build /usr/local/StreamClient
ENV PATH $PATH:/usr/local/StreamClient
CMD ["/bin/bash", "-c", "dotnet /usr/local/StreamClient/StreamClient.dll"]
