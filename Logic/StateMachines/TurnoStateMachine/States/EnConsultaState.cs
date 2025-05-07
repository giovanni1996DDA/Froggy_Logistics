using Dao;
using Logic.Enums;
using Logic.Exceptions;
using Logic.StateMachines.TurnoStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StateMachines.TurnoStateMachine.States
{
    internal class EnConsultaState : ITurnosState
    {
        public void Agendar(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede agendar un turno que ya está en consulta");
        }

        public void Atender(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede atender un turno que ya fué atendido.");
        }

        public void Cerrar(Turno turno)
        {
            turno.Estado = (int)EstadosTurnosEnum.Cerrado;
        }

        public void Recepcionar(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede recepcionar un turno que ya está en consulta.");

        }
    }
}
