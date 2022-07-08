using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorsBehaviour : MonoBehaviour
{
    private GameObject vehicle = null;

    public Transform closestVehicle = null;
    private List<Transform> insideVehicles = new List<Transform> { };

    private float time = 0f;
    private float delay = 0.2f;

    // ======= UNITY FUNCTIONS =======
    //Runned at the Start
    private void Awake()
    {
        vehicle = transform.parent.transform.parent.gameObject;
    }

    //Runned Every Frame
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= delay) {
            time = 0;
            UpdateClosestVehicle();
        }
    }

    //Checks Collisions Enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VehicleBehaviour>() != null && collision.gameObject.transform != vehicle.transform)
        {
            insideVehicles.Add(collision.gameObject.transform);
            UpdateClosestVehicle();
            /*
            if (currentVehicle == null || Vector2.Distance(vehicle.transform.position, collision.transform.position) < Vector2.Distance(vehicle.transform.position, currentVehicle.position))
            {
                currentVehicle = collision.gameObject.transform;
                vehicle.GetComponent<VehicleBehaviour>().currentVehicle = currentVehicle;
            }*/
        }
    }

    //Checks Collisions Exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VehicleBehaviour>() != null)
        {
            insideVehicles.Remove(collision.gameObject.transform);
            UpdateClosestVehicle();
        }
    }


    private void UpdateClosestVehicle() {
        if (insideVehicles.Count == 0)
        {
            closestVehicle = null;
            return;
        }

        int index = 0;
        float minDist = Vector3.Distance(insideVehicles[0].position, vehicle.transform.position);
        for (int i = 1; i < insideVehicles.Count; ++i) {
            float dist = Vector3.Distance(insideVehicles[i].position, vehicle.transform.position);
            if (minDist > dist) {
                index = i;
                minDist = dist;
            }
        }

        closestVehicle = insideVehicles[index];
    }

}
