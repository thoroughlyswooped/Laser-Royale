using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool lockRotation;
    public bool lockTranslation;
    Rotate rotate;
    int m_ogSortLayer;
    LayerMask m_ogLayer;
    Color m_ogColor;
    SpriteRenderer m_sRenderer;

    public void Start()
    {
        if(Rotate.instance != null)
        {
            rotate = Rotate.instance;
        }
        m_sRenderer = GetComponent<SpriteRenderer>();
        m_ogSortLayer = m_sRenderer.sortingLayerID;
        m_ogColor = m_sRenderer.color;

        m_ogLayer = gameObject.layer;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rotate.GetCurrentTrans() == transform)
        {
            SetCantBeDroppedState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit collision");
        if (rotate.GetCurrentTrans() == transform)
        {
            RevertToNormalState();
        }
    }

    void SetCantBeDroppedState()
    {
        // disable the collider on this object
        GetComponent<Collider2D>().isTrigger = true;

        // move to layer above other objects
        m_sRenderer.sortingLayerName = "OverGrid";
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // color red so player knows object can't be dropped
        GetComponent<SpriteRenderer>().color = Color.red; //TODO: expose this color and store the sprite render in start 

        // update can be dropped in rotate script
        rotate.SetCanBeDropped(transform, false);
    }

    public void RevertToNormalState()
    {
        // disable the collider on this object
        GetComponent<Collider2D>().isTrigger = false;

        // move to layer above other objects
        m_sRenderer.sortingLayerID = m_ogSortLayer;
        gameObject.layer = m_ogLayer;

        // color red so player knows object can't be dropped
        GetComponent<SpriteRenderer>().color = m_ogColor ; //TODO: expose this color and store the sprite render in start 

        // update can be dropped in rotate script
        rotate.SetCanBeDropped(transform, true);
    }
}
