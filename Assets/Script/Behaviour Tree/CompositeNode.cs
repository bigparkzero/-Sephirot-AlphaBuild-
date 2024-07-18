using System.Collections.Generic;

public abstract class CompositeNode : INode
{
    protected List<INode> children = new List<INode>();

    public void AddChild(INode node)
    {
        children.Add(node);
    }

    public abstract NodeStatus Execute();
}