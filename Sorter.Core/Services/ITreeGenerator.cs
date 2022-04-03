namespace Sorter.Core.Services
{
    public interface ITreeGenerator
    {
        ITree GenerateTree(StreamReader reader, byte[] fileBuffer);
    }
}
