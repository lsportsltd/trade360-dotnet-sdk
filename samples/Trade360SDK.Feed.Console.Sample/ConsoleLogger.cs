namespace Trade360SDK.Feed.Console.Sample
{
    internal class ConsoleLogger : ILogger
    {
        public void WriteError(string errorMessage)
        {
            System.Console.Error.WriteLine(errorMessage);
        }

        public void WriteWarning(string warningMessage)
        {
            System.Console.WriteLine(warningMessage);
        }
    }
}