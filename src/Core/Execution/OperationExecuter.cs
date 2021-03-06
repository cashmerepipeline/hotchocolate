﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Language;

namespace HotChocolate.Execution
{
    internal class OperationExecuter
    {
        private static readonly Dictionary<OperationType, IExecutionStrategy> _executionStrategy =
            new Dictionary<OperationType, IExecutionStrategy>
            {
                { OperationType.Query, new QueryExecutionStrategy() },
                { OperationType.Mutation, new MutationExecutionStrategy() },
                { OperationType.Subscription, new SubscriptionExecutionStrategy() }
            };

        private readonly ISchema _schema;
        private readonly DocumentNode _queryDocument;
        private readonly OperationDefinitionNode _operation;
        private readonly DirectiveLookup _directiveLookup;
        private readonly TimeSpan _executionTimeout;
        private readonly VariableValueBuilder _variableValueBuilder;
        private readonly IExecutionStrategy _strategy;

        public OperationExecuter(
            ISchema schema,
            DocumentNode queryDocument,
            OperationDefinitionNode operation)
        {
            _schema = schema
                ?? throw new ArgumentNullException(nameof(schema));
            _queryDocument = queryDocument
                ?? throw new ArgumentNullException(nameof(queryDocument));
            _operation = operation
                ?? throw new ArgumentNullException(nameof(operation));

            _executionTimeout = schema.Options.ExecutionTimeout;

            _variableValueBuilder = new VariableValueBuilder(schema, _operation);

            if (!_executionStrategy.TryGetValue(_operation.Operation,
                out IExecutionStrategy strategy))
            {
                throw new NotSupportedException();
            }
            _strategy = strategy;

            var directiveCollector = new DirectiveCollector(_schema);
            directiveCollector.VisitDocument(_queryDocument);
            _directiveLookup = directiveCollector.CreateLookup();
        }

        public async Task<IExecutionResult> ExecuteAsync(
            OperationRequest request,
            CancellationToken cancellationToken)
        {
            var requestTimeoutCts =
                new CancellationTokenSource(_executionTimeout);

            var combinedCts =
                CancellationTokenSource.CreateLinkedTokenSource(
                    requestTimeoutCts.Token, cancellationToken);

            IExecutionContext executionContext =
                CreateExecutionContext(
                    request,
                    cancellationToken);

            try
            {
                return await _strategy.ExecuteAsync(
                    executionContext, combinedCts.Token);
            }
            finally
            {
                executionContext.Dispose();
                combinedCts.Dispose();
                requestTimeoutCts.Dispose();
            }
        }

        private IExecutionContext CreateExecutionContext(
            OperationRequest request,
            CancellationToken cancellationToken)
        {
            VariableCollection variables = _variableValueBuilder
                .CreateValues(request.VariableValues);

            var executionContext = new ExecutionContext(
                _schema, _directiveLookup, _queryDocument,
                _operation, request, variables,
                cancellationToken);

            return executionContext;
        }
    }
}
