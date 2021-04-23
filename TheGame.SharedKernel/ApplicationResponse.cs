using System;
using System.Linq;
using System.Reflection;

namespace TheGame.SharedKernel
{
    public abstract class ApplicationResponse
    {
        internal OperationResult Result { get; set; }

        public virtual OperationResult GetResult() => Result;
    }

    public abstract class ApplicationResponse<T> : ApplicationResponse
    {
        new internal OperationResult<T> Result { get; set; }

        new public OperationResult<T> GetResult()
        {
            foreach (var resultProperty in GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.Name == "Result"))
            {
                var result = resultProperty.GetValue(this);
                if (result != null)
                {
                    var innerResult = (OperationResult)result;

                    return innerResult.Succeeded ?
                        OperationResult<T>.Successful(((OperationResult<T>)result).Data) :
                        OperationResult<T>.Failure(innerResult.Message, innerResult.OperationCode, innerResult.FailureDetails.ToArray());
                }
            }

            throw new InvalidProgramException($"Error when trying to provide {nameof(OperationResult)}.{nameof(OperationResult<T>.Data)} property with a value");
        }
    }
}
