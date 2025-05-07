using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public class Unsubscriber : IDisposable
    {
        private List<IObserver<object>> _observers;
        private IObserver<object> _observer;

        public Unsubscriber(List<IObserver<object>> observers, IObserver<object> observer)
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
