using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }
    public Vector2 Vector { get; private set; }

    public Line(Vector2 startPos, Vector2 endPos)
    {
        Start = startPos;
        End = endPos;
        Vector = endPos - startPos;
    }

}
