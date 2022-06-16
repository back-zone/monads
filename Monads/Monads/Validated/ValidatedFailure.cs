using System;

namespace Back.Zone.Monads.Validated;

public sealed class ValidatedFailure<TAFailureType, TASuccessType> : Validated<TAFailureType, TASuccessType>
{
    private readonly TAFailureType _failure;

    public ValidatedFailure(TAFailureType failure)
    {
        _failure = failure;
    }

    public static implicit operator ValidatedFailure<TAFailureType, TASuccessType>(TAFailureType failureType) =>
        new(failureType);
    
    public override TASuccessType GetSuccess()=> throw new Exception("GetSuccess() called on ValidatedFailure!");
    
    public override TAFailureType GetFailure() => _failure;

    public override bool IsSuccess() => false;

    public void Deconstruct(out TAFailureType failure) => failure = _failure;
}