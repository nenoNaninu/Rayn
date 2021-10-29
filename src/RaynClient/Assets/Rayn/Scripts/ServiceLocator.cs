using System.Threading;
using Cysharp.Threading.Tasks;

namespace Rayn
{
    public static class ServiceLocator
    {
        public static void Register<TKey, TValue>(TValue value)
            where TValue : TKey
        {
            Cache<TKey>.Instance = value;
            Cache<TKey>.HasValue = true;
            Cache<TKey>.Source.TrySetResult();
        }

        public static void Register<T>(T value)
        {
            Cache<T>.Instance = value;
            Cache<T>.HasValue = true;
            Cache<T>.Source.TrySetResult();
        }

        public static async UniTask<TKey> GetServiceAsync<TKey>(CancellationToken cancellation = default)
        {
            if (Cache<TKey>.HasValue)
            {
                return Cache<TKey>.Instance;
            }

            await Cache<TKey>.Source.Task.AttachExternalCancellation(cancellation);

            return Cache<TKey>.Instance;
        }

        public static void Clear<TKey>()
        {
            if (Cache<TKey>.HasValue)
            {
                Cache<TKey>.Instance = default;
                Cache<TKey>.HasValue = false;
                Cache<TKey>.Source = new UniTaskCompletionSource();
            }
        }

        private static class Cache<T>
        {
            public static UniTaskCompletionSource Source { get; set; } = new UniTaskCompletionSource();
            public static T Instance { get; set; }
            public static bool HasValue { get; set; } = false;
        }
    }
}
