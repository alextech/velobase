namespace Storage;

public abstract class Node
{
    public static int Order = 4;
    public static int SplitIndex = 2;
    public List<int> Keys { get; init; } = new List<int>(Node.Order);

    public bool IsFull => Keys.Count == Order;
    public BranchNode? Parent;

    public bool IsRoot => Parent is null; 

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
    internal Node RootNode = new LeafNode();

    public int? Search(int value)
    {
        Node leafNode = FindLeaf(value);
        return leafNode.FindValue(value);
    }

    private LeafNode FindLeaf(int value)
    {
        Node currentNode = RootNode;
        while (currentNode is BranchNode node)
        {
            int i = 0;
            for (; i < node.Keys.Count; i++)
            {
                if (value < node.Keys[i]) break;
            }

            currentNode = node.Children[i];
        }

        return (LeafNode)currentNode;
    }


    public void Insert(int value)
    {
        LeafNode target = FindLeaf(value);
        if (target.IsFull)
        {
            (LeafNode left, LeafNode right) = target.Split();
            if (target.IsRoot)
            {
                BranchNode newRoot = new BranchNode();
                newRoot.Insert(left);
                newRoot.Insert(right);

                RootNode = newRoot;
            }
            else
            {
                BranchNode targetParent = target.Parent!;
                while (targetParent.IsFull)
                {
                    (BranchNode leftBranch, BranchNode rightBranch) = targetParent.Split();
                    if (targetParent.IsRoot)
                    {
                        BranchNode newRoot = new BranchNode();
                        newRoot.Insert(leftBranch);
                        newRoot.Insert(rightBranch);

                        RootNode = newRoot;
                    }

                    targetParent = right.Keys.First() < rightBranch.Keys.First() ? leftBranch : rightBranch;
                }
                
                targetParent.Insert(right);
            }
            target = value < right.Keys.First() ? left : right;
        }
        target.Insert(value);
    }
}

public class LeafNode : Node
{
    public void Insert(int value)
    {
        if (IsFull)
        {
            throw new Exception("Trying to insert into full leaf node");
        }
        int i = 0;
        for (; i < Keys.Count; i++)
        {
            if (value > Keys[i]) continue;

            break;
        }
        
        Keys.Insert(i, value);
    }

    public (LeafNode left, LeafNode right) Split()
    {
        LeafNode right = new LeafNode()
        {
            Keys = Keys.GetRange(SplitIndex, Order - SplitIndex)
        };
        
        Keys.RemoveRange(SplitIndex, Order - SplitIndex);
        return (this, right);
    }
}

public class BranchNode : Node
{
    public List<Node> Children { get; init; } = new List<Node>(Order + 1);

    public void Insert(Node node)
    {
        if (IsFull)
        {
            throw new Exception("Trying to insert into full node node");
        }
        
        node.Parent = this;
        if (Children.Count == 0)
        {
            Children.Add(node);
            return;
        }
        
        int i = 0;
        int branchKey = node is BranchNode branch ? branch.Children.First().Keys.First() : node.Keys.First();
        for (; i < Keys.Count; i++)
        {
            if (branchKey > Keys[i]) continue;

            break;
        }
        
        Keys.Insert(i, branchKey);
        Children.Insert(i+1, node);
    }

    public (BranchNode left, BranchNode right) Split()
    {
        BranchNode rightNode = new BranchNode()
        {
            Keys = Keys.GetRange(SplitIndex + 1, Order - SplitIndex - 1),
            Children = Children.GetRange(SplitIndex + 1, Order - SplitIndex),
        };
        
        Keys.RemoveRange(SplitIndex, Order - SplitIndex);
        Children.RemoveRange(SplitIndex + 1, Order - SplitIndex);

        return (this, rightNode);
    }
}