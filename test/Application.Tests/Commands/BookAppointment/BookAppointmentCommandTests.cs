using Application.Commands.BookAppointment;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.BookAppointment;

public partial class BookAppointmentCommandTests : DatabaseFixture
{
    private readonly ILogger<BookAppointmentCommandHandler> _logger;
    private readonly IValidator<BookAppointmentCommand> _commandValidator;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookAppointmentCommandHandler _sut;

    public BookAppointmentCommandTests()
    {
        _logger = Substitute.For<ILogger<BookAppointmentCommandHandler>>();
        _unitOfWork = new UnitOfWork(_dbContext);
        _appointmentRepository = new AppointmentRepository(_dbContext);
        _commandValidator = new BookAppointmentCommandValidator(_appointmentRepository);
        _sut = new BookAppointmentCommandHandler(_logger, _commandValidator, _appointmentRepository, _unitOfWork);
    }
}
