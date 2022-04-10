using Sorter.Core.Helpers;

namespace Sorter.Core.Model
{
    internal class FileRow
    {
        public int Number
        {
            get
            {
                return _number;
            }
        }
        public int _number;
        public byte[] String
        {
            get
            {
                return _string;
            }
        }

        private byte[] _string;

        public int FileNum
        {
            get
            {
                return _fileNum;
            }
        }
        private int _fileNum;

        public FileRow (byte[] bytes, int fileNum)
        {
            var dotPos = bytes.First(b => b == 46); //'.'
            _number = bytes.ToInt(dotPos);

            _string = new byte[dotPos - 1];
            Array.Copy(bytes, 0, _string, 0, dotPos - 1);
        }

        public FileRow(Span<byte> bytes, int fileNum)
        {
            var dotPos = bytes.IndexOf<byte>(46); //'.'
            _number = bytes.ToInt(dotPos);

            _string = new byte[dotPos];
            for(int i = 0; i < dotPos; i++)
                _string[i] = bytes[i];

            _fileNum = fileNum;
        }
    }

    internal class FileRowComparer : IComparer<FileRow>
    {
        public int Compare(FileRow? x, FileRow? y)
        {
            if (x == null || y == null)
                return 0;

            if (x.Number < y.Number)
                return -1;
            else
                return 1;

            var minStirngLen = Math.Min(x.String.Length, y.String.Length);
            for(var i = 0; i < minStirngLen; i++)
            {
                if (x.String[i] < y.String[i])
                    return -1;
                else
                    return 1;
            }

            if (x.String.Length < y.String.Length)
                return -1;
            else if (x.String.Length > y.String.Length)
                return 1;

            return 0;
        }
    }
}
