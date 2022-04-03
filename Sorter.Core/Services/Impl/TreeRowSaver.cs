using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public class TreeRowSaver
    {
        private ISmallFileTree _tree;
        private IFileWriter _fileWriter;
        private byte[] _stringBuffer;
        private int _stringBufferLength;
        private int _treeMaxStringLength = 0;

        public TreeRowSaver(ISmallFileTree tree, IFileWriter fileWriter, int treeMaxStringLength)
        {
            _tree = tree;
            _fileWriter = fileWriter;
            _treeMaxStringLength = treeMaxStringLength;

            _stringBuffer = new byte[_treeMaxStringLength];
        }

        public int? SaveRowAndRemove()
        {
            _stringBufferLength = 0;
            return SaveRowAndRemoveInternal(_tree.GetRoot(), _stringBuffer, ref _stringBufferLength);
        }

        private int? SaveRowAndRemoveInternal(SmallFileDictionaryNode node, byte[] stringBuffer, ref int stringBufferLength)
        {
            if (node.Numbers.Any())
            {
                var firstNumber = node.Numbers.OrderBy(n => n.Item1).First();
                _fileWriter.WriteToFile(firstNumber.Item1, stringBuffer, stringBufferLength);
                _tree.RemoveNumber(node, firstNumber);

                return firstNumber.Item2;
            }

            foreach (var key in node.Childs.Keys.OrderBy(k => k))
            {
                stringBuffer[stringBufferLength++] = key;

                var currentNode = node.Childs[key];
                var smallFileNum = SaveRowAndRemoveInternal(currentNode, stringBuffer, ref stringBufferLength);

                _tree.RemoveNode(node, currentNode, key);

                if (smallFileNum != null)
                    return smallFileNum;

            }

            return null;
        }
    }
}
