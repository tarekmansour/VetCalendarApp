using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Queries;

public record GetAvailableTimeSlotsQuery() : IRequest<Result<TimeSlotCollectionDto>>;
