using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListSerialization
{
    internal class ListRandom
    {
        public ListNode? Head;
        public ListNode? Tail;
        public int Count = 0;

        private readonly Random _random = new();
        private readonly int _fileSize = 1024;
        private readonly int _intSize = 4;

        public void Add(string value)
        {
            if (Head is null)
            {
                Head = new ListNode() { Data = value };
                Head.Random = Head;
                Tail = Head;
            }
            else
            {              
                Tail.Next = new ListNode() { Previous = Tail, Data = value };
                Tail = Tail.Next;
                SetRandom();
            }

            Count++;
        }
        
        public void Serialize(Stream s)
        {
            var buffer = new Byte[0];
            buffer = buffer.Concat(BitConverter.GetBytes(Count)).ToArray();

            GetAllNodeBytes(Head, ref buffer);
            GetAllRandomNodeBytes(Head, ref buffer);

            try
            {
                s.Write(buffer);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void Deserialize(Stream s)
        {
            Clear();

            var buffer = new Byte[_fileSize];

            try
            {
                s.Read(buffer);
                Count = BitConverter.ToInt32(buffer, 0);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            var indx = SetAllNodes(buffer, _intSize);
            SetAllRandomNodes(buffer, indx);

        }
        private void SetRandom()
        {
            int randNode;
            var target = Head;

            while (target != null)
            {
                randNode = _random.Next(0, Count);
                target.Random = GetNode(Head, randNode);
                target = target.Next;
            }
        }

        private ListNode? GetNode(ListNode target, int indx)
        {
            if (indx == 0) return target;
            if (target is null) return null;

            return GetNode(target.Next, indx - 1);
        }

        private ListNode? GetIndex(ListNode target, ListNode find, ref int indx)
        {
            if (target == find) return null;
            indx += 1;
            return GetIndex(target.Next, find, ref indx);
        }        

        private ListNode? GetAllNodeBytes(ListNode target, ref byte[] buffer)
        {
            if (target is null) return null;

            buffer = buffer.Concat(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(target.Data)))
                .Concat(Encoding.UTF8.GetBytes(target.Data)).ToArray();

            return GetAllNodeBytes(target.Next, ref buffer);
        }

        private ListNode? GetAllRandomNodeBytes(ListNode target, ref byte[] buffer)
        {
            if (target is null) return null;

            var indx = 0;
            GetIndex(Head, target.Random, ref indx);
            buffer = buffer.Concat(BitConverter.GetBytes(indx)).ToArray();

            return GetAllRandomNodeBytes(target.Next, ref buffer);
        }

        private int SetAllNodes(byte[] buffer, int indx)
        {
            var curCount = 0;
            Head = new ListNode();
            Tail = Head;

            try
            {
                while (true)
                {               
                    var size = BitConverter.ToInt32(buffer, indx);
                    indx += _intSize;
                    Tail.Data = Encoding.UTF8.GetString(buffer, indx, size);
                    indx += size;

                    curCount++;
                    if (curCount == Count) break;

                    Tail.Next = new ListNode();
                    Tail.Next.Previous = Tail;
                    Tail = Tail.Next;                 
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return indx;
        }

        private void SetAllRandomNodes(byte[] buffer, int indx)
        {            
            var target = Head;

            try
            {
                for (var i = 0; i < Count; i++)
                {                
                    var nodeIndx = BitConverter.ToInt32(buffer, indx);
                    indx += _intSize;

                    target.Random = GetNode(Head, nodeIndx);
                    target = target.Next;                
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void Clear()
        {
            var target = Tail;
            ListNode previous;
            while (target != null)
            {
                previous = target;
                target.Random = null;
                target = target.Previous;
                previous = null;
            }
            Count = 0;
        }
    }
}
