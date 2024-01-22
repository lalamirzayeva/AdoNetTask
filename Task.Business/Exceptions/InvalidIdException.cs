namespace AdoNetTask.Business.Exceptions;

public class InvalidIdException:Exception
{
    public InvalidIdException(string message):base(message) { }
}
