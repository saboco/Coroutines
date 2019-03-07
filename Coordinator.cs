using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Coroutines
{
    public sealed class Coordinator : IEnumerable, INotifyCompletion
    {
        // Execute actions in the queue until it’s empty. Actions add *more*
        // actions (continuations) to the queue by awaiting this coordinator.
        public void Start()
        {
            while (actions.Count > 0)
            {
                actions.Dequeue().Invoke();
            }
        }

        // Used by collection initializer to specify the coroutines to run
        public void Add(Action<Coordinator> coroutine)
        {
            actions.Enqueue(() => coroutine(this));
        } 

        private readonly Queue<Action> actions = new Queue<Action>(); 

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException("IEnumerable only supported to enable collection initializers"); 
        }

        // Force await to yield control
        public bool IsCompleted => false;

        // Used by await expressions to get an awaiter
        public Coordinator GetAwaiter()
        {
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            // Put the continuation at the end of the queue, ready to
            // execute when the other coroutines have had a go.
            actions.Enqueue(continuation);
        }

        public void GetResult()
        {
            // Our await expressions are void, and we never need to throw
            // an exception, so this is a no-op.
        } 
    }
}
