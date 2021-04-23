using System;
using TheGame.SharedKernel;
using Xunit;

namespace TheGame.Tests.UnitTests.SharedKernel
{
    public class OperationResultTests
    {
        [Fact]
        public void Should_have_Succeeded_property_equals_true_when_Successful_method_is_invoked()
        {
            //Arrange
            //Act
            var sut = OperationResult.Successful();

            //Assert
            Assert.True(sut.Succeeded);
        }

        [Fact]
        public void Should_have_Succeeded_property_equals_false_when_Failure_method_is_invoked()
        {
            //Arrange
            //Act
            var sut = OperationResult.Failure(new FailureDetail("Field A", "Reason X"));

            //Assert
            Assert.False(sut.Succeeded);
        }

        [Fact]
        public void Should_throw_exception_when_not_passing_null_argument_to_Failure()
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => OperationResult.Failure((FailureDetail[])null));
        }

        //And so on...
    }
}
