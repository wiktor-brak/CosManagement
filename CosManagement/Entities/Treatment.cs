namespace CosManagement.Entities;

public class Treatment : BaseEntity
{
	public string? Name { get; set; }
	public decimal BasePrice { get; set; }

	public Guid CategoryId { get; set; }
	public Category? Category { get; set; } = new();

	public List<Appointment> Appointments { get; set; } = new();
}