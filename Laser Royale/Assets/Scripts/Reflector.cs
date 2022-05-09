using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : HittableObject
{
    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange, GameObject laser = null)
    {
        var pos = hitInfo.point;
        Debug.Log($"dir: <{dir.x}, {dir.y}>");
        Vector2 newDir = Vector2.Reflect(dir, hitInfo.normal);

        // Stop hitting yourself
        LayerMask mask = LayerMask.NameToLayer("Ignore Raycast");
        LayerMask ogLayer = gameObject.layer;
        gameObject.layer = mask;
        RaycastHit2D hit = Physics2D.Raycast(pos, newDir, maxCastRange);
        gameObject.layer = ogLayer;

        //get base points from this ray
        List<Vector2> basePoints = new List<Vector2>() {pos};

        // Hit something
        if (hit)
        {
            if (hit.collider.CompareTag("Hittable") && hit.collider.gameObject != gameObject)
            {
                basePoints.AddRange(hit.collider.gameObject.GetComponent<HittableObject>().Hit(newDir, hit, maxCastRange));
            }
            else
            {
                basePoints.Add(hit.point);
            }
        }
        //Didn't hit anything
        else
        {
            Vector2 maxRangePoint = pos + (newDir * maxCastRange);
            basePoints.Add(maxRangePoint);
        }


        return basePoints.ToArray();

    }
}
