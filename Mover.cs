using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    LineManager lineManager;
    LineWidth lineWidth;
    public LineRenderer trajectory;

    private float raycastLength = 100000f;
    public EnemyDirection enemyDirection;

    public float initialSpeedLimit = 3f;
    public float moveSpeed;

    private int playerLayer = 1 << 14;

    Color objectColor;

    void Start()
    {
        lineManager = FindObjectOfType<LineManager>();
        lineWidth = FindObjectOfType<LineWidth>();

        GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        objectColor = GetComponent<Renderer>().material.color;

        trajectory.GetComponent<LineRenderer>().material.color = objectColor;
        trajectory.startWidth = lineWidth.WidthCalculation(90f);
        trajectory.endWidth = lineWidth.WidthCalculation(90f);
        trajectory.receiveShadows = false;
        trajectory.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        moveSpeed = Random.Range(10f, initialSpeedLimit);

        lineManager.moverTrajectories.Add(this, default);
    }

    private void FixedUpdate()
    {
        CastLine();
        Move();
    }

    private void Move()
    {
        if (moveSpeed < 0) moveSpeed = 0;

        if (enemyDirection == EnemyDirection.right) transform.position += transform.right * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.left)  transform.position += -transform.right * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.up)    transform.position += transform.up * moveSpeed * Time.deltaTime;
        if (enemyDirection == EnemyDirection.down)  transform.position += -transform.up * moveSpeed * Time.deltaTime;
    }

    private void CastLine()
    {
        if (moveSpeed <= 0)
        {
            trajectory.SetPosition(0, new Vector3(5000f, 5000f, 0f));
            trajectory.SetPosition(1, new Vector3(5000f, 5000f, 0f));
            lineManager.moverTrajectories[this] = new Line(trajectory.GetPosition(0), trajectory.GetPosition(1));
            return;
        }

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
    }

    private void DrawIntersectionLine(Vector2 direction, int layerMaskIndex)
    {
        Vector2 pos = transform.position;

        trajectory.SetPosition(0, pos);

        RaycastHit2D hit;

        int layerMask = (1 << layerMaskIndex) | playerLayer;
        layerMask = ~layerMask;
        hit = Physics2D.Raycast(pos, direction, raycastLength, layerMask);

        trajectory.SetPosition(1, hit.point);

        Vector2 hitPoint = trajectory.GetPosition(1);

        List<Vector2> intersectionPoints = lineManager.GetIntersections(this);

        if (intersectionPoints.Count <= 0)
            return;

        Vector2 closestIntersectionPoint = Vector2.one * 10000000;

        foreach (Vector2 point in intersectionPoints)
        {

            if (Vector2.Distance(point, pos) < Vector2.Distance(closestIntersectionPoint, pos))
                closestIntersectionPoint = point;

        }

        if (Vector2.Distance(closestIntersectionPoint, pos) < Vector2.Distance(hitPoint, pos)
            && (closestIntersectionPoint - pos).normalized == (hitPoint - pos).normalized)
        { trajectory.SetPosition(1, closestIntersectionPoint); }

        lineManager.moverTrajectories[this] = new Line(trajectory.GetPosition(0), trajectory.GetPosition(1));
    }

}
