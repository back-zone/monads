using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.EitherMonad;

public static class EitherExtentions
{
    public static async Task<Either<TLeftType, TRightTypeB>> MapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<TRightTypeB>> right
        )
    {
        var either = await eitherTask;
        return await either.MapAsync(right);
    }

    public static async Task<Either<TLeftType, TRightTypeB>> FlatMapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<Either<TLeftType, TRightTypeB>>> right
        )
    {
        var either = await eitherTask;
        return await either.FlatMapAsync(right);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, Task<TUnifiedType>> left,
            Func<TRightTypeA, Task<TUnifiedType>> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? await left(either.Left!) : await right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, TUnifiedType> left,
            Func<TRightTypeA, Task<TUnifiedType>> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? left(either.Left!) : await right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, Task<TUnifiedType>> left,
            Func<TRightTypeA, TUnifiedType> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? await left(either.Left!) : right(either.Right!);
    }
}