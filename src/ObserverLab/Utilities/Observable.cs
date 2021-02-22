using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverLab
{
    public class Observable<T> : IObservable<T>, IDisposable
    {
        // Fields
        private readonly object _syncRoot = new object();

        private List<IObserver<T>> _observerList = new List<IObserver<T>>();

        private List<IObserver<T>> _observerListCache = null;


        // Constructors
        public Observable()
        {

        }

        public void Dispose()
        {
            // ObserverList
            List<IObserver<T>> observerList = null;

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_observerList == null) return;

                // Clear
                observerList = _observerList;
                _observerList = null;

                // Cache
                _observerListCache = null;
            }

            // Notify
            foreach (var observer in observerList)
            {
                // OnCompleted
                observer.OnCompleted();
            }
        }


        // Methods
        public void Write(T value)
        {
            // ObserverList
            var observerList = _observerListCache;
            if (observerList == null)
            {
                // Sync
                lock (_syncRoot)
                {
                    // Require
                    if (_observerList == null) throw new ObjectDisposedException(this.GetType().Name);

                    // Cache
                    if (_observerListCache == null)
                    {
                        _observerListCache = _observerList.ToList();
                    }
                    observerList = _observerListCache;
                }
            }

            // Notify
            foreach (var observer in observerList)
            {
                // OnNext
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            #region Contracts

            if (observer == null) throw new ArgumentException(nameof(observer));

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_observerList == null) throw new ObjectDisposedException(this.GetType().Name);

                // Add
                if (_observerList.Contains(observer) == false)
                {
                    _observerList.Add(observer);
                }

                // Cache
                _observerListCache = null;
            }

            // Return
            return new Subscription(() => this.Unsubscribe(observer));
        }

        private void Unsubscribe(IObserver<T> observer)
        {
            #region Contracts

            if (observer == null) throw new ArgumentException(nameof(observer));

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_observerList == null) return;

                // Remove
                if (_observerList.Contains(observer) == true)
                {
                    _observerList.Remove(observer);
                }

                // Cache
                _observerListCache = null;
            }
        }


        // Class
        private class Subscription : IDisposable
        {
            // Fields
            private readonly Action _removeAction = null;


            // Constructors
            public Subscription(Action removeAction)
            {
                #region Contracts

                if (removeAction == null) throw new ArgumentException(nameof(removeAction));

                #endregion

                // Default
                _removeAction = removeAction;
            }

            public void Dispose()
            {
                // Execute
                _removeAction();
            }
        }
    }
}
