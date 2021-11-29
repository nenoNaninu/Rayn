using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlThreadReader : IThreadDbReader
{
    private const string SearchThreadByThreadIdQuery = "select * from rayn_db.threads where ThreadId = @ThreadId;";
    private const string SearchThreadByUserIdQuery = "select * from rayn_db.threads where AuthorID = @AuthorID;";

    private readonly MySqlConnection _connection;

    public MySqlThreadReader(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId)
    {
        var searchResult = await _connection.QueryAsync<ThreadModel>(SearchThreadByThreadIdQuery, new { ThreadId = threadId });

        return searchResult.FirstOrDefault();
    }

    public async ValueTask<IEnumerable<ThreadModel>> SearchThreadByUserId(Guid userId)
    {
        var searchResult = await _connection.QueryAsync<ThreadModel>(SearchThreadByUserIdQuery, new { AuthorID = userId });

        if (searchResult is null)
        {
            return Array.Empty<ThreadModel>();
        }

        return searchResult;
    }
}
