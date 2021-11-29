using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlCommentAccessor : ICommentAccessor
{
    private const string ReadQuery = "select * from rayn_db.comments where ThreadId = @ThreadId;";
    private const string InsertQuery = "insert into rayn_db.comments (ThreadId, WrittenTime, Message) values (@ThreadId, @WrittenTime, @Message);";

    private readonly MySqlConnection _connection;

    public MySqlCommentAccessor(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId)
    {
        var searchResult = await _connection.QueryAsync<CommentModel>(ReadQuery, new { ThreadId = threadId });

        // EFと違ってToArray挟む必要ないかも?
        return searchResult.ToArray();
    }

    public async ValueTask InsertCommentAsync(string message, Guid threadId, DateTime writtenTime)
    {
        await _connection.ExecuteAsync(InsertQuery, new { ThreadId = threadId, WrittenTime = writtenTime, Message = message });
    }
}
