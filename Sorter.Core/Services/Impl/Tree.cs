using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public class Tree : ITree
    {
        private Dictionary<byte, DictionaryNode> _tree;
        private int _maxStringLength = 0;

        public double CountOfRows = 0;

        public Tree()
        {
            _tree = new Dictionary<byte, DictionaryNode>()
            {
                { (byte) 32,  new DictionaryNode() } // ' '
            };
        }

        public DictionaryNode GetRoot()
        {
            return _tree[32]; // ' '
        }

        public int GetMaxStringLength()
        {
            return _maxStringLength;
        }

        public void AddToTree(Span<byte> data)
        {
            while (true)
            {
                var currentDataRowLastElem = data.IndexOf((byte)10); // '\n'
                if (currentDataRowLastElem < 0)
                    break;

                var dataRow = data.Slice(0, currentDataRowLastElem - 1);
                _tree[32].AddToNode(dataRow, -1); // ' '
                CountOfRows += 1;

                if (dataRow.Length > _maxStringLength)
                    _maxStringLength = dataRow.Length;

                data = data.Slice(currentDataRowLastElem + 1);
            }
        }

        public void RemoveNumber(DictionaryNode node, int number)
        {
            node.Numbers.Remove(number);
            CountOfRows--;
        }

        public void RemoveNode(DictionaryNode parentNode, DictionaryNode childNode, byte childNodeKey)
        {
            if (!childNode.Childs.Any() && !childNode.Numbers.Any())
                parentNode.Childs.Remove(childNodeKey);
        }
    }
}
