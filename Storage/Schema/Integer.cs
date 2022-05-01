namespace Storage.Schema;

public class Integer : IField
{
    public byte DbType { get; } = 1;
    public int Size { get; } = 4;
}