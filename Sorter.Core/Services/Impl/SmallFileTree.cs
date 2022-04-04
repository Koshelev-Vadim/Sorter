using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public class SmallFileTree : ISmallFileTree
    {

        private Dictionary<byte, SmallFileDictionaryNode> _tree;
        private int _maxStringLength = 0;

        public double CountOfRows = 0;

        public SmallFileTree()
        {
            _tree = new Dictionary<byte, SmallFileDictionaryNode>()
            {
                { (byte) 32,  new SmallFileDictionaryNode() } // ' '
            };
        }

        public SmallFileDictionaryNode GetRoot()
        {
            return _tree[32]; // ' '
        }

        public int GetMaxStringLength()
        {
            return _maxStringLength;
        }

        public void AddToTree(Span<byte> data, int? tempFileNum)
        {
            while (true)
            {
                var currentDataRowLastElem = data.IndexOf((byte)10); // '\n'
                if (currentDataRowLastElem <= 0)
                    break;

                var dataRow = data.Slice(0, currentDataRowLastElem - 1);
                _tree[32].AddToNode(dataRow, -1, tempFileNum); // ' '
                CountOfRows += 1;

                if (dataRow.Length > _maxStringLength)
                    _maxStringLength = dataRow.Length;

                data = data.Slice(currentDataRowLastElem + 1);
            }
        }

        public void RemoveNumber(SmallFileDictionaryNode node, Tuple<int, int?> number)
        {
            node.Numbers.Remove(number);
            CountOfRows--;
        }

        public void RemoveNode(SmallFileDictionaryNode parentNode, SmallFileDictionaryNode childNode, byte childNodeKey)
        {
            if (!childNode.Childs.Any() && !childNode.Numbers.Any())
                parentNode.Childs.Remove(childNodeKey);
        }

    }
}
