// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Text;
using Storage;
using Storage.Schema;
using String = System.String;

const String FILE_PATH = "C:\\devel\\velobase\\test.dat";

WriteSampleData();
Reader reader = new Reader(FILE_PATH);
// list of anonymous classes
IEnumerable<IEnumerable<object>> rows = reader.FetchAllRows();

// reader.FetchByPK(5);
// reader.FetchByIndexId("index_name", 5);
// SELECT * FROM FILE_PATH WHERE id = 5

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
    
    
    writer.BeginWritingRow();

        writer.WriteValue(8);
        writer.WriteValue(9);
        writer.WriteValue("More text");
        writer.WriteValue(10);

    // write to disk
    writer.CommitRow();



    
    // INSERT INTO tbl_name('col1', 'col2') VALUES('val1', 'val2');
}
