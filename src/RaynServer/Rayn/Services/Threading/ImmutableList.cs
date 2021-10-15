using System;

namespace Rayn.Services.Threading
{
    public class ImmutableList<T>
    {
        public static readonly ImmutableList<T> Empty = new();

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
            int length = _data.Length;

            var newData = new T[length + 1];

            Array.Copy(_data, newData, length);
            newData[length] = value;

            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            int index = Array.IndexOf(_data, value);

            if (index < 0)
            {
                return this;
            }

            int length = _data.Length;

            if (length == 1)
            {
                return Empty;
            }

            var newData = new T[length - 1];

            Array.Copy(_data, 0, newData, 0, index);
            Array.Copy(_data, index + 1, newData, index, length - index - 1);

            return new ImmutableList<T>(newData);
        }
    }

}