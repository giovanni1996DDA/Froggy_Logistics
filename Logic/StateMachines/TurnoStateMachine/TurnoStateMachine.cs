using Dao;
using Logic.Enums;
using Logic.StateMachines.TurnoStateMachine.Interfaces;
using Logic.StateMachines.TurnoStateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StateMachines.TurnoStateMachine
{
    public class TurnoStateMachine
    {
        Turno _turno;

        ITurnosState _turnosState;
        public TurnoStateMachine(Turno turno) 
        {
            _turno = turno;

            setState();
        }

        private void setState()
        {
            switch (_turno.Estado)
            {
                case (int)EstadosTurnosEnum.Agendado:
                    _turnosState = new AgendadoState();
                    break;

                case (int)EstadosTurnosEnum.Recepcionado:
                    _turnosState = new RecepcionadoState();
                    break;

                case (int)EstadosTurnosEnum.EnConsulta:
                    _turnosState = new EnConsultaState();
                    break;

                case (int)EstadosTurnosEnum.Cerrado:
                    _turnosState = new CerradoState();
                    break;

                default:
                    _turnosState = new NewTurnoState();
                    break;
            }
        }
        public void Agendar()
        {
            _turnosState.Agendar(_turno);
            setState();
        }
        public void Recepcionar() 
        { 
            _turnosState.Recepcionar(_turno);
            setState();
        }
        public void Atender()
        {
            _turnosState.Atender(_turno);
            setState();
        }
        public void Cerrar() 
        { 
            _turnosState.Cerrar(_turno); 
            setState();
        }
    }
}
