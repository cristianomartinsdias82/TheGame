using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.SharedKernel;

namespace TheGame.Common.Behaviors
{
    public class ApplicationValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ApplicationResponse, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ApplicationValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Any())
                {
                    var failedResponse = Activator.CreateInstance<TResponse>();
                    failedResponse.Result = OperationResult.Failure(failures.Select(f => new FailureDetail(f.PropertyName, f.ErrorMessage)).ToArray());
                    
                    return failedResponse;
                }
            }

            return await next();
        }
    }
}
