namespace CosManagement.Interfaces;

public interface IMultiSourceResponse<TDto>
{
	public Guid Id { get; set; }

	public TDto? Dto { get; set; }
}