using UnityEngine;

public class ChasePlayerNode : INode
{
    private Transform characterTransform;
    private Transform playerTransform;
    private float speed;

    public ChasePlayerNode(Transform characterTransform, Transform playerTransform, float speed)
    {
        this.characterTransform = characterTransform;
        this.playerTransform = playerTransform;
        this.speed = speed;
    }

    public NodeStatus Execute()
    {
        characterTransform.position = Vector3.MoveTowards(characterTransform.position, playerTransform.position, speed * Time.deltaTime);
        return NodeStatus.Running;
    }
}
