using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    public sealed class Maybe<ValueType>
    {
        private readonly IEnumerable<ValueType> values;

        public Maybe(ValueType value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            values = new[] { value };
        }

        public Maybe() =>
            values = new ValueType[0];

        public ValueType ValueOrDefault(ValueType defaultValue) =>
            values.DefaultIfEmpty(defaultValue).Single();

        public void Apply(Action<ValueType> action)
        {
            if (values.Any())
                action.Invoke(values.Single());
        }

        public T ApplyOrDefault<T>(Func<ValueType, T> func, T defaultValue)
        {
            if (values.Any())
                return func.Invoke(values.Single());
            return defaultValue;
        }

        public void IfEmpty(Action action)
        {
            if (!values.Any())
                action.Invoke();
        }
    }

    public sealed class Maybe
    {
        public static Maybe<T> Create<T>(T value)
        {
            if (value == null)
                return new Maybe<T>();
            return new Maybe<T>(value);
        }

        public static Maybe<T> Create<T>(T? value) where T:struct
        {
            if (!value.HasValue)
                return new Maybe<T>();
            return new Maybe<T>(value.Value);
        }
    }
}
