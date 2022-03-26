using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse> {

        // RESOLVE ERROR:   MediatR IPipelineBehavior<TRequest, TResponse> errors as The type 'TRequest' cannot be used as type parameter 'TRequest' in the generic type or method
        // LINK:            https://stackoverflow.com/questions/70654329/mediatr-ipipelinebehaviortrequest-tresponse-errors-as-the-type-trequest-can

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators ?? throw new ArgumentNullException(nameof(validators));

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            if (_validators.Any()) {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new ValidationException(failures);

            }

            return await next();
        }
    }
}
