using Application.Commands.CreateClient;
using Domain.Errors;
using FluentAssertions;

namespace Application.Tests.Commands
{
    public partial class CreateClientCommandTests
    {      
        [Theory(DisplayName = "CreateClientCommand with invalid Name")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidName_Should_ReturnsError(string? invalidName)
        {
            //Arrange
            var command = new CreateClientCommand(invalidName!, "0611223344", "MmeDupont@yahoo.fr");

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.FirstOrDefault()!.Code.Should().Be(ClientErrorCodes.InvalidClientName);
            result.Errors.FirstOrDefault()!.Description.Should().Be(ClientErrorMessages.ClientNameShouldNotBeEmpty);
        }

        [Theory(DisplayName = "CreateClientCommand with invalid email")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidEmail_Should_ReturnsError(string? invalidEmail)
        {
            //Arrange
            var command = new CreateClientCommand("Mme Dupont", "0611223344", invalidEmail!);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.FirstOrDefault()!.Code.Should().Be(ClientErrorCodes.InvalidClientEmail);
            result.Errors.FirstOrDefault()!.Description.Should().Be(ClientErrorMessages.ClientEmailShouldNotBeEmpty);
        }
    }
}
