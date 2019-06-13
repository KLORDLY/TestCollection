using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDValidCheck
{
    public class AlgorithmClass
    {
        static readonly int[] factors = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        static readonly int[] remainders = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        static readonly char[] lastPositions = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        const int DIVIDEND = 11;
        const int ID_LENGTH = 18;

        public AlgorithmClass()
        {
        }

        public bool IsValid(string id)
        {
            if (id.Length != ID_LENGTH)
                return false;
            int num = 0;
            for (int i = 0; i < id.Length - 1; i++)
            {
                try
                {
                    num += int.Parse(id[i].ToString()) * factors[i];
                }
                catch
                {
                }
            }
            int remainder = num % DIVIDEND;
            for (int i = 0; i < remainders.Length; i++)
            {
                if (remainder == remainders[i] && lastPositions[i] == id[id.Length - 1])
                {
                    return true;
                }
            }
            return false;
        }

        public string GetLastPosition(string id)
        {
            if (id.Length != ID_LENGTH - 1)
                return string.Empty;
            int num = 0;
            for (int i = 0; i < id.Length; i++)
            {
                try
                {
                    num += int.Parse(id[i].ToString()) * factors[i];
                }
                catch
                {
                }
            }
            int remainder = num % DIVIDEND;
            for (int i = 0; i < remainders.Length; i++)
            {
                if (remainder == remainders[i])
                {
                    return lastPositions[i].ToString();
                }
            }
            return string.Empty;
        }
    }
}
