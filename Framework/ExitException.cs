namespace Framework
{
    public class ExitException : Exception
    {
        public ExitException() : base()
        {

        }
        public ExitException(string message) : base(message)
        {

        }
        public ExitException(string message, Exception e) : base(message, e)
        {

        }
    }
}
