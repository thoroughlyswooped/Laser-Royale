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
        Vector2 newDir = Vector2.Reflect(dir, hitInfo.normal);
        if (Portal_1 == portal) 
        {
            //Vector2 startObj = Portal_1.transform.position;
            //Vector2 endObj = Portal_2.transform.position;

            
            newDir = Portal_1.transform.worldToLocalMatrix.MultiplyPoint(-dir);
            //newDir = Portal_2.transform.localToWorldMatrix.MultiplyPoint(temp);

            oldPos = Portal_1.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
            newPos = Portal_2.transform.localToWorldMatrix.MultiplyPoint(oldPos);
           // oldPos = hitInfo.point;
            //newPos = Portal_2.transform.position;
        }
        else
        {
            //Vector2 endObj = Portal_1.transform.position;
            //Vector2 startObj = Portal_2.transform.position;
            //Vector2 temp = Portal_2.transform.worldToLocalMatrix.MultiplyPoint(newDir);
            newDir = Portal_2.transform.localToWorldMatrix.MultiplyPoint(-dir);

            oldPos = Portal_2.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
            newPos = Portal_1.transform.localToWorldMatrix.MultiplyPoint(oldPos);
            //newPos = Portal_1.transform.position;
        }

       

        // Stop hitting yourself
        LayerMask mask = LayerMask.NameToLayer("Ignore Raycast");
        LayerMask ogLayer = gameObject.layer;
        gameObject.layer = mask;
        RaycastHit2D hit = Physics2D.Raycast(newPos, dir, maxCastRange);
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
