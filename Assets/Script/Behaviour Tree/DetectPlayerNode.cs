using UnityEngine;

public class DetectPlayerNode : INode
{
    private Transform characterTransform;
    private Transform playerTransform;
    private float detectionRange;

    public DetectPlayerNode(Transform characterTransform, Transform playerTransform, float detectionRange)
    {
        this.characterTransform = characterTransform;
        this.playerTransform = playerTransform;
        this.detectionRange = detectionRange;
    }

    public NodeStatus Execute()
    {
        if (Vector3.Distance(characterTransform.position, playerTransform.position) <= detectionRange)
        {
            Debug.Log("Player detected");
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
