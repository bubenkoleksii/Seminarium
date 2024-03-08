﻿using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Errors;

public static class ErrorActionResultHandler
{
    public static IActionResult Handle(Error error) =>
        error.Type switch
        {
            "validation" => new BadRequestObjectResult(error),
            "already_exists" => new ObjectResult(error) { StatusCode = 409 },
            "not_found" => new NotFoundObjectResult(error),
            _ => new ObjectResult(error) { StatusCode = 500 }
        };
}
