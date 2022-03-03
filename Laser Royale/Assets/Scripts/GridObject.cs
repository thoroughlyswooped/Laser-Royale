using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool lockRotation;
    public bool lockTranslation;


    private void OnMouseDown()
    {
        if(!(lockRotation && lockTranslation))
        {
            //TODO: Ezra: add functionality for translation or mode swapping
            Rotate.instance.SetCurrTrans(transform);
        }
    }
}
