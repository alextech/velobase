using System.Collections;
using System.Text;
using Storage.Schema;
using Char = Storage.Schema.Char;

namespace Storage;

public class Reader
{
    private readonly string _filePath;
    private Table? _table;

    public Reader(string filePath)
    {
        _filePath = filePath;
    }

    public Table ReadSchema()
    {
        using (FileStream stream = new FileStream(_filePath, FileMode.Open))
        {
            int numFields = stream.ReadByte();

            byte[] schemaBytes = new byte[numFields];
            stream.Read(schemaBytes, 0, numFields);

            _table = new Table();
            for (int fieldIndex = 0; fieldIndex < numFields; fieldIndex++)
            {
                IField field = schemaBytes[fieldIndex] switch
                {
                    1 => new Integer(),
                    2 => new Schema.Char(),
                    _ => throw new ArgumentOutOfRangeException($"Encountered unkonwn field type {schemaBytes[fieldIndex]}.")
                };
                
                _table.AddField(field);
            }
            
            stream.Close();

            return _table;
        }
    }

    public IEnumerable<IEnumerable<object>> FetchAllRows()
    {
        if (_table == null)
        {
            ReadSchema();
        }

        int dataOffset = 1 + _table!.NumFields;

        int rowSize = _table.Fields.Sum(f => f.Size);
        byte[] rowBytes = new byte[rowSize];

        List<List<object>> rows = new List<List<object>>();
        using (FileStream stream = new FileStream(_filePath, FileMode.Open))
        {
            stream.Seek(dataOffset, SeekOrigin.Begin);

            while (stream.Read(rowBytes, 0, rowSize) > 0)
            {
                List<object> rowData = new List<object>();
                int currentFieldIndex = 0;
                // SELECT * 
                foreach (IField field in _table.Fields)
                {
                    rowData.Add(field.Decode(rowBytes, currentFieldIndex));

                    currentFieldIndex += field.Size;
                }

                rows.Add(rowData);
            }
            stream.Close();
            
            // assume that end of file is end of fixed size data

        }

        return rows;
    }
}