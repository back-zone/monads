using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.OptionMonad;
using Back.Zone.Monads.Validated;

namespace Back.Zone.Monads.TryMonad;

public static class Try
{
    [Obsolete("Try is deprecated, use IO instead.")]
    public static Try<TTryTypeA> From<TTryTypeA>(TTryTypeA? value)
    {
        try
        {
            return value != null
                ? new Success<TTryTypeA>(value)
                : new Failure<TTryTypeA>(new NullReferenceException($"{typeof(TTryTypeA).Name} is null"));
        }
        catch (Exception e)
        {
            return new Failure<TTryTypeA>(e);
        }
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static Try<TTryTypeA> From<TTryTypeA>(Func<TTryTypeA> func)
    {
        try
        {
            var result = func();
            return new Success<TTryTypeA>(result);
        }
        catch (Exception e)
        {
            return new Failure<TTryTypeA>(e);
        }
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Try<TTryTypeA>> FromAsync<TTryTypeA>(
        Task<TTryTypeA> task
    )
    {
        try
        {
            var result = await task;
            return new Success<TTryTypeA>(result);
        }
        catch (Exception e)
        {
            return new Failure<TTryTypeA>(e);
        }
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Try<TTryTypeA>> FromAsync<TTryTypeA>(
        Func<Task<TTryTypeA>> func
    )
    {
        try
        {
            var result = await func();
            return new Success<TTryTypeA>(result);
        }
        catch (Exception e)
        {
            return new Failure<TTryTypeA>(e);
        }
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Try<TTryTypeB>> MapAsync<TTryTypeA, TTryTypeB>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<TTryTypeA, Task<TTryTypeB>> func
    )
    {
        var result = await tryTask;
        return await result.MapAsync(func);
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Try<TTryTypeB>> FlatmapAsync<TTryTypeA, TTryTypeB>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<TTryTypeA, Task<Try<TTryTypeB>>> func
    )
    {
        var result = await tryTask;
        return await result.FlatmapAsync(func);
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<TUnifiedType> FoldAsync<TTryTypeA, TUnifiedType>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, Task<TUnifiedType>> successFunc
    )
    {
        var result = await tryTask;
        return await result.FoldAsync(exceptionFunc, successFunc);
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static Either<Exception, TRightType> ToEither<TRightType>(
        this Try<TRightType> tryT
    )
    {
        return tryT.Fold(
            exception => new Either<Exception, TRightType>(exception),
            right => new Either<Exception, TRightType>(right)
        );
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static Either<TLeftType, TRightTypeA> ToEither<TLeftType, TRightType, TRightTypeA>(
        this Try<TRightType> tryA,
        Func<Exception, TLeftType> left,
        Func<TRightType, TRightTypeA> right)
    {
        return tryA.Fold<Either<TLeftType, TRightTypeA>>(
            exception => left(exception),
            other => right(other)
        );
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Either<Exception, TRightType>> ToEitherAsync<TRightType>(
        this Task<Try<TRightType>> tryTask
    )
    {
        var result = await tryTask;

        return result.ToEither();
    }

    [Obsolete("Try is deprecated, use IO instead.")]
    public static Option<TRightType> ToOption<TRightType>(
        this Try<TRightType> tryT
    )
    {
        return tryT.Fold(
            _ => new None<TRightType>(),
            right => right.ToOption()
        );
    }
    
    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Option<TRightType>> ToOptionAsync<TRightType>(
        this Task<Try<TRightType>> tryTask
    )
    {
        var result = await tryTask;

        return result.ToOption();
    }
    
    [Obsolete("Try is deprecated, use IO instead.")]
    public static Validated<Exception, TRightType> ToValidated<TRightType>(
        this Try<TRightType> tryT
    ) => tryT.Fold<Validated<Exception, TRightType>>(
        exception => new ValidatedFailure<Exception, TRightType>(exception),
        right => new ValidatedSuccess<Exception, TRightType>(right)
    );
    
    [Obsolete("Try is deprecated, use IO instead.")]
    public static async Task<Validated<Exception, TRightType>> ToValidatedAsync<TRightType>(
        this Task<Try<TRightType>> tryTask
    ) => (await tryTask).ToValidated();
}