using NUnit.Framework;

namespace Storage.Test;

public class TreeTest
{

    [Test]
    public void SearchTest()
    {
        Tree tree = new Tree();
        tree.Insert(8);
        tree.Insert(15);
        tree.Insert(24);
        tree.Insert(31);
        tree.Insert(35);
        tree.Insert(40);
        tree.Insert(44);
        tree.Insert(45);
        tree.Insert(47);
        tree.Insert(48);
        tree.Insert(50);
        tree.Insert(51);
        tree.Insert(52);
        tree.Insert(53);
        tree.Insert(55);
        tree.Insert(57);
        tree.Insert(58);
        
        
        int? result = tree.Search(47);
        Assert.AreEqual(47, result);
    }
    
    [Test]
    public void InsertIntoLeafNodeTest()
    {
        Node.Order = 3;
        LeafNode node = new LeafNode();
        node.Insert(34);
        node.Insert(24);
        Assert.IsFalse(node.IsFull);
        
        node.Insert(31);
        
        Assert.AreEqual(24, node.Keys[0]);
        Assert.AreEqual(31, node.Keys[1]);
        Assert.AreEqual(34, node.Keys[2]);
        
        Assert.IsTrue(node.IsFull);
    }
    
    [Test]
    public void SplitLeafNodeTest()
    {
        Node.Order = 4;
        Node.SplitIndex = 2;
        LeafNode node = new LeafNode();
    
        node.Insert(10);
        node.Insert(20);
        node.Insert(30);
        node.Insert(40);
        
        (Node left, Node right) = node.Split();
        Assert.AreEqual(2, left.Keys.Count);
        Assert.AreEqual(2, right.Keys.Count);
        
        Assert.AreEqual(10, left.Keys.ElementAt(0));
        Assert.AreEqual(20, left.Keys.ElementAt(1));
        
        Assert.AreEqual(30, right.Keys.ElementAt(0));
        Assert.AreEqual(40, right.Keys.ElementAt(1));
        
    }

    [Test]
    public void SplitBranchNodeTest()
    {
        Node.Order = 4;
        Node.SplitIndex = 2;
        BranchNode node = new BranchNode();
        node.Insert(new LeafNode() { Keys = new List<int>() {15}});
        node.Insert(new LeafNode() { Keys = new List<int>() {24}});
        node.Insert(new LeafNode() { Keys = new List<int>() {35}});
        node.Insert(new LeafNode() { Keys = new List<int>() {44}});
        node.Insert(new LeafNode() { Keys = new List<int>() {47}});

        (BranchNode left, BranchNode right) = node.Split();
        Assert.AreEqual(2, left.Keys.Count);
        Assert.AreEqual(1, right.Keys.Count);
        Assert.AreEqual(3, left.Children.Count);
        Assert.AreEqual(2, right.Children.Count);
        
        Assert.AreEqual(24, left.Keys.ElementAt(0));
        Assert.AreEqual(35, left.Keys.ElementAt(1));
        
        Assert.AreEqual(47, right.Keys.ElementAt(0));
    }
    
    [Test]
    public void InsertUpToTest()
    {
        Tree tree = new Tree();
    
        tree.Insert(34);
        tree.Insert(24);
        tree.Insert(31);
    
        Node treeRoot = tree.RootNode;
        
        Assert.AreEqual(3, treeRoot.Keys.Count);
        Assert.AreEqual(24, treeRoot.Keys.ElementAt(0));
        Assert.AreEqual(31, treeRoot.Keys.ElementAt(1));
        Assert.AreEqual(34, treeRoot.Keys.ElementAt(2));
    }
    
    [Test]
    public void InsertRootOverflowTest()
    {
        Node.Order = 4;
        Node.SplitIndex = 2;
        Tree tree = new Tree();
    
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(30);
        tree.Insert(40);
        
        tree.Insert(25);
        
        
        Assert.AreEqual(1, tree.RootNode.Keys.Count);
        Assert.AreEqual(30, tree.RootNode.Keys.ElementAt(0));
        
        Assert.IsInstanceOf<BranchNode>(tree.RootNode);
        BranchNode root = (tree.RootNode as BranchNode)!;
        Assert.AreEqual(2, root.Children.Count);
    
        Node left = root.Children.ElementAt(0);
        Node right = root.Children.ElementAt(1);
        
        Assert.AreEqual(3, left.Keys.Count);
        Assert.AreEqual(2, right.Keys.Count);

        Assert.AreEqual(10, left.Keys.ElementAt(0));
        Assert.AreEqual(20, left.Keys.ElementAt(1));
        Assert.AreEqual(25, left.Keys.ElementAt(2));
        Assert.AreEqual(30, right.Keys.ElementAt(0));
        Assert.AreEqual(40, right.Keys.ElementAt(1));
    }

    [Test]
    public void InsertLeafOverflowTest()
    {
        Node.Order = 4;
        Node.SplitIndex = 2;
        Tree tree = new Tree();
    
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(30);
        tree.Insert(40);
        tree.Insert(35); 
        tree.Insert(50);
        tree.Insert(55); // force imbalance towards right, so it fills up and needs to be split
        
        
        Assert.AreEqual(2, tree.RootNode.Keys.Count);
        Assert.AreEqual(30, tree.RootNode.Keys.ElementAt(0));
        Assert.AreEqual(40, tree.RootNode.Keys.ElementAt(1));
        
        Assert.IsInstanceOf<BranchNode>(tree.RootNode);
        BranchNode root = (tree.RootNode as BranchNode)!;
        Assert.AreEqual(3, root.Children.Count);
    
        Node firstChild = root.Children.ElementAt(0);
        Node secondChild = root.Children.ElementAt(1);
        Node thirdChild = root.Children.ElementAt(2);
        
        Assert.AreEqual(2, firstChild.Keys.Count);
        Assert.AreEqual(2, secondChild.Keys.Count);
        Assert.AreEqual(3, thirdChild.Keys.Count);

        Assert.AreEqual(10, firstChild.Keys.ElementAt(0));
        Assert.AreEqual(20, firstChild.Keys.ElementAt(1));
        Assert.AreEqual(30, secondChild.Keys.ElementAt(0));
        Assert.AreEqual(35, secondChild.Keys.ElementAt(1));
        Assert.AreEqual(40, thirdChild.Keys.ElementAt(0));
        Assert.AreEqual(50, thirdChild.Keys.ElementAt(1));
        Assert.AreEqual(55, thirdChild.Keys.ElementAt(2));
        
    }
    
}