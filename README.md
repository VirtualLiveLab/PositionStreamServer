# Position Stream Server

[![CircleCI](https://circleci.com/gh/MIKUEC2020/PositionStreamServer/tree/master.svg?style=svg&circle-token=0a7fc5748581995b2d99a947dc8d276e0dea4a4d)](https://circleci.com/gh/MIKUEC2020/PositionStreamServer/tree/master)

3Dアバターの座標を多人数で共有するサーバー

<img src="https://github.com/MIKUEC2020/PositionStreamServer/blob/master/ReadmeResources/UnityDemo.gif" width="320px">

- 「体の座標」と「体のY軸周りの回転」と「頭の姿勢」を共有します。
- 1500人程度まで共有が可能です。
- 直近の100人だけ送信します。

## 通信量

X[MB/sec] = 48[byte] * N[人] * 100[人] * 30[fps] / 1000000[/MB]

## 依存SDK

- dotnet core 3.1 SDK
https://dotnet.microsoft.com/download

## ビルド

`git clone git@github.com:MIKUEC2020/PositionStreamServer.git`

`cd StreamServer && dotnet build`

## 実行

`cd StreamServer && dotnet run`

## テスト

`cd StreamServer.Test && dotnet test`

## トラブルシューティング

### Socketバッファサイズ

1000接続くらいするとCPU使用率・ネットワーク帯域が余っているのにConnected・Disconnectedを繰り返すという症状が現れることがある。

以下のコマンドによりパケットロスを起こしていることが分かり、これに対処するにはSocketバッファの最大サイズを増やす必要がある。

```
$ netstat -s
...
Udp:
    10665511 packet receive errors
    10665511 receive buffer errors
...
```

StreamServerで必要なバッファの量は受信送信合わせて最大で

```
48[bytes/packet] * N[人] * 100[人] * 30[fps] + 48[bytes/packet] * N[人] * 30[fps]
```

であり、`N=1000`であれば約150MBである。実際には受信することによりバッファが空くのでこれほどは必要ないが念のため確保しておく。

```sh
# バッファの最大サイズを確認
$ sysctl net.core.rmem_max
net.core.rmem_max = 212992 # デフォルトではけっこう少ない

# デフォルトサイズも適当に増やしておく
$ sudo sysctl -w net.core.rmem_default=50000000
$ sudo sysctl -w net.core.rmem_max=150000000
```