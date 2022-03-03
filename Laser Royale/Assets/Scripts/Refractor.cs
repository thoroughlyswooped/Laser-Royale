using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Refractor : HittableObject
{
    private float _rfNdx = 1.3f;

    public float RefractionIndex
    {
        get => _rfNdx;
        set
        {
            if (value == 0.0) throw new ArgumentException("Refraction index cannot be zero");
            _rfNdx = value;
        }
    }

    public bool splitBeam;

    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange)
    {
        var pos = hitInfo.point;
        var newDir = Refract(dir, hitInfo.normal, 1, RefractionIndex);

        // Stop hitting yourself
        //TODO: does this need to change to refract as we leave the object? i.e. we do want to hit ourself
        LayerMask mask = LayerMask.NameToLayer("Ignore Raycast");
        LayerMask ogLayer = gameObject.layer;
        gameObject.layer = mask;
        RaycastHit2D hit = Physics2D.Raycast(hitInfo.point, newDir, maxCastRange);
        //TODO: refract additional times upon entry with variations of the refraction index?
        //TODO: should also check the color of the beam and only split if it is white
        /*if (splitBeam)
        {
            //TODO: probably wrong way to go about this too. LOL
            var hit2 = Physics2D.Raycast(hitInfo.point, Refract(dir, hitInfo.normal, 1,RefractionIndex * (float) 1.02),
                maxCastRange);
            var hit3 = Physics2D.Raycast(hitInfo.point, Refract(dir, hitInfo.normal, 1,RefractionIndex * (float) 0.98),
                maxCastRange);
        }*/

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
            var maxRangePoint = pos + newDir * maxCastRange;
            basePoints.Add(maxRangePoint);
        }

        return basePoints.ToArray();
    }

    private static Vector2 Refract(Vector2 vec1, Vector2 vec2, float inRefractionIndex, float outRefractionIndex)
    {
        var sinTheta2 = (inRefractionIndex / outRefractionIndex) * Mathf.Sin(Vector2.SignedAngle(vec1, vec2));
        // if (sinTheta2 > 1) return Vector2.zero; // how do we stop the refraction when it passes this critical angle?
        var outgoingAngle = Mathf.Rad2Deg * Mathf.Asin(sinTheta2);
        return new Vector2(Mathf.Cos(outgoingAngle), Mathf.Sin(outgoingAngle));
    }
}