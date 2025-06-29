using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exeptions
{
    public  sealed class ProductNotFoundEx(int id): NotFoundEx($"Product With Is -{id}- Not Found");

}
