namespace Storage;

public interface INode
{
    int? Search(int value);
}
public abstract class Node : INode
{
    public List<int> Keys { get; init; }
    public abstract int? Search(int value);
}

public class LeafNode : Node
{
    public override int? Search(int value)
    {
        for (int i = 0; i < Tree.Order; i++)
        {
            if (Keys[i] == value)
            {
                return Keys[i];
            }
        }

        return null;
    }
}

public class BranchNode : Node
{
    public List<Node> Children { get; init; }
    public override int? Search(int value)
    {
        int i = 0;
        for (; i < Tree.Order; i++)
        {
            if (value < Keys[i]) break;
        }

        return Children[i].Search(value);
    }
}

public class Tree
{
    public static int Order = 2; // 0-based tree order
    private Node _root;
    public Tree(Node treeSnapshot)
    {
        _root = treeSnapshot;
    }

    public int? Search(int value)
    {
        return _root.Search(value);
    }
}
