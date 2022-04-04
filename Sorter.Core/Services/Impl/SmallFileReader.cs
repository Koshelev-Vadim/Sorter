namespace Sorter.Core.Services
{
    public class SmallFileReader : ISmallFileReader
    {
        private StreamReader _reader;
        private byte[] _fileBuffer;
        private int _fileBufferSize;
        private int _currentOffset;
        private bool _fileIsClosed;

        public void Open(int numOfFile, double bufferSizeInMegabytes)
        {
            _reader = File.OpenText(Path.Combine("temp", numOfFile.ToString()));
            _fileBuffer = new byte[(int)(bufferSizeInMegabytes * 1024 * 1024)];
            _currentOffset = _fileBufferSize = _fileBuffer.Length;

            //_fileBufferSize = _reader.Read(_fileBuffer, _currentOffset, _fileBuffer.Length - _currentOffset);
        }

        public bool IsClosed()
        {
            return _fileIsClosed;
        }

        public Span<byte> GetNextString()
        {
            var span = _fileBuffer.AsSpan(_currentOffset, _fileBufferSize - _currentOffset);
            var stringLastSymbol = span.IndexOf((byte)10); //'\n'

            if (stringLastSymbol < 0)
            {
                if (!LoadNewBuffer())
                    return null;

                span = _fileBuffer.AsSpan(_currentOffset, _fileBufferSize - _currentOffset);
                stringLastSymbol = span.IndexOf((byte)10);//'\n'
            }

            var result = new Span<byte>(_fileBuffer, _currentOffset, stringLastSymbol + 1);
            _currentOffset += stringLastSymbol + 1;

            return result;
        }

        private bool LoadNewBuffer()
        {
            int i = 0;
            //Array.Copy(_fileBuffer, _currentOffset, _fileBuffer, 0, _fileBuffer.Length - _currentOffset);

            while (_currentOffset + i < _fileBuffer.Length)
            {
                _fileBuffer[i] = _fileBuffer[_currentOffset + i];
                i++;
            }

            _currentOffset = i;

            _fileBufferSize = _reader.BaseStream.Read(_fileBuffer, _currentOffset, _fileBuffer.Length - _currentOffset);
            if (_fileBufferSize == 0)
            {
                _fileIsClosed = true;
                _reader.Close();

                return false;
            }

            _fileBufferSize += _currentOffset;
            _currentOffset = 0;

            return true;
        }

    }
}
