using UnityEngine;

public class Rotate : MonoBehaviour
{
    Transform _currentTrans;
    bool mousePressed = false;

    public static Rotate instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is already a 'Rotate' script in the scene but you are trying  to instantiate another one (there should only be one).");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mousePressed)
        {
            MouseDown();
        }
        if (_currentTrans)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _currentTrans.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 0, Vector3.forward);
            _currentTrans.rotation = rotation;
        }

        // Reset mouse pressed for next frame
        mousePressed = false;
    }

    public void SetCurrTrans(Transform trans)
    {
        // Called from local OnMouseDown() so mouse has been pressed this frame
        mousePressed = true;

        //Clear or set the current transform
        _currentTrans = trans == _currentTrans ? null: trans;
    }

    
    public void MouseDown()
    {
        mousePressed = true;

        if (_currentTrans)
        {
            // Clear current transform selection
            _currentTrans = null;
        }
    }
}
