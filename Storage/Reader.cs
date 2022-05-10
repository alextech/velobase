using System.Collections;
using System.Text;
using Storage.Schema;
using Char = Storage.Schema.Char;

namespace Storage;

public class Reader
{
    private readonly string _filePath;
    private Table? _table;
    private int _rowsPerPage = 10;

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

        List<IField> fields = _table.Fields.ToList();
        int rowSize = fields.Sum(f => f.Size);

        byte[] rowBytes = new byte[rowSize];
        
        // 8192 = 8kb
        // rows per page 10

        using (FileStream stream = new FileStream(_filePath, FileMode.Open))
        {
            stream.Seek(dataOffset, SeekOrigin.Begin);

            while (stream.Read(rowBytes, 0, rowSize) > 0)
            {
                int currentFieldOffset = 0;

                List<object> rowData = new List<object>();
                foreach (IField field in fields)
                {
                    rowData.Add(field.Decode(rowBytes, currentFieldOffset));
                    currentFieldOffset += field.Size;
                }
                
                yield return rowData;
            }
            stream.Close();
            
            // assume that end of file is end of fixed size data

        }
    }
}