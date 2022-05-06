namespace Storage.Schema;

public interface IField
{
    public byte DbType { get; }
    public int Size { get; }
    object Decode(byte[] rowBytes, int offset);
    byte[] Encode(object value);
}