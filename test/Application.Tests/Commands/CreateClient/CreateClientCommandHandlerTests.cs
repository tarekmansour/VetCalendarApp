using Application.Commands.CreateClient;
using FluentAssertions;

namespace Application.Tests.Commands;

public partial class CreateClientCommandTests
{
    [Fact(DisplayName = "new CreateClientCommandHandler with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new CreateClientCommandHandler(
            _logger,
            _validator,
            null!,
            _unitOfWork);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new CreateClientCommandHandler with null unitOfWork")]
    public void WithNullUnitOfWork_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new CreateClientCommandHandler(
            _logger,
            _validator,
            _clientRepository,
            null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        var command = new CreateClientCommand("Mme Dupont", "0611223344", "dupon@gmail.com");

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
