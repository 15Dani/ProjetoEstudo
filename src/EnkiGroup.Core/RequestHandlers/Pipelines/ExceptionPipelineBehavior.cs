using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EnkiGroup.Shared;
using MediatR;

namespace EnkiGroup.Core.RequestHandlers.Pipelines
{
    public sealed class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly MethodInfo _operationResultError;
        private readonly Type _type = typeof(TResponse);
        private readonly Type _typeOperationResult = typeof(OperationResult);

        public ExceptionPipelineBehavior()
        {
            if (_type.IsGenericType)
            {
                _operationResultError = _typeOperationResult.GetMethods().FirstOrDefault(m => m.Name == "Error" && m.IsGenericMethod);
                _operationResultError = _operationResultError.MakeGenericMethod(_type.GetGenericArguments().First());
            }
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next?.Invoke();
            }
            catch (Exception e)
            {
                return _type == _typeOperationResult
                    ? (TResponse)Convert.ChangeType(OperationResult.Error(e), _type)
                    : (TResponse)Convert.ChangeType(_operationResultError.Invoke(null, new object[] { e }), _type);
            }
        }
    }
}
