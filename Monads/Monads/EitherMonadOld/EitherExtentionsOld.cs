using System;
using System.Threading.Tasks;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.EitherMonadOld;

public static class EitherExtentionsOld
{
    public static TUnifiedType Fold<TUnifiedType>(
        this EitherOld<TUnifiedType, TUnifiedType> eitherOld
    ) => eitherOld.Fold(l => l, r => r);

    public static async Task<EitherOld<TLeftType, TRightTypeB>> MapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<EitherOld<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<TRightTypeB>> right
        )
    {
        var either = await eitherTask;
        return await either.MapAsync(right);
    }

    public static async Task<EitherOld<TLeftType, TRightTypeB>> FlatmapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<EitherOld<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<EitherOld<TLeftType, TRightTypeB>>> right
        )
    {
        var either = await eitherTask;
        return await either.FlatmapAsync(right);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<EitherOld<TLeftType, TRightTypeA>> eitherTask,
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
            this Task<EitherOld<TLeftType, TRightTypeA>> eitherTask,
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
            this Task<EitherOld<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, Task<TUnifiedType>> left,
            Func<TRightTypeA, TUnifiedType> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? await left(either.Left!) : right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        this Task<EitherOld<TUnifiedType, TUnifiedType>> either
    ) => (await either).Fold(te => te, ta => ta);

    public static Option<TRightType> ToOption<TLeftType, TRightType>(
        this EitherOld<TLeftType, TRightType> eitherOld
    )
    {
        return eitherOld.Fold<Option<TRightType>>(_ => new None<TRightType>(), leftSide => new Some<TRightType>(leftSide));
    }

    public static async Task<Option<TRightType>> ToOptionAsync<TLeftType, TRightType>(
        this Task<EitherOld<TLeftType, TRightType>> either
    )
    {
        var result = await either;
        return result.Fold<Option<TRightType>>(_ => new None<TRightType>(), leftSide => new Some<TRightType>(leftSide));
    }

    public static EitherOld<TEB, TA> MapError<TE, TEB, TA>(
        this EitherOld<TE, TA> eitherOld,
        Func<TE, TEB> errorMapper
    ) => eitherOld
        .Fold<EitherOld<TEB, TA>>(te => errorMapper(te), ta => ta);

    public static async Task<EitherOld<TEB, TA>> MapErrorAsync<TE, TEB, TA>(
        this Task<EitherOld<TE, TA>> eitherTask,
        Func<TE, TEB> errorMapper
    ) => (await eitherTask).MapError(errorMapper);

    public static async Task<EitherOld<TE, TB>> MapAsync<TE, TA, TB>(
        this Task<EitherOld<TE, TA>> eitherTask,
        Func<TA, TB> map
    ) => (await eitherTask).Map(map);

    public static async Task<EitherOld<TE, TB>> FlatmapAsync<TE, TA, TB>(
        this Task<EitherOld<TE, TA>> eitherTask,
        Func<TA, EitherOld<TE, TB>> flatmap
    ) => (await eitherTask).Flatmap(flatmap);
}