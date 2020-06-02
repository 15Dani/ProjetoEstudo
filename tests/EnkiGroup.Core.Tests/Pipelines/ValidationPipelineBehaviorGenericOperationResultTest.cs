using EnkiGroup.Core.RequestHandlers.Pipelines;
using EnkiGroup.Shared;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EnkiGroup.Core.Tests.Pipelines
{
    public class ValidationPipelineBehaviorGenericOperationResultTest
    {
        private readonly AbstractValidator<SampleRequestGeneric> _validator;
        private readonly IPipelineBehavior<SampleRequestGeneric, OperationResult<long>> _sut;

        public ValidationPipelineBehaviorGenericOperationResultTest()
        {
            _validator = new SampleRequestValidatorGeneric();
            _sut = new ValidationPipelineBehavior<SampleRequestGeneric, OperationResult<long>>(_validator);
        }

        [Fact]
        public async Task WhenModelContainsErrorsShouldReturnOperationResultWithError()
        {
            //Arrange
            var request = new SampleRequestGeneric();

            //Act
            var (success, result, exception) = await _sut.Handle(request, CancellationToken.None, null);

            //Assert
            Assert.False(success);
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task WhenModelIsValidShouldReturnOperationResultSuccess()
        {
            //Arrange
            var request = new SampleRequestGeneric { Name = "Not null" };

            static Task<OperationResult<long>> Next() => Task.FromResult(OperationResult.Success(1L));

            //Act
            var (success, result, exception) = await _sut.Handle(request, CancellationToken.None, Next);

            //Assert
            Assert.True(success);
            Assert.Null(exception);
            Assert.NotEqual(0, result);
        }
    }

    public class SampleRequestGeneric : IRequest<OperationResult<long>>, IValidatable
    {
        public string Name { get; set; }
    }

    public class SampleRequestValidatorGeneric : AbstractValidator<SampleRequestGeneric>
    {
        public SampleRequestValidatorGeneric()
            => RuleFor(x => x.Name).NotNull();
    }
}
