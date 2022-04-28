// See https://aka.ms/new-console-template for more information

const String FILE_PATH = "C:\\devel\\velobase\\test.dat";

WriteSampleData();
using (FileStream stream = new FileStream(FILE_PATH, FileMode.Open))
{
    byte[] data = new byte[2];

    stream.Read(data, 0, 2);

    ushort val1 = BitConverter.ToUInt16(data, 0);
    Console.WriteLine(val1);
}

void WriteSampleData()
{
    const ushort data = 5;

    using (BinaryWriter writer = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
    {
        writer.Write(data);

        writer.Close();
    }
}
