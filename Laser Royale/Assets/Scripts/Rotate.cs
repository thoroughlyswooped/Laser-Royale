using UnityEngine;

public class Rotate : MonoBehaviour
{
    Transform _currentTrans;
    bool mousePressed = false;
    public EditMode currEditMode {get; private set; }
    Vector2 dif;

    public static Rotate instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is already a 'Rotate' script in the scene but you are trying  to instantiate another one (there should only be one).");
        }

        //TODO: Update this dynamically
        currEditMode = EditMode.Translate;
    }

    // Update is called once per frame
    void Update()
    {
        // Get edit mode input
        if (Input.GetKeyDown(KeyCode.T))
        {
            currEditMode = EditMode.Translate;

            _currentTrans = null;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            currEditMode = EditMode.Rotate;

            _currentTrans = null;
        }

        // Get Mouse Input
        if (Input.GetMouseButtonDown(0) && !mousePressed)
        {
            MouseDown();
        }
        if(Input.GetMouseButtonUp(0) && !mousePressed)
        {
            MouseUp();
        }

        // Update elements
        if (_currentTrans)
        {
            if (currEditMode == EditMode.Rotate)
            {
                Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _currentTrans.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 0, Vector3.forward);
                _currentTrans.rotation = rotation;
            }
            else if(currEditMode == EditMode.Translate)
            {
                _currentTrans.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + dif;
            }
        }

        // Reset mouse pressed for next frame
        mousePressed = false;
    }

    public void SetCurrTrans(Transform trans)
    {
        // Called from local OnMouseDown() so mouse has been pressed this frame
        mousePressed = true;


        //Clear or set the current transform
        _currentTrans = trans == _currentTrans ? null : trans;

        if(_currentTrans && currEditMode == EditMode.Translate)
        {
            dif = _currentTrans.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
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

    public void MouseUp()
    {
        mousePressed = true;

        if (_currentTrans && currEditMode == EditMode.Translate)
        {
            // Clear current transform selection
            _currentTrans = null;
        }
    }
}

public enum EditMode { Rotate, Translate}
