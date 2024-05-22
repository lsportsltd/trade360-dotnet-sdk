namespace Trade360SDK.Feed
{
    public interface ILogger
    {
        void WriteError(string errorMessage);
        void WriteWarning(string warningMessage);
    }
}
