public enum NodeStatus { Success, Failure, Running }

public interface INode
{
    NodeStatus Execute();
}
