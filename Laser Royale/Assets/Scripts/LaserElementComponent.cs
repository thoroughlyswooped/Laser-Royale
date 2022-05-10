// TODO: this could be much better defined probably
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserElementComponent : MonoBehaviour
{
    public LaserElement element;
}

public struct LaserElement
{
    public Transform transform;
    public LineRenderer lineRenderer;
    public GameObject sparks;
    public bool impact;
    public LayerMask layerMask;
    public Light2D light;
};
