using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMath
{
    public static Vector2 Get2DPerp(Vector2 pos)
    {
        return new Vector2(-pos.y, pos.x);
    }

    public static bool IsParallel(Line a, Line b)
    {
        Vector2 aPerp = Get2DPerp(a.Vector);
        if (Vector2.Dot(aPerp, b.Vector) == 0)
            return true;
        else
            return false;
    }

    // Can be used if implementing the Line struct
    public static Vector2 GetLineIntersection(Line a, Line b)
    {
        Vector2 c = b.Start - a.Start;
        Vector2 u = b.Vector;
        Vector2 v = a.Vector;
        Vector2 uPerp = Get2DPerp(u);

        float t = Vector2.Dot(uPerp, c) / Vector2.Dot(uPerp, v);

        Vector2 intersectionPoint = Vector2.Lerp(a.Start, a.End, t);

        return intersectionPoint;
    }

    // Can be used with 4 vector2's supplied as start and end positions for two lines
    public static Vector2 GetIntersection(Vector2 startPosA, Vector2 endPosA, Vector2 startPosB, Vector2 endPosB)
    {
        Vector2 c = startPosB - startPosA;
        Vector2 u = endPosB - startPosB;
        Vector2 v = endPosA - startPosA;
        Vector2 uPerp = Get2DPerp(u);

        float t = Vector2.Dot(uPerp, c) / Vector2.Dot(uPerp, v);

        Vector2 intersectionPoint = Vector2.Lerp(startPosA, endPosA, t);

        return intersectionPoint;
    }
}
