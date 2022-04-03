namespace Sorter.Core.Services
{
    public interface IProcessor
    {
        void Process(string fileToRead, string fileToWrite, int batchSizeInMegabytes);
    }
}
