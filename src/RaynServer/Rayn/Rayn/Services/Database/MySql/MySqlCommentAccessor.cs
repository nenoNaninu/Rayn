﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql
{
    public class MySqlCommentAccessor : ICommentAccessor
    {

        private readonly IDatabaseConfig _databaseConfig;
        private const string ReadQuery = "select * from rayn_db.comments where ThreadId = @ThreadId;";
        private const string InsertQuery = "insert into rayn_db.comments (ThreadId, WrittenTime, Message) values (@ThreadId, @WrittenTime, @Message);";

        public MySqlCommentAccessor(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId)
        {
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            var searchResult = await conn.QueryAsync<CommentModel>(ReadQuery, new { ThreadId = threadId });

            // EFと違ってToArray挟む必要ないかも?
            return searchResult.ToArray();
        }

        public async ValueTask InsertCommentAsync(string message, Guid threadId, DateTime writtenTime)
        {
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            await conn.ExecuteAsync(InsertQuery, new { ThreadId = threadId, WrittenTime = writtenTime, Message = message });
        }
    }
}