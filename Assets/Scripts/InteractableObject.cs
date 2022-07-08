using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isClicked;

    // ======= UNITY FUNCTIONS =======
    //Runned Every Frame
    private void Update()
    {
        if (isClicked)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
    }

    // ======= OBJECT FUNCTIONS =======
    //Executed When Object gets Clicked
    public void Clicked()
    {
        isClicked = true;
        if (transform.parent != null)
        {
            transform.GetComponentInParent<PointBehaviour>().isClicked = true;
        }
        else
        {
            GetComponent<PointBehaviour>().isClicked = true;
        }
    }

    //Executed When Object gets Clicked Again
    public void UnClicked()
    {
        isClicked = false;
        if (transform.parent != null)
        {
            transform.GetComponentInParent<PointBehaviour>().isClicked = false;
        }
        else
        {
            GetComponent<PointBehaviour>().isClicked = false;
        }
    }

}
