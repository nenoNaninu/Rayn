ローカル環境ではUserSecretsに以下みたいな形でDBの接続文字列とか入れてる。

```
{
    "DatabaseConfig": {
        "ConnectionString": "Server=...; Port=...",
        "InMemoryMode": false
    }
}
```

データベース用意してなくてもInMemoryModeをtrueにしておけばインメモリで動く。本番環境では環境変数に。