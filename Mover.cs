using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public LineRenderer trajectory;
    private LineManager lineManager;
    public List<Vector2> intersectionPoints = new List<Vector2>();
    Vector2 closestIntersectionPoint;

    public float lineWidth = 1f;
    private float raycastLength = 1000f;
    public EnemyDirection enemyDirection;

    public float initialSpeedLimit = 10f;
    public float moveSpeed;

    Color objectColor;

    void Start()
    {
        lineManager = FindObjectOfType<LineManager>();

        GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        objectColor = GetComponent<Renderer>().material.color;
        trajectory.GetComponent<LineRenderer>().material.color = objectColor;

        trajectory.startWidth = lineWidth;
        trajectory.endWidth = lineWidth;
        trajectory.receiveShadows = false;
        trajectory.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        moveSpeed = Random.Range(10f, initialSpeedLimit);

        lineManager.moverTrajectories.Add(this, default);
    }

    private void Update()
    {
        CastLine();
        FindIntersections();
        Move();
    }

    private void Move()
    {
        if (enemyDirection == EnemyDirection.right) transform.position += transform.right * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.left)  transform.position += -transform.right * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.up)    transform.position += transform.up * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.down)  transform.position += -transform.up * moveSpeed * Time.deltaTime;
    }

    private void CastLine()
    {
        trajectory.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0f));

        if (enemyDirection == EnemyDirection.right)
        {
            DrawIntersectionLine(transform.right, 8);
        }
        else if (enemyDirection == EnemyDirection.left)
        {
            DrawIntersectionLine(-transform.right, 9);
        }
        else if (enemyDirection == EnemyDirection.up)
        {
            DrawIntersectionLine(transform.up, 10);
        }
        else if (enemyDirection == EnemyDirection.down)
        {
            DrawIntersectionLine(-transform.up, 11);
        }

        lineManager.moverTrajectories[this] = new Line(trajectory.GetPosition(0), trajectory.GetPosition(1));
    }

    private void DrawIntersectionLine(Vector2 direction, int layerMaskIndex)
    {
        RaycastHit2D hit;

        int layerMask = 1 << layerMaskIndex;
        layerMask = ~layerMask;
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, raycastLength, layerMask);

        trajectory.SetPosition(1, hit.point);

        Vector2 hitPoint = trajectory.GetPosition(1);
        Vector2 pos = transform.position;

        List<Vector2> intersectionPoints = lineManager.GetIntersections(this);

        foreach (Vector2 point in intersectionPoints)
        {

            if (Vector2.Distance(point, pos) < Vector2.Distance(closestIntersectionPoint, pos)
                && (point - pos).normalized == (hitPoint - pos).normalized)
                closestIntersectionPoint = point;

        }

        if ((closestIntersectionPoint - pos).magnitude < (hitPoint - pos).magnitude
                      && (closestIntersectionPoint - pos).normalized == (hitPoint - pos).normalized)
        { trajectory.SetPosition(1, closestIntersectionPoint); }
    }

    private void FindIntersections()
    {
        foreach (var trajectory in lineManager.moverTrajectories)
        {
            if (trajectory.Key == this) continue;

            Vector2? intersectionPoint;

            intersectionPoint = VectorMath.GetLineIntersection(trajectory.Value, lineManager.moverTrajectories[this]);

            if (intersectionPoint != null)
            {
                intersectionPoints.Add(intersectionPoint.Value);
            }
        }
    }

}
