using System;
using System.Collections.Generic;
using HotChocolate.Language;

namespace HotChocolate.Execution
{
    public class QueryRequest
    {
        public QueryRequest(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("message", nameof(query));
            }
            Query = query;
        }

        public QueryRequest(string query, string operationName)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("message", nameof(query));
            }
            Query = query;
            OperationName = operationName;
        }


        public string Query { get; }

        public string OperationName { get; set; }

        public IReadOnlyDictionary<string, object> VariableValues { get; set; }

        public object InitialValue { get; set; }

        public IServiceProvider Services { get; set; }
    }
}
