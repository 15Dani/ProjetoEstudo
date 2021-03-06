﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EnkiGroup.Shared;
using EnkiGroup.Shared.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EnkiGroup.Core.RequestHandlers.Pipelines
{
    public sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IValidatable
    {
        private readonly AbstractValidator<TRequest> _validator;
        private readonly MethodInfo _operationResultError;
        private readonly Type _type = typeof(TResponse);
        private readonly Type _typeOperationResultGeneric = typeof(OperationResult<>);
        private readonly Type _typeOperationResult = typeof(OperationResult);

        public ValidationPipelineBehavior(AbstractValidator<TRequest> validator)
        {
            _validator = validator;
            if (_type.IsGenericType)
            {
                _operationResultError = _typeOperationResult.GetMethods().FirstOrDefault(m => m.Name == "Error" && m.IsGenericMethod);
                _operationResultError = _operationResultError.MakeGenericMethod(_type.GetGenericArguments().First());
            }
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            ValidationResult validationResult;
            ModeloInvalidoException validationError;
            if (_type == _typeOperationResult)
            {
                validationResult = _validator.Validate(request);
                if (validationResult.IsValid)
                    return next?.Invoke();

                validationError = new ModeloInvalidoException(validationResult.Errors.GroupBy(v => v.PropertyName, v => v.ErrorMessage).ToDictionary(v => v.Key, v => v.Select(y => y)));
                return Task.FromResult((TResponse)Convert.ChangeType(OperationResult.Error(validationError), _type));
            }

            if (!_type.IsGenericType || _type.GetGenericTypeDefinition() != _typeOperationResultGeneric)
                return next?.Invoke();

            validationResult = _validator.Validate(request);
            if (validationResult.IsValid)
                return next?.Invoke();

            validationError = new ModeloInvalidoException(validationResult.Errors.GroupBy(v => v.PropertyName, v => v.ErrorMessage).ToDictionary(v => v.Key, v => v.Select(y => y)));
            return Task.FromResult((TResponse)Convert.ChangeType(_operationResultError.Invoke(null, new object[] { validationError }), _type));
        }
    }
}
