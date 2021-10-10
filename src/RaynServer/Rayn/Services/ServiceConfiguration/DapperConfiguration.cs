using System;
using Dapper;
using Rayn.Services.Database.DapperHelper;

namespace Rayn.Services.ServiceConfiguration
{
    public static class DapperConfiguration
    {
        public static void Configure()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.AddTypeHandler(new NullAbleGuidTypeHandler());
        }
    }
}