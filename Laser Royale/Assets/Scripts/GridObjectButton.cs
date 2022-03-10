using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridObjectButton : MonoBehaviour
{
    bool m_canBeDropped;
    Rotate rotate;

    Vector2 dif;

    public GridObject gridObject;
    public Transform gridObjectInGameParent;

    public void Start()
    {
        if (Rotate.instance != null)
        {
            rotate = Rotate.instance;
        }
       
    }

    public void CustomOnMouseDown()
    {
        if (rotate.currEditMode == EditMode.Translate)
        {
            // hide button image
            GetComponent<Image>().enabled = false;

            // show actual game object
            gridObject.gameObject.SetActive(true);

            // record difference between mouse and center of object
            gridObject.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
            dif = gridObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // set rotate current trans
            rotate.SetCurrTrans(gridObject.transform, dif);
        }
    }


    // this might be more performant if invoked repeating when the player is actually moving the object not all the time
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    void MouseUp()
    {
        // if the object can be dropped, drop it
        if (m_canBeDropped)
        {
            gridObject.transform.parent = gridObjectInGameParent;
        }
    }
    
}
