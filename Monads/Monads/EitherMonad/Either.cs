using System;
using System.Threading.Tasks;
using Back.Zone.Monads.IOMonad;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.EitherMonad;

public abstract class Either<TE, TA>
{
    public abstract bool IsLeft();

    public abstract TE LeftValue();

    public abstract TA RightValue();

    public static implicit operator Either<TE, TA>(TE left) => new Left<TE, TA>(left);

    public static implicit operator Either<TE, TA>(TA right) => new Right<TE, TA>(right);

    public static Either<Exception, TA> From(Func<TA> builder) =>
        IO.From(builder).ToEither();

    public static async Task<Either<Exception, TA>> FromAsync(Func<Task<TA>> builderAsync) =>
        await IO.FromAsync(builderAsync).ToEitherAsync();

    public static Either<Exception, TA> Pure(TA pureValue) =>
        IO.Pure(pureValue).ToEither();

    public static async Task<Either<Exception, TA>> PureAsync(Task<TA> pureValueAsync) =>
        await IO.PureAsync(pureValueAsync).ToEitherAsync();

    public Either<TE, TB> Map<TB>(Func<TA, TB> mapper)
    {
        var left = new Left<TE, TB>(LeftValue());
        var right = new Right<TE, TB>(mapper(RightValue()));

        return IsLeft() ? left : right;
    }

    public async Task<Either<TE, TB>> MapAsync<TB>(Func<TA, Task<TB>> asyncMapper)
    {
        var left = new Left<TE, TB>(LeftValue());
        var right = new Right<TE, TB>(await asyncMapper(RightValue()));

        return IsLeft() ? left : right;
    }

    public Either<TE, TB> FlatMap<TB>(Func<TA, Either<TE, TB>> flatMapper)
    {
        var left = new Left<TE, TB>(LeftValue());
        var right = flatMapper(RightValue());

        return IsLeft() ? left : right;
    }

    public async Task<Either<TE, TB>> FlatMapAsync<TB>(Func<TA, Task<Either<TE, TB>>> asyncFlatMapper)
    {
        var left = new Left<TE, TB>(LeftValue());
        var right = await asyncFlatMapper(RightValue());

        return IsLeft() ? left : right;
    }

    public TB Fold<TB>(Func<TE, TB> leftHandler, Func<TA, TB> rightHandler)
    {
        return IsLeft()
            ? leftHandler(LeftValue())
            : rightHandler(RightValue());
    }

    public async Task<TB> FoldAsync<TB>(Func<TE, TB> leftHandler, Func<TA, Task<TB>> rightHandlerAsync)
    {
        return IsLeft()
            ? leftHandler(LeftValue())
            : await rightHandlerAsync(RightValue());
    }

    public async Task<TB> FoldAsync<TB>(Func<TE, Task<TB>> leftHandlerAsync, Func<TA, TB> rightHandler)
    {
        return IsLeft()
            ? await leftHandlerAsync(LeftValue())
            : rightHandler(RightValue());
    }

    public async Task<TB> FoldAsync<TB>(Func<TE, Task<TB>> leftHandlerAsync, Func<TA, Task<TB>> rightHandlerAsync)
    {
        return IsLeft()
            ? await leftHandlerAsync(LeftValue())
            : await rightHandlerAsync(RightValue());
    }

    public TA CheckError(Func<TE, TA> leftHandler)
    {
        return IsLeft()
            ? leftHandler(LeftValue())
            : RightValue();
    }
    
    public async Task<TA> CheckErrorAsync(Func<TE, Task<TA>> leftHandlerAsync)
    {
        return IsLeft()
            ? await leftHandlerAsync(LeftValue())
            : RightValue();
    }

    public Either<TEE, TA> MapError<TEE>(Func<TE, TEE> leftHandler)
    {
        return IsLeft()
            ? leftHandler(LeftValue())
            : new Right<TEE, TA>(RightValue());
    }

    public async Task<Either<TEE, TA>> MapErrorAsync<TEE>(Func<TE, Task<TEE>> leftHandlerAsync)
    {
        return IsLeft()
            ? await leftHandlerAsync(LeftValue())
            : new Right<TEE, TA>(RightValue());
    }

    public async Task<Either<TEE, TA>> MapErrorAsync<TEE>(Func<TE, TEE> leftHandlerAsync)
    {
        return IsLeft()
            ? await Task.FromResult(leftHandlerAsync(LeftValue()))
            : new Right<TEE, TA>(RightValue());
    }

    public Either<TA, TE> Swap()
    {
        return IsLeft()
            ? new Left<TA, TE>(RightValue())
            : new Right<TA, TE>(LeftValue());
    }

    public async Task<Either<TA, TE>> SwapAsync()
    {
        return await Task.FromResult(Swap());
    }

    public TA GetOrElse(
        TA elseRightValue
    )
    {
        return IsLeft()
            ? elseRightValue
            : RightValue();
    }

    public async Task<TA> GetOrElseAsync(
        Task<TA> elseRightValueAsync
    )
    {
        return IsLeft()
            ? await elseRightValueAsync
            : RightValue();
    }

    public Option<TA> ToOption()
    {
        return IsLeft()
            ? new None<TA>()
            : new Some<TA>(RightValue());
    }

    public IO<TA> ToIO()
    {
        return IsLeft()
            ? new Failure<TA>(new Exception("Left"))
            : new Success<TA>(RightValue());
    }
}