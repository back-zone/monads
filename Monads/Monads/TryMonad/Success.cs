using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad
{
    public sealed class Success<TA> : Try<TA>
    {
        private readonly TA _value;

        public Success(TA value)
        {
            _value = value;
        }

        public override Try<TB> FlatMap<TB>(Func<TA, Try<TB>> func) => func(_value);

        public override TB Fold<TB>(Func<Exception, TB> exceptionFunc, Func<TA, TB> func) => func(_value);

        public override async Task<TB> FoldAsync<TB>(Func<Exception, TB> exceptionFunc, Func<TA, Task<TB>> func) =>
            await func(_value);

        public override TA Get() => _value;

        public override bool IsFailure() => false;

        public override bool IsSuccess() => true;

        public override Try<TB> Map<TB>(Func<TA, TB> func) => TryMonad.Try.From(func(_value));

        public override Try<TB> Transform<TB>(Func<Exception, Try<TB>> exFunc, Func<TA, Try<TB>> func) => func(_value);

        public void Deconstruct(out TA value)
        {
            value = _value;
        }
    }
}