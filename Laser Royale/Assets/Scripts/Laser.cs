using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser
{
    private Vector2 _dir, _pos;
    
    internal readonly GameObject LaserObj;
    private readonly LineRenderer _laser;
    private readonly List<Vector2> _laserIndices = new List<Vector2>();
    
    public Laser(Vector2 pos, Vector2 dir, Material material)
    {
        _laser = new LineRenderer();
        LaserObj = new GameObject()
        {
            name = "Laser"
        };
        _laser = LaserObj.AddComponent<LineRenderer>();
        _laser.startWidth = 0.1f;
        _laser.endWidth = 0.1f;
        _laser.material = material;
        _laser.startColor = Color.red;
        CastRay(pos, dir, _laser);
    }

     void CastRay(Vector2 pos, Vector2 dir, LineRenderer laser)
    {
        _laserIndices.Add(pos);
        var ray = new Ray(pos, dir);
        var hit = Physics2D.Raycast(pos, dir, float.PositiveInfinity);
        
        if (hit)
        {
           
            CheckHit(hit, dir, laser);
        }
        else
        {
            _laserIndices.Add(ray.GetPoint(30));
        }
        UpdateLaser();

    }

    private void UpdateLaser()
    {
        var count = 0;
        _laser.positionCount = _laserIndices.Count;
        foreach (var vector in _laserIndices)
        {
            _laser.SetPosition(count, vector);
            count++;
        }
    }

    private void CheckHit(RaycastHit2D hitInfo, Vector2 direction, LineRenderer laser)
    {
        if (hitInfo.collider.gameObject.tag.Equals("Reflector"))
        {
            var pos = hitInfo.point;
            _dir = Vector2.Reflect(direction, hitInfo.normal);
            
            CastRay(pos, _dir, this._laser);
        }
        else
        {
            _laserIndices.Add(hitInfo.point);
        }
    }
}
