namespace Storage;

public class Node
{
    public List<int> Keys { get; init; } = new List<int>();
    public List<Node>? Children { get; init; }
    public bool IsLeaf => Children == null;

    public int? FindValue(int value)
    {
        foreach (int keyValue in Keys)
        {
            if (value == keyValue) return keyValue;
        }

        return null;
    }
}

public class Tree
{
    private readonly Node _root;

    public Tree(Node root)
    {
        _root = root;
    }

    public int? Search(int value)
    {
        Node leafNode = FindLeaf(value);
        return leafNode.FindValue(value);
    }

    private Node FindLeaf(int value)
    {
        Node currentNode = _root;
        while (!currentNode.IsLeaf)
        {
            int i = 0;
            for (; i < currentNode.Keys.Count; i++)
            {
                if (value < currentNode.Keys[i]) break;
            }

            currentNode = currentNode.Children![i];
        }

        return currentNode;
    }
}
