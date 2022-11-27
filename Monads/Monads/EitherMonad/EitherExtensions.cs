using System;
using System.Threading.Tasks;
using Back.Zone.Monads.IOMonad;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.EitherMonad;

public static class EitherExtensions
{
    public static async Task<Either<TE, TB>> MapAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TA, TB> mapper
    )
    {
        return (await eitherTask).Map(mapper);
    }

    public static async Task<Either<TE, TB>> MapAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TA, Task<TB>> asyncMapper
    )
    {
        return await (await eitherTask).MapAsync(asyncMapper);
    }

    public static async Task<Either<TE, TB>> FlatMapAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TA, Either<TE, TB>> flatMapper
    )
    {
        return (await eitherTask).FlatMap(flatMapper);
    }

    public static async Task<Either<TE, TB>> FlatMapAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TA, Task<Either<TE, TB>>> flatMapper
    )
    {
        return await (await eitherTask).FlatMapAsync(flatMapper);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TB> leftHandler,
        Func<TA, TB> rightHandler)
    {
        return (await eitherTask).Fold(leftHandler, rightHandler);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TB>> leftAsyncHandler,
        Func<TA, TB> rightHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftAsyncHandler, rightHandler);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TB> leftHandler,
        Func<TA, Task<TB>> rightAsyncHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftHandler, rightAsyncHandler);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TB>> leftAsyncHandler,
        Func<TA, Task<TB>> rightAsyncHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftAsyncHandler, rightAsyncHandler);
    }

    public static async Task<TA> CheckErrorAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TA> leftHandler)
    {
        return (await eitherTask).CheckError(leftHandler);
    }

    public static async Task<TA> CheckErrorAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TA>> leftAsyncHandler)
    {
        return await (await eitherTask).CheckErrorAsync(leftAsyncHandler);
    }

    public static async Task<Either<TEE, TA>> MapErrorAsync<TE, TA, TEE>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TEE> leftHandler
    )
    {
        return (await eitherTask).MapError(leftHandler);
    }

    public static async Task<Either<TEE, TA>> MapErrorAsync<TE, TA, TEE>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TEE>> leftHandlerAsync
    )
    {
        return await (await eitherTask).MapErrorAsync(leftHandlerAsync);
    }

    public static async Task<Either<TA, TE>> SwapAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask
    )
    {
        return await (await eitherTask).SwapAsync();
    }

    public static async Task<TA> GetOrElseAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        TA elseRightValue
    )
    {
        return (await eitherTask).GetOrElse(elseRightValue);
    }

    public static async Task<TA> GetOrElseAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        Task<TA> elseRightValueAsync
    )
    {
        return await (await eitherTask).GetOrElseAsync(elseRightValueAsync);
    }

    public static async Task<Option<TA>> ToOption<TE, TA>(
        this Task<Either<TE, TA>> eitherTask
    )
    {
        return (await eitherTask).ToOption();
    }

    public static async Task<IO<TA>> ToIO<TE, TA>(
        this Task<Either<TE, TA>> eitherTask
    )
    {
        return (await eitherTask).ToIO();
    }
}