using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace PyLauncher
{
    class Readcmd
    {
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        public struct COORD
        {
            public short X;
            public short Y;
        }
        //CHAR_INFO struct, which was a union in the old days
        // so we want to use LayoutKind.Explicit to mimic it as closely
        // as we can
        [StructLayout(LayoutKind.Explicit)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public char AsciiChar;
            [FieldOffset(2)] //2 bytes seems to work properly
            UInt16 Attributes;
        }

        // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutput.html
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutput(
            IntPtr hConsoleOutput,
            [Out] CHAR_INFO[,] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpReadRegion
            );

        public string readvalue(ref System.IntPtr ptr, short a)
        {
            SMALL_RECT srctReadRect = new SMALL_RECT
            {
                Top = 0,
                Left = 0,
                Bottom = 1,
                Right = 80
            };
            CHAR_INFO[,] chiBuffer = new CHAR_INFO[2, 80]; // [2][80];10 lines,  with 50 characters

            COORD coordBufSize = new COORD
            {
                X = 80,
                Y = 2
            };
            COORD coordBufCoord = new COORD
            {
                X = 0,
                Y = 0
            };

            bool fSuccess;
            int i = 0;
            int j = 0;
            string chartostring = "start";
            string previousstring = "";

            short g = a;
            short h = (short)(g + 1);

            srctReadRect.Top = g;
            srctReadRect.Bottom = h;
            int count = 0;

            //System.Console.WriteLine(g + "." + h);
            while (count < 1)//Hunting:if it find 1 empty rows with text then it will stop reading
            {
                previousstring = chartostring;
                srctReadRect.Top = g;
                srctReadRect.Bottom = h;

                fSuccess = ReadConsoleOutput(ptr, chiBuffer, coordBufSize, coordBufCoord, ref srctReadRect);

                i = 0;
                j = 0;
                chartostring = "";
                while (j < coordBufSize.Y)
                {
                    while (i < coordBufSize.X)
                    {
                        if (chiBuffer[j, i].UnicodeChar != 0 && chiBuffer[j, i].UnicodeChar != 32)
                            chartostring += chiBuffer[j, i].UnicodeChar;
                        i++;
                    }
                    i = 0;
                    j++;
                }

                if (chartostring.Length == 0)//The character length is zero, reverse the top of the source rect
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
                g += 1;
                h += 1;
            }
            return previousstring;
        }
    }
}
