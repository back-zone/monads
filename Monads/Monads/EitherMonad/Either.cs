using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.EitherMonad;

public readonly struct Either<TLeftType, TRightTypeA>
{
    public readonly TLeftType? Left;

    public readonly TRightTypeA? Right;

    public readonly bool IsLeft;

    internal Either(TLeftType left)
    {
        IsLeft = true;
        Left = left;
        Right = default;
    }

    internal Either(TRightTypeA right)
    {
        IsLeft = false;
        Left = default;
        Right = right;
    }

    public static implicit operator Either<TLeftType, TRightTypeA>(TLeftType left)
    {
        return new Either<TLeftType, TRightTypeA>(left);
    }

    public static implicit operator Either<TLeftType, TRightTypeA>(TRightTypeA right)
    {
        return new Either<TLeftType, TRightTypeA>(right);
    }

    public Either<TLeftType, TRightTypeB> Map<TRightTypeB>(
        Func<TRightTypeA, TRightTypeB> right
    ) => IsLeft ? Left! : right(Right!);

    public async Task<Either<TLeftType, TRightTypeB>> MapAsync<TRightTypeB>(
        Func<TRightTypeA, Task<TRightTypeB>> right
    ) => IsLeft ? Left! : await right(Right!);

    public Either<TLeftType, TRightTypeB> Flatmap<TRightTypeB>(
        Func<TRightTypeA, Either<TLeftType, TRightTypeB>> right
    ) => IsLeft ? Left! : right(Right!);

    public async Task<Either<TLeftType, TRightTypeB>> FlatmapAsync<TRightTypeB>(
        Func<TRightTypeA, Task<Either<TLeftType, TRightTypeB>>> right
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