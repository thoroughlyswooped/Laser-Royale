using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    public Material material;
    private Laser beam;

    // Update is called once per frame
    void Update()
    {
        if (beam != null)
        {
            Destroy(beam.LaserObj);
        }
        beam = new Laser(gameObject.transform.position, gameObject.transform.right, material);
    }
}
