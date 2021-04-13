using System.Collections.Generic;

namespace TheGame.SharedKernel
{
    public class OperationResult
    {
        private IList<FailureDetail> _failureDetails;

        public bool Succeeded { get => (_failureDetails?.Count ?? 0) == 0; }
        public string Message { get; private set; }
        public string OperationCode { get; private set; }
        public IEnumerable<FailureDetail> FailureDetails { get => _failureDetails; }

        private OperationResult(string message, string operationCode) : this(message, operationCode, default) { }

        private OperationResult(string message, string operationCode, params FailureDetail[] failureDetails)
        {
            Message = message;
            OperationCode = operationCode;
            _failureDetails = failureDetails;
            _failureDetails = new List<FailureDetail>();
        }

        public static OperationResult Successful()
            => new OperationResult(default, default, default);

        public static OperationResult Successful(string message, string operationCode)
            => new OperationResult(message, operationCode);

        public static OperationResult Successful(string message)
            => new OperationResult(message, default);

        public static OperationResult Failure(params FailureDetail[] failureDetails)
            => new OperationResult(default, default, failureDetails);

        public static OperationResult Failure(string message, params FailureDetail[] failureDetails)
            => new OperationResult(message, default, failureDetails);

        public static OperationResult Failure(string message, string operationCode, params FailureDetail[] failureDetails)
            => new OperationResult(message, operationCode, failureDetails);
    }
}
