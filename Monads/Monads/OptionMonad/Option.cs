using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.IOMonad;

namespace Back.Zone.Monads.OptionMonad;

public abstract class Option<TA>
{
    public abstract bool IsEmpty();

    public abstract TA Get();

    public bool IsDefined() => !IsEmpty();

    public static implicit operator Option<TA>(TA value) => new Some<TA>(value);

    public static Option<TA> From(Func<TA> builder) =>
        IO.From(builder).ToOption();

    public static async Task<Option<TA>> FromAsync(Func<Task<TA>> builderAsync) =>
        await IO.FromAsync(builderAsync).ToOptionAsync();

    public static Option<TA> Pure(TA pureValue) =>
        IO.Pure(pureValue).ToOption();

    public static async Task<Option<TA>> PureAsync(Task<TA> pureValueAsync) =>
        await IO.PureAsync(pureValueAsync).ToOptionAsync();

    public Option<TB> Map<TB>(Func<TA, TB> mapper)
    {
        return IsEmpty() ? new None<TB>() : mapper(Get());
    }

    public async Task<Option<TB>> MapAsync<TB>(Func<TA, Task<TB>> asyncMapper)
    {
        return IsEmpty() ? new None<TB>() : await asyncMapper(Get());
    }

    public Option<TB> FlatMap<TB>(Func<TA, Option<TB>> flatMapper)
    {
        return IsEmpty() ? new None<TB>() : flatMapper(Get());
    }

    public async Task<Option<TB>> FlatMapAsync<TB>(Func<TA, Task<Option<TB>>> asyncFlatMapper)
    {
        return IsEmpty() ? new None<TB>() : await asyncFlatMapper(Get());
    }

    public TB Fold<TB>(TB none, Func<TA, TB> some)
    {
        return IsEmpty() ? none : some(Get());
    }

    public async Task<TB> FoldAsync<TB>(
        TB none,
        Func<TA, Task<TB>> some)
    {
        return IsEmpty() ? none : await some(Get());
    }

    public async Task<TB> FoldAsync<TB>(
        Task<TB> none,
        Func<TA, Task<TB>> some)
    {
        return IsEmpty() ? await none : await some(Get());
    }

    public async Task<TB> FoldAsync<TB>(
        Task<TB> none,
        Func<TA, TB> some)
    {
        return IsEmpty() ? await none : some(Get());
    }

    public TA GetOrElse(
        TA elseValue
    )
    {
        return IsEmpty() ? elseValue : Get();
    }

    public async Task<TA> GetOrElseAsync(
        Task<TA> elseRightValueAsync
    )
    {
        return IsEmpty()
            ? await elseRightValueAsync
            : Get();
    }

    public Either<Exception, TA> ToEither()
    {
        return IsEmpty()
            ? new Left<Exception, TA>(new NullReferenceException("None"))
            : new Right<Exception, TA>(Get());
    }

    public IO<TA> ToIO()
    {
        return IsEmpty()
            ? new Failure<TA>(new NullReferenceException("None"))
            : new Success<TA>(Get());
    }
}

public static class Option
{
    public static Option<TA> From<TA>(Func<TA> f) => Option<TA>.From(f);

    public static async Task<Option<TA>> FromAsync<TA>(Func<Task<TA>> f) => await Option<TA>.FromAsync(f);

    public static Option<TA> Pure<TA>(TA value) => Option<TA>.Pure(value);

    public static async Task<Option<TA>> PureAsync<TA>(Task<TA> pureAsync) => await Option<TA>.PureAsync(pureAsync);

    public static async Task<Either<Exception, TA>> ToEitherAsync<TA>(
        this Task<Option<TA>> optionTask
    )
    {
        return (await optionTask).ToEither();
    }

    public static async Task<IO<TA>> ToIOAsync<TA>(
        this Task<Option<TA>> optionTask
    )
    {
        return (await optionTask).ToIO();
    }
}