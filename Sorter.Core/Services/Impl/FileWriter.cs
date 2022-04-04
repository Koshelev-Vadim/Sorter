
using Sorter.Core.Helpers;
using System.Text;

namespace Sorter.Core.Services
{
    public class FileWriter : IFileWriter, IDisposable
    {
        
        private FileStream _fileStream;
        private StreamWriter _streamWriter;
        private int _currentFileBufferPostition;
        private byte[] _fileBuffer;

        public FileWriter(string fileName, byte[] fileBuffer)
        {
            _fileBuffer = fileBuffer;

            _fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _streamWriter = new StreamWriter(_fileStream, Encoding.ASCII);

            _currentFileBufferPostition = 0;
        }

        public void Close()
        {
            _streamWriter.BaseStream.Write(_fileBuffer, 0, _currentFileBufferPostition);

            _streamWriter.Close();
            _fileStream.Close();
        }

        public void WriteToFile(int num, byte[] stringBuffer, int stringBufferLength)
        {
            var futureStringLength = stringBufferLength + 1 + 10 + 2; //Int.MaxValue = 2,147,483,647 - 10 symbols
            if (_currentFileBufferPostition + futureStringLength > _fileBuffer.Length)
            {
                _streamWriter.BaseStream.Write(_fileBuffer, 0, _currentFileBufferPostition);
                _currentFileBufferPostition = 0;
            }

            //IntegerHelpers.CopyToBytesArray(num, _fileBuffer, ref _currentFileBufferPostition);
            var numBytes = Encoding.ASCII.GetBytes(num.ToString());
            Array.Copy(numBytes, 0, _fileBuffer, _currentFileBufferPostition, numBytes.Length);
            _currentFileBufferPostition += numBytes.Length;

            // !!!!!!!!!!!!!!!!!!!!!!!!
            //numString.CopyTo(0, _fileBuffer, _currentFileBufferPostition, numString.Length);
            //_currentFileBufferPostition += numString.Length;

            _fileBuffer[_currentFileBufferPostition++] = 46; //'.'

            Array.Copy(stringBuffer, 0, _fileBuffer, _currentFileBufferPostition, stringBufferLength);
            _currentFileBufferPostition += stringBufferLength;

            //int i = 0;
            //while (i < stringBufferLength)
            //{
            //    _fileBuffer[_currentFileBufferPostition++] = stringBuffer[i];
            //    i++;
            //}


            _fileBuffer[_currentFileBufferPostition++] = 13; //'\r'
            _fileBuffer[_currentFileBufferPostition++] = 10; //'\n'
        }

        public void Dispose()
        {
            Close();
        }
    }
}
