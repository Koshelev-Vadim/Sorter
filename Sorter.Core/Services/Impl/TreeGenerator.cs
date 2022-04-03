
using System;

namespace Sorter.Core.Services
{
    public class TreeGenerator : ITreeGenerator
    {
        private byte[] _fileBuffer;
        private byte[] _tempFileBuffer;

        public TreeGenerator()
        {
            _tempFileBuffer = new byte[0];
        }

        public ITree GenerateTree(StreamReader reader, byte[] fileBuffer)
        {
            _fileBuffer = fileBuffer;

            var _treeBuilder = new Tree();

            Array.Copy(_tempFileBuffer, 0, _fileBuffer, 0, _tempFileBuffer.Length);

            var readedBytes = reader.BaseStream.Read(_fileBuffer, _tempFileBuffer.Length, _fileBuffer.Length - _tempFileBuffer.Length);
            if (readedBytes == 0)
                return null;

            var span = _fileBuffer.AsSpan(0, _tempFileBuffer.Length + readedBytes);

            var lastRowElement = span.LastIndexOf((byte)10); //'\n'
            if (lastRowElement == -1)
                lastRowElement = span.Length;

            _treeBuilder.AddToTree(span.Slice(0, lastRowElement + 1));

            _tempFileBuffer = new byte[span.Length - lastRowElement - 1];
            Array.Copy(_fileBuffer, lastRowElement + 1, _tempFileBuffer, 0, _tempFileBuffer.Length);

            //int i = 0;
            //while (lastRowElement + i + 1 < span.Length)
            //{
            //    _tempFileBuffer[i] = _fileBuffer[lastRowElement + i + 1];
            //    i++;
            //}

            //startOfReadForBuffer = i;

            return _treeBuilder;
        }

    }
}
