using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Ontos.Generators.Core.Collections;

internal static class EquatableArray
{
    public static EquatableArray<T> AsEquatableArray<T>(this ImmutableArray<T> array)
        where T : IEquatable<T> => new(array);

    public static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> source)
        where T : IEquatable<T> => new(source.ToImmutableArray());
}

internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    private readonly T[]? _array = Unsafe.As<ImmutableArray<T>, T[]?>(ref array);

    public ref readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref AsImmutableArray().ItemRef(index);
    }

    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => AsImmutableArray().IsEmpty;
    }

    public bool IsDefaultOrEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => AsImmutableArray().IsDefaultOrEmpty;
    }

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => AsImmutableArray().Length;
    }

    public static EquatableArray<T> Empty => default;

    /// <inheritdoc/>
    public bool Equals(EquatableArray<T> array)
    {
        return AsSpan().SequenceEqual(array.AsSpan());
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> array && Equals(this, array);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        if (_array is  null) return 0;

        unchecked
        {
            int hash = 17;

            foreach (var item in _array)
                hash = (hash * 397) ^ (item?.GetHashCode() ?? 0);

            return hash;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ImmutableArray<T> AsImmutableArray()
    {
        return Unsafe.As<T[]?, ImmutableArray<T>>(ref Unsafe.AsRef(in this._array));
    }

    public static EquatableArray<T> FromImmutableArray(ImmutableArray<T> array)
    {
        return new(array);
    }


    public ReadOnlySpan<T> AsSpan()
    {
        return AsImmutableArray().AsSpan();
    }

    public T[] ToArray()
    {
        return [.. AsImmutableArray()];
    }

    public ImmutableArray<T>.Enumerator GetEnumerator()
    {
        return AsImmutableArray().GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return ((IEnumerable<T>)AsImmutableArray()).GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)AsImmutableArray()).GetEnumerator();
    }


    public static implicit operator EquatableArray<T>(ImmutableArray<T> array) => FromImmutableArray(array);
    public static implicit operator ImmutableArray<T>(EquatableArray<T> array) => array.AsImmutableArray();

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);
    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}