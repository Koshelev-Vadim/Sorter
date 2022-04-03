namespace Sorter.Core.Services
{
    public interface ITreeDataSaver
    {
        void SaveSortedData(ITree tree, IFileWriter fileWriter);

    }
}
