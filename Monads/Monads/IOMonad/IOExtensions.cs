using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.IOMonad;

public static class IOExtensions
{
    public static async Task<IO<TB>> MapAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<TA, Task<TB>> asyncMapper)
    {
        return await (await ioTask).MapAsync(asyncMapper);
    }

    public static async Task<IO<TB>> MapAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<TA, TB> asyncMapper)
    {
        return (await ioTask).Map(asyncMapper);
    }

    public static async Task<IO<TB>> FlatMapAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<TA, Task<IO<TB>>> flatMapperAsync)
    {
        return await (await ioTask).FlatMapAsync(flatMapperAsync);
    }

    public static async Task<IO<TB>> FlatMapAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<TA, IO<TB>> flatMapperAsync)
    {
        return (await ioTask).FlatMap(flatMapperAsync);
    }

    public static async Task<TB> FoldAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<Exception, TB> failureFunc,
        Func<TA, TB> successFuncAsync
    )
    {
        return await (await ioTask).FoldAsync(failureFunc, successFuncAsync);
    }

    public static async Task<TB> FoldAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<Exception, TB> failureFunc,
        Func<TA, Task<TB>> successFuncAsync
    )
    {
        return await (await ioTask).FoldAsync(failureFunc, successFuncAsync);
    }

    public static async Task<TB> FoldAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<Exception, Task<TB>> failureFuncAsync,
        Func<TA, TB> successFunc
    )
    {
        return await (await ioTask).FoldAsync(failureFuncAsync, successFunc);
    }

    public static async Task<TB> FoldAsync<TA, TB>(
        this Task<IO<TA>> ioTask,
        Func<Exception, Task<TB>> failureFuncAsync,
        Func<TA, Task<TB>> successFuncAsync)
    {
        return await (await ioTask).FoldAsync(failureFuncAsync, successFuncAsync);
    }
    
    public static async Task<TA> CheckErrorAsync<TA>(
        this Task<IO<TA>> ioTask,
        Func<Exception, TA> failureHandler)
    {
        return await (await ioTask).CheckErrorAsync(failureHandler);
    }

    public static async Task<TA> CheckErrorAsync<TA>(
        this Task<IO<TA>> ioTask,
        Func<Exception, Task<TA>> failureHandlerAsync)
    {
        return await (await ioTask).CheckErrorAsync(failureHandlerAsync);
    }
}