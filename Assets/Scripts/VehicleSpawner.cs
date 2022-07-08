using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject vehiclePref = null;

    private int carColliding = 0;

    // ======= UNITY FUNCTIONS =======
    //Runned at the Start
    private void Awake()
    {
        StartCoroutine(SpawnVehicles());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VehicleBehaviour>())
        {
            carColliding++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VehicleBehaviour>())
        {
            carColliding--;
        }
    }


    // ======= OBJECT FUNCTIONS =======
    private IEnumerator SpawnVehicles()
    {
        while (true)
        {
            if (carColliding < 1)
            {
                Instantiate(vehiclePref, this.transform.position, Quaternion.identity);
                FindObjectOfType<CameraManager>().UpdateVisuals();
                yield return new WaitForSeconds(4f);
            }
            yield return null;
        }
    }

    //Updates Visuals
    public void UpdateSize(float size)
    {
        transform.GetChild(0).localScale = new Vector2(size, size);
    }



}
