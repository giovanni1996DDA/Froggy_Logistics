using Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StateMachines.TurnoStateMachine.Interfaces
{
    public interface ITurnosState
    {
        void Agendar(Turno turno);
        void Recepcionar(Turno turno);
        void Atender(Turno turno);
        void Cerrar(Turno turno);

    }
}
