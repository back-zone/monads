using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.Validated;

public abstract class Validated<TAFailureType, TASuccessType>
{
    public abstract TASuccessType GetSuccess();

    public abstract TAFailureType GetFailure();

    public abstract bool IsSuccess();

    public bool IsFailure() => !IsSuccess();

    public static implicit operator Validated<TAFailureType, TASuccessType>(TAFailureType failureType) =>
        new ValidatedFailure<TAFailureType, TASuccessType>(failureType);

    public static implicit operator Validated<TAFailureType, TASuccessType>(TASuccessType successType) =>
        new ValidatedSuccess<TAFailureType, TASuccessType>(successType);

    public Validated<TAFailureType, TBSuccessType> Map<TBSuccessType>(
        Func<TASuccessType, TBSuccessType> func
    ) => IsSuccess()
        ? new ValidatedSuccess<TAFailureType, TBSuccessType>(func(GetSuccess()))
        : new ValidatedFailure<TAFailureType, TBSuccessType>(GetFailure());

    public async Task<Validated<TAFailureType, TBSuccessType>> MapAsync<TBSuccessType>(
        Func<TASuccessType, Task<TBSuccessType>> func
    ) => IsSuccess()
        ? new ValidatedSuccess<TAFailureType, TBSuccessType>(await func(GetSuccess()))
        : new ValidatedFailure<TAFailureType, TBSuccessType>(GetFailure());

    public Validated<TAFailureType, TBSuccessType> Flatmap<TBSuccessType>(
        Func<TASuccessType, Validated<TAFailureType, TBSuccessType>> func
    ) => IsSuccess()
        ? func(GetSuccess())
        : new ValidatedFailure<TAFailureType, TBSuccessType>(GetFailure());

    public async Task<Validated<TAFailureType, TBSuccessType>> FlatmapAsync<TBSuccessType>(
        Func<TASuccessType, Task<Validated<TAFailureType, TBSuccessType>>> func
    ) => IsSuccess()
        ? await func(GetSuccess())
        : new ValidatedFailure<TAFailureType, TBSuccessType>(GetFailure());

    public TBSuccessType Fold<TBSuccessType>(
        Func<TAFailureType, TBSuccessType> failureFunc,
        Func<TASuccessType, TBSuccessType> successFunc
    ) => IsSuccess()
        ? successFunc(GetSuccess())
        : failureFunc(GetFailure());

    public async Task<TBSuccessType> FoldAsync<TBSuccessType>(
        Func<TAFailureType, Task<TBSuccessType>> failureFunc,
        Func<TASuccessType, TBSuccessType> successFunc
    ) => IsSuccess()
        ? successFunc(GetSuccess())
        : await failureFunc(GetFailure());
    
    public async Task<TBSuccessType> FoldAsync<TBSuccessType>(
        Func<TAFailureType, TBSuccessType> failureFunc,
        Func<TASuccessType, Task<TBSuccessType>> successFunc
    ) => IsSuccess()
        ? await successFunc(GetSuccess())
        : failureFunc(GetFailure());
    
    public async Task<TBSuccessType> FoldAsync<TBSuccessType>(
        Func<TAFailureType, Task<TBSuccessType>> failureFunc,
        Func<TASuccessType, Task<TBSuccessType>> successFunc
    ) => IsSuccess()
        ? await successFunc(GetSuccess())
        : await failureFunc(GetFailure());
}