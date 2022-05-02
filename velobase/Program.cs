// See https://aka.ms/new-console-template for more information

using System.Text;
using Storage;
using Storage.Schema;
using String = System.String;

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
    // float val5 = BitConverter.ToSingle(page, 15);
    
    Console.WriteLine(val1);
    Console.WriteLine(val2);
    Console.WriteLine(val3);
    Console.WriteLine(val4);
    // Console.WriteLine(val5);
}

void WriteSampleData()
{
    // CREATE TABLE()
    Table table = new Table();
    table.AddField(new Integer());
    table.AddField(new Integer());
    table.AddField(new Storage.Schema.Char());
    table.AddField(new Integer());

    Writer writer = new Writer(table, FILE_PATH);
    writer.WriteTableSchema();

    // SQL
    // begin transaction
    // INSERT INTO tbl_name VALUES('val1', 'val2');
    writer.BeginWritingRow();

        writer.WriteValue(5);
        writer.WriteValue(6);
        writer.WriteValue("Some text");
        writer.WriteValue(7);

    // write to disk
    writer.CommitRow();



    
    // INSERT INTO tbl_name('col1', 'col2') VALUES('val1', 'val2');
}
