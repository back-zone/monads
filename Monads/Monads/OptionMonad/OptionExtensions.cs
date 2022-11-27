using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.OptionMonad;

public static class OptionExtensions
{
    public static async Task<Option<TB>> MapAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Func<TA, TB> mapper)
    {
        return (await optionTask).Map(mapper);
    }

    public static async Task<Option<TB>> MapAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Func<TA, Task<TB>> asyncMapper)
    {
        return await (await optionTask).MapAsync(asyncMapper);
    }

    public static async Task<Option<TB>> FlatMapAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Func<TA, Option<TB>> asyncFlatMapper)
    {
        return (await optionTask).FlatMap(asyncFlatMapper);
    }

    public static async Task<Option<TB>> FlatMapAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Func<TA, Task<Option<TB>>> asyncFlatMapper)
    {
        return await (await optionTask).FlatMapAsync(asyncFlatMapper);
    }

    public static async Task<Option<TB>> FoldAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        TB none,
        Func<TA, Task<TB>> some
    )
    {
        return await (await optionTask).FoldAsync(none, some);
    }

    public static async Task<Option<TB>> FoldAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Task<TB> none,
        Func<TA, Task<TB>> some
    )
    {
        return await (await optionTask).FoldAsync(none, some);
    }

    public static async Task<Option<TB>> FoldAsync<TA, TB>(
        this Task<Option<TA>> optionTask,
        Task<TB> none,
        Func<TA, TB> some
    )
    {
        return await (await optionTask).FoldAsync(none, some);
    }

    public static async Task<Option<TA>> GetOrElseAsync<TA>(
        this Task<Option<TA>> optionTask,
        TA elseRightValue
    )
    {
        return (await optionTask).GetOrElse(elseRightValue);
    }
}