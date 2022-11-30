namespace posts.Services;

public class Optional<T>
{
    public static readonly Optional<T> Empty = new(default(T));

    private readonly T? _value;

    private Optional(T? value)
    {
        _value = value;
    }

    public T Value => _value!;

    public bool HasValue => _value != null;

    public static implicit operator T(Optional<T> r) => r.Value;
    public static implicit operator Optional<T>(T? v) => new(v);
}