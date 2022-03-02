using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 dir= Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-0,Vector3.forward);
        transform.rotation = rotation;
    }
}
