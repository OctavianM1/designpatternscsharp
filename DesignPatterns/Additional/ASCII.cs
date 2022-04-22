using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DesignPatterns.Additional;

// ascii urf-16
public class str : IEquatable<str?>
{
    [NotNull]
    protected readonly byte[] buffer;

    public str()
    {
        buffer = new byte[] {};
    }

    public str(string s)
    {
        buffer = Encoding.ASCII.GetBytes(s);
    }

    protected str(byte[] buffer)
    {
        this.buffer = buffer;
    }

    public static implicit operator str(string s)
    {
        return new str(s);
    }

    public static bool operator ==(str? left, str? right)
    {
        return EqualityComparer<str>.Default.Equals(left, right);
    }

    public static bool operator !=(str? left, str? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return Encoding.ASCII.GetString(buffer);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as str);
    }

    public bool Equals(str? other)
    {
        return other != null &&
               EqualityComparer<byte[]>.Default.Equals(buffer, other.buffer);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public static str operator +(str first, str second)
    {
        var bytes = new byte[first.buffer.Length + second.buffer.Length];
        first.buffer.CopyTo(bytes, 0);
        second.buffer.CopyTo(bytes, first.buffer.Length);
        return new str(bytes);
    }

    public char this[int index]
    {
        get => (char)buffer[index];
        set => buffer[index] = (byte)value;
    }
}
