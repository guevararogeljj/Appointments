using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommand : IRequest<Response<Guid>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
}
