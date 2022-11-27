using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.IOMonad;

public abstract class IO<TA>
{
    public abstract bool IsSuccess();

    public abstract TA Get();

    public abstract Exception Error();

    public IO<TB> Map<TB>(Func<TA, TB> mapper)
    {
        return IsSuccess()
            ? IO.From(() => mapper(Get()))
            : Failure.From<TB>(Error());
    }

    public async Task<IO<TB>> MapAsync<TB>(
        Func<TA, Task<TB>> asyncMapper
    )
    {
        return IsSuccess()
            ? await IO.FromAsync(async () => await asyncMapper(Get()))
            : Failure.From<TB>(Error());
    }

    public IO<TB> FlatMap<TB>(Func<TA, IO<TB>> flatMapper)
    {
        return IsSuccess()
            ? flatMapper(Get())
            : Failure.From<TB>(Error());
    }

    public async Task<IO<TB>> FlatMapAsync<TB>(
        Func<TA, Task<IO<TB>>> flatMapperAsync
    )
    {
        return IsSuccess()
            ? await flatMapperAsync(Get())
            : Failure.From<TB>(Error());
    }

    public TB Fold<TB>(
        Func<Exception, TB> failureFunc,
        Func<TA, TB> successFunc)
    {
        return IsSuccess()
            ? successFunc(Get())
            : failureFunc(Error());
    }

    public async Task<TB> FoldAsync<TB>(
        Func<Exception, TB> failureFunc,
        Func<TA, TB> successFuncAsync
    )
    {
        return IsSuccess()
            ? await Task.FromResult(successFuncAsync(Get()))
            : await Task.FromResult(failureFunc(Error()));
    }

    public async Task<TB> FoldAsync<TB>(
        Func<Exception, TB> failureFunc,
        Func<TA, Task<TB>> successFuncAsync
    )
    {
        return IsSuccess()
            ? await successFuncAsync(Get())
            : failureFunc(Error());
    }

    public async Task<TB> FoldAsync<TB>(
        Func<Exception, Task<TB>> failureFunc,
        Func<TA, TB> successFuncAsync
    )
    {
        return IsSuccess()
            ? successFuncAsync(Get())
            : await failureFunc(Error());
    }

    public async Task<TB> FoldAsync<TB>(
        Func<Exception, Task<TB>> failureFunc,
        Func<TA, Task<TB>> successFuncAsync
    )
    {
        return IsSuccess()
            ? await successFuncAsync(Get())
            : await failureFunc(Error());
    }
    
    public TA CheckError(Func<Exception, TA> failureHandler)
    {
        return IsSuccess()
            ? Get()
            : failureHandler(Error());
    }

    public async Task<TA> CheckErrorAsync(
        Func<Exception, Task<TA>> failureHandlerAsync
    )
    {
        return IsSuccess()
            ? Get()
            : await failureHandlerAsync(Error());
    }

    public async Task<TA> CheckErrorAsync(
        Func<Exception, TA> failureHandler
    )
    {
        return await Task.FromResult(CheckError(failureHandler));
    }

    public static implicit operator IO<TA>(TA value) => new Success<TA>(value);

    public static implicit operator IO<TA>(Exception exception) => new Failure<TA>(exception);

    public Either<Exception, TA> ToEither()
    {
        return IsSuccess()
            ? Right.From<Exception, TA>(Get())
            : Left.From<Exception, TA>(Error());
    }

    public Option<TA> ToOption()
    {
        return IsSuccess()
            ? new Some<TA>(Get())
            : new None<TA>();
    }
    
    public static IO<TA> From(Func<TA> builder)
    {
        try
        {
            var result = builder();
            return Success.From(result);
        }
        catch (Exception e)
        {
            return Failure<TA>.From(e);
        }
    }

    public static async Task<IO<TA>> FromAsync(Func<Task<TA>> asyncBuilder)
    {
        try
        {
            var result = await asyncBuilder();
            return new Success<TA>(result);
        }
        catch (Exception exception)
        {
            return new Failure<TA>(exception);
        }
    }

    public static IO<TA> Pure(TA value) =>
        value != null
            ? Success.From(value)
            : Failure<TA>.From(new Exception("Pure called with null value"));

    public static async Task<IO<TA>> PureAsync(Task<TA> asyncBuilder)
    {
        try
        {
            var result = await asyncBuilder;
            return Pure(result);
        }
        catch (Exception exception)
        {
            return new Failure<TA>(exception);
        }
    }
}

public static class IO
{
    public static IO<TA> From<TA>(Func<TA> f) => IO<TA>.From(f);

    public static async Task<IO<TA>> FromAsync<TA>(Func<Task<TA>> f) => await IO<TA>.FromAsync(f);

    public static IO<TA> Pure<TA>(TA value) => IO<TA>.Pure(value);

    public static async Task<IO<TA>> PureAsync<TA>(Task<TA> pureAsync) => await IO<TA>.PureAsync(pureAsync);

    public static async Task<Either<Exception, TA>> ToEitherAsync<TA>(
        this Task<IO<TA>> ioTask
    )
    {
        return (await ioTask).ToEither();
    }

    public static async Task<Option<TA>> ToOptionAsync<TA>(
        this Task<IO<TA>> ioTask
    )
    {
        return (await ioTask).ToOption();
    }
}