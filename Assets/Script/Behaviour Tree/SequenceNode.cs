public class SequenceNode : CompositeNode
{
    public override NodeStatus Execute()
    {
        foreach (INode node in children)
        {
            NodeStatus status = node.Execute();
            if (status != NodeStatus.Success)
            {
                return status;
            }
        }
        return NodeStatus.Success;
    }
}
