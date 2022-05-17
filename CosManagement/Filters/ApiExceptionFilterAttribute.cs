using CosManagement.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CosManagement.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
	private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

	public ApiExceptionFilterAttribute()
	{
		_exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
			{
				{ typeof(LoginFailException), HandleLoginFailException },
				{ typeof(ValidationException), HandleValidationException },
				{ typeof(NotFoundException), HandleNotFoundException },
				{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
				{ typeof(ForbiddenAccessException), HandleForbiddenAccessException },
				{ typeof(UserExitstsException), HandleUserExistsException },
			};
	}

	public override void OnException(ExceptionContext context)
	{
		HandleException(context);

		base.OnException(context);
	}

	private void HandleException(ExceptionContext context)
	{
		Type type = context.Exception.GetType();
		if (_exceptionHandlers.ContainsKey(type))
		{
			_exceptionHandlers[type].Invoke(context);
			return;
		}

		if (!context.ModelState.IsValid)
		{
			HandleInvalidModelStateException(context);
			return;
		}

		HandleUnknownException(context);
	}

	private void HandleValidationException(ExceptionContext context)
	{
		var exception = (ValidationException)context.Exception;

		var details = new ValidationProblemDetails(exception.Errors!);

		context.Result = new BadRequestObjectResult(details);

		context.ExceptionHandled = true;
	}

	private void HandleUserExistsException(ExceptionContext context)
	{
		var details = new ValidationProblemDetails()
		{
			Title = "User with specified email already exists",
		};

		context.Result = new BadRequestObjectResult(details);

		context.ExceptionHandled = true;
	}

	private void HandleInvalidModelStateException(ExceptionContext context)
	{
		var details = new ValidationProblemDetails(context.ModelState);

		context.Result = new BadRequestObjectResult(details);

		context.ExceptionHandled = true;
	}

	private void HandleNotFoundException(ExceptionContext context)
	{
		var exception = (NotFoundException)context.Exception;
		var message = exception.Message;

		var details = new ProblemDetails()
		{
			Title = "The specified resource was not found.",
		};

		if (message.StartsWith(NotFoundException.MessagePrefix))
		{
			details.Detail = message;
		}

		context.Result = new NotFoundObjectResult(details);

		context.ExceptionHandled = true;
	}

	private void HandleUnauthorizedAccessException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status401Unauthorized,
			Title = "You cannot access this resource",
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status401Unauthorized
		};

		context.ExceptionHandled = true;
	}

	private void HandleLoginFailException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status401Unauthorized,
			Title = "You have entered an invalid email or password",
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status401Unauthorized
		};

		context.ExceptionHandled = true;
	}

	private void HandleForbiddenAccessException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status403Forbidden,
			Title = "Forbidden access",
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status403Forbidden
		};

		context.ExceptionHandled = true;
	}

	private void HandleUnknownException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An error occurred while processing your request.",
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status500InternalServerError
		};

		context.ExceptionHandled = true;
	}
}