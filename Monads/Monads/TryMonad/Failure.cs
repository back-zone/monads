using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad
{
    public sealed class Failure<TA> : Try<TA>
    {
        private readonly Exception _exception;

        public Failure(Exception exception)
        {
            _exception = exception;
        }

        public override Try<TB> FlatMap<TB>(Func<TA, Try<TB>> func) => throw _exception;

        public override TB Fold<TB>(Func<Exception, TB> exceptionFunc, Func<TA, TB> func) => exceptionFunc(_exception);

        public override async Task<TB> FoldAsync<TB>(Func<Exception, TB> exceptionFunc, Func<TA, Task<TB>> func) =>
            await Task.FromResult(exceptionFunc(_exception));

        public override TA Get() => throw _exception;

        public override bool IsFailure() => true;

        public override bool IsSuccess() => false;

        public override Try<TB> Map<TB>(Func<TA, TB> func) => throw _exception;

        public override Try<TB> Transform<TB>(Func<Exception, Try<TB>> exFunc, Func<TA, Try<TB>> func) =>
            exFunc(_exception);

        public TB Recover<TB>(Func<Exception, TB> recFunc) => recFunc(_exception);

        public void Deconstruct(out Exception exception)
        {
            exception = _exception;
        }
    }
}