using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Camera cam;
    public Transform square;
    public float distanceFromCamera;
    Rigidbody2D r;

    void Start()
    {
        distanceFromCamera = Vector3.Distance(square.position, cam.transform.position);
        r = square.GetComponent<Rigidbody2D>();
    }

    Vector3 lastPos;

    void OnMouseDrag()
    {
  
        Vector3 pos = Input.mousePosition;
        pos.z = distanceFromCamera;
        pos = cam.ScreenToWorldPoint(pos);
        r.velocity = (pos - square.position) * 10;
    }
    
    void OnMouseUp()
    {
        r.velocity = Vector3.zero;
    }
}