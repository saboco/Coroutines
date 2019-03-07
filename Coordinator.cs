using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Coroutines
{
    public sealed class Coordinator<T>
    {
        private readonly Queue<Action> _actions;
        private readonly Awaitable _awaitable;
        private T _currentValue;
        private bool _valuePresent;

        public Coordinator(params Func<Coordinator<T>, T, Task<T>>[] coroutines)
        {
            // We can’t refer to "this" in the variable initializer. We can use
            // the same awaitable for all yield calls.
            _awaitable = new Awaitable(this);
            _actions = new Queue<Action>(coroutines.Select(ConvertCoroutine));
        }

        public Awaitable Yield(T value)
        {
            SupplyValue(value);
            return _awaitable;
        }

        // Execute actions in the queue until it’s empty. Actions add *more*
        // actions (continuations) to the queue by awaiting this coordinator.
        public T Start(T initialValue)
        {
            SupplyValue(initialValue);

            while (_actions.Count > 0)
            {
                _actions.Dequeue().Invoke();
            }

            return ConsumeValue();
        }

        // Converts a coroutine into an action which consumes the current value,
        // calls the coroutine, and attaches a continuation to it so that the return
        // value is used as the new value.
        private Action ConvertCoroutine(Func<Coordinator<T>, T, Task<T>> coroutine)
        {
            return () =>
            {
                var task = coroutine(this, ConsumeValue());
                task.ContinueWith(ignored => SupplyValue(task.Result),
                    TaskContinuationOptions.ExecuteSynchronously);
            };
        }

        private void SupplyValue(T value)
        {
            if (_valuePresent)
            {
                throw new InvalidOperationException
                    ("Attempt to supply value when one is already present");
            }
            _currentValue = value;
            _valuePresent = true;
        }

        private T ConsumeValue()
        {
            if (!_valuePresent)
            {
                throw new InvalidOperationException
                    ("Attempt to consume value when it isn’t present");
            }
            var oldValue = _currentValue;
            _valuePresent = false;
            _currentValue = default(T);
            return oldValue;
        } 

        public sealed class Awaitable : INotifyCompletion
        {
            private readonly Coordinator<T> _coordinator;

            internal Awaitable(Coordinator<T> coordinator)
            {
                _coordinator = coordinator;
            }

            public Awaitable GetAwaiter()
            {
                return this;
            }

            public bool IsCompleted => false;

            public void OnCompleted(Action continuation)
            {
                _coordinator._actions.Enqueue(continuation);
            }

            public T GetResult()
            {
                return _coordinator.ConsumeValue();
            }
        }
    }
}
