using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.EitherMonad
{
    public readonly struct Either<TL, TR>
    {
        private readonly TL? _left;

        private readonly TR? _right;

        private readonly bool _isLeft;

        internal Either(TL left)
        {
            _isLeft = true;
            _left = left;
            _right = default;
        }

        internal Either(TR right)
        {
            _isLeft = false;
            _left = default;
            _right = right;
        }

        public static implicit operator Either<TL, TR>(TL left)
        {
            return new Either<TL, TR>(left);
        }

        public static implicit operator Either<TL, TR>(TR right)
        {
            return new Either<TL, TR>(right);
        }

        public Either<TAL, TBR> Map<TAL, TBR>(Func<TL, TAL> left, Func<TR, TBR> right) => _isLeft ? left(_left!) : right(_right!);
        
        public TB Fold<TB>(Func<TL, TB> left, Func<TR, TB> right) => _isLeft ? left(_left!) : right(_right!);

        public async Task<TB> FoldAsync<TB>(Func<TL, Task<TB>> left, Func<TR, Task<TB>> right) =>
            _isLeft ? await left(_left!) : await right(_right!);

        public async Task<TB> FoldAsync<TB>(Func<TL, TB> left, Func<TR, Task<TB>> right) =>
            _isLeft ? left(_left!) : await right(_right!);

        public async Task<TB> FoldAsync<TB>(Func<TL, Task<TB>> left, Func<TR, TB> right) =>
            _isLeft ? await left(_left!) : right(_right!);
    }
}