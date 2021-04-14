using System.Threading;
using System.Threading.Tasks;

namespace TheGame.SharedKernel.Validation
{
    public interface IDataInputValidation<T>
    {
        Task<OperationResult> TryValidateAsync(T target, CancellationToken cancellationToken);
    }
}