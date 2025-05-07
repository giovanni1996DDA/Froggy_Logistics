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
    internal class AgendadoState : ITurnosState
    {
        public void Agendar(Turno turno)
        {
            throw new StateChangeNotPossibleException("El turno ya se encuentra agendado.");
        }

        public void Atender(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede atender un turno que no está recepcionado.");
        }

        public void Cerrar(Turno turno)
        {
            turno.Estado = (int)EstadosTurnosEnum.Cerrado;
        }

        public void Recepcionar(Turno turno)
        {
            turno.Estado = (int)EstadosTurnosEnum.Recepcionado;
        }
    }
}
