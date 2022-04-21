using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.TryMonad;

public static class Try
{
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

    public static async Task<Try<TTryTypeB>> MapAsync<TTryTypeA, TTryTypeB>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<TTryTypeA, Task<TTryTypeB>> func
    )
    {
        var result = await tryTask;
        return await result.MapAsync(func);
    }

    public static async Task<Try<TTryTypeB>> FlatmapAsync<TTryTypeA, TTryTypeB>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<TTryTypeA, Task<Try<TTryTypeB>>> func
    )
    {
        var result = await tryTask;
        return await result.FlatmapAsync(func);
    }

    public static async Task<TUnifiedType> FoldAsync<TTryTypeA, TUnifiedType>(
        this Task<Try<TTryTypeA>> tryTask,
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, Task<TUnifiedType>> successFunc
    )
    {
        var result = await tryTask;
        return await result.FoldAsync(exceptionFunc, successFunc);
    }

    public static Either<Exception, TRightType> ToEither<TRightType>(
        this Try<TRightType> tryT
    )
    {
        return
            tryT
                .Fold(
                    exception => new Either<Exception, TRightType>(exception),
                    right => new Either<Exception, TRightType>(right)
                );
    }

    public static async Task<Either<Exception, TRightType>> ToEitherAsync<TRightType>(
        this Task<Try<TRightType>> tryTask
    )
    {
        var result = await tryTask;

        return result.ToEither();
    }

    public static Option<TRightType> ToOption<TRightType>(
        this Try<TRightType> tryT
    )
    {
        return
            tryT
                .Fold<Option<TRightType>>(
                    _ => new None<TRightType>(),
                    right => right != null
                        ? new Some<TRightType>(right)
                        : new None<TRightType>()
                );
    }

    public static async Task<Option<TRightType>> ToOptionAsync<TRightType>(
        this Task<Try<TRightType>> tryTask
    )
    {
        var result = await tryTask;

        return result.ToOption();
    }
}