using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemsPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject vehicleSpawnerPref = null;


    // ======= UNITY FUNCTIONS =======
    //Runned at the Start
    private void Awake()
    {
    }

    //Runned Every Frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (hit)
            {
                if (hit.collider.GetComponent<PointBehaviour>())
                {
                    Instantiate(vehicleSpawnerPref, hit.transform.position, Quaternion.identity);
                    FindObjectOfType<CameraManager>().UpdateVisuals();
                }
            }
        }
    }


    // ======= OBJECT FUNCTIONS =======


}
