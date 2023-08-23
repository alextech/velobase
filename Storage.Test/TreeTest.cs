using NUnit.Framework;

namespace Storage.Test;

public class TreeTest
{
    private Tree? _tree;

    [SetUp]
    public void Setup()
    {
        _tree = new Tree(new BranchNode()
        {
            Keys = new List<int>() { 24, 40 },
            Children = new List<Node>()
            {
                new BranchNode()
                {
                    Keys = new List<int>() { 8, 15 },
                    Children = new List<Node>()
                    {
                        new LeafNode() { Keys = new List<int>(4) { 2, 4, 7 } },
                        new LeafNode() { Keys = new List<int>(4) { 8, 11, 13 } },
                        new LeafNode() { Keys = new List<int>(4) { 15, 17, 21 } },
                    }
                },
                new BranchNode()
                {
                    Keys = new List<int>() { 31, 35 },
                    Children = new List<Node>()
                    {
                        new LeafNode() { Keys = new List<int>(4) { 24, 26, 27 } },
                        new LeafNode() { Keys = new List<int>(4) { 31, 33, 34 } },
                        new LeafNode() { Keys = new List<int>(4) { 35, 37, 39 } },
                    }
                },
                new BranchNode()
                {
                    Keys = new List<int>() { 47, 51, 55 },
                    Children = new List<Node>()
                    {
                        new LeafNode() { Keys = new List<int>() { 40, 44, 45 } },
                        new LeafNode() { Keys = new List<int>() { 47, 48, 50 } },
                        new LeafNode() { Keys = new List<int>() { 51, 52, 53 } },
                        new LeafNode() { Keys = new List<int>() { 55, 57, 58 } },
                    }
                }
            }
        });
    }

    [Test]
    public void SearchTest()
    {
        int? result = _tree.Search(47);
        Assert.AreEqual(47, result);
    }

    [Test]
    public void SearchLeafNodeTest()
    {
        LeafNode leafNode = new LeafNode() { Keys = new List<int>() { 47, 48, 50 } };
        int? result = leafNode.Search(47);
        Assert.NotNull(result);
        Assert.AreEqual(47, result!.Value);
    }
    
    [Test]
    public void SearchBranchNodeTest()
    {
        BranchNode branchNode = new BranchNode()
        {
            Keys = new List<int>() { 47, 51, 55 },
            Children = new List<Node>()
            {
                new LeafNode() { Keys = new List<int>() { 40, 44, 45 } },
                new LeafNode() { Keys = new List<int>() { 47, 48, 50 } },
                new LeafNode() { Keys = new List<int>() { 51, 52, 53 } },
                new LeafNode() { Keys = new List<int>() { 55, 57, 58 } },
            }
        };
        
        int? result = branchNode.Search(47);
        Assert.NotNull(result);
        Assert.AreEqual(47, result!.Value);
    }
    
    

    // [Test]
    // public void InsertUpToTest()
    // {
    //     Tree.Order = 3;
    //     Tree.SplitIndex = 1;
    //     
    //     Node tree = new Node();
    //
    //     tree.Insert(34);
    //     tree.Insert(24);
    //     tree.Insert(31);
    //
    //     Assert.AreEqual(3, tree.Keys.Count);
    //     Assert.AreEqual(24, tree.Keys.ElementAt(0));
    //     Assert.AreEqual(31, tree.Keys.ElementAt(1));
    //     Assert.AreEqual(34, tree.Keys.ElementAt(2));
    // }
    
    // [Test]
    // public void InsertOverflowTest()
    // {
    //     Tree.Order = 3;
    //     Tree.SplitIndex = 1;
    //     
    //     Node tree = new Node();
    //
    //     tree = tree.Insert(31);
    //     tree = tree.Insert(24);
    //     tree = tree.Insert(34);
    //     tree = tree.Insert(11);
    //     
    //     Assert.AreEqual(1, tree.Keys.Count);
    //     Assert.AreEqual(31, tree.Keys.ElementAt(0));
    //     
    //     Assert.NotNull(tree.Children);
    //     Assert.AreEqual(2, tree.Children!.Count);
    //
    //     Node left = tree.Children.ElementAt(0);
    //     Node right = tree.Children.ElementAt(1);
    //     
    //     Assert.AreEqual(2, left.Keys.Count);
    //     Assert.AreEqual(2, right.Keys.Count);
    //     
    //     Assert.AreEqual(11, left.Keys.ElementAt(0));
    //     Assert.AreEqual(24, left.Keys.ElementAt(1));
    //     Assert.AreEqual(31, right.Keys.ElementAt(0));
    //     Assert.AreEqual(34, right.Keys.ElementAt(1));
    // }
    //
    // [Test]
    // public void InsertOverFlowForthRank()
    // {
    //     Tree.Order = 4;
    //     Tree.SplitIndex = 2;
    //     
    //     
    //     Node tree = new Node();
    //     
    //     tree = tree.Insert(24);
    //     tree = tree.Insert(25);
    //     tree = tree.Insert(26);
    //     tree = tree.Insert(27);
    //     tree = tree.Insert(28);
    //     tree = tree.Insert(29);
    //     tree = tree.Insert(30);
    //     
    //     Assert.AreEqual(2, tree.Keys.Count);
    //     Assert.AreEqual(26, tree.Keys.ElementAt(0));
    //     Assert.AreEqual(28, tree.Keys.ElementAt(1));
    //     
    //     Assert.NotNull(tree.Children);;
    //     Assert.AreEqual(3, tree.Children!.Count);
    //
    //     Node leaf_1 = tree.Children.ElementAt(0);
    //     Node leaf_2 = tree.Children.ElementAt(1);
    //     Node leaf_3 = tree.Children.ElementAt(2);
    //
    //     Assert.AreEqual(24, leaf_1.Keys.ElementAt(0));
    //     Assert.AreEqual(25, leaf_1.Keys.ElementAt(1));
    //     
    //     Assert.AreEqual(26, leaf_2.Keys.ElementAt(0));
    //     Assert.AreEqual(27, leaf_2.Keys.ElementAt(1));
    //     
    //     Assert.AreEqual(28, leaf_3.Keys.ElementAt(0));
    //     Assert.AreEqual(29, leaf_3.Keys.ElementAt(1));
    //     Assert.AreEqual(30, leaf_3.Keys.ElementAt(2));
    // }
}