using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public interface ITokenGenerator<T>
    {
        string GenerateToken(T entity);
    }
}
