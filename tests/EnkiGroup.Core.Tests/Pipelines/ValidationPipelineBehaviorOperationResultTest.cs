using EnkiGroup.Core.RequestHandlers.Pipelines;
using EnkiGroup.Shared;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EnkiGroup.Core.Tests.Pipelines
{
    public class ValidationPipelineBehaviorOperationResultTest
    {
        private readonly AbstractValidator<SampleRequest> _validator;
        private readonly IPipelineBehavior<SampleRequest, OperationResult> _sut;

        public ValidationPipelineBehaviorOperationResultTest()
        {
            _validator = new SampleRequestValidator();
            _sut = new ValidationPipelineBehavior<SampleRequest, OperationResult>(_validator);
        }

        [Fact]
        public async Task WhenModelContainsErrorsShouldReturnOperationResultWithError()
        {
            //Arrange
            var request = new SampleRequest();

            //Act
            var (success, exception) = await _sut.Handle(request, CancellationToken.None, null);

            //Assert
            Assert.False(success);
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task WhenModelIsValidShouldReturnOperationResultSuccess()
        {
            //Arrange
            var request = new SampleRequest { Name = "Not null" };

            static Task<OperationResult> Next() => Task.FromResult(OperationResult.Success());

            //Act
            var (success, exception) = await _sut.Handle(request, CancellationToken.None, Next);

            //Assert
            Assert.True(success);
            Assert.Null(exception);
        }
    }

    public class SampleRequest : IRequest<OperationResult>, IValidatable
    {
        public string Name { get; set; }
    }

    public class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
            => RuleFor(x => x.Name).NotNull();
    }
}
