using System.Collections.Generic;
using UnityEngine;

public class BeamPortal : MonoBehaviour
{
    public GameObject Portal_1;
    public GameObject Portal_2;

    public Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange, GameObject portal)
    {
        Vector2 oldPos;
        Vector2 newPos;
        Vector2 newDir;
        dir = Vector2.Reflect(dir, hitInfo.normal);

        if (Portal_1 == portal) 
        {
            newDir = Portal_2.transform.localToWorldMatrix.MultiplyPoint(dir) - Portal_2.transform.position;

            oldPos = Portal_1.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
            newPos = Portal_2.transform.localToWorldMatrix.MultiplyPoint(oldPos);
        }
        else
        {
            newDir = Portal_1.transform.localToWorldMatrix.MultiplyPoint(dir) - Portal_1.transform.position;

            oldPos = Portal_2.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
            newPos = Portal_1.transform.localToWorldMatrix.MultiplyPoint(oldPos);
        }

       

        // Stop hitting yourself
        LayerMask mask = LayerMask.NameToLayer("Ignore Raycast");
        LayerMask ogLayer = gameObject.layer;
        gameObject.layer = mask;
        RaycastHit2D hit = Physics2D.Raycast(newPos, newDir, maxCastRange);
        gameObject.layer = ogLayer;

        //get base points from this ray
        List<Vector2> basePoints = new List<Vector2>() { hitInfo.point, newPos };

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
            Vector2 maxRangePoint = newPos + (newDir * maxCastRange);
            basePoints.Add(maxRangePoint);
        }


        return basePoints.ToArray();

    }
}
