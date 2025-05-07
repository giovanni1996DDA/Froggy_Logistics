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
    public class NewTurnoState : ITurnosState
    {
        public void Agendar(Turno turno)
        {
            turno.Estado = (int)EstadosTurnosEnum.Agendado;
        }

        public void Atender(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede atender un turno que no ha sido agendado.");

        }

        public void Cerrar(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede cerrar un turno que no ha sido agendado.");
        }

        public void Recepcionar(Turno turno)
        {
            throw new StateChangeNotPossibleException("No se puede recepcionar un turno que no se encuentra agendado.");
        }
    }
}
