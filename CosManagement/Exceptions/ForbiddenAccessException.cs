using System.Runtime.Serialization;

namespace CosManagement.Exceptions;

[Serializable]
public class ForbiddenAccessException : Exception
{
	public ForbiddenAccessException() : base()
	{
	}

	protected ForbiddenAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}