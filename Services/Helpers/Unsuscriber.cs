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
        private List<IObserver<AppUser>> _observers;
        private IObserver<AppUser> _observer;

        public Unsubscriber(List<IObserver<AppUser>> observers, IObserver<AppUser> observer)
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
