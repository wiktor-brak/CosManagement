namespace CosManagement.Dtos.Treatments;

public abstract class BaseTreatmentDto
{
	public string? Name { get; set; }
	public decimal BasePrice { get; set; }
}