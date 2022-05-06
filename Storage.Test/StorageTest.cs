using NUnit.Framework;
using Storage.Schema;

namespace Storage.Test;

public class StorageTest
{
    [Test]
    public void ReadWrite()
    {
        // ============== SETUP =============
        // CREATE TABLE()
        Table table = new Table();
        table.AddField(new Integer());
        table.AddField(new Integer());
        table.AddField(new Storage.Schema.Char());
        table.AddField(new Integer());

        string tablePath = AppDomain.CurrentDomain.BaseDirectory + "/StorageTest.ReadWrite.dat";

        Writer writer = new Writer(table, tablePath);
        writer.WriteTableSchema();

        // SQL
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
        writer.CommitRow();
        
        // ========= RUN ================
        Reader reader = new Reader(tablePath);
        IEnumerable<IEnumerable<object>> rows = reader.FetchAllRows();
        
        // ========= VERIFY =============

        IEnumerable<IEnumerable<object>> tableRows = rows.ToList();
        Assert.AreEqual(2, tableRows.Count());
        
        IEnumerable<object> row = tableRows.ElementAt(0).ToList();
        Assert.AreEqual(5, row.ElementAt(0));
        Assert.AreEqual(6, row.ElementAt(1));
        Assert.AreEqual("Some text", row.ElementAt(2));
        Assert.AreEqual(7, row.ElementAt(3));
        
        
        row = tableRows.ElementAt(1).ToList();
        Assert.AreEqual(8, row.ElementAt(0));
        Assert.AreEqual(9, row.ElementAt(1));
        Assert.AreEqual("More text", row.ElementAt(2));
        Assert.AreEqual(10, row.ElementAt(3));
    }
}