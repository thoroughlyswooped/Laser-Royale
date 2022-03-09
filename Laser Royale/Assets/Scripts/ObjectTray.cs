using UnityEngine;

public class ObjectTray : MonoBehaviour
{
    private bool m_isOpen;
    [Range(.01f, 1f)]
    public float openSpeed;
    public float refreshRate;
    public Transform panel;
    public Transform buttonPos;
    public Transform button;
    public Transform buttonArrow;
    float smallNumber = 0.01f;

    public void Awake()
    {
        // Close the tray when loading the game
        panel.localScale = new Vector3(panel.localScale.x, 0f, panel.localScale.z);
        button.position = buttonPos.position;
    }

    public void ToggleOpenClose()
    {
        // Stop all invokes in case we are in the middle of opening/closing the tray
        CancelInvoke();
        if (m_isOpen)
        {
            InvokeRepeating("CloseTray", 0f, refreshRate);
        }
        else
        {
            InvokeRepeating("OpenTray", 0f, refreshRate);
        }
        m_isOpen = !m_isOpen;
    }

    void OpenTray()
    {
        float y = 1f;
        if (1 - panel.localScale.y > smallNumber)
        {
            y = Mathf.Lerp(panel.localScale.y, 1, openSpeed);
        }
        else
        {
            buttonArrow.rotation = Quaternion.Euler(0, 0, 0f);
        }
       
        panel.localScale = new Vector3(panel.localScale.x, y, panel.localScale.z);
        
        button.position = buttonPos.position;
    }

    void CloseTray()
    {
        float y = 0f;

        if (panel.localScale.y > smallNumber)
        {
            y = Mathf.Lerp(panel.localScale.y, 0, openSpeed);
        }
        else
        {
            buttonArrow.rotation = Quaternion.Euler(0, 0, 180f);
        }

        panel.localScale = new Vector3(panel.localScale.x, y, panel.localScale.z);

        button.position = buttonPos.position;
    }
}
