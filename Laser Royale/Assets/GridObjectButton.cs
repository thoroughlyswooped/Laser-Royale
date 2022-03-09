using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridObjectButton : MonoBehaviour
{
    bool m_isDragging;
    bool m_canBeDropped;

    Vector2 dif;

    public GridObject gridObject;
    public Transform gridObjectInGameParent;

    public void CustomOnMouseDown()
    {
        Debug.Log("OnButtonClick");

        // hide button image
        GetComponent<Image>().enabled = false;

        // show actual game object
        gridObject.gameObject.SetActive(true);

        // record difference between mouse and center of object
        gridObject.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
        dif = gridObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_isDragging = true;
    }

    public void MouseOverCustom()
    {

    }

    // this might be more performant if invoked repeating when the player is actually moving the object not all the time
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        if (m_isDragging)
        {
            gridObject.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + dif;
        }
    }

    void MouseUp()
    {
        m_isDragging = false;

        // if the object can be dropped, drop it
        if (m_canBeDropped)
        {
            gridObject.transform.parent = gridObjectInGameParent;
        }
    }
    
}
