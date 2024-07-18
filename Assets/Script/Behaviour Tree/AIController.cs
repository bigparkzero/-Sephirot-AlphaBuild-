using UnityEngine;

public class AIController : MonoBehaviour
{
    private INode rootNode;

    public Transform[] waypoints;
    public Transform playerTransform;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
  
    void Start()
    {
        InitializeBehaviorTree();
    }

    void Update()
    {
        if (rootNode != null)
        {
            rootNode.Execute();
        }
    }

    void InitializeBehaviorTree()
    {
        var patrolNode = new PatrolNode(transform, waypoints, patrolSpeed);
        var detectPlayerNode = new DetectPlayerNode(transform, playerTransform, detectionRange);
        var chasePlayerNode = new ChasePlayerNode(transform, playerTransform, chaseSpeed);

        var detectAndChase = new SequenceNode();
        detectAndChase.AddChild(detectPlayerNode);
        detectAndChase.AddChild(chasePlayerNode);

        var root = new SelectorNode();
        root.AddChild(detectAndChase);
        root.AddChild(patrolNode);

        rootNode = root;
    }

}
