using UnityEngine;

[System.Serializable]
public class Rotate : MonoBehaviour
{
    Transform _currentTrans;
    bool m_mousePressed = false;
    [SerializeField]
    bool m_canBeDropped = true;
    public EditMode currEditMode {get; private set; }
    Vector2 dif;
    Vector2 m_ogPosition;

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
        if (Input.GetMouseButtonDown(0) && !m_mousePressed)
        {
            MouseDown();
        }
        if(Input.GetMouseButtonUp(0) && !m_mousePressed)
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
        m_mousePressed = false;
    }

    public void SetCurrTrans(Transform trans)
    {
        // Called from local OnMouseDown() so mouse has been pressed this frame
        m_mousePressed = true;


        // Clear or set the current transform
        _currentTrans = trans == _currentTrans ? null : trans;

        if(_currentTrans && currEditMode == EditMode.Translate)
        {
            m_ogPosition = _currentTrans.position;
            dif = _currentTrans.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


    public void MouseDown()
    {
        m_mousePressed = true;

        if (_currentTrans)
        {
            // Clear current transform selection
            _currentTrans = null;
        }
    }

    public void MouseUp()
    {
        m_mousePressed = true;

        if (_currentTrans && currEditMode == EditMode.Translate)
        {
            if (!m_canBeDropped)
            {
                // return object to original position
                _currentTrans.position = m_ogPosition;
            }

            _currentTrans.GetComponent<GridObject>().RevertToNormalState();

            // Clear current transform selection
            _currentTrans = null;
        }
    }

    // TODO: convert this to getter setter CurrTrans
    public Transform GetCurrentTrans()
    {

        return _currentTrans;
    }

    public void SetCanBeDropped(Transform trans, bool canBeDropped)
    {
        if(trans == _currentTrans)
        {
            Debug.Log($"can be dropped set to {canBeDropped}");
            m_canBeDropped = canBeDropped;
        }
    }
}

public enum EditMode { Rotate, Translate}
