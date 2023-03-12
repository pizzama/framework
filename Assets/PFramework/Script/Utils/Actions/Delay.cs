using System;
using System.Collections;
using UnityEngine;

/*
* example 
        Delay.Create(1f, () => Debug.Log("I will be executed after 1 second."));
        
        Delay.WaitUntil(() => Time.time >= 5f, () => Debug.Log("I will wait until the provided condition is true."));
*/

namespace PUtils
{
    public sealed class Delay
    {
        private IEnumerator coroutine;

        /// <summary>
        /// Indicates if the delay has completed and the provided action was invoked.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Indicates if the delay has been paused.
        /// </summary>
        public bool IsPaused { get; private set; }

        private Action action;

        private Delay()
        {
        }

        private static IEnumerator DelayCoroutine(Delay delay, float delayInSeconds, bool ignoreTimeScale, int framesToSkip)
        {
            while (framesToSkip > 1) //TODO: check if > 1 is still correct (is coroutine started on next frame by Unity?)
            {
                if (!delay.IsPaused)
                    framesToSkip--;

                yield return null;
            }

            while (delayInSeconds > 0)
            {
                if (!delay.IsPaused)
                {
                    if (ignoreTimeScale)
                        delayInSeconds -= Time.unscaledDeltaTime;
                    else
                        delayInSeconds -= Time.deltaTime;
                }

                yield return null;
            }

            delay.InvokeAction();
        }

        private static IEnumerator WaitCoroutine(Delay delay, Func<bool> condition, float timeout = -1, bool ignoreTimeScale = false, bool skipEvaluationForFirstFrame = false)
        {
            if (skipEvaluationForFirstFrame)
                yield return null;

            float elapsedTime = 0;
            while (!condition())
            {
                if (timeout >= 0)
                {
                    if (ignoreTimeScale)
                        elapsedTime += Time.unscaledDeltaTime;
                    else
                        elapsedTime += Time.deltaTime;

                    if (elapsedTime >= timeout)
                        break;
                }

                yield return null;
            }

            delay.InvokeAction();
        }

        private void InvokeAction()
        {
            if (IsComplete)
                return;

            action.Invoke();

            IsComplete = true;
        }

        private static Delay CreateInternal(float delayInSeconds, Action action, bool ignoreTimeScale, int framesToSkip)
        {
            var delay = new Delay
            {
                action = action
            };

            StartInternal(delay, delayInSeconds, ignoreTimeScale, framesToSkip);

            return delay;
        }

        private static void StartInternal(Delay delay, float delayInSeconds, bool ignoreTimeScale, int framesToSkip)
        {
            if (delay.IsComplete)
                return;

            if (delayInSeconds <= 0 && framesToSkip <= 0)
            {
                delay.InvokeAction();
                return;
            }

            delay.coroutine = DelayCoroutine(delay, delayInSeconds, ignoreTimeScale, framesToSkip);

            DelayMonoBehaviour.StartDelay(delay.coroutine);
        }

        /// <summary>
        /// Executes an action after a given delay.
        /// </summary>
        public static Delay Create(float delay, Action action, bool ignoreTimeScale = false)
        {
            return CreateInternal(delay, action, ignoreTimeScale, 0);
        }

        /// <summary>
        /// Skips a frame and then executes the given action.
        /// </summary>
        public static Delay SkipFrame(Action onNextFrame)
        {
            return CreateInternal(0, onNextFrame, true, 1);
        }

        /// <summary>
        /// Skips a number of frame and then executes the given action.
        /// </summary>
        public static Delay SkipFrames(int framesToSkip, Action onNextFrame)
        {
            return CreateInternal(0, onNextFrame, true, framesToSkip);
        }

        public static Delay WaitUntil(Func<bool> condition, Action action, float timeout = -1, bool ignoreTimeScale = false)
        {
            var d = new Delay
            {
                action = action
            };

            if (condition()) //TODO: unit test e.g. null
            {
                d.InvokeAction();
                return d;
            }

            d.coroutine = WaitCoroutine(d, condition, timeout, ignoreTimeScale, true);

            if (!DelayMonoBehaviour.StartDelay(d.coroutine))
                return null;

            return d;
        }

        /// <summary>
        /// Cancels all ongoing delays.
        /// </summary>
        public static void StopAll()
        {
            DelayMonoBehaviour.Instance.StopAllCoroutines();
        }

        /// <summary>
        /// Stops the ongoing delay, but does not execute the provided action.
        /// </summary>
        /// <remarks><see cref="IsComplete"/> is not set to TRUE by this method.</remarks>
        public void Stop()
        {
            DelayMonoBehaviour.StopDelay(coroutine);
        }

        /// <summary>
        /// Stops the delay and executes the provided action.
        /// </summary>
        public void Complete()
        {
            Stop();
            InvokeAction();
        }

        /// <summary>
        /// Pauses the delay.
        /// </summary>
        /// <remarks>If the delay was not completed when <see cref="Pause"/> was called, the action will be executed after <see cref="Resume"/> has been called and once the delay is complete.</remarks>
        public void Pause()
        {
            if (IsPaused)
                return;

            IsPaused = true;
        }

        /// <summary>
        /// Resumes the delay.
        /// </summary>
        /// <remarks>The delay will wait the remaining time, before it will execute the provided action.</remarks>
        public void Resume()
        {
            if (!IsPaused)
                return;

            IsPaused = false;
        }
    }

    internal sealed class DelayMonoBehaviour : MonoBehaviour
    {
        private static DelayMonoBehaviour instance;

        private static bool applicationIsQuitting;

        internal static DelayMonoBehaviour Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return null;
                }

                if (instance == null)
                {
                    var obj = new GameObject("Delay").AddComponent<DelayMonoBehaviour>();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static bool StartDelay(IEnumerator coroutine)
        {
            if (Instance == null)
                return false;

            Instance.StartDelayInternal(coroutine);

            return true;
        }

        private void StartDelayInternal(IEnumerator coroutine)
        {
            if (applicationIsQuitting)
                return;

            StartCoroutine(coroutine);
        }

        public static bool StopDelay(IEnumerator coroutine)
        {
            if (Instance == null)
                return false;

            Instance.StopDelayInternal(coroutine);

            return true;
        }

        private void StopDelayInternal(IEnumerator coroutine)
        {
            if (applicationIsQuitting)
                return;

            StopCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            applicationIsQuitting = true;
            StopAllCoroutines();
        }
    }
}