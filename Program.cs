using LinkedListSerialization;

SeriliazeTest();
DeseriliazeTest();


void Init(ListRandom list)
{
    for (int i = 0; i < 10; i++)
    {
        list.Add(i.ToString() + " element");
    }    
}

void SeriliazeTest()
{
    var list = new ListRandom();    
    Init(list);

    Console.WriteLine("Serialization:");
    PrintList.PrintAll(list.Head);
    Console.WriteLine();

    using (FileStream fstream = new FileStream("list.dat", FileMode.OpenOrCreate))
    {
        list.Serialize(fstream);
    }

    list.Clear();
    list = null;
}

void DeseriliazeTest()
{
    var list = new ListRandom();

    using (FileStream fstream = new FileStream("list.dat", FileMode.OpenOrCreate))
    {
        list.Deserialize(fstream);
    }

    Console.WriteLine("Deserialization:");
    PrintList.PrintAll(list.Head);
}