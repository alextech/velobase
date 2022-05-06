namespace Storage.Schema;

public class Integer : IField
{
    public byte DbType { get; } = 1;
    public int Size { get; } = 4;

    public byte[] Encode(object value)
    {
        if (value is not int intValue)
        {
            throw new ArgumentException($"Integer encoder received unexpected type ${value.GetType()}.");
        }

        return BitConverter.GetBytes(intValue);
    }

    public object Decode(byte[] rowBytes, int offset)
    {
        return BitConverter.ToInt32(rowBytes, offset);
    }
}