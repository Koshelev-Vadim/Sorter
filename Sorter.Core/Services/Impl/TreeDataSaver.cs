using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public class TreeDataSaver : ITreeDataSaver
    {
        private IFileWriter _fileWriter;

        public void SaveSortedData(ITree tree, IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;

            var stringBuffer = new byte[tree.GetMaxStringLength()];
            var stringBufferLength = 0;

            SaveToFile(tree.GetRoot(), stringBuffer, ref stringBufferLength);
        }

        private void SaveToFile(DictionaryNode node, byte[] stringBuffer, ref int stringBufferLength)
        {
            if (node.Numbers.Any())
            {
                foreach (var num in node.Numbers.Where(n => n > 0).OrderBy(n => n).ToList())
                {
                    _fileWriter.WriteToFile(num, stringBuffer, stringBufferLength);
                }
            }

            foreach (var key in node.Childs.Keys.OrderBy(k => k))
            {
                stringBuffer[stringBufferLength++] = key;
                SaveToFile(node.Childs[key], stringBuffer, ref stringBufferLength);
            }

            stringBufferLength--;
        }
    }
}
