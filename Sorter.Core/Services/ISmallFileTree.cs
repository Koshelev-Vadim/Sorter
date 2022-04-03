using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public interface ISmallFileTree
    {
        void AddToTree(Span<byte> data, int? tempFileNum);
        int GetMaxStringLength();
        SmallFileDictionaryNode GetRoot();

        void RemoveNode(SmallFileDictionaryNode parentNode, SmallFileDictionaryNode childNode, byte childNodeKey);
        void RemoveNumber(SmallFileDictionaryNode node, Tuple<int, int?> number);
    }
}
