using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Common;
using MediatR;
using PaginationX;

namespace Appointments.Application.Features.Appointments.Queries.GetPagedAsync;

public class GetPagedAsyncQuery :  IRequest<Response<PagedResult<AppointmentDto>>>
{
    public PaginationRequest Pagination { get; set; }
    public GetPagedAsyncQuery(PaginationRequest pagination)
    {
        Pagination = pagination;
    }

    public GetPagedAsyncQuery()
    {
        
    }
}