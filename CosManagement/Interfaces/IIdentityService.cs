namespace CosManagement.Interfaces;

public interface IIdentityService
{
	Task<string> GetUserNameAsync(string userId);
}