using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad
{
    public abstract class Try<TA>
    {
        public abstract bool IsFailure();

        public abstract bool IsSuccess();

        public abstract TA Get();

        public abstract Try<TB> Map<TB>(Func<TA, TB> func);

        public abstract TB Fold<TB>(Func<Exception, TB> exceptionFunc, Func<TA, TB> func);

        public abstract Task<TB> FoldAsync<TB>(Func<Exception, TB> exceptionFunc, Func<TA, Task<TB>> func);

        public abstract Try<TB> FlatMap<TB>(Func<TA, Try<TB>> func);

        public abstract Try<TB> Transform<TB>(Func<Exception, Try<TB>> exFunc, Func<TA, Try<TB>> func);
    }

    public static class Try
    {
        public static Try<TA> From<TA>(Func<TA> func)
        {
            try
            {
                return new Success<TA>(func());
            }
            catch (Exception e)
            {
                return new Failure<TA>(e);
            }
        }

        public static Try<TA> From<TA>(TA value)
        {
            try
            {
                return new Success<TA>(value);
            }
            catch (Exception e)
            {
                return new Failure<TA>(e);
            }
        }

        public static async Task<Try<TA>> FromAsync<TA>(
            Func<Task<TA>> valueFunc
        )
        {
            try
            {
                return new Success<TA>(await valueFunc());
            }
            catch (Exception e)
            {
                return new Failure<TA>(e);
            }
        }

        public static async Task<Try<TA>> FromAsync<TA>(
            Task<TA> value
        )
        {
            try
            {
                return new Success<TA>(await value);
            }
            catch (Exception e)
            {
                return new Failure<TA>(e);
            }
        }

        public static async Task<TB> FoldAsync<TB, TA>(
            this Task<Try<TA>> tryTa,
            Func<Exception, TB> exceptionFunc, Func<TA, TB> func) => (await tryTa).Fold(exceptionFunc, func);

        public static async Task<TB> FoldAsync<TB, TA>(
            this Task<Try<TA>> tryTa,
            Func<Exception, TB> exceptionFunc, Func<TA, Task<TB>> func) =>
            await (await tryTa).FoldAsync(exceptionFunc, func);

        public static TA GetOrElse<TA>(
            this Try<TA> tryTa,
            TA elseValue
        ) => tryTa switch
        {
            Success<TA>(var value) => value,
            _ => elseValue
        };

        public static TA GetOrElse<TA>(
            this Try<TA> tryTa,
            Func<TA> elseFunc
        ) => tryTa switch
        {
            Success<TA>(var value) => value,
            _ => elseFunc()
        };

        public static async Task<TA> GetOrElse<TA>(
            this Task<Try<TA>> tryTaTask,
            TA elseValue
        ) => await tryTaTask switch
        {
            Success<TA>(var value) => value,
            _ => elseValue
        };

        public static async Task<TA> GetOrElse<TA>(
            this Task<Try<TA>> tryTaTask,
            Func<TA> elseFunc
        ) => await tryTaTask switch
        {
            Success<TA>(var value) => value,
            _ => elseFunc()
        };

        public static async Task<TA> GetOrElse<TA>(
            this Task<Try<TA>> tryTaTask,
            Task<TA> elseValue
        ) => await tryTaTask switch
        {
            Success<TA>(var value) => value,
            _ => await elseValue
        };

        public static async Task<TA> GetOrElse<TA>(
            this Task<Try<TA>> tryTaTask,
            Func<Task<TA>> elseFunc
        ) => await tryTaTask switch
        {
            Success<TA>(var value) => value,
            _ => await elseFunc()
        };
    }
}