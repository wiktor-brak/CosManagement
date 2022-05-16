using System.Runtime.Serialization;

namespace CosManagement.Exceptions;

[Serializable]
public class LoginFailException : Exception
{
	public LoginFailException() : base()
	{
	}

	protected LoginFailException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}