namespace Sorter.Core.Services
{
    public interface IFileWriter
    {
        void WriteToFile(int num, byte[] stringBuffer, int stringBufferLength);
        void Close();
    }
}
