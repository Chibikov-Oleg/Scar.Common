using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Scar.Common.MVVM.Commands
{
    public class ApplicationCommandManager : ICommandManager
    {
        readonly IList<Action> _raiseCanExecuteChangedActions = new List<Action>();
        readonly SynchronizationContext _synchronizationContext;
        readonly object _locker = new object();

        public ApplicationCommandManager(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));
        }

        public void AddRaiseCanExecuteChangedAction(ref Action raiseCanExecuteChangedAction)
        {
            lock (_locker)
            {
                _raiseCanExecuteChangedActions.Add(raiseCanExecuteChangedAction);
            }
        }

        public void RemoveRaiseCanExecuteChangedAction(Action raiseCanExecuteChangedAction)
        {
            lock (_locker)
            {
                _raiseCanExecuteChangedActions.Remove(raiseCanExecuteChangedAction);
            }
        }

        public void AssignOnPropertyChanged(ref PropertyChangedEventHandler propertyEventHandler)
        {
            propertyEventHandler += OnPropertyChanged;
        }

        public void RefreshCommandStates()
        {
            _synchronizationContext.Send(
                x =>
                {
                    lock (_locker)
                    {
                        foreach (var raiseCanExecuteChangedAction in _raiseCanExecuteChangedActions)
                        {
                            raiseCanExecuteChangedAction?.Invoke();
                        }
                    }
                },
                null);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // this if clause is to prevent an infinity loop
            if (e.PropertyName != "CanExecute")
            {
                RefreshCommandStates();
            }
        }
    }
}
