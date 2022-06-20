using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.MIO;

public readonly struct MIO<TA>
{
    public readonly Exception? Exception;

    public readonly TA? Value;

    public readonly bool HasFailed;

    internal MIO(Exception exception)
    {
        Exception = exception;
        HasFailed = true;
        Value = default;
    }

    internal MIO(TA? value)
    {
        if (value is not null)
        {
            Exception = default;
            HasFailed = false;
            Value = value;
        }
        else
        {
            Exception = new NullReferenceException($"{nameof(value)} is null");
            HasFailed = true;
            Value = default;
        }
    }

    public static MIO<TA> From(TA? value)
    {
        try
        {
            return new MIO<TA>(value);
        }
        catch (Exception e)
        {
            return new MIO<TA>(e);
        }
    }

    public static MIO<TA> From(Func<TA?> f)
    {
        try
        {
            return new MIO<TA>(f());
        }
        catch (Exception e)
        {
            return new MIO<TA>(e);
        }
    }

    public static async Task<MIO<TA>> FromAsync(Task<TA?> f)
    {
        try
        {
            return new MIO<TA>(await f);
        }
        catch (Exception e)
        {
            return new MIO<TA>(e);
        }
    }

    public static async Task<MIO<TA>> FromAsync(Func<Task<TA?>> f)
    {
        try
        {
            return new MIO<TA>(await f());
        }
        catch (Exception e)
        {
            return new MIO<TA>(e);
        }
    }

    public MIO<TB> Map<TB>(Func<TA, TB> f) =>
        HasFailed ? new MIO<TB>(Exception!) : new MIO<TB>(f(Value!));

    public async Task<MIO<TB>> MapAsync<TB>(Func<TA, Task<TB>> f) =>
        HasFailed ? new MIO<TB>(Exception!) : new MIO<TB>(await f(Value!));

    public MIO<TB> Flatmap<TB>(Func<TA, MIO<TB>> f) =>
        HasFailed ? new MIO<TB>(Exception!) : f(Value!);

    public async Task<MIO<TB>> FlatmapAsync<TB>(Func<TA, Task<MIO<TB>>> f) =>
        HasFailed ? new MIO<TB>(Exception!) : await f(Value!);
    
    public TA MapError(Func<Exception, TA> f) =>
        HasFailed ? f(Exception!) : Value!;
    
    public async Task<TA> MapErrorAsync(Func<Exception, Task<TA>> f) =>
        HasFailed ? await f(Exception!) : Value!;
}