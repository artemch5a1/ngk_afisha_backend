using EventService.Domain.Enums;
using EventService.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Extensions;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new ObjectResult(result.Value) { StatusCode = (int)ApiErrorType.Ok };

        return new ObjectResult(result.ErrorMessages)
        {
            StatusCode = (int)result.ErrorType
        };
    }
    
    public static ActionResult<TNewModel> ToActionResult<TNewModel, T>(this Result<T> result, Func<T, TNewModel> mapper)
    {
        if (result.IsSuccess)
        {
            if (result.Value is null)
                return new ObjectResult(result.Value) { StatusCode = 204 };
            
            TNewModel newModel = mapper(result.Value);
            
            return new ObjectResult(newModel) { StatusCode = 200 };
        }
        return new ObjectResult(result.ErrorMessages)
        {
            StatusCode = (int)result.ErrorType
        };
    }
}