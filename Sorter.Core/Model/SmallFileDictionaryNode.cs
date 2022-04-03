using Sorter.Core.Helpers;

namespace Sorter.Core.Model
{
    public class SmallFileDictionaryNode
    {
        // первое значение - само число, второе значение - номер временного файла, если есть
        private List<Tuple<int, int?>> _numbers;
        private Dictionary<byte, SmallFileDictionaryNode> _childs;

        public SmallFileDictionaryNode()
        {
            _childs = new Dictionary<byte, SmallFileDictionaryNode>(10);

            // TODO: Возможно поменять значение по умолчанию
            _numbers = new List<Tuple<int, int?>>(10);
        }

        public Dictionary<byte, SmallFileDictionaryNode> Childs
        {
            get { return _childs; }
        }

        public List<Tuple<int, int?>> Numbers
        {
            get { return _numbers; }
        }

        public void AddToNode(ReadOnlySpan<byte> data, int number, int? tempFileNum)
        {
            if (data.IsEmpty)
            {
                _numbers.Add(new Tuple<int, int?>(number, tempFileNum));
                return;
            }

            var indexOfDot = data.IndexOf((byte)46); //'.'
            if (indexOfDot >= 0)
            {
                number = data.ToInt(indexOfDot);
                data = data.Slice(indexOfDot + 1);
            }

            if (!_childs.ContainsKey(data[0]))
                _childs.Add(data[0], new SmallFileDictionaryNode());

            _childs[data[0]].AddToNode(data.Slice(1), number, tempFileNum);
        }

    }
}
