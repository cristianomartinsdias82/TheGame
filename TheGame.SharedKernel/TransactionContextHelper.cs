using System;
using System.Transactions;

namespace TheGame.SharedKernel
{
    /// <summary>
    /// Classe com abstrações de métodos que executam contexo transacional.
    /// Para estes métodos funcionarem, será necessário ter o serviço Windows "Distributed Transaction Coordinator" em operação e
    /// referenciar o componente System.Transactions do mscorlib
    /// </summary>
    public static class TransactionContextHelper
    {
        /// <summary>
        /// Executa uma determinada ação informada por parâmetro em um contexto transacional.
        /// Após a execução da ação, a transação se completará automaticamente.
        /// Para abortar a transação, faça com que o método informado dispare uma exceção!
        /// Nota: neste overload, o método informado não recebe nenhum parâmetro e não tem retorno
        /// </summary>
        /// <param name="action">Ponteiro para a ação que se deseja executar em um contexto transacional</param>
        /// <param name="scopeOption"></param>
        /// <param name="throwOnError"></param>
        public static void Execute(Action action, TransactionScopeOption scopeOption = TransactionScopeOption.Required, double fromSecondsTransactionTimeout = 60, bool throwOnError = true)
        {
            try
            {
                using (var transaction = new TransactionScope(scopeOption, TimeSpan.FromSeconds(fromSecondsTransactionTimeout)))
                {
                    action();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                var errorMesssage = ex.Message;
                //TODO: Log exc info

                if (throwOnError)
                    throw;
            }
        }

        /// <summary>
        /// Executa uma determinada ação informada por parâmetro em um contexto transacional.
        /// Após a execução da ação, a transação se completará automaticamente.
        /// Para abortar a transação, faça com que o método informado dispare uma exceção!
        /// Nota: neste overload, o método informado recebe um parâmetro TInput e não tem retorno
        /// </summary>
        /// <param name="action">Ponteiro para a ação que se deseja executar em um contexto transacional</param>
        /// <param name="scopeOption"></param>
        /// <param name="throwOnError"></param>
        public static void Execute<TInput>(Action<TInput> action, TInput data, TransactionScopeOption scopeOption = TransactionScopeOption.Required, double fromSecondsTransactionTimeout = 60, bool throwOnError = true)
        {
            try
            {
                using (var transaction = new TransactionScope(scopeOption, TimeSpan.FromSeconds(fromSecondsTransactionTimeout)))
                {
                    action(data);

                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                if (throwOnError)
                    throw;
            }
        }

        /// <summary>
        /// Executa uma determinada ação informada por parâmetro em um contexto transacional.
        /// Após a execução da ação, a transação se completará automaticamente.
        /// Para abortar a transação, faça com que o método informado dispare uma exceção!
        /// Nota: neste overload, o método informado não recebe nenhum parâmetro e tem retorno do tipo TReturn
        /// </summary>
        /// <param name="action">Ponteiro para a ação que se deseja executar em um contexto transacional</param>
        /// <param name="scopeOption"></param>
        /// <param name="throwOnError"></param>
        public static TReturn Execute<TReturn>(Func<TReturn> action, TransactionScopeOption scopeOption = TransactionScopeOption.Required, double fromSecondsTransactionTimeout = 60, bool throwOnError = true)
        {
            TReturn result = default(TReturn);

            try
            {
                using (var transaction = new TransactionScope(scopeOption, TimeSpan.FromSeconds(fromSecondsTransactionTimeout)))
                {
                    result = action();

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

        /// <summary>
        /// Executa uma determinada ação informada por parâmetro em um contexto transacional.
        /// Após a execução da ação, a transação se completará automaticamente.
        /// Para abortar a transação, faça com que o método informado dispare uma exceção!
        /// Nota: neste overload, o método informado recebe um parâmetro do tipo TInput e tem retorno do tipo TReturn
        /// </summary>
        /// <param name="action">Ponteiro para a ação que se deseja executar em um contexto transacional</param>
        /// <param name="scopeOption"></param>
        /// <param name="throwOnError"></param>
        public static TReturn Execute<TInput, TReturn>(Func<TInput, TReturn> action, TInput data, TransactionScopeOption scopeOption = TransactionScopeOption.Required, double fromSecondsTransactionTimeout = 60, bool throwOnError = true)
        {
            TReturn result = default(TReturn);

            try
            {
                using (var transaction = new TransactionScope(scopeOption, TimeSpan.FromSeconds(fromSecondsTransactionTimeout)))
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
