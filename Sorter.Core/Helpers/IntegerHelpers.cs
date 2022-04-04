using System.Text;

namespace Sorter.Core.Helpers
{
    public static class IntegerHelpers
    {
        public static int ToInt(this ReadOnlySpan<byte> data, int numLength)
        {
            int res = 0;
            for (var i = 0; i < numLength; i++)
                res = res * 10 + (data[i] - 48); // '0'

            return res;
        }

    }
}
