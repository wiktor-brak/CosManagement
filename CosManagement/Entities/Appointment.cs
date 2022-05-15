namespace CosManagement.Entities;

public class Appointment : BaseEntity
{
	public DateTime Date { get; set; }
	public string? Note { get; set; }

	public Guid ClientId { get; set; }
	public Client? Client { get; set; }

	public List<Treatment> Treatments { get; set; } = new();
}