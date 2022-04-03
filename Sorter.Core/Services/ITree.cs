using Sorter.Core.Model;

namespace Sorter.Core.Services
{
    public interface ITree
    {
        void AddToTree(Span<byte> data);
        int GetMaxStringLength();
        DictionaryNode GetRoot();

        void RemoveNode(DictionaryNode parentNode, DictionaryNode childNode, byte childNodeKey);
        void RemoveNumber(DictionaryNode node, int number);
    }
}
