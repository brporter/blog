namespace posts.Services;

public class RepositoryValue<T>
{
    private readonly T? _value;

    private RepositoryValue(T? value)
    {
        _value = value;
    }

    public T Value => _value!;

    public bool HasValue => _value != null;

    public static implicit operator T(RepositoryValue<T> r) => r.Value;
    public static implicit operator RepositoryValue<T>(T v) => new(v);
}