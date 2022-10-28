using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.EitherMonad;

public static class EitherExtensions
{
    public static async Task<Either<TE, TB>> MapAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TA, TB> asyncMapper
    )
    {
        return (await eitherTask).Map(asyncMapper);
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
        Func<TE, Task<TB>> leftHandler,
        Func<TA, TB> rightHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftHandler, rightHandler);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TB> leftHandler,
        Func<TA, Task<TB>> rightHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftHandler, rightHandler);
    }

    public static async Task<TB> FoldAsync<TE, TA, TB>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TB>> leftHandler,
        Func<TA, Task<TB>> rightHandler
    )
    {
        return await (await eitherTask).FoldAsync(leftHandler, rightHandler);
    }

    public static async Task<TA> CheckErrorAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TA> leftHandlerAsync)
    {
        return await (await eitherTask).CheckErrorAsync(leftHandlerAsync);
    }

    public static async Task<TA> CheckErrorAsync<TE, TA>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TA>> leftHandlerAsync)
    {
        return await (await eitherTask).CheckErrorAsync(leftHandlerAsync);
    }

    public static async Task<Either<TEE, TA>> MapErrorAsync<TE, TA, TEE>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, TEE> leftHandler
    )
    {
        return await (await eitherTask).MapErrorAsync(leftHandler);
    }

    public static async Task<Either<TEE, TA>> MapErrorAsync<TE, TA, TEE>(
        this Task<Either<TE, TA>> eitherTask,
        Func<TE, Task<TEE>> leftHandlerAsync
    )
    {
        return await (await eitherTask).MapErrorAsync(leftHandlerAsync);
    }
}