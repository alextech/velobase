using System.Text;
using Storage.Schema;
using Char = Storage.Schema.Char;

namespace Storage;

public class Writer
{
    private readonly Table _table;
    private readonly string _filePath;
    private byte[]? _rowBytes;
    private IEnumerator<IField> _fieldIterator;
    private int _currentByteOffset;
    private int _currentFieldIndex;

    public Writer(Table table, string filePath)
    {
        _table = table;
        _filePath = filePath;
    }

    public void WriteTableSchema()
    {
        // TODO warn file exists
        // ReSharper disable once ConvertToUsingDeclaration
        using (BinaryWriter writer = new BinaryWriter(new FileStream(_filePath, FileMode.Create)))
        {
            writer.Write((byte)_table.NumFields);
            foreach (IField field in _table.Fields)
            {
                writer.Write(field.DbType);
            }
            
            writer.Close();
        }
    }

    private void WriteTableData()
    {
        if (_rowBytes == null)
        {
            throw new Exception("Trying to write empty byte array to row.");
        }
        
        using (BinaryWriter writer = new BinaryWriter(new FileStream(_filePath, FileMode.Append)))
        {
            writer.Write(_rowBytes);
            
            writer.Close();
        }
    }

    public void BeginWritingRow()
    {
        int rowSize = _table.Fields.Sum(field => field.Size);

        _rowBytes = new byte[rowSize];

        _fieldIterator = _table.Fields.GetEnumerator();
        _fieldIterator.MoveNext();
        
        _currentByteOffset = 0;
        _currentFieldIndex = 0;
    }

    public void WriteValue(object value)
    {
        try
        {
            WriteValue(_fieldIterator.Current.Encode(value));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException($"Wrong value type being written to column at index {_currentFieldIndex}.", e);
        }
    }
    private void WriteValue(byte[] value)
    {
        if (_rowBytes == null)
        {
            throw new Exception("Tried writing values before running BeginWritingRow().");
        }
        
        Array.Copy(
            value, 0, 
            _rowBytes, _currentByteOffset, _fieldIterator.Current.Size
        );

        
        _currentByteOffset += _fieldIterator.Current.Size;
        _fieldIterator.MoveNext();
        _currentFieldIndex++;
    }

    public void CommitRow()
    {
        WriteTableData();
        _rowBytes = null;
    }
}