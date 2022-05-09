using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;

namespace Lightbug.LaserMachine
{



public class LaserMachine : GridObject {
    public EdgeCollider2D edgeCollider;
    /*struct LaserElement 
    {
        public Transform transform;        
        public LineRenderer lineRenderer;
        public GameObject sparks;
        public bool impact;
        
    };*/

    HashSet<LaserElement> elementsList = new HashSet<LaserElement>();

        HashSet<GameObject> laserPool = new HashSet<GameObject>();
    

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
        public int currLaserCount = 0;
        public int poolSize = 15; 

        public static LaserMachine instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("There is already a laser machine and you are trying to create another one.");
            }
        }

        void OnEnable()
    {
        m_currentProperties = m_overrideExternalProperties ? m_inspectorProperties : m_data.m_properties;
        

        m_currentProperties.m_initialTimingPhase = Mathf.Clamp01(m_currentProperties.m_initialTimingPhase);
        m_time = m_currentProperties.m_initialTimingPhase * m_currentProperties.m_intervalTime;
        

        float angleStep = m_currentProperties.m_angularRange / m_currentProperties.m_raysNumber;        

        m_assignSparks = m_data.m_laserSparks != null;
        m_assignLaserMaterial = m_data.m_laserMaterial != null;

            for(int i = 0; i < poolSize; i++)
            {
                laserPool.Add(CreateNewLaser(transform, angleStep));
            }

        for (int i = 0; i < m_currentProperties.m_raysNumber ; i++)
        {
                //CreateNewLaser(transform, angleStep);
                GetLaserFromPool(transform, angleStep);
        }
        
	}

        public GameObject CreateNewLaser(Transform trans, float angleStep)
        {

            LaserElement element = new LaserElement();
            GameObject newObj = new GameObject("lineRenderer_" + currLaserCount.ToString());

            if (m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics2D)
                newObj.transform.position = (Vector2)trans.position;
            else
                newObj.transform.position = trans.position;

            newObj.transform.rotation = trans.rotation;
            newObj.transform.Rotate(Vector3.up, currLaserCount * angleStep);
            //newObj.transform.position += newObj.transform.forward * m_currentProperties.m_minRadialDistance;

            newObj.AddComponent<LineRenderer>();

            if (m_assignLaserMaterial)
                newObj.GetComponent<LineRenderer>().material = m_data.m_laserMaterial;

            newObj.GetComponent<LineRenderer>().receiveShadows = false;
            newObj.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            newObj.GetComponent<LineRenderer>().startWidth = m_currentProperties.m_rayWidth;
            newObj.GetComponent<LineRenderer>().useWorldSpace = true;
            newObj.GetComponent<LineRenderer>().SetPosition(0, newObj.transform.position);
            newObj.GetComponent<LineRenderer>().SetPosition(1, newObj.transform.position + trans.forward * m_currentProperties.m_maxRadialDistance);
            newObj.transform.SetParent(trans);
            newObj.GetComponent<LineRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            edgeCollider = newObj.GetComponent<EdgeCollider2D>();


            if (m_assignSparks)
            {
                GameObject sparks = Instantiate(m_data.m_laserSparks);
                sparks.transform.SetParent(newObj.transform);
                sparks.SetActive(false);
                element.sparks = sparks;
            }
           // element.light = newObj.AddComponent<Light2D>();
           // element.light.lightType = Light2D.LightType.Freeform;
            

            newObj.tag = "LASER"; //New tag
            element.transform = newObj.transform;
            element.lineRenderer = newObj.GetComponent<LineRenderer>();
            element.impact = false;

            newObj.AddComponent<LaserElementComponent>().element = element;

            //elementsList.Add(element);

            //currLaserCount++;

            return newObj;
        }


        public GameObject GetLaserFromPool(Transform trans, float angleStep)
        {
            if(laserPool.Count == 0)
            {
                // Pool is out of lasers, we need to make some more
                for (int i = 0; i < 3; i++)
                {
                    laserPool.Add(CreateNewLaser(transform, angleStep));
                }

            }
            GameObject newObj = laserPool.First();//new GameObject("lineRenderer_" + currLaserCount.ToString());
            laserPool.Remove(newObj);
            newObj.SetActive(true);

            LaserElement element = newObj.GetComponent<LaserElementComponent>().element;

            if (m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics2D)
                newObj.transform.position = (Vector2)trans.position;
            else
                newObj.transform.position = trans.position;

            newObj.transform.rotation = trans.rotation;
            newObj.transform.Rotate(Vector3.up, currLaserCount * angleStep);
            newObj.GetComponent<LineRenderer>().SetPosition(0, newObj.transform.position);
            newObj.GetComponent<LineRenderer>().SetPosition(1, newObj.transform.position + trans.forward * m_currentProperties.m_maxRadialDistance);
            newObj.transform.SetParent(trans);
            edgeCollider = newObj.GetComponent<EdgeCollider2D>();


            if (m_assignSparks)
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
            element.layerMask = m_currentProperties.m_layerMask | LayerMask.NameToLayer("Portal_2");
            element.layerMask = element.layerMask | LayerMask.NameToLayer("Portal_1");

            elementsList.Add(element);

            currLaserCount++;

            return newObj;
        }

        public void ReturnLaserToPool(GameObject laserObj)
        {
            Debug.Log($"set {laserObj.name} to inactive");
            laserObj.SetActive(false);
            laserPool.Add(laserObj);
            currLaserCount--;

            elementsList.Remove(laserObj.GetComponent<LaserElementComponent>().element); // this might have to be changed to a flag that is then removed after the itteration in update
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
        
        for (int i = 0; i < elementsList.Count; i++)
        {
            LaserElement element = elementsList.ElementAt(i);
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
                        element.layerMask
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
                        LayerMask elemogLayer = element.transform.gameObject.layer;

                        element.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        hitInfo2D = Physics2D.Linecast( 
                        element.transform.position,
                        element.transform.position + element.transform.forward * m_currentProperties.m_maxRadialDistance,
                        m_currentProperties.m_layerMask 
                    );
                    
                    // Reset layer 
                    gameObject.layer = ogLayer;
                        element.transform.gameObject.layer = elemogLayer;




                    if (hitInfo2D.collider)
                    {
                        List<Vector2> linePoints = new List<Vector2>();
                        if (hitInfo2D.collider.CompareTag("Hittable") )
                        {
                            // We hit something that is marked as hittable, call recursively to hit functions
                            linePoints = new List<Vector2>(hitInfo2D.collider.gameObject.GetComponent<HittableObject>().Hit(element.transform.forward, hitInfo2D, m_currentProperties.m_maxRadialDistance, element.transform.gameObject));

                                //add the start point to the begining of the list as it was removed when setting linepoints
                                linePoints.Insert(0, element.transform.position);
                        }

                        //Line Points is different from default
                        if (linePoints.Count != 0)
                        {
                            element.lineRenderer.positionCount = linePoints.Count;

                            for (int j = 0; j < linePoints.Count; j++)
                            {
                                Vector2 point = linePoints[j];
                                element.lineRenderer.SetPosition(j, point);
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
