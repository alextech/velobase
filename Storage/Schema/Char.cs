namespace Storage.Schema;

public class Char : IField
{
    public byte DbType { get; } = 2;
    public int Size { get; } = 255;
}