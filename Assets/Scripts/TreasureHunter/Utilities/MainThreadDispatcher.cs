using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TreasureHunter.Utilities
{
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
    {
        private static readonly Queue<Action> m_ExecutionQueue = new Queue<Action>();

        public override void Awake()
        {
            base.Awake();

            StartCoroutine(RunQueue());
        }

        public void RunOnMainThread(IEnumerator action)
        {
            lock (m_ExecutionQueue)
            {
                m_ExecutionQueue.Enqueue(() => { StartCoroutine(action); });
            }
        }

        public void RunOnMainThread(Action action)
        {
            RunOnMainThread(ActionWrapper(action));
        }

        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            RunOnMainThread(ActionWrapper(WrappedAction));
            return tcs.Task;
        }

        private IEnumerator ActionWrapper(Action action)
        {
            action();
            yield break;
        }

        private IEnumerator RunQueue()
        {
            while (true)
            {
                yield return new WaitUntil(IsQueueAvailable);

                lock (m_ExecutionQueue)
                {
                    m_ExecutionQueue.Dequeue().Invoke();
                }
            }
        }

        private bool IsQueueAvailable()
        {
            return m_ExecutionQueue.Count > 0;
        }
    }
}