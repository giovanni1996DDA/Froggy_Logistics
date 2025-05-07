using Services.Dao.Factory;
using Services.Dao.Implementations.SQLServer;
using Services.Dao.Interfaces;
using Services.Domain;
using Services.Helpers;
using Services.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    /// <summary>
    /// Servicio para gestionar usuarios en el sistema.
    /// Proporciona funcionalidad para registrar, actualizar, validar y obtener usuarios.
    /// </summary>
    public class UserService : Logic<User>, IObservable<User>
    {
        #region Singleton
        /// <summary>
        /// Instancia única de la clase UserService (patrón Singleton).
        /// </summary>
        private static readonly UserService instance = new UserService();

        /// <summary>
        /// Obtiene la instancia única del servicio de usuarios.
        /// </summary>
        public static UserService Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Constructor privado para implementar el patrón Singleton.
        /// </summary>
        private UserService() { }
        #endregion

        List<IObserver<User>> observers = new List<IObserver<User>>();

        /// <summary>
        /// Registra un usuario en la base de datos.
        /// </summary>
        /// <param name="user">El objeto usuario a registrar.</param>
        /// <exception cref="InvalidUserException">Si los datos del usuario no son válidos.</exception>
        /// <exception cref="UserAlreadyRegisteredException">Si el nombre de usuario ya está registrado.</exception>
        public void RegisterUser(User user)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            Guid userId = Guid.NewGuid();

            if (!isValid(user, results))
                throw new InvalidUserException(results.FirstOrDefault()?.ErrorMessage);

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserDao userRepo = context.Repositories.UserRepository;

                User validatingUser = new User()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                };

                List<FilterProperty> filters = BuildFilters(validatingUser);

                // Verifica si existe un usuario con ese UserName
                if (userRepo.Exists(filters))
                    throw new UserAlreadyRegisteredException();
            }

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserDao userRepo = context.Repositories.UserRepository;

                user.Id = userId;

                userRepo.Create(user);

                context.SaveChanges();
            }

            observers.ForEach(o => o.OnNext(user));

            AccesoService.Instance.CreateRelations(user);
        }
        public void Login(User user) 
        {
            try
            {
                user = GetOne(user);

                SessionManager.SetUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene una lista de usuarios que cumplen con los criterios del objeto de búsqueda.
        /// </summary>
        /// <param name="user">Objeto usuario con los criterios de búsqueda.</param>
        /// <returns>Lista de usuarios que coinciden con los criterios.</returns>
        /// <exception cref="NoUsersFoundException">Si no se encuentran usuarios que coincidan con los criterios.</exception>
        public List<User> Get(User user)
        {
            List<User> returningUsers = new List<User>();

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserDao userRepo = context.Repositories.UserRepository;

                List<FilterProperty> filters = BuildFilters(user);

                returningUsers = userRepo.Get(filters);
            }

            if (returningUsers.Count == 0)
                throw new NoUsersFoundException();

            foreach (User returningUser in returningUsers)
            {
                try
                {
                    AccesoService.Instance.GetAccesos(returningUser);
                }
                catch (NoRolesFoundException)
                {
                }
                catch (NoPermissionsFoundException)
                {
                }
            }

            //Acá deberia llenar especialidades, pero no me deja llamar a EspecialidadService por referencia circular.

            return returningUsers;
        }
        public void Delete(User user)
        {
            user.IsAnulated = true;

            try
            {
                UpdateUser(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public User GetOne(User user)
        {
            try
            {
                User returning = Get(user).FirstOrDefault();
                return returning;
            }
            catch (NoUsersFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Actualiza los datos de un usuario en la base de datos.
        /// </summary>
        /// <param name="user">El objeto usuario a actualizar.</param>
        /// <exception cref="UserDoesNotExistException">Si el usuario no existe en la base de datos.</exception>
        public void UpdateUser(User user)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!isValid(user, results))
                throw new InvalidUserException(results.FirstOrDefault()?.ErrorMessage);

            using (var context = FactoryDao.UnitOfWork.Create())
            {
                IUserDao userRepo = context.Repositories.UserRepository;

                List<FilterProperty> filters = BuildFilters(user);

                if (!userRepo.Exists(filters))
                    throw new UserDoesNotExistException();

                userRepo.Update(filters);

                context.SaveChanges();
            }

            AccesoService.Instance.ClearRelations(user);

            AccesoService.Instance.CreateRelations(user);

            observers.ForEach(o => o.OnNext(user));
        }

        /// <summary>
        /// Obtiene una lista de permisos a partir de una lista de accesos.
        /// </summary>
        /// <param name="accesos">Lista de accesos del usuario.</param>
        /// <returns>Lista de permisos asociados a los accesos del usuario.</returns>
        public List<Permiso> GetPermisos(List<Acceso> accesos)
        {
            List<Permiso> permisos = new List<Permiso>();
            GetAllPermisos(accesos, permisos);
            return permisos;
        }

        /// <summary>
        /// Obtiene de forma recursiva todos los permisos a partir de una lista de accesos.
        /// </summary>
        /// <param name="accesos">Lista de accesos inicial.</param>
        /// <param name="permisosReturn">Lista de permisos que se irá llenando recursivamente.</param>
        private void GetAllPermisos(List<Acceso> accesos, List<Permiso> permisosReturn)
        {
            foreach (var acceso in accesos)
            {
                // Si tiene hijos, es un rol con accesos
                if (acceso.HasChildren)
                {
                    GetAllPermisos((acceso as Rol).Accesos, permisosReturn);
                    continue;
                }

                // Si no tiene hijos, es un permiso y se verifica si ya fue agregado
                if (permisosReturn.Any(o => o.Id == acceso.Id))
                    continue;

                permisosReturn.Add(acceso as Permiso);
            }
        }

        /// <summary>
        /// Obtiene una lista de roles a partir de una lista de accesos.
        /// </summary>
        /// <param name="accesos">Lista de accesos del usuario.</param>
        /// <returns>Lista de roles asociados a los accesos del usuario.</returns>
        public List<Rol> GetRoles(List<Acceso> accesos)
        {
            List<Rol> roles = new List<Rol>();
            GetAllRoles(accesos, roles);
            return roles;
        }

        /// <summary>
        /// Obtiene de forma recursiva todos los roles a partir de una lista de accesos.
        /// </summary>
        /// <param name="accesos">Lista de accesos inicial.</param>
        /// <param name="rolesReturn">Lista de roles que se irá llenando recursivamente.</param>
        private void GetAllRoles(List<Acceso> accesos, List<Rol> rolesReturn)
        {
            foreach (var acceso in accesos)
            {
                // Si no tiene hijos, no es un rol, por lo que se omite
                if (!acceso.HasChildren)
                    continue;

                if (!rolesReturn.Any(o => o.Id == acceso.Id))
                    rolesReturn.Add(acceso as Rol);

                GetAllRoles((acceso as Rol).Accesos, rolesReturn);
            }
        }

        /// <summary>
        /// Valida si un usuario cumple con los requisitos de validación de datos.
        /// </summary>
        /// <param name="usr">El objeto usuario a validar.</param>
        /// <param name="results">Lista de resultados de validación.</param>
        /// <returns>True si el usuario es válido, de lo contrario, false.</returns>
        private bool isValid(User usr, List<ValidationResult> results)
        {
            var valContext = new ValidationContext(usr, serviceProvider: null, items: null);
            return Validator.TryValidateObject(usr, valContext, results, true);
        }

        public IDisposable Subscribe(IObserver<User> observer)
        {
            observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }
    }
}
