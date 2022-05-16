namespace CosManagement.Interfaces;

public interface IGetAllQuery
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}