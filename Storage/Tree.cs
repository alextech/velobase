namespace Storage;

public class Node
{
    public List<int> Keys { get; init; }
    public List<Node> Children { get; init; }
}

public class Tree
{
    private Node _root;
    public Tree(Node treeSnapshot)
    {
        _root = treeSnapshot;
    }
}
