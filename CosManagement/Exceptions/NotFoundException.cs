using System.Runtime.Serialization;

namespace CosManagement.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
	public static string MessagePrefix { get; set; } = "Cannot find";

	public NotFoundException()
		: base()
	{
	}

	public NotFoundException(string message)
		: base(message)
	{
	}

	public NotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public NotFoundException(string name, object key)
		: base($"Entity \"{name}\" ({key}) was not found.")
	{
	}

	protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}