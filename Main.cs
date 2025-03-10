using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class Main
    {
        int[] row = { 1, 2, 3, 4, 8, 5, 3, 8, 9, 7, 6 };

        public Main()
        {
            GetHasAllDigits(row);
        }

        public bool GetHasAllDigits(int[] row)
        {
            int[] distinct = row.Distinct().OrderBy(x => x).ToArray(); // Ensure the array is sorted
            if (distinct.Length != 9 || distinct[0] != 1 || distinct[8] != 9)
            {
                return false;
            }

            for (int i = 1; i < distinct.Length; i++)
            {
                if (distinct[i] != distinct[i - 1] + 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
