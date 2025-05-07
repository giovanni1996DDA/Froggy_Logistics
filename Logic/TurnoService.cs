using Dao;
using Logic.Enums;
using Logic.Exceptions;
using Logic.StateMachines.TurnoStateMachine;
using Services.Domain;
using Services.Facade;
using Services.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Logic
{
    public class TurnoService
    {
        private static TurnoService _instance = new TurnoService();

        public static TurnoService Instance
        {
            get
            {
                return _instance;
            }
        }
        private TurnoService()
        {
        }

        public void Crear(Turno protoTurno)
        {
            using (var context = new DentalCareDBEntities())
            {
                protoTurno.Id = protoTurno.Id == Guid.Empty ? Guid.NewGuid() : protoTurno.Id;

                context.Turno.Add(protoTurno);

                context.SaveChanges();
            }
        }

        public void Agendar(Turno turno)
        {
            try
            {
                TurnoStateMachine sm = new TurnoStateMachine(turno);

                sm.Agendar();

                Crear(turno);
            }
            catch (StateChangeNotPossibleException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public List<Turno> GetBetween(DateTime from, DateTime to)
        {
            using (var context = new DentalCareDBEntities())
            {
                // Cargar turnos junto con los datos del paciente
                var turnos = context.Turno
                    .Where(t => t.FechaHora >= from && t.FechaHora <= to)
                    .Include(t => t.Paciente1) // Cargar la información del paciente
                    .Include(t => t.Paciente1.TipoDocumento1)
                    .ToList();

                if (!turnos.Any())
                    throw new NoTurnosFoundException();

                FillPersonsData(turnos);

                return turnos;
            }
        }
        public List<Turno> GetToday()
        {
            using (var context = new DentalCareDBEntities())
            {
                // Cargar turnos junto con los datos del paciente
                var turnos = context.Turno
                    .Where(t => DbFunctions.TruncateTime(t.FechaHora) == DateTime.Today)
                    .Include(t => t.Paciente1) // Cargar la información del paciente
                    .Include(t => t.Paciente1.TipoDocumento1)
                    .ToList();

                if (!turnos.Any())
                    throw new NoTurnosFoundException();

                // Asignar nombres completos de paciente y profesional
                FillPersonsData(turnos);

                return turnos;
            }
        }
        private void FillPersonsData(List<Turno> turnos)
        {
            turnos.ForEach(turno =>
            {
                try
                {
                    // Construir el nombre completo del paciente                        
                    turno.NombreCompletoPaciente = $"{turno.Paciente1.Apellido}, {turno.Paciente1.Nombre}";

                    // Obtener y asignar el nombre completo del profesional
                    var profesional = UserFacade.GetOne(new User { Id = turno.Profesional });

                    turno.NombreCompletoProfesional = $"{profesional.Apellido}, {profesional.Nombre}";
                }
                catch (NoUsersFoundException ex)
                {
                    MessageBox.Show($"{ex.Message}. Revisar Logs.",
                                    "Error de inconsistencia en BBDD.",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}. Revisar Logs.",
                                    "Ocurrió un error inesperado.",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            });
        }
        public void Atender(Turno turno)
        {
            try
            {
                TurnoStateMachine sm = new TurnoStateMachine(turno);

                sm.Atender();

                Update(turno, ignoreOverlapping: true);

            }
            catch(StateChangeNotPossibleException)
            {
                throw;
            }
            catch (TurnoDoesNotExistException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(Turno protoTurno, bool ignoreOverlapping = false)
        {
            // Validar el turnoPrototipo para asegurar que los datos esenciales están completos
            if (protoTurno == null) throw new ArgumentNullException(nameof(protoTurno));
            if (protoTurno.Profesional == Guid.Empty) throw new ArgumentException("El profesional es obligatorio.");
            if (protoTurno.Paciente == Guid.Empty) throw new ArgumentException("El paciente es obligatorio.");

            if (isOverlapping(protoTurno) && !ignoreOverlapping)
                throw new TurnoIsOverlappingException();

            using (var context = new DentalCareDBEntities())
            {
                // Buscar el turno en la base de datos
                var existingTurno = context.Turno.FirstOrDefault(t => t.Id == protoTurno.Id);

                if (existingTurno == null)
                {
                    throw new TurnoDoesNotExistException();
                }

                // Actualizar propiedades
                existingTurno.Profesional = protoTurno.Profesional;
                existingTurno.Paciente = protoTurno.Paciente;
                existingTurno.FechaHora = protoTurno.FechaHora;
                existingTurno.Estado = protoTurno.Estado;
                existingTurno.Especialidad = protoTurno.Especialidad;

                // Guardar cambios
                context.SaveChanges();
            }
        }

        private bool isOverlapping(Turno protoTurno)
        {
            using (var context = new DentalCareDBEntities())
            {
                // Buscar el turno en la base de datos
                Turno existingTurno = context.Turno.FirstOrDefault(t => t.Profesional == protoTurno.Profesional
                                                                     && t.FechaHora == protoTurno.FechaHora);

                //Si es null, no se overlapea
                return !(existingTurno == null);
            }
        }

        public List<Turno> GetRecepcionados()
        {
            using (var context = new DentalCareDBEntities())
            {
                // Cargar turnos junto con los datos del paciente
                var turnos = context.Turno
                    .Where(t => DbFunctions.TruncateTime(t.FechaHora) == DateTime.Today 
                             && t.Estado == (int)EstadosTurnosEnum.Recepcionado)
                    .Include(t => t.Paciente1) // Cargar la información del paciente
                    .Include(t => t.Paciente1.TipoDocumento1)
                    .ToList();

                if (!turnos.Any())
                    throw new NoTurnosFoundException();

                // Asignar nombres completos de paciente y profesional
                FillPersonsData(turnos);

                return turnos;
            }
        }
    }
}
