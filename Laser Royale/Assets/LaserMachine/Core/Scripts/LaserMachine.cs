using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lightbug.LaserMachine
{



public class LaserMachine : GridObject {
    public EdgeCollider2D edgeCollider;
    struct LaserElement 
    {
        public Transform transform;        
        public LineRenderer lineRenderer;
        public GameObject sparks;
        public bool impact;
        
    };

    List<LaserElement> elementsList = new List<LaserElement>();
    

    [Header("External Data")]
    
    [SerializeField] LaserData m_data;

    [Tooltip("This variable is true by default, all the inspector properties will be overridden.")]
    [SerializeField] bool m_overrideExternalProperties = true;

    [SerializeField] LaserProperties m_inspectorProperties = new LaserProperties();
    

    LaserProperties m_currentProperties;// = new LaserProperties();
        
    float m_time = 0;
    bool m_active = true;
    bool m_assignLaserMaterial;
    bool m_assignSparks;
  		
    

    void OnEnable()
    {
        m_currentProperties = m_overrideExternalProperties ? m_inspectorProperties : m_data.m_properties;
        

        m_currentProperties.m_initialTimingPhase = Mathf.Clamp01(m_currentProperties.m_initialTimingPhase);
        m_time = m_currentProperties.m_initialTimingPhase * m_currentProperties.m_intervalTime;
        

        float angleStep = m_currentProperties.m_angularRange / m_currentProperties.m_raysNumber;        

        m_assignSparks = m_data.m_laserSparks != null;
        m_assignLaserMaterial = m_data.m_laserMaterial != null;

        for (int i = 0; i < m_currentProperties.m_raysNumber ; i++)
        {
            LaserElement element = new LaserElement();

            GameObject newObj = new GameObject("lineRenderer_" + i.ToString());

            if( m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics2D )
                newObj.transform.position = (Vector2)transform.position;
            else
                newObj.transform.position = transform.position;

            newObj.transform.rotation = transform.rotation;
            newObj.transform.Rotate( Vector3.up , i * angleStep );
            newObj.transform.position += newObj.transform.forward * m_currentProperties.m_minRadialDistance;

            newObj.AddComponent<LineRenderer>();

            if( m_assignLaserMaterial )
                newObj.GetComponent<LineRenderer>().material = m_data.m_laserMaterial;

            newObj.GetComponent<LineRenderer>().receiveShadows = false;
            newObj.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            newObj.GetComponent<LineRenderer>().startWidth = m_currentProperties.m_rayWidth;
            newObj.GetComponent<LineRenderer>().useWorldSpace = true;
            newObj.GetComponent<LineRenderer>().SetPosition(0, newObj.transform.position);
            newObj.GetComponent<LineRenderer>().SetPosition(1, newObj.transform.position + transform.forward * m_currentProperties.m_maxRadialDistance);
            newObj.transform.SetParent(transform);
            edgeCollider = newObj.GetComponent<EdgeCollider2D>();

            
            if( m_assignSparks )
            {
                GameObject sparks = Instantiate(m_data.m_laserSparks);
                sparks.transform.SetParent(newObj.transform);
                sparks.SetActive(false);
                element.sparks = sparks;
            }

            newObj.tag = "LASER"; //New tag
            element.transform = newObj.transform;
            element.lineRenderer = newObj.GetComponent<LineRenderer>();
            element.impact = false;

            elementsList.Add(element);
        }
        
	}
        
       
	void Update () {

        if (m_currentProperties.m_intermittent)
        {
            m_time += Time.deltaTime;

            if (m_time >= m_currentProperties.m_intervalTime)
            {
                m_active = !m_active;
                m_time = 0;
                return;
            }
        }

        RaycastHit2D hitInfo2D;
        RaycastHit hitInfo3D;
        
        foreach (LaserElement element in elementsList)
        {
            if ( m_currentProperties.m_rotate )
            {
                if ( m_currentProperties.m_rotateClockwise )
                    element.transform.RotateAround(transform.position, transform.up, Time.deltaTime * m_currentProperties.m_rotationSpeed);    //rotate around Global!!
                else
                    element.transform.RotateAround(transform.position, transform.up, -Time.deltaTime * m_currentProperties.m_rotationSpeed);
            }


            if (m_active)
            {
                element.lineRenderer.enabled = true;
                element.lineRenderer.SetPosition(0, element.transform.position);

                if(m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics3D)
                {
                    Physics.Linecast(
                        element.transform.position,
                        element.transform.position + element.transform.forward * m_currentProperties.m_maxRadialDistance,
                        out hitInfo3D ,
                        m_currentProperties.m_layerMask
                    );  


                    if (hitInfo3D.collider)
                    {
                        element.lineRenderer.SetPosition(1, hitInfo3D.point);

                        if( m_assignSparks )
                        {
                            element.sparks.transform.position = hitInfo3D.point; //new Vector3(rhit.point.x, rhit.point.y, transform.position.z);
                            element.sparks.transform.rotation = Quaternion.LookRotation( hitInfo3D.normal ) ;
                        }

                        /*
                        EXAMPLE : In this line you can add whatever functionality you want, 
                        for example, if the hitInfoXD.collider is not null do whatever thing you wanna do to the target object.
                        DoAction();
                        */

                    }
                    else
                    {
                        element.lineRenderer.SetPosition(1, element.transform.position + element.transform.forward * m_currentProperties.m_maxRadialDistance);

                    }

                    if( m_assignSparks )
                        element.sparks.SetActive( hitInfo3D.collider != null );
                }

                else // 2-D Physics here
                {
                    // Don't hit this object with line cast by changing to new layer temporarily
                    LayerMask ogLayer = gameObject.layer;
                    gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                    hitInfo2D = Physics2D.Linecast( 
                        element.transform.position,
                        element.transform.position + element.transform.forward * m_currentProperties.m_maxRadialDistance,
                        m_currentProperties.m_layerMask 
                    );
                    
                    // Reset layer 
                    gameObject.layer = ogLayer;


                    
                    if (hitInfo2D.collider)
                    {
                        List<Vector2> linePoints = new List<Vector2>();
                        if (hitInfo2D.collider.CompareTag("Hittable") )
                        {
                            // We hit something that is marked as hittable, call recursively to hit functions
                            linePoints = new List<Vector2>(hitInfo2D.collider.gameObject.GetComponent<HittableObject>().Hit(element.transform.forward, hitInfo2D, m_currentProperties.m_maxRadialDistance));

                                //add the start point to the begining of the list as it was removed when setting linepoints
                                linePoints.Insert(0, element.transform.position);
                        }

                        //Line Points is different from default
                        if (linePoints.Count != 0)
                        {
                            element.lineRenderer.positionCount = linePoints.Count;

                            for (int i = 0; i < linePoints.Count; i++)
                            {
                                Vector2 point = linePoints[i];
                                element.lineRenderer.SetPosition(i, point);
                            }
                        }
                        else
                        {
                            element.lineRenderer.positionCount = 2;
                            element.lineRenderer.SetPosition(1, hitInfo2D.point);
                        }

                        if( m_assignSparks )
                        {
                            Vector3 lastPoint = element.lineRenderer.GetPosition(element.lineRenderer.positionCount - 1);
                            element.sparks.transform.position = lastPoint;
                            Vector2 lastDir = (element.lineRenderer.GetPosition(element.lineRenderer.positionCount - 2) - lastPoint).normalized;
                            element.sparks.transform.rotation = Quaternion.LookRotation(lastDir);//hitInfo2D.normal ) ;
                        }
                        
                        /*
                        EXAMPLE : In this line you can add whatever functionality you want, 
                        for example, if the hitInfoXD.collider is not null do whatever thing you wanna do to the target object.
                        DoAction();
                        */

                    }
                    else
                    {
                        // We didn't hit anything, put point at max range in correct direction
                        element.lineRenderer.SetPosition(1, element.transform.position + element.transform.forward * m_currentProperties.m_maxRadialDistance);

                    }

                    // Only activate sparks if we hit something
                    // TODO: Ezra: this could be a cool place to have different particle effects for different hit objects
                    if( m_assignSparks )
                        element.sparks.SetActive( hitInfo2D.collider != null );

                }              

                





            }
            else
            {
                element.lineRenderer.enabled = false;

                if( m_assignSparks )
                    element.sparks.SetActive(false);
            }
        }
        
    }

    /*
    EXAMPLE : 
    void DoAction()
    {

    }
    */

	
}


}
