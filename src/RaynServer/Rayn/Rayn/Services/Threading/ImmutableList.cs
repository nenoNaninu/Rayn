using System;

namespace Rayn.Services.Threading
{
    public sealed class ImmutableList<T>
    {
        public static readonly ImmutableList<T> Empty = new ();
        private readonly T[] _data;

        public T[] Data => _data;

        private ImmutableList()
        {
            _data = Array.Empty<T>();
        }

        public ImmutableList(T[] data)
        {
            _data = data;
        }

        public ImmutableList<T> Add(T value)
        {
            var newData = new T[_data.Length + 1];
            Array.Copy(_data, newData, _data.Length);
            newData[_data.Length] = value;
            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            int i = this.IndexOf(value);
            if (i < 0) return this;
            
            int length = _data.Length;
            if (length == 1) return Empty;

            var newData = new T[length - 1];

            Array.Copy(_data, 0, newData, 0, i);
            Array.Copy(_data, i + 1, newData, i, length - i - 1);

            return new ImmutableList<T>(newData);
        }

        public int IndexOf(T value)
        {
            for (int i = 0; i < _data.Length; ++i)
            {
                if (object.Equals(_data[i], value)) return i;
            }
            return -1;
        }
    }
}