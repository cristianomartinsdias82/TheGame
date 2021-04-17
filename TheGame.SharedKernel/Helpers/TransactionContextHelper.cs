using System;
using System.Transactions;

namespace TheGame.SharedKernel.Helpers
{
    public static class TransactionContextHelper
    {
        public static TReturn Execute<TInput, TReturn>(
            Func<TInput, TReturn> action,
            TInput data,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required,
            double fromSecondsTransactionTimeout = 60,
            bool throwOnError = true,
            bool transactionFlowsAcrossThreadContinuationsWithAsyncAwaitPatterns = true)
        {
            TReturn result = default(TReturn);

            try
            {
                using (var transaction = new TransactionScope(scopeOption, TimeSpan.FromSeconds(fromSecondsTransactionTimeout), transactionFlowsAcrossThreadContinuationsWithAsyncAwaitPatterns ? TransactionScopeAsyncFlowOption.Enabled : TransactionScopeAsyncFlowOption.Suppress))
                {
                    result = action(data);

                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                if (throwOnError)
                    throw;
            }

            return result;
        }
    }
}