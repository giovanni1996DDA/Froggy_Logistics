using Services.Dao.Factory;
using Services.Dao.Implementations.SQLServer;
using Services.Dao.Interfaces;
using Services.Dao.Interfaces.UnitOfWork;
using Services.Domain;
using Services.Facade;
using Services.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.Logic
{
    /// <summary>
    /// Proporciona servicios relacionados con la gestión de accesos, roles y permisos.
    /// </summary>
    public class AccesoService : Logic<Acceso>
    {
        private static AccesoService instance = new AccesoService();

        /// <summary>
        /// Instancia única del servicio de acceso (patrón Singleton).
        /// </summary>
        public static AccesoService Instance { get { return instance; } }

        /// <summary>
        /// Constructor privado para implementar el patrón Singleton.
        /// </summary>
        private AccesoService()
        {
        }

        /// <summary>
        /// Obtiene una lista de accesos en función del tipo de acceso (Rol o Permiso).
        /// </summary>
        /// <param name="acceso">Objeto de tipo Acceso.</param>
        /// <returns>Una lista de objetos de tipo Acceso.</returns>
        /// <exception cref="ArgumentException">Si el tipo de acceso no es soportado.</exception>
        public List<Acceso> Get(Acceso acceso)
        {
            List<Acceso> returning = new List<Acceso>();
            switch (acceso)
            {
                case Rol rol:
                    returning = Get(rol).Select(r => (Acceso)r).ToList();
                    break;

                case Permiso permiso:
                    returning = Get(permiso).Select(r => (Acceso)r).ToList();
                    break;

                default:
                    throw new ArgumentException("Tipo de acceso no soportado");
            }
            return returning;
        }

        /// <summary>
        /// Obtiene una lista de roles asociados.
        /// </summary>
        /// <param name="rol">Objeto de tipo Rol.</param>
        /// <returns>Una lista de roles.</returns>
        /// <exception cref="NoRolesFoundForUserException">Si no se encuentran roles para el usuario.</exception>
        public List<Rol> Get(Rol rol)
        {
            List<Rol> returning = new List<Rol>();

            int hidratationLevel = int.Parse(ConfigurationManager.AppSettings["AccessesHidrationLvl"]);

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao rolRepo = context.Repositories.RolRepository;

                List<FilterProperty> filters = BuildFilters(rol);

                returning = rolRepo.Get(filters);
            }

            if (returning.Count == 0) throw new NoRolesFoundForUserException();

            foreach (Rol returningrole in returning)
            {
                FillRol(returningrole, hidratationLevel);
            }

            return returning;
        }

        /// <summary>
        /// Obtiene una lista de permisos asociados.
        /// </summary>
        /// <param name="permiso">Objeto de tipo Permiso.</param>
        /// <returns>Una lista de permisos.</returns>
        /// <exception cref="NoPermissionsFoundException">Si no se encuentran permisos asociados.</exception>
        public List<Permiso> Get(Permiso permiso)
        {
            List<Permiso> returning = new List<Permiso>();

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IPermisoDao rolRepo = context.Repositories.PermisoRepository;

                List<FilterProperty> filters = BuildFilters(permiso);

                returning = rolRepo.Get(filters);
            }

            if (returning.Count == 0) throw new NoPermissionsFoundException();

            return returning;
        }

        private List<Acceso> Get()
        {
            List<Acceso> returning = new List<Acceso>();

            try
            {
                returning.AddRange(Get(new Rol()));
            }
            catch (NoRolesFoundForUserException)
            {
                try
                {
                    returning.AddRange(Get(new Permiso()));
                }
                catch (NoPermissionsFoundException)
                {
                    throw new NoAccesosFoundException();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}. Revisar logs.", "Error inesperado.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return returning;
        }

        public Acceso GetOne(Acceso acc)
        {
            try
            {
                if (acc is Rol rol)
                    return GetOne(rol);

                if (acc is Permiso permiso)
                    return GetOne(permiso);

                return null;
            }
            catch (NoRolesFoundException)
            {
                throw;
            }
            catch (NoPermissionsFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}. Revisar logs.", "Error inesperado.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public Rol GetOne(Rol rol)
        {
            Rol returning = null;

            int hidratationLevel = int.Parse(ConfigurationManager.AppSettings["AccessesHidrationLvl"]);

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao rolRepo = context.Repositories.RolRepository;

                List<FilterProperty> filters = BuildFilters(rol);

                returning =  rolRepo.GetOne(filters);
            }

            if (returning == null) throw new NoRolesFoundException();

            FillRol(returning, hidratationLevel);

            return returning;
        }

        public Permiso GetOne(Permiso permiso)
        {
            Permiso returning = null;

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IPermisoDao rolRepo = context.Repositories.PermisoRepository;

                List<FilterProperty> filters = BuildFilters(permiso);

                returning = rolRepo.GetOne(filters);
            }

            if (returning == null) throw new NoPermissionsFoundException();

            return returning;
        }

        /// <summary>
        /// Crea o actualiza un acceso (Rol o Permiso) en la base de datos.
        /// </summary>
        /// <param name="acceso">Objeto de tipo Acceso.</param>
        /// <exception cref="ArgumentException">Si el tipo de acceso no es soportado.</exception>
        public void CreateOrUpdate(Acceso acceso)
        {
            switch (acceso)
            {
                case Rol rol:
                    CreateOrUpdate(rol);
                    break;

                case Permiso permiso:
                    CreateOrUpdate(permiso);
                    break;

                default:
                    throw new ArgumentException("Tipo de acceso no soportado");
            }
        }
        public bool Exists(Rol rol)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao repo = context.Repositories.RolRepository;

                List<FilterProperty> filters = BuildFilters(rol);

                return repo.Exists(filters);
            }
        }
        public bool Exists(Permiso permiso)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IPermisoDao repo = context.Repositories.PermisoRepository;

                List<FilterProperty> filters = BuildFilters(permiso);

                return repo.Exists(filters);
            }
        }

        /// <summary>
        /// Crea o actualiza un rol en la base de datos dependiendo de su existencia.
        /// </summary>
        /// <param name="rol">Objeto de tipo Rol.</param>
        /// <exception cref="EmptyRoleException">Si el rol no tiene accesos.</exception>
        public void CreateOrUpdate(Rol rol)
        {
            if (rol.Accesos.Count == 0) 
                throw new EmptyRoleException();

            bool roleExists = false;

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao repo = context.Repositories.RolRepository;

                List<FilterProperty> filters = BuildFilters(rol);

                roleExists = repo.Exists(filters);
            }

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao repo = context.Repositories.RolRepository;

                if (roleExists)
                {
                    List<FilterProperty> filters = BuildFilters(rol);

                    repo.Update(filters);

                    DeleteChildRelations(context, rol);

                }
                else
                {
                    rol.Id = Guid.NewGuid();
                    repo.Create(rol);
                }

                foreach (Acceso acceso in rol.Accesos)
                {
                    CreateRelation(context, rol, acceso);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Crea o actualiza un permiso en la base de datos dependiendo de su existencia.
        /// </summary>
        /// <param name="permiso">Objeto de tipo Permiso.</param>
        public void CreateOrUpdate(Permiso permiso)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IPermisoDao permisoRepo = context.Repositories.PermisoRepository;

                List<FilterProperty> filters = BuildFilters(permiso);

                if (permisoRepo.Exists(filters))
                {
                    filters = BuildFilters(permiso);

                    permisoRepo.Update(filters);
                }
                else
                {
                    permiso.Id = Guid.NewGuid();
                    permisoRepo.Create(permiso);
                }
                context.SaveChanges();
            }

        }
        /// <summary>
        /// Crea una relación entre un usuario y un acceso (Rol o Permiso).
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="user">Objeto de tipo Usuario.</param>
        /// <param name="acceso">Objeto de tipo Acceso.</param>
        /// <exception cref="ArgumentException">Si el tipo de acceso no es soportado.</exception>
        public void CreateRelations(AppUser user)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                user.Accesos.ForEach(acceso =>
                {
                    switch (acceso)
                    {
                        case Rol rol:
                            CreateRelation(context, user, rol);
                            break;

                        case Permiso permiso:
                            CreateRelation(context, user, permiso);
                            break;

                        default:
                            throw new ArgumentException("Tipo de acceso no soportado");
                    }
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Crea una relación entre un rol y un acceso (Rol o Permiso).
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="rol">Objeto de tipo Rol.</param>
        /// <param name="acceso">Objeto de tipo Acceso.</param>
        /// <exception cref="ArgumentException">Si el tipo de acceso no es soportado.</exception>
        public void CreateRelation(IUnitOfWorkAdapter context, Rol rol, Acceso acceso)
        {
            switch (acceso)
            {
                case Rol childRol:
                    CreateRelation(context, rol, childRol);
                    break;

                case Permiso permiso:
                    CreateRelation(context, rol, permiso);
                    break;

                default:
                    throw new ArgumentException("Tipo de acceso no soportado");
            }
        }

        /// <summary>
        /// Crea una relación entre un usuario y un rol.
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="user">Objeto de tipo Usuario.</param>
        /// <param name="role">Objeto de tipo Rol.</param>
        private void CreateRelation(IUnitOfWorkAdapter context, AppUser user, Rol role)
        {
            UserRolRelation relation = new UserRolRelation()
            {
                ID_User = user.ID_User,
                ID_Rol = role.Id,
            };

            IUserRolDao repo = context.Repositories.UserRolRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = user.ID_User,
                    PropertyName = "ID_User",
                    Operation = FilterOperation.Equals
                },
                new FilterProperty()
                {
                    Value = role.Id,
                    PropertyName = "ID_Rol",
                    Operation = FilterOperation.Equals
                }
            };

            if (repo.Exists(filters)) return;

            repo.Create(relation);
        }

        /// <summary>
        /// Crea una relación entre un usuario y un permiso.
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="user">Objeto de tipo Usuario.</param>
        /// <param name="permiso">Objeto de tipo Permiso.</param>
        private void CreateRelation(IUnitOfWorkAdapter context, AppUser user, Permiso permiso)
        {
            UserPermisoRelation relation = new UserPermisoRelation()
            {
                ID_User = user.ID_User,
                ID_Permiso = permiso.Id
            };

            IUserPermisoDao repo = context.Repositories.UserPermisoRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = user.ID_User,
                    PropertyName = "ID_User",
                    Operation = FilterOperation.Equals
                },
                new FilterProperty()
                {
                    Value = permiso.Id,
                    PropertyName = "ID_User",
                    Operation = FilterOperation.Equals
                }
            };

            if (repo.Exists(filters)) return;

            repo.Create(relation);
        }

        /// <summary>
        /// Crea una relación entre un rol padre y un rol hijo.
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="rolPadre">Objeto de tipo Rol (padre).</param>
        /// <param name="rolHijo">Objeto de tipo Rol (hijo).</param>
        private void CreateRelation(IUnitOfWorkAdapter context, Rol rolPadre, Rol rolHijo)
        {
            RolRolRelation relation = new RolRolRelation()
            {
                ID_Rol_Padre = rolPadre.Id,
                ID_Rol_Hijo = rolHijo.Id,
            };

            IRolRolDao repo = context.Repositories.RolRolRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = rolPadre.Id,
                    PropertyName = "ID_Rol_Padre",
                    Operation = FilterOperation.Equals
                },
                new FilterProperty()
                {
                    Value = rolHijo.Id,
                    PropertyName = "ID_Rol_Hijo",
                    Operation = FilterOperation.Equals
                }
            };

            if (repo.Exists(filters)) return;

            repo.Create(relation);
        }

        /// <summary>
        /// Crea una relación entre un rol y un permiso.
        /// </summary>
        /// <param name="context">Contexto de la unidad de trabajo.</param>
        /// <param name="rol">Objeto de tipo Rol.</param>
        /// <param name="permiso">Objeto de tipo Permiso.</param>
        private void CreateRelation(IUnitOfWorkAdapter context, Rol rol, Permiso permiso)
        {
            RolPermisoRelation relation = new RolPermisoRelation()
            {
                ID_Rol = rol.Id,
                ID_Permiso = permiso.Id,
            };

            IRolPermisoDao repo = context.Repositories.RolPermisoRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = rol.Id,
                    PropertyName = "ID_Rol",
                    Operation = FilterOperation.Equals
                },
                new FilterProperty()
                {
                    Value = permiso.Id,
                    PropertyName = "ID_Permiso",
                    Operation = FilterOperation.Equals
                }
            };

            if (repo.Exists(filters)) return;

            repo.Create(relation);
        }

        /// <summary>
        /// Obtiene todos los accesos de un usuario desde la base de datos.
        /// </summary>
        /// <param name="user">Objeto de tipo Usuario.</param>
        /// <exception cref="NoRolesFoundForUserException">Si no se encuentran roles asociados al usuario.</exception>
        public void GetAccesos(AppUser user)
        {
            List<UserRolRelation> urRelation = null;

            int hidratationLevel = int.Parse(ConfigurationManager.AppSettings["AccessesHidrationLvl"]);

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserRolDao userRolRepo = context.Repositories.UserRolRepository;

                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Operation = FilterOperation.Equals,
                        PropertyName = "ID_User",
                        Value = user.ID_User
                    }
                };

                urRelation = userRolRepo.Get(filters);
            }

            foreach (UserRolRelation relation in urRelation)
            {
                //Busco la relacion user - rol
                using (var context = FactoryDao.UnitOfWork.Create())
                {
                    IRolDao rolRepo = context.Repositories.RolRepository;

                    //Agrego el rol de cada relación
                    user.Accesos.Add(rolRepo.GetOne(BuildFilters(new Rol() { Id = relation.ID_Rol })));
                }
            }

            //A esta altura, user solo tiene roles cargados, asi que los lleno
            foreach (Rol rol in user.Accesos)
            {
                FillRol(rol, --hidratationLevel);
            }

            List<UserPermisoRelation> upRelation = null;

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Operation = FilterOperation.Equals,
                        PropertyName = "ID_User",
                        Value = user.ID_User
                    }
                };

                upRelation = context.Repositories.UserPermisoRepository.Get(filters);
            }

            //Si no tiene permisos asociados, no sigo operando
            if (upRelation.Count == 0) return;

            //Busco la relacion user - permiso
            foreach (UserPermisoRelation relation in upRelation)
            {
                Permiso fetchedPermiso = null;

                using (var context = FactoryDao.UnitOfWork.Create())
                {
                    fetchedPermiso = context.Repositories.PermisoRepository.GetOne(BuildFilters(new Permiso() { Id = relation.ID_Permiso }));
                }

                if (fetchedPermiso == null) throw new DatabaseInconsistencyException();

                //Agrego el permiso de cada relacion
                user.Accesos.Add(fetchedPermiso);
            }
        }

        /// <summary>
        /// Rellena un rol con sus accesos hasta el nivel de hidratación especificado.
        /// </summary>
        /// <param name="rol">Objeto de tipo Rol.</param>
        /// <param name="hidratationLevel">Nivel de profundidad de hidratación.</param>
        private void FillRol(Rol rol, int hidratationLevel)
        {
            if (hidratationLevel == 0) return;

            List<RolRolRelation> rrRelation = null;

            using (var context = FactoryDao.UnitOfWork.Create())
            {

                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Value = rol.Id,
                        PropertyName = "ID_Rol_Padre",
                        Operation = FilterOperation.Equals
                    },
                };

                rrRelation = context.Repositories.RolRolRepository.Get(filters);
            }

            // Levanto todos los roles que tenga el rol
            foreach (RolRolRelation relation in rrRelation)
            {
                Rol fetchedRol = null;

                using (var context = FactoryDao.UnitOfWork.Create())
                {
                    fetchedRol = context.Repositories.RolRepository.GetOne(BuildFilters(new Rol() { Id = relation.ID_Rol_Hijo }));
                }

                if (fetchedRol == null) throw new DatabaseInconsistencyException();

                //Agregar nivel de hidratacion para que el programa no explote
                FillRol(fetchedRol, --hidratationLevel);

                rol.Accesos.Add(fetchedRol);
            }

            List<RolPermisoRelation> rpRelation = null;

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Value = rol.Id,
                        PropertyName = "ID_Rol",
                        Operation = FilterOperation.Equals
                    },
                };

                rpRelation = context.Repositories.RolPermisoRepository.Get(filters);
            }

            // Si no hay permisos asociados entonces no hago nada
            if (rpRelation.Count == 0) return;

            foreach (RolPermisoRelation relation in rpRelation)
            {
                using (var context = FactoryDao.UnitOfWork.Create())
                {

                    List<FilterProperty> filters = new List<FilterProperty>()
                    {
                        new FilterProperty()
                        {
                            Value = relation.ID_Permiso,
                            PropertyName = "Id",
                            Operation = FilterOperation.Equals
                        },
                    };

                    Permiso protoPermiso = context.Repositories.PermisoRepository
                                            .GetOne(BuildFilters(new Permiso() { Id = relation.ID_Permiso }));

                    if (protoPermiso == null) throw new DatabaseInconsistencyException();

                    rol.Accesos.Add(protoPermiso);
                }
            }
        }

        /// <summary>
        /// Valida si un objeto Acceso cumple con las reglas de validación.
        /// </summary>
        /// <param name="acceso">Objeto de tipo Acceso.</param>
        /// <param name="results">Lista donde se almacenan los resultados de la validación.</param>
        /// <returns>True si el objeto es válido, false si no lo es.</returns>
        private bool isValid(Acceso acceso, List<ValidationResult> results)
        {
            var valContext = new ValidationContext(acceso, serviceProvider: null, items: null);

            return Validator.TryValidateObject(acceso, valContext, results, true);
        }

        public void AgregarRolaRol(Rol rolPadre, Rol rolHijo)
        {
            //Completo los accesos del rol hijo
            rolHijo = GetOne(rolHijo);

            //si el que estoy intentando agregar como hijo ya tiene al padre como hijo, se arroja error de recursividad
            if (GetAllChildrenRoles(rolHijo).Any(rh => rh.Id == rolPadre.Id))
                throw new RecursiveRoleAdditionException();

            if (rolPadre.Accesos.Any(rol => rol.Id == rolHijo.Id))
                throw new RoleAlreadyExistInFatherException();

            rolPadre.Add(rolHijo);
        }
        public void AgregarPermisoaRol(Rol rolPadre, Permiso permisoHijo)
        {
            if (rolPadre.Accesos.Any(rol => rol.Id == permisoHijo.Id))
                throw new PermisoAlreadyExistInFatherException();

            rolPadre.Add(permisoHijo);
        }
        private List<Rol> GetAllChildrenRoles(Rol rol)
        {
            List<Rol> returningRoles = new List<Rol>();

            foreach (var acceso in rol.Accesos)
            {
                // Si no tiene hijos, no es un rol, por lo que se omite
                if (!acceso.HasChildren)
                    continue;

                if (!returningRoles.Any(o => o.Id == acceso.Id))
                    returningRoles.Add(acceso as Rol);

                returningRoles.AddRange(GetAllChildrenRoles((acceso as Rol)));
            }
            return returningRoles;
        }

        private List<Permiso> GetAllChildrenPermisos(Rol rol)
        {
            List<Permiso> returningPermiso = new List<Permiso>();

            foreach (var acceso in rol.Accesos)
            {
                // Si no tiene hijos, no es un rol, por lo que se omite
                if (acceso.HasChildren)
                    continue;

                if (!returningPermiso.Any(o => o.Id == acceso.Id))
                    returningPermiso.Add(acceso as Permiso);
            }
            return returningPermiso;
        }

        public List<Permiso> GetAllPermisosFromUser(AppUser usr)
        {
            List<Permiso> returning = new List<Permiso>();

            GetAllPermisos(usr.Accesos, returning);

            return returning;
        }
        private void GetAllPermisos(List<Acceso> accesos, List<Permiso> returning)
        {
            foreach (Acceso acceso in accesos)
            {
                if (acceso.HasChildren)
                {
                    GetAllPermisos((acceso as Rol).Accesos, returning);
                }
                else
                {
                    returning.Add(acceso as Permiso);
                }
            }
        }
        public void RemoveRolFromRol(Rol rolPadre, Rol rolHijo)
        {
            rolPadre.Remove(rolHijo);
        }
        public void RemovePermisoFromRol(Rol rolPadre, Permiso permisoHijo)
        {
            rolPadre.Remove(permisoHijo);
        }
        public void ClearRelations(AppUser user)
        {
            ClearRoleRelations(user);
            ClearPermisoRelations(user);
        }
        private void ClearRoleRelations(AppUser user)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserRolDao repo = context.Repositories.UserRolRepository;

                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Operation = FilterOperation.Equals,
                        PropertyName = "ID_User",
                        Value = user.ID_User
                    }
                };

                repo.Delete(filters);

                context.SaveChanges();
            }
        }
        private void ClearPermisoRelations(AppUser user)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserPermisoDao repo = context.Repositories.UserPermisoRepository;

                List<FilterProperty> filters = new List<FilterProperty>()
                {
                    new FilterProperty()
                    {
                        Operation = FilterOperation.Equals,
                        PropertyName = "ID_User",
                        Value = user.ID_User
                    }
                };

                repo.Delete(filters);

                context.SaveChanges();
            }
        }
        public void Delete(Rol rol)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IRolDao repo = context.Repositories.RolRepository;

                DeleteFatherRelations(context, rol);
                DeleteChildRelations(context, rol);

                List<FilterProperty> filters = BuildFilters(rol);

                repo.Delete(filters);

                context.SaveChanges();
            }
        }
        public void Delete(Permiso permiso)
        {
            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IPermisoDao repo = context.Repositories.PermisoRepository;

                List<FilterProperty> filters = BuildFilters(permiso);

                repo.Delete(filters);

                DeleteFatherRelations(context, permiso);

                context.SaveChanges();
            }
        }
        private void DeleteChildRelations(IUnitOfWorkAdapter context, Rol rol)
        {
            IRolRolDao rrRepo = context.Repositories.RolRolRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = rol.Id,
                    PropertyName = "ID_Rol_Padre",
                    Operation = FilterOperation.Equals
                },
            };

            rrRepo.Delete(filters);

            IRolPermisoDao rpRepo = context.Repositories.RolPermisoRepository;

            filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = rol.Id,
                    PropertyName = "ID_Rol",
                    Operation = FilterOperation.Equals
                },
            };

            rpRepo.Delete(filters);
        }
        private void DeleteFatherRelations(IUnitOfWorkAdapter context, Rol rol)
        {
            IRolRolDao rrRepo = context.Repositories.RolRolRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = rol.Id,
                    PropertyName = "ID_Rol_Hijo",
                    Operation = FilterOperation.Equals
                },
            };

            rrRepo.Delete(filters);
        }
        private void DeleteFatherRelations(IUnitOfWorkAdapter context, Permiso permiso)
        {
            IRolPermisoDao rpRepo = context.Repositories.RolPermisoRepository;
            IUserPermisoDao upRepo = context.Repositories.UserPermisoRepository;

            List<FilterProperty> filters = new List<FilterProperty>()
            {
                new FilterProperty()
                {
                    Value = permiso.Id,
                    PropertyName = "ID_Permiso",
                    Operation = FilterOperation.Equals
                },
            };

            rpRepo.Delete(filters);

            upRepo.Delete(filters);
        }
    }
}
