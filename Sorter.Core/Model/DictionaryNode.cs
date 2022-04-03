using Sorter.Core.Helpers;

namespace Sorter.Core.Model
{
    public class DictionaryNode
    {
        // первое значение - само число, второе значение - номер временного файла, если есть
        private List<int> _numbers;
        private Dictionary<byte, DictionaryNode> _childs;

        public DictionaryNode()
        {
            _childs = new Dictionary<byte, DictionaryNode>(100);

            // TODO: Возможно поменять значение по умолчанию
            _numbers = new List<int>(100);
        }

        public Dictionary<byte, DictionaryNode> Childs
        {
            get { return _childs; }
        }

        public List<int> Numbers
        {
            get { return _numbers; }
        }

        public void AddToNode(ReadOnlySpan<byte> data, int number)
        {
            if (data.IsEmpty)
            {
                _numbers.Add(number);
                return;
            }

            var indexOfDot = data.IndexOf((byte)46); //'.'
            if (indexOfDot >= 0)
            {
                number = data.ToInt(indexOfDot);
                data = data.Slice(indexOfDot + 1);
            }

            if (!_childs.ContainsKey(data[0]))
                _childs.Add(data[0], new DictionaryNode());

            _childs[data[0]].AddToNode(data.Slice(1), number);
        }
    }
}
