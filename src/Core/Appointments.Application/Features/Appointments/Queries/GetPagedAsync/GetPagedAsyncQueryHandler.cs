using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Common;
using AutoMapper;
using MediatR;
using PaginationX;

namespace Appointments.Application.Features.Appointments.Queries.GetPagedAsync;

public class GetPagedAsyncQueryHandler : IRequestHandler<GetPagedAsyncQuery, Response<PagedResult<AppointmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetPagedAsyncQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Response<PagedResult<AppointmentDto>>> Handle(GetPagedAsyncQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<PagedResult<AppointmentDto>>();
        try
        {
            var appointments = await _unitOfWork.Appointments.GetPagedAsync(request.Pagination);
            if (appointments.Items.Any())
            {
                var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments.Items);
                response.Result = new PagedResult<AppointmentDto>(
                    appointmentDtos.ToList(),
                    appointments.TotalCount,
                    appointments.CurrentPage,
                    appointments.PageSize
                );
            }
            else
            {
                response.Error = new Error(
                    "NoAppointmentsFound",
                    "No appointments found for the specified criteria."
                );
            }
        }
        catch (Exception ex)
        {
            response.Error = new Error(
                "GetPagedAsyncError",
                $"An error occurred while retrieving paged appointments: {ex.Message}"
            );
        }
        
        return response;
    }
}