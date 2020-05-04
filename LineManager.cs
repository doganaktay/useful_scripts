using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public Dictionary<Mover, Line> moverTrajectories = new Dictionary<Mover, Line>();

    public List<Vector2> GetIntersections(Mover mover)
    {
        List<Vector2> intersectionPoints = new List<Vector2>();

        foreach(var trajectory in moverTrajectories)
        {
            if (trajectory.Key == mover) continue;

            Vector2? intersectionPoint;

            intersectionPoint = VectorMath.GetLineIntersection(trajectory.Value, moverTrajectories[mover]);

            if (intersectionPoint != null)
            {
                intersectionPoints.Add(intersectionPoint.Value);
            }
        }

        return intersectionPoints;
    }

}
