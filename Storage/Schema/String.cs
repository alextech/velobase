namespace Storage.Schema;

public class String : IField
{
    public byte DbType { get; } = 2;
    public int Size { get; } = 255;
}