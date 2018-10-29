using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Lazer : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string splitTag;
    public string TargetTag;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    public GameObject sparks;

    public LayerMask mask;

    //private bool hitTarget;
    bool laserHit = false;
    //public GameObject door;
    public GameObject button;

    void Start()
    {
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    void Update()
    {
        //if (hitTarget == true && door.transform.position.y > -10)
        //{
        //    Vector3 pos = door.transform.position;
        //    pos.y -= (3 * Time.deltaTime);
        //    door.transform.position = pos;
        //}

            if (timer >= updateFrequency)
            {
                timer = 0;
                StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;

        
    }

    IEnumerator RedrawLaser()
    {
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 prevDirection = Vector3.zero;
        Vector3 lastLaserPosition = transform.position; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;

        bool notHitting = false;
        

        while (loopActive)
        {
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance, mask) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == TargetTag)))
            {
                notHitting = false;
                if (hit.transform.gameObject.GetComponent<LaserButton>() != null)
                {
                    button = hit.transform.gameObject;
                }

                sparks.SetActive(true);
                sparks.transform.position = mLineRenderer.GetPosition(mLineRenderer.positionCount - 1);

                laserReflected++;
                vertexCounter += 3;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(.05f, .05f);
                

                if (hit.transform.gameObject.tag == bounceTag)
                {
                    lastLaserPosition = hit.point;
                    prevDirection = laserDirection;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                }
                if (hit.transform.gameObject.tag == splitTag)
                {
                        laserSplit++;
                        Object go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
                }
            }
            else
            {
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                notHitting = true;
                sparks.SetActive(false);
                loopActive = false;
            }

            // did we stop hitting something
            if (notHitting && button != null)
            {
                laserHit = false;
                button.GetComponent<LaserButton>().OnLaserStopHitting();
                button = null;
            }

            // do a thing if we hit something
            if (button != null)
            {
                if(laserHit == false)
                {
                    button.GetComponent<LaserButton>().OnLaserHit();
                    laserHit = true;
                }
                else if (laserHit == true)
                {
                    button.GetComponent<LaserButton>().OnLaser();
                }
                
            }

            if (laserReflected > maxBounce)
                loopActive = false;
        }
        

        yield return new WaitForEndOfFrame();
    }
}