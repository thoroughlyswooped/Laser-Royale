using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool lockRotation;
    public bool lockTranslation;
    Rotate rotate;

    public void Start()
    {
        if(Rotate.instance != null)
        {
            rotate = Rotate.instance;
        }
    }

    private void OnMouseDown()
    {
        if (!lockTranslation && rotate.currEditMode == EditMode.Translate)
        {
            Rotate.instance.SetCurrTrans(transform);
        }
        else if(!lockRotation && rotate.currEditMode == EditMode.Rotate)
        {
            Rotate.instance.SetCurrTrans(transform);
        }
    }
}
