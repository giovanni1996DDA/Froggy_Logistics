using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class Unsubscriber : IDisposable
    {
        private List<IObserver<User>> _observers;
        private IObserver<User> _observer;

        public Unsubscriber(List<IObserver<User>> observers, IObserver<User> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers != null && _observer != null)
            {
                _observers.Remove(_observer);
            }
        }
    }
}
