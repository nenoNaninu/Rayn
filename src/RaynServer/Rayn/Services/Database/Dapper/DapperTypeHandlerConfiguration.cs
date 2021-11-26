using System;
using Dapper;
using Rayn.Services.Database.Dapper;

namespace Rayn.Services.Dapper;

public static class DapperTypeHandlerConfiguration
{
    public static void Configure()
    {
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        SqlMapper.AddTypeHandler(new NullAbleGuidTypeHandler());
    }
}
