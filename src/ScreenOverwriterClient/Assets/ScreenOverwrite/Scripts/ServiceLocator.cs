using System.Threading;
using Cysharp.Threading.Tasks;

namespace ScreenOverwriter
{
    public static class ServiceLocator
    {
        public static void Register<TKey, TValue>(TValue value) 
            where TValue : TKey
        {
            Cache<TKey>.Instance = value;
            Cache<TKey>.Enable = true;
            Cache<TKey>.Source.TrySetResult();
        }

        public static async UniTask<TKey> ResolveAsync<TKey>(CancellationToken cancellation = default)
        {
            if (Cache<TKey>.Enable)
            {
                return Cache<TKey>.Instance;
            }

            await Cache<TKey>.Source.Task.AttachExternalCancellation(cancellation);

            return Cache<TKey>.Instance;
        }

        public static void Clear<TKey>()
        {
            if (Cache<TKey>.Enable)
            {
                Cache<TKey>.Instance = default;
                Cache<TKey>.Enable = false;
                Cache<TKey>.Source = new UniTaskCompletionSource();
            }
        }

        private static class Cache<T>
        {
            public static UniTaskCompletionSource Source { get; set; } = new UniTaskCompletionSource();
            public static T Instance { get; set; }
            public static bool Enable { get; set; } = false;
        }
    }
}