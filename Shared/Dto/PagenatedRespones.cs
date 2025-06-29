using Shared.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public record PagenatedRespones<TData>(int pageIndex, int pageSize, int Count, IEnumerable<TData> data);

}
