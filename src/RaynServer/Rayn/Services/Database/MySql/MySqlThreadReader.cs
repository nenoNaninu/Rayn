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

    private readonly IDatabaseConfig _databaseConfig;

    public MySqlThreadReader(IDatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId)
    {
        using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

        var searchResult = await conn.QueryAsync<ThreadModel>(SearchThreadByThreadIdQuery, new { ThreadId = threadId });

        return searchResult.FirstOrDefault();
    }

    public async ValueTask<IEnumerable<ThreadModel>> SearchThreadByUserId(Guid userId)
    {
        using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);
        var searchResult = await conn.QueryAsync<ThreadModel>(SearchThreadByUserIdQuery, new { AuthorID = userId });

        if (searchResult is null)
        {
            return Array.Empty<ThreadModel>();
        }

        return searchResult;
    }
}
