using Dao;
using Logic.Exceptions;
using Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PacienteService
    {
        private static PacienteService _instance = new PacienteService();

        public static PacienteService Instance { 
            get 
            { 
                return _instance;
            }
        }
        private PacienteService()
        {
        }

        public void CreatePaciente(Paciente paciente)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                Paciente pacienteExistente = context.Paciente
                                                    .FirstOrDefault(p => p.NumeroDocumento == paciente.NumeroDocumento);

                if (pacienteExistente != null)
                    throw new PacienteAlreadyExistsException();

                paciente.Id = Guid.NewGuid();

                LogicHelpers.SetGuidEmptyToNull(paciente);

                context.Paciente.Add(paciente);

                context.SaveChanges();
            }
        }

        public Paciente GetOneByDocument(Paciente paciente)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                Paciente pacienteExistente = context.Paciente
                                                    .FirstOrDefault(p => (p.NumeroDocumento == paciente.NumeroDocumento && p.TipoDocumento == paciente.TipoDocumento));

                if (pacienteExistente == null)
                    throw new PacienteDoesNotExistsException();

                return pacienteExistente;

            }
        }
        public void UpdatePaciente(Paciente paciente)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Buscar el paciente existente por su Número de Documento y Tipo de Documento
                Paciente pacienteExistente = context.Paciente
                                                    .FirstOrDefault(p => p.NumeroDocumento == paciente.NumeroDocumento
                                                                      && p.TipoDocumento == paciente.TipoDocumento);

                // Si no existe, lanzamos una excepción
                if (pacienteExistente == null)
                    throw new PacienteDoesNotExistsException();

                // Aquí actualizamos las propiedades del paciente existente con los nuevos valores
                pacienteExistente.Nombre = paciente.Nombre;
                pacienteExistente.Apellido = paciente.Apellido;
                pacienteExistente.Direccion = paciente.Direccion;
                pacienteExistente.FechaNacimiento = paciente.FechaNacimiento;
                pacienteExistente.ObraSocial = paciente.ObraSocial;
                pacienteExistente.Telefono = paciente.Telefono;
                pacienteExistente.Email = paciente.Email;

                LogicHelpers.SetGuidEmptyToNull(pacienteExistente);

                context.SaveChanges();
            }
        }

        public Paciente GetById(Paciente paciente)
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                // Buscar el paciente existente por su Número de Documento y Tipo de Documento
                Paciente pacienteExistente = context.Paciente
                                                    .FirstOrDefault(p => p.Id == paciente.Id);

                if (pacienteExistente == null)
                    throw new PacienteDoesNotExistsException();
                
                LogicHelpers.SetGuidEmptyToNull(pacienteExistente);

                return pacienteExistente;
            }
        }
    }
}
