using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    //*********https://stackoverflow.com/questions/51905268/how-to-find-closest-point-on-line************
    public static Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 end, Vector2 point)
    {
        //Get heading
        Vector2 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector2 lhs = point - origin;
        float dotP = Vector2.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }

    // ********************https://www.programmersought.com/article/46434409883/**************************************
    public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
    {
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
        //print(d);
        return d * direct.normalized + point;
    }
}
