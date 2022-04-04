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

        public static void CopyToBytesArray(int num, byte[] data, ref int dataPosFrom)
        {

            //Encoding.ASCII.GetBytes()

            123.ToString();
            bool numIsStarted = false;
            int positionValue;
            int partNumb = 0;
            for (var i = 9; i >= 0; i--)
            {
                positionValue = (int)((num - partNumb) / (Math.Pow(10, i)));
                if (positionValue > 0)
                    numIsStarted = true;

                if (numIsStarted)
                {
                    data[dataPosFrom++] = (byte)(positionValue + 48); // '0'
                    partNumb += (int)Math.Pow(10, i) * positionValue;
                }
            }
        }
    }
}
