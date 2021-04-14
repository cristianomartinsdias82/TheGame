using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TheGame.SharedKernel.Validation
{
    public abstract class DataInputValidator<T> : AbstractValidator<T>, IDataInputValidation<T>
    {
        public async virtual Task<OperationResult> TryValidateAsync(T target, CancellationToken cancellationToken)
        {
            EnsureInstanceNotNull(target);

            var validationResults = await ValidateAsync(target, cancellationToken);

            if (validationResults.IsValid)
                return OperationResult.Successful();

            return OperationResult.Failure(validationResults.Errors.Select(x => new FailureDetail(x.PropertyName, x.ErrorMessage)).ToArray());
        }
    }
}
