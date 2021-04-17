using System.Collections.Generic;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.SharedKernel
{
    public sealed class OperationResult<T> : OperationResult
    {
        public T Data { get; private set; }

        private OperationResult(T data, string message, string operationCode) : this(data, message, operationCode, default) { }

        private OperationResult(T data, string message, string operationCode, params FailureDetail[] failureDetails) : base(message, operationCode, failureDetails)
        {
            Data = data;
        }

        public static OperationResult<T> Successful(T data)
            => new OperationResult<T>(data, default, default);

        public static OperationResult<T> Successful(T data, string message)
            => new OperationResult<T>(data, message, default);

        public static OperationResult<T> Successful(T data, string message, string operationCode)
            => new OperationResult<T>(data, message, operationCode);

        new public static OperationResult<T> Failure(string message)
            => new OperationResult<T>(default, message, default);

        new public static OperationResult<T> Failure(params FailureDetail[] failureDetails)
            => new OperationResult<T>(default, default, default, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));

        new public static OperationResult<T> Failure(string message, params FailureDetail[] failureDetails)
            => new OperationResult<T>(default, message, default, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));

        new public static OperationResult<T> Failure(string message, string operationCode, params FailureDetail[] failureDetails)
            => new OperationResult<T>(default, message, operationCode, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));
    }

    public class OperationResult
    {
        private IList<FailureDetail> _failureDetails;

        public bool Succeeded { get => (_failureDetails?.Count ?? 0) == 0; }
        public string Message { get; private set; }
        public string OperationCode { get; private set; }
        public IEnumerable<FailureDetail> FailureDetails { get => _failureDetails; }

        protected OperationResult(string message, string operationCode) : this(message, operationCode, default) { }

        protected OperationResult(string message, string operationCode, params FailureDetail[] failureDetails)
        {
            Message = message;
            OperationCode = operationCode;
            _failureDetails = failureDetails;
        }

        public static OperationResult Successful()
            => new OperationResult(default, default, default);

        public static OperationResult Successful(string message, string operationCode)
            => new OperationResult(message, operationCode);

        public static OperationResult Successful(string message)
            => new OperationResult(message, default);

        public static OperationResult Failure(string message)
            => new OperationResult(message, default, default);

        public static OperationResult Failure(params FailureDetail[] failureDetails)
            => new OperationResult(default, default, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));

        public static OperationResult Failure(string message, params FailureDetail[] failureDetails)
            => new OperationResult(message, default, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));

        public static OperationResult Failure(string message, string operationCode, params FailureDetail[] failureDetails)
            => new OperationResult(message, operationCode, failureDetails ?? throw ArgNullEx(nameof(failureDetails)));
    }
}
