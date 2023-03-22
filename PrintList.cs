using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListSerialization
{
    internal static class PrintList
    {
        public static void PrintAll(ListNode firts)
        {
            GetPrintData(firts);
        }

        private static ListNode? GetPrintData(ListNode? next)
        {
            if (next is null) return null;

            Console.WriteLine(next.Data + " (random: " + next.Random?.Data + ")");
            return GetPrintData(next.Next);
        }
    }
}
