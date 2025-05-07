using Dao;
using Logic.Exceptions;
using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
    public class EspecialidadService : IObserver<User>
    {
        private static readonly EspecialidadService _instance = new EspecialidadService();

        private bool isSuscribed = false;
        private EspecialidadService()
        {
            UserService.Instance.Subscribe(this);
        }
        public static EspecialidadService Instance
        {
            get
            {
                return _instance;
            }
        }
        public List<Especialidad> GetByUser(User user)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Buscar la especialidad asociada al usuario en la tabla UsuarioEspecialidad
                List<Especialidad> especialidad = context.UsuarioEspecialidad
                                                    .Where(ue => ue.UserId == user.Id)  // Filtrar por el usuario pasado como parámetro
                                                    .Select(ue => ue.Especialidad)
                                                    .ToList();  // Seleccionar la especialidad asociada

                return especialidad;
            }
        }
        public List<Especialidad> Get()
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Obtener todas las especialidades de la tabla Especialidad
                especialidades.AddRange(context.Especialidad.AsNoTracking().ToList());

                // Si la lista de especialidades está vacía, lanzar una excepción
                if (especialidades == null || !especialidades.Any())
                {
                    throw new NoEspecialidadFoundException();
                }
            }
            return especialidades;
        }
        public Especialidad GetById(Especialidad protoEspecialidad)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                Especialidad especialidad = context.Especialidad
                                                    .Where(e => e.Id == protoEspecialidad.Id)                                       
                                                    .FirstOrDefault();

                if (especialidad == null)
                {
                    throw new NoEspecialidadFoundException();
                }
                return especialidad;
            }
        }
        public async Task<List<Especialidad>> GetAsync()
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Cargar las especialidades de forma asincrónica sin cargar relaciones
                var especialidades = await context.Especialidad
                                                  .AsNoTracking()
                                                  .ToListAsync(); // Uso de ToListAsync()

                if (!especialidades.Any())
                {
                    throw new NoEspecialidadFoundException();
                }

                return especialidades;
            }
        }
        public Especialidad GetOneByUser(User user)
        {
            // Buscar la especialidad asociada al usuario en la tabla UsuarioEspecialidad
            Especialidad especialidad = GetByUser(user).FirstOrDefault();

            return especialidad;
        }
        public List<User> GetUsersByEspecialidad(Especialidad especialidad)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                
                List<User> protoUsers = new List<User>();

                var usuariosEspecialidadRelation = context.UsuarioEspecialidad
                                                          .Where(ue => ue.EspecialidadId == especialidad.Id)
                                                          .ToList();

                usuariosEspecialidadRelation.ForEach(ue => 
                {
                    protoUsers.Add(new User
                    {
                        Id = ue.UserId
                    });
                });

                return protoUsers;
            }
        }
        public void DeleteUserRelations(User user)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Cargar las especialidades de forma asincrónica sin cargar relaciones
                var UserEspecialidadRelation= context.UsuarioEspecialidad
                                                     .Where(ue => ue.UserId == user.Id)
                                                     .ToList();

                if (UserEspecialidadRelation.Any())
                    context.UsuarioEspecialidad.RemoveRange(UserEspecialidadRelation);
                
                context.SaveChanges();
            }
        }

        public void CreateUserRelations(User user)
        {
            user.Especialidades.ForEach(esp =>
            {
                using (var context = new DentalCareDBEntities())
                {
                    var nuevaUsuarioEspecialidad = new UsuarioEspecialidad
                    {
                        UserId = new Guid(user.Id.Value.ToByteArray()),
                        EspecialidadId = new Guid(esp.Id.ToByteArray())
                    };

                    context.UsuarioEspecialidad.Add(nuevaUsuarioEspecialidad);

                    context.SaveChanges();
                }
            });
        }

        public void OnNext(User user)
        {
            DeleteUserRelations(user);

            CreateUserRelations(user);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
