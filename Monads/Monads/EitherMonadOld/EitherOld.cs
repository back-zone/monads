using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.EitherMonadOld;

public readonly struct EitherOld<TLeftType, TRightTypeA>
{
    public readonly TLeftType? Left;

    public readonly TRightTypeA? Right;

    public readonly bool IsLeft;

    internal EitherOld(TLeftType left)
    {
        IsLeft = true;
        Left = left;
        Right = default;
    }

    internal EitherOld(TRightTypeA right)
    {
        IsLeft = false;
        Left = default;
        Right = right;
    }

    public static implicit operator EitherOld<TLeftType, TRightTypeA>(TLeftType left)
    {
        return new EitherOld<TLeftType, TRightTypeA>(left);
    }

    public static implicit operator EitherOld<TLeftType, TRightTypeA>(TRightTypeA right)
    {
        return new EitherOld<TLeftType, TRightTypeA>(right);
    }

    public EitherOld<TLeftType, TRightTypeB> Map<TRightTypeB>(
        Func<TRightTypeA, TRightTypeB> right
    ) => IsLeft ? Left! : right(Right!);

    public async Task<EitherOld<TLeftType, TRightTypeB>> MapAsync<TRightTypeB>(
        Func<TRightTypeA, Task<TRightTypeB>> right
    ) => IsLeft ? Left! : await right(Right!);

    public EitherOld<TLeftType, TRightTypeB> Flatmap<TRightTypeB>(
        Func<TRightTypeA, EitherOld<TLeftType, TRightTypeB>> right
    ) => IsLeft ? Left! : right(Right!);

    public async Task<EitherOld<TLeftType, TRightTypeB>> FlatmapAsync<TRightTypeB>(
        Func<TRightTypeA, Task<EitherOld<TLeftType, TRightTypeB>>> right
    ) => IsLeft ? Left! : await right(Right!);

    public TUnifiedType Fold<TUnifiedType>(Func<TLeftType, TUnifiedType> left, Func<TRightTypeA, TUnifiedType> right) =>
        IsLeft ? left(Left!) : right(Right!);

    public async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<TLeftType, Task<TUnifiedType>> left,
        Func<TRightTypeA, Task<TUnifiedType>> right
    ) => IsLeft ? await left(Left!) : await right(Right!);

    public async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<TLeftType, TUnifiedType> left,
        Func<TRightTypeA, Task<TUnifiedType>> right
    ) => IsLeft ? left(Left!) : await right(Right!);

    public async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<TLeftType, Task<TUnifiedType>> left,
        Func<TRightTypeA, TUnifiedType> right
    ) => IsLeft ? await left(Left!) : right(Right!);
}