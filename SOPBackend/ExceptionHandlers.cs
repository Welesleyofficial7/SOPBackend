using SOPContracts.Exceptions;
using UnauthorizedAccessException = SOPContracts.Exceptions.UnauthorizedAccessException;

namespace SOPBackend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionHandlers : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidArgumentException ex1)
        {
            context.Result = new BadRequestObjectResult(new { Status = "error", Message = ex1.Message });
        }
        else if (context.Exception is NotFoundException ex2)
        {
            context.Result = new NotFoundObjectResult(new { Status = "error", Message = ex2.Message });
        }
        else if (context.Exception is UnauthorizedAccessException ex3)
        {
            context.Result = new UnauthorizedObjectResult(new { Status = "error", Message = ex3.Message });
        }
        else if (context.Exception is ConflictException ex4)
        {
            context.Result = new ConflictObjectResult(new { Status = "error", Message = ex4.Message });
        }
        else
        {
            context.Result = new ObjectResult(new { Status = "error", Message = "unexpected error occurred." })
            {
                StatusCode = 500
            };
        }
    }
}
