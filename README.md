# [Reaction is All You Need. (Raynð§)](https://raynw.azurewebsites.net/)
ç»é¢å±æã§ãã³ãã³ã¨ãããããªã¿ããã«ã³ã¡ã³ããæµãããã®ã½ããã¦ã§ã¢ãã»ã¼ã»ã¼C#è£½ã

- ã¢ããª: [https://raynw.azurewebsites.net/](https://raynw.azurewebsites.net/)
- Full demo video: [yotube](https://youtu.be/fFvW8ybUVVY)

![https://youtu.be/fFvW8ybUVVY](img/demo.gif)

# Server(ASP.NET Core)
## Require
- [.NET6](https://dotnet.microsoft.com/download/dotnet/6.0)

## Usage
Linux/Macã ã¨ããããããä»¥ä¸ã®ã³ãã³ããå¿è¦ã«ãªããããããªããWindowsã§Visual Studioä½¿ã£ã¦ããå ´åã¯ç¹ã«æ°ã«ããå¿è¦ãªãã

```
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
```

ã­ã°ã¨ãæ®ãæ°ããªãã®ã§ããã°DBã¨ããå»ºã¦ãªãã§è¯ããAzure App Serviceãªã©åä½ã§ç°¡åã«åãããã

ã­ã¼ã«ã«ã®éçºç°å¢ã§ã¯UserSecretsã«ä»¥ä¸ã¿ãããªå½¢ã§è¨­å®ãã¦DBã®æ¥ç¶æå­åãGoogle OAuthã®ããã®ClientIdç­ãä¿å­ãGoogleã®OAuthãä½¿ãããã®ClientIdç­ã¯äºãåå¾ãã¦ãããã¨ã[åè](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-5.0)

```
{
    "DatabaseConfiguration": {
        "ConnectionString": "Server=...; Port=...",
        "InMemoryMode": false
    },
    "Authentication:Google": {
        "ClientId": "xxxxx",
        "ClientSecret": "yyyy"
    }
}
```

`DatabaseConfiguration:InMemoryMode`ã`true`ã«ãã¦ããã°ã¤ã³ã¡ã¢ãªã§åããããã¼ã¿ãã¼ã¹ç¨æãã¦ãªãã¦ãè¯ãã
ãã®å ´å`DatabaseConfiguration:ConnectionString`ã«ã¯ç©ºæå­(`""`)ãè¨­å®ãã¦ããã°OKã
ãã®ä»ç°å¢ã§ã¯ç°å¢å¤æ°ãªã©GenericHostãæ¾ã£ã¦ãããã¨ããã«ä¸è¨ã¨åç­ã®ãã®ãè¨­å®ã
ä¾ãã°ãã¦ã¼ã¶ã¼ã·ã¼ã¯ã¬ããã§ã¯ãªãç°å¢å¤æ°ã«è¨­å®ããå ´åã¯ä»¥ä¸ã¿ãããªæãã

```
export DatabaseConfiguration__ConnectionString="Server=...; Port=..."
export DatabaseConfiguration__InMemoryMode="false"
export Authentication__Google__ClientId="xxxxxxx"
export Authentication__Google__ClientSecret="yyyyyyyy"
```

ä¸è¨ã®è¨­å®ãåºæ¥ããä»¥ä¸ã®æä½ã§ãµã¼ããèµ·åã§ãã¾ãã
```
cd src/RaynServer/Rayn
dotnet run --project Rayn.csproj
```

# Client(Unity 2020.3.1)
Windows/Macä¸¡å¯¾å¿

## Require
ãã®ãªãã¸ããªã«ã¯å«ã¾ãã¦ããªãã®ã§ããããã.unitypackageè½ã¨ãã¦ãã¦ãã ããã
- OSS
  -  [neuecc/UniRx 7.1.0](https://github.com/neuecc/UniRx/releases/tag/7.1.0)
  -  [neuecc/Utf8Json](https://github.com/neuecc/Utf8Json/releases/tag/v1.3.7)
  -  [Cysharp/UniTask 2.2.4](https://github.com/Cysharp/UniTask/releases/tag/2.2.4)
  -  [kirurobo/UniWindowController 0.8.0](https://github.com/kirurobo/UniWindowController/releases/tag/v0.8.0)
-  Asset store
   - [Modern UI Pack](https://assetstore.unity.com/packages/tools/gui/modern-ui-pack-150824?locale=ja-JP)


ã¾ãSignalRã®DLLã
git bashç­ã§ä»¥ä¸ã®æ§ãªæä½ãè¡ããã¦ã³ã­ã¼ããã¾ãã
```
cd src/Prepare
./download_signalr_dlls.sh
```
å¿è¦ãªdllã`dlls`ã¨ãããã£ã¬ã¯ããªã«åºåãããã®ã§ãåºåããã¦ããdllãå¨ã¦`src/RaynClient/Assets/Plugins/SignalR`ã«æãå¥ãã¦ãã ããã

Unityãªã®ã§ã³ã¡ã³ãã«åããã¦3Dããããããã¨ãã®æ¹é ãå®¹æã§ãã
