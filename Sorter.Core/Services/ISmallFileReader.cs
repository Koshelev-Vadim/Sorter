namespace Sorter.Core.Services
{
    public interface ISmallFileReader
    {
        void Open(int numOfFile, double bufferSizeInMegabytes);
        Span<byte> GetNextString();
    }
}
