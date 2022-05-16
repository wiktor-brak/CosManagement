using System.Runtime.Serialization;

namespace CosManagement.Exceptions;

[Serializable]
public class UserExitstsException : Exception
{
	public UserExitstsException() : base()
	{
	}

	protected UserExitstsException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}