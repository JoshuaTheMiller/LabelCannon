using System;
using System.Threading.Tasks;

namespace Service.LabelQuery
{
    public interface IQueryExecutor<TQuery, TOut>
    {
        Task<TOut> ExecuteQuery(string hostname, TQuery query, Action<int, int, long> progressReporter);
    }
}