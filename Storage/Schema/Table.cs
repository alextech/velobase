namespace Storage.Schema;

public class Table
{
    private List<IField> _fields = new List<IField>();

    public IReadOnlyCollection<IField> Fields => _fields.AsReadOnly();

    public void AddField(IField field)
    {
        _fields.Add(field);
    }

    public int NumFields => _fields.Count;
}