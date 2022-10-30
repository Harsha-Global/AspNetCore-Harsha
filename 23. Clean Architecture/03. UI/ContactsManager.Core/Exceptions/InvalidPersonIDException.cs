namespace Exceptions
{
 public class InvalidPersonIDException : ArgumentException
 {
  public InvalidPersonIDException() : base()
  {
  }

  public InvalidPersonIDException(string? message) : base(message)
  {
  }

  public InvalidPersonIDException(string? message, Exception? innerException)
  {
  }
 }
}