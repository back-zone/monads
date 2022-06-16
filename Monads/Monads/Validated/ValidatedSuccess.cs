using System;

namespace Back.Zone.Monads.Validated;

public sealed class ValidatedSuccess<TAFailureType, TASuccessType> : Validated<TAFailureType, TASuccessType>
{
    private readonly TASuccessType _success;
    
    public ValidatedSuccess(TASuccessType success)
    {
        _success = success;
    }

    public override TASuccessType GetSuccess() => _success;

    public static implicit operator ValidatedSuccess<TAFailureType, TASuccessType>(TASuccessType successType) =>
        new(successType);
    
    public override TAFailureType GetFailure() => throw new Exception("GetFailure() called on ValidatedSuccess!");

    public override bool IsSuccess() => true;
    
    public void Deconstruct(out TASuccessType success) => success = _success;
}