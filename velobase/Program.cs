// See https://aka.ms/new-console-template for more information

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Storage;
using Storage.Schema;
using velobase.SampleSales;
using Char = Storage.Schema.Char;

const string FILE_PATH = "C:/devel/velobase/test.dat";
const string SALES_RECORDS_DB_PATH = "C:/devel/velobase/sales_records.dat";
const string SALES_RECORDS_CSV_PATH = "c:/devel/velobase/1000_Sales_Records.csv";

// WriteSampleData();

// WriteSalesRecordsDb();


Reader reader = new Reader(SALES_RECORDS_DB_PATH);
IEnumerable<IEnumerable<object>> rows = reader.FetchAllRows();
List<IEnumerable<object>> rowObjects = rows.ToList().ToList();

foreach (var rowObject in rowObjects)
{
    Console.WriteLine(rowObject.ToList()[0]);
}

return;

// reader.FetchByPK(5);
// reader.FetchByIndexId("index_name", 5);
// SELECT * FROM FILE_PATH WHERE id = 5

void WriteSampleData()
{
    // CREATE TABLE()
    Table table = new Table();
    table.AddField(new Integer());
    table.AddField(new Integer());
    table.AddField(new Char());
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

void WriteSalesRecordsDb()
{
    Table table = new Table();
    table.AddField(new Char()); // region
    table.AddField(new Char()); // country
    table.AddField(new Char()); // item_type
    table.AddField(new Char()); // sales_channel
    table.AddField(new Char()); // order_priority
    table.AddField(new Integer()); // order_date
    table.AddField(new Integer()); // order_id
    table.AddField(new Integer()); // ship_date
    table.AddField(new Integer()); // units_sold
    table.AddField(new Integer()); // unit_price
    table.AddField(new Integer()); // unit_cost
    table.AddField(new Integer()); // total_revenue
    table.AddField(new Integer()); // total_cost
    table.AddField(new Integer()); // total_profit

    Writer writer = new Writer(table, SALES_RECORDS_DB_PATH);
    writer.WriteTableSchema();
    
    WriteSalesRecordsData(table);
}

void WriteSalesRecordsData(Table table)
{
    IEnumerable<SaleRecord> salesRecords = ReadSalesRecordsCsv();

    Writer writer = new Writer(table, SALES_RECORDS_DB_PATH);
    foreach (SaleRecord salesRecord in salesRecords)
    {
        writer.BeginWritingRow();
        
        writer.WriteValue(salesRecord.Region);
        writer.WriteValue(salesRecord.Country);
        writer.WriteValue(salesRecord.ItemType);
        writer.WriteValue(salesRecord.SalesChannel);
        writer.WriteValue(salesRecord.OrderPriority);
        writer.WriteValue((int)(salesRecord.OrderDate.ToUnixTimeSeconds() % int.MaxValue));
        writer.WriteValue(salesRecord.OrderID);
        writer.WriteValue((int)(salesRecord.ShipDate.ToUnixTimeSeconds() % int.MaxValue));
        writer.WriteValue(salesRecord.UnitsSold);
        writer.WriteValue((int)salesRecord.UnitPrice);
        writer.WriteValue((int)salesRecord.UnitCost);
        writer.WriteValue((int)salesRecord.TotalRevenue);
        writer.WriteValue((int)salesRecord.TotalCost);
        writer.WriteValue((int)salesRecord.TotalProfit);
        
        writer.CommitRow();
    }

}

IEnumerable<SaleRecord> ReadSalesRecordsCsv()
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.Replace(" ", ""),
    };

    using (StreamReader reader = new StreamReader(SALES_RECORDS_CSV_PATH))
    using (CsvReader csv = new CsvReader(reader, config))
    {
        IEnumerable<SaleRecord> saleRecords = csv.GetRecords<SaleRecord>();

        IEnumerable<SaleRecord> readSaleRecords = saleRecords.ToList();

        return readSaleRecords;
    }
}