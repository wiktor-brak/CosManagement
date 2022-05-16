namespace CosManagement.Dtos.Appointments;

public abstract class BaseAppointmentDto
{
	public DateTime Date { get; set; }
	public string? Note { get; set; }
}