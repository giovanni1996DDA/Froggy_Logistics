using Services.Dao.Implementations.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        IAppUserDao UserRepository { get; }
        IRolDao RolRepository { get; }
        IRolRolDao RolRolRepository { get; }
        IPermisoDao PermisoRepository { get; }
        IUserRolDao UserRolRepository { get; }
        IUserPermisoDao UserPermisoRepository { get; }
        IRolPermisoDao RolPermisoRepository { get; }
        IDocTypeDao DocTypeRepository { get; }
        LoggerDao LoggerRepository { get; }
    }
}
