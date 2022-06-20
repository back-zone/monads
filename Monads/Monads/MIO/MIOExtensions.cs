using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.OptionMonad;
using Back.Zone.Monads.Validated;

namespace Back.Zone.Monads.MIO;

public static class MIOExtensions
{
    public static MIO<TB> Map<TA, TB>(this MIO<TA> mio, Func<TA, TB> f) =>
        mio.Map(f);

    public static MIO<TB> Flatmap<TA, TB>(this MIO<TA> mio, Func<TA, MIO<TB>> f) =>
        mio.Flatmap(f);

    public static async Task<MIO<TB>> MapAsync<TA, TB>(this Task<MIO<TA>> mio, Func<TA, Task<TB>> f)
    {
        var result = await mio;

        return await result.MapAsync(f);
    }

    public static async Task<MIO<TB>> FlatmapAsync<TA, TB>(this Task<MIO<TA>> mio, Func<TA, Task<MIO<TB>>> f)
    {
        var result = await mio;

        return await result.FlatmapAsync(f);
    }

    public static async Task<TA> MapError<TA>(
        this Task<MIO<TA>> mio,
        Func<Exception, Task<TA>> f)
    {
        var result = await mio;
        if (result.HasFailed)
        {
            return await f(result.Exception!);
        }

        return result.Value!;
    }

    public static async Task<TA> MapErrorAsync<TA>(
        this Task<MIO<TA>> mio,
        Func<Exception, TA> f) => (await mio).MapError(f);

    public static Option<TA> ToOption<TA>(this MIO<TA> mio) =>
        mio.HasFailed ? new None<TA>() : new Some<TA>(mio.Value!);

    public static async Task<Option<TA>> ToOptionAsync<TA>(this Task<MIO<TA>> mio) =>
        (await mio).ToOption();

    public static Either<Exception, TA> ToEither<TA>(this MIO<TA> mio) =>
        mio.HasFailed ? new Either<Exception, TA>(mio.Exception!) : new Either<Exception, TA>(mio.Value!);

    public static async Task<Either<Exception, TA>> ToEitherAsync<TA>(this Task<MIO<TA>> mio) =>
        (await mio).ToEither();

    public static Validated<Exception, TA> ToValidated<TA>(this MIO<TA> mio) =>
        mio.HasFailed
            ? new ValidatedFailure<Exception, TA>(mio.Exception!)
            : new ValidatedSuccess<Exception, TA>(mio.Value!);

    public static async Task<Validated<Exception, TA>> ToValidatedAsync<TA>(this Task<MIO<TA>> mio) =>
        (await mio).ToValidated();
}