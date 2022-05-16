namespace CosManagement.Dtos.Clients;

public abstract class BaseClientDto
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public string? AdditionalInformations { get; set; }
}