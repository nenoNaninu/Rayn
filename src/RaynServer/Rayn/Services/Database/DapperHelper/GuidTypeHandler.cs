using System;
using System.Data;
using Dapper;

namespace Rayn.Services.Database.DapperHelper
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            var bytes = value.ToByteArray();
            parameter.Value = bytes;
        }

        public override Guid Parse(object value) => new Guid((byte[])value);
    }

    public class NullAbleGuidTypeHandler : SqlMapper.TypeHandler<Guid?>
    {
        public override void SetValue(IDbDataParameter parameter, Guid? value)
        {
            if (value != null)
            {
                var bytes = value.Value.ToByteArray();
                parameter.Value = bytes;
            }
            else
            {
                parameter.Value = null;
            }
        }

        public override Guid? Parse(object value)
        {
            if (value != null)
            {
                return new Guid((byte[])value);
            }
            else
            {
                return null;
            }
        }
    }
}
