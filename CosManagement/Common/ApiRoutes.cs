namespace CosManagement.Common;

public static class ApiRoutes
{
	public static string Base { get; set; } = "/api";

	public static string Clients { get; set; } = Base + "/clients";
	public static string Client { get; set; } = Base + "/clients/{id}";

	public static string Appointments { get; set; } = Base + "/appointments";
	public static string Appointment { get; set; } = Base + "/appointments/{id}";

	public static string Treatments { get; set; } = Base + "/treatments";
	public static string Treatment { get; set; } = Base + "/treatments/{id}";

	public static string Categories { get; set; } = Base + "/categories";
	public static string Category { get; set; } = Base + "/categories/{id}";

	public static string Login { get; set; } = Base + "/login";
	public static string Register { get; set; } = Base + "/register";
}