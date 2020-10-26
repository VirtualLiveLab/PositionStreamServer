# Position Stream Server

[![CircleCI](https://circleci.com/gh/MIKUEC2020/PositionStreamServer/tree/master.svg?style=svg&circle-token=0a7fc5748581995b2d99a947dc8d276e0dea4a4d)](https://circleci.com/gh/MIKUEC2020/PositionStreamServer/tree/master)

3Dアバターの座標を多人数で共有するサーバー

<img src="https://github.com/MIKUEC2020/PositionStreamServer/blob/master/ReadmeResources/UnityDemo.gif" width="320px">

- 「体の座標」と「体のY軸周りの回転」と「頭の姿勢」を共有します。
- 1500人程度まで共有が可能です。

### 通信量

X[MB/sec] = 48[byte] * N[人] * 100[人] * 30[fps] / 1000000[/MB]

### 依存SDK

- dotnet core 3.1 SDK
https://dotnet.microsoft.com/download

### ビルド

`git clone git@github.com:MIKUEC2020/PositionStreamServer.git`

`cd StreamServer && dotnet build`

### 実行

`cd StreamServer && dotnet run`

### テスト

`cd StreamServer.Test && dotnet test`
