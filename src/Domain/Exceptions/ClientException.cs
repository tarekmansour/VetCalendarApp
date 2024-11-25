namespace Domain.Exceptions;

public class ClientException : Exception
{
    public ClientException()
    {

    }

    public ClientException(string message) : base(message)
    {

    }
}