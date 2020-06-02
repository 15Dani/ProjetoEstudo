using EnkiGroup.Core.RequestHandlers.Pipelines;
using EnkiGroup.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EnkiGroup.Core.Tests.Pipelines
{
    public class ExceptionPipelineBehaviorTest
    {
        private readonly ExceptionPipelineBehavior<SampleRequestGeneric, OperationResult<long>> _sut;

        public ExceptionPipelineBehaviorTest()
            => _sut = new ExceptionPipelineBehavior<SampleRequestGeneric, OperationResult<long>>();

        [Fact]
        public async Task WhenHandlerThrowsAnExceptionPipelineConvertToOperationResultWithError()
        {
            //Arrange
            static Task<OperationResult<long>> Next() => throw new Exception("Fail");

            //Act
            var result = await _sut.Handle(null, CancellationToken.None, Next);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task WhenHandlerDoesNotThrowsAnExceptionPipelineDoesNotChangeResult()
        {
            //Arrange
            static Task<OperationResult<long>> Next() => Task.FromResult(OperationResult.Success(1L));

            //Act
            var result = await _sut.Handle(null, CancellationToken.None, Next);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1L, result.Result);
        }
    }
}
