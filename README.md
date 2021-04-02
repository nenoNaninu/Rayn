# Reaction is All You Need. (Rayn🌧)
画面共有でニコ〇コとかビ〇ビリみたいにコメントを流すためのソフトウェア。ほぼほぼC#製。

# Server(ASP.NET Core)
## Require
- [.NET5](https://dotnet.microsoft.com/download/dotnet/5.0)

## Usage
Linux/Macだともしかしたら以下のコマンドが必要になるかもしれない。WindowsでVisual Studio使っている場合は特に気にする必要なし。

```
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

ログとか残す気がないのであればDBとかを建てないで良い。Azure App Serviceなど単体で簡単に動かせる。

ローカルの開発環境ではUserSecretsに以下みたいな形で設定してDBの接続文字列とかを保存。

```
{
    "DatabaseConfig": {
        "ConnectionString": "Server=...; Port=...",
        "InMemoryMode": false
    }
}
```

InMemoryModeをtrueにしておけばインメモリで動くためデータベース用意してなくてもOK。その他環境では環境変数などGenericHostが拾ってくれるところに`DatabaseConfig`は設定。

# Client(Unity 2020.3.1)
Windows/Mac両対応

## Require
このリポジトリには含まれていないので、それぞれ.unitypackage落としてきてください。
-  [neuecc/UniRx 7.1.0](https://github.com/neuecc/UniRx/releases/tag/7.1.0)
-  [neuecc/Utf8Json](https://github.com/neuecc/Utf8Json/releases/tag/v1.3.7)
-  [Cysharp/UniTask 2.2.4](https://github.com/Cysharp/UniTask/releases/tag/2.2.4)
-  [kirurobo/UniWindowController 0.8.0](https://github.com/kirurobo/UniWindowController/releases/tag/v0.8.0)
-  [nenoNaninu/RxWebSocket 2.1.6](https://github.com/nenoNaninu/RxWebSocket/releases/tag/2.1.6)