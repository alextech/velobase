using System.Text;

namespace Storage.Schema;

public class Char : IField
{
    public byte DbType { get; } = 2;
    public int Size { get; } = 255;
    public byte[] Encode(object value)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException($"String encoder received unexpected type {value.GetType()}.");
        }
        
        byte[] bval = new byte[Size];
        Encoding.UTF8.GetBytes(stringValue, 0, stringValue.Length, bval, 0);
        return bval;
    }

    public object Decode(byte[] rowBytes, int offset)
    {
        // trim null bytes
        string stringValue = Encoding.UTF8.GetString(rowBytes, offset, Size);
        stringValue = stringValue.Trim('\0');
        return stringValue;
    }
}