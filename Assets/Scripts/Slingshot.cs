using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    public float velocity = 8f;

    [Header("Set Dynamicaly")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody progectileRigidbody;

    

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

   private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }
    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile);
        projectile.transform.position = launchPos;
        progectileRigidbody = projectile.GetComponent<Rigidbody>(); 
        progectileRigidbody.isKinematic = true;
        
    }
    private void OnMouseExit() {
        launchPoint.SetActive(false);
    }

    private void Update()
    {
        if (!aimingMode) { return; }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMegnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMegnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMegnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            aimingMode=false;
            progectileRigidbody.isKinematic = false;
            progectileRigidbody.velocity = -mouseDelta * velocity;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemotion.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
