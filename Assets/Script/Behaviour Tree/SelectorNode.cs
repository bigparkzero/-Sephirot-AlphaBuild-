public class SelectorNode : CompositeNode
{
    public override NodeStatus Execute()
    {
        foreach (INode node in children)
        {
            NodeStatus status = node.Execute();
            if (status == NodeStatus.Success)
            {
                return NodeStatus.Success;
            }
        }
        return NodeStatus.Failure;
    }
}
