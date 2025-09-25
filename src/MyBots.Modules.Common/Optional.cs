namespace MyBots.Modules.Common;

public readonly record struct Optional<T>
{
    private readonly T _value;
    public bool HasValue { get; }

    public Optional(T value)
    {
        _value = value!;
        HasValue = true;
    }

    public static Optional<T> None => default;

    public static Optional<T> Some(T value) => new(value);

    public T ValueOrThrow()
    {
        if (!HasValue) throw new InvalidOperationException("Optional has no value.");
        return _value;
    }

    public T GetValueOrDefault() => HasValue ? _value : default!;

    public T GetValueOrDefault(T defaultValue) => HasValue ? _value : defaultValue;

    public bool TryGetValue(out T value)
    {
        value = HasValue ? _value : default!;
        return HasValue;
    }

    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
    {
        ArgumentNullException.ThrowIfNull(onSome);
        ArgumentNullException.ThrowIfNull(onNone);
        return HasValue ? onSome(_value) : onNone();
    }

    public void Match(Action<T> onSome, Action onNone)
    {
        ArgumentNullException.ThrowIfNull(onSome);
        ArgumentNullException.ThrowIfNull(onNone);
        if (HasValue) onSome(_value); else onNone();
    }

    public override string ToString() => HasValue ? $"Some({_value})" : "None";

    // Operators
    public static implicit operator Optional<T>(T value) => new(value);
    public static explicit operator T(Optional<T> optional) => optional.ValueOrThrow();
}
