using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlThreadCreator : IThreadCreator
{
    private const string CreateThreadQuery =
@"insert into threads(ThreadId, OwnerId, ThreadTitle, BeginningDate, DateOffset, CreatedDate, AuthorId) 
values (@ThreadId, @OwnerId, @ThreadTitle, @BeginningDate, @DateOffset, @CreatedDate, @AuthorId)";

    private readonly MySqlConnection _connection;

    public MySqlThreadCreator(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask CreateThreadAsync(ThreadModel thread)
    {
        // 例外処理は後で...
        await _connection.ExecuteAsync(CreateThreadQuery, thread);
    }
}
