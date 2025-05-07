using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create();
    }
}
