namespace CosManagement.Dtos.Clients;

public abstract class BaseClientDto
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public int Phone { get; set; }
	public string? AdditionalInformations { get; set; }
}