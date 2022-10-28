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
}