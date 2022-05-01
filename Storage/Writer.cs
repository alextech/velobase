using Storage.Schema;

namespace Storage;

public class Writer
{
    public void WriteTableSchema(Table table, string filePath)
    {
        // TODO warn file exists
        // ReSharper disable once ConvertToUsingDeclaration
        using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
        {
            writer.Write((byte)table.NumFields);
            foreach (IField field in table.Fields)
            {
                writer.Write(field.DbType);
            }
            
            writer.Close();
        }
    }

    public void WriteTableData(Table table, byte[] data, string filePath)
    {
        using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
        {
            writer.Write(data);
        }
    }
}