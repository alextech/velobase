// See https://aka.ms/new-console-template for more information

using System.Text;

const String FILE_PATH = "C:\\devel\\velobase\\test.dat";

WriteSampleData();
using (FileStream stream = new FileStream(FILE_PATH, FileMode.Open))
{
    byte[] page = new byte[19];

    stream.Read(page, 0, 19);

    ushort val1 = BitConverter.ToUInt16(page, 0);
    ushort val2 = BitConverter.ToUInt16(page, 2);
    string val3 = Encoding.UTF8.GetString(page, 4, 9);
    ushort val4 = BitConverter.ToUInt16(page, 13);
    float val5 = BitConverter.ToSingle(page, 15);
    
    Console.WriteLine(val1);
    Console.WriteLine(val2);
    Console.WriteLine(val3);
    Console.WriteLine(val4);
    Console.WriteLine(val5);
}

void WriteSampleData()
{
    const ushort data = 5;

    using (BinaryWriter writer = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
    {
        const string svar = "Some text";
        byte[] bvar = Encoding.UTF8.GetBytes(svar);
        
        writer.Write((ushort)5);
        writer.Write((ushort)6);
        writer.Write(bvar);
        writer.Write((ushort)7);
        writer.Write(1.250F);

        writer.Close();
    }
}
