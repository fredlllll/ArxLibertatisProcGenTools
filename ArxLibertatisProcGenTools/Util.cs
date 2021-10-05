using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public static class Util
    {
        public static char[] StringToChars(string str, int arrayLength)
        {
            char[] retval = new char[arrayLength];
            for(int i =0; i< Math.Min(str.Length, arrayLength); i++)
            {
                retval[i] = str[i];
            }
            return retval;
        }
    }
}
