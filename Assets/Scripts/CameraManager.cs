using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // ======= UNITY FUNCTIONS =======
    //Runned Every Frame
    private void Update()
    {
        Move();
        Zoom();
        if (Input.GetAxis("Zoom") != 0)
        {
            UpdateVisuals();
        }
    }

    // ======= OBJECT FUNCTIONS =======
    //Moving Camera
    private void Move()
    {
        float hor = Input.GetAxis("Horizontal") * Mathf.Sqrt(GetComponent<Camera>().orthographicSize / 10);
        float ver = Input.GetAxis("Vertical") * Mathf.Sqrt(GetComponent<Camera>().orthographicSize / 10);
        transform.position += new Vector3(hor, ver, 0);
    }

    //Zooming
    private void Zoom()
    {
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize + Input.GetAxis("Zoom") * Mathf.Sqrt(GetComponent<Camera>().orthographicSize / 10), 1, 50);
    }

    //Updates Icons in the Scene
    public void UpdateVisuals()
    {
        float size = Mathf.Clamp(GetComponent<Camera>().orthographicSize / 5, 1, 10);
        foreach (PointBehaviour p in FindObjectsOfType<PointBehaviour>())
        {
            p.UpdateSize(size);
        }
        foreach (VehicleBehaviour v in FindObjectsOfType<VehicleBehaviour>())
        {
            v.UpdateSize(size/4);
        }
        foreach (VehicleSpawner vS in FindObjectsOfType<VehicleSpawner>())
        {
            vS.UpdateSize(size*1.5f);
        }
    }

}
