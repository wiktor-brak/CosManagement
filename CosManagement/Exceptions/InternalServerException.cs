using System.Runtime.Serialization;

namespace CosManagement.Exceptions;

[Serializable]
public class InternalServerException : Exception
{
	public InternalServerException() : base()
	{
	}

	protected InternalServerException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}