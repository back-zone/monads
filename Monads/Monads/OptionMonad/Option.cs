using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.OptionMonad
{
    public abstract class Option<TA>
    {
        public abstract bool IsEmpty();

        public abstract TA Get();

        public bool IsDefined() => !IsEmpty();

        public TA GetOrElse(Func<TA> elseFunc) => IsEmpty() ? elseFunc() : Get();

        public TA GetOrElse(TA elseVal) => IsEmpty() ? elseVal : Get();

        public Option<TB> Map<TB>(Func<TA, TB> func) =>
            IsEmpty() ? new None<TB>() : new Some<TB>(func(Get()));

        public TB Fold<TB>(TB ifEmptyValue, Func<TA, TB> func) => IsEmpty() ? ifEmptyValue : func(Get());

        public TB Fold<TB>(Func<TB> ifEmpty, Func<TA, TB> func) => IsEmpty() ? ifEmpty() : func(Get());

        public async Task<TB> FoldAsync<TB>(TB ifEmptyValue, Func<TA, Task<TB>> func) =>
            IsEmpty() ? ifEmptyValue : await func(Get());

        public async Task<TB> FoldAsync<TB>(Func<TB> ifEmpty, Func<TA, Task<TB>> func) =>
            IsEmpty() ? ifEmpty() : await func(Get());

        public Option<TB> FlatMap<TB>(Func<TA, Option<TB>> func) => IsEmpty() ? new None<TB>() : func(Get());

        public Option<TA> Filter(Func<TA, bool> predicate) => IsEmpty() || predicate(Get()) ? this : new None<TA>();

        public Option<TA> FilterNot(Func<TA, bool> predicate) => IsEmpty() || !predicate(Get()) ? this : new None<TA>();
    }

    public static class Option
    {
        public static Option<TA> From<TA>(TA? value) => value is not null ? new Some<TA>(value) : new None<TA>();

        public static Option<TA> From<TA>(Func<TA?> func)
        {
            try
            {
                return From(func());
            }
            catch (Exception)
            {
                return new None<TA>();
            }
        }

        public static async Task<Option<TA>> FromAsync<TA>(Func<Task<TA?>> func)
        {
            try
            {
                return From(await func());
            }
            catch (Exception)
            {
                return new None<TA>();
            }
        }

        public static async Task<Option<TA>> FromAsync<TA>(Task<TA> value)
        {
            try
            {
                return From(await value);
            }
            catch (Exception)
            {
                return new None<TA>();
            }
        }
    }
}