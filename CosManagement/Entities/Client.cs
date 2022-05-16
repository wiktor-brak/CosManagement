namespace CosManagement.Entities;

public class Client : BaseEntity
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? AdditionalInformations { get; set; }

	public List<Appointment> Appointments { get; set; } = new();
}