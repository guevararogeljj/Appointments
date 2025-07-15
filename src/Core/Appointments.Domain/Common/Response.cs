
namespace Appointments.Domain.Common;

public class Response<T>
{
    public Error? Error { get; set; }
    public T? Result { get; set; }
}
