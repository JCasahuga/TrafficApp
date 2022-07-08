using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{

    private float rotSpeed = 20;
    [SerializeField]
    [Range(0, 120)]
    private float targetSpeed = 40;
    private float targetSpeedReference = 40;

    [SerializeField]
    public float currentSpeed = 40;

    private float acceleration = 14.5f;
    private float deacceleration = 35.3f;

    private CubicCurvesGenerator ccG = null;

    public Vector3[] wayPoints = null;

    private int iterations = 32;
    public int currentIteration = 1;

    public Transform currentVehicle;

    public int roadType = 0; // 0 == Lane, 1 == Roundabout, 2 == Yield Right, 3 == Yield Left, 4 == Incorp Left, 5 == Incorp Right 
    // ML dividir en is<RoadType> -> no tenir en compte right o left

    public float startTime = 0;
    public float endTime = 0;

    public float distance = 0;

    public bool brake = true;

    // ======= UNITY FUNCTIONS =======
    //Runned at the Start
    void Awake()
    {
        ccG = FindObjectOfType<CubicCurvesGenerator>();
    }

    //Runned Every Frame
   /* void Update()
    {
        if (wayPoints.Length != 0)
        {
            SetWaypointPositions();

            // Checks if cars are close and updates the speed accordingly
            if (currentVehicle != null || true)
            {
                UpdateTargetSpeed();
            }
            brakeAgent.currentSpeed = currentSpeed;


            /*if (currentVehicle != null)
            {
                brakeAgent.dCarInFront = Vector2.Distance(transform.position, currentVehicle.position);
                brakeAgent.sCarInFront = currentVehicle.GetComponent<VehicleBehaviour>().currentSpeed;
            }
            else
            {
                brakeAgent.dCarInFront = 100000;
                brakeAgent.sCarInFront = 100000;
            }*/
       /* }
    }*/

    private void FixedUpdate()
    {
        if (wayPoints.Length != 0)
        {
            SetWaypointPositions();
            // Checks if cars are close and updates the speed accordingly
            UpdateTargetSpeed();
        }
    }

    //Checks Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PointBehaviour>())
        {
            endTime = Time.time - startTime;
            if (wayPoints.Length > 0 && Vector2.Distance(collision.transform.position, wayPoints[3]) > 1)
            {
                return;
            }
            if (collision.gameObject.GetComponent<PointBehaviour>().nextHandles.Count != 0)
            {
                startTime = Time.time;
                currentIteration = 1;
                wayPoints = collision.gameObject.GetComponent<PointBehaviour>().GetNextWayPoints();
                roadType = collision.gameObject.GetComponent<PointBehaviour>().GetRoadType();
            } else {
                //Destroy(this.gameObject);
            }
        }
    }

    // ======= OBJECT FUNCTIONS =======
    //Sets Waypoint
    private void SetWaypointPositions()
    {
        Move(ccG.CalculateQuadraticBezierPoint((float)currentIteration/(float)iterations, wayPoints));
    }

    //Move Towards points
    private void Move(Vector3 wp)
    {
        Vector2 direction = wp - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotSpeed);
        Vector3 pastPosition = this.transform.position;
        this.transform.position = Vector2.MoveTowards(transform.position, wp, 0.5168f * currentSpeed * Time.deltaTime / 2);
        distance += Vector3.Distance(pastPosition, this.transform.position);

        if (Vector2.Distance(transform.position, wp) < 0.125f)
        {
            if (currentIteration < iterations)
            {
                currentIteration++;
            }
            else
            {
                wayPoints = new Vector3[] { };
            }
        }

        //currentSpeed = targetSpeed;

        if (currentSpeed < targetSpeed - 0.01f || currentSpeed > targetSpeed + 0.01f)
        {
            if (targetSpeed > currentSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime * Mathf.Sign(targetSpeed - currentSpeed);
            }
            else if (targetSpeed < currentSpeed)
            {
                currentSpeed += deacceleration * Time.deltaTime * Mathf.Sign(targetSpeed - currentSpeed);
            }
            currentSpeed = Mathf.Max(0, currentSpeed);
        }
        else
        {
            currentSpeed = targetSpeed;
        }
    }

    //Updates SpeedTarget Dynamically
    private void UpdateTargetSpeed()
    {
        if (brake) targetSpeed = 0;
        else targetSpeed = targetSpeedReference;
        /*
        float d = Vector2.Distance(transform.position, currentVehicle.position);

        if (isLane)
        {
            if (d < 15.15f / (120 / targetSpeedReference) && d > 0.005f * targetSpeedReference)
            {
                targetSpeed = targetSpeedReference * (d / 15.15f);
            }
            else if (d < 0.005f * targetSpeedReference)
            {
                targetSpeed = 0;
            }
        }*/
    }

    //Removes Front Car and Sets Speed to Normal
    public void NoCarInteracting()
    {
        currentVehicle = null;

        //targetSpeed = targetSpeedReference;
    }

    //Updates Visuals
    public void UpdateSize(float size)
    {
        transform.GetChild(0).localScale = new Vector2(size, size);
    }

    //Updates SpeedTarget
    public void SetSpeed(int i)
    {
        targetSpeed = i;
        targetSpeedReference = i;
    }

}
