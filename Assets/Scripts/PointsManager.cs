using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject point = null;

    [SerializeField]
    private PointBehaviour addedPoint;
    [SerializeField]
    private PointBehaviour selectedPoint;

    public GameObject clickedObject;

    private int currentMode = 0;

    // ======= UNITY FUNCTIONS =======
    //Runned Every Frame
    private void Update()
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
                if (hit.collider.GetComponent<InteractableObject>())
                {
                    if (currentMode == 1)
                    {
                        if (addedPoint != null)
                        {
                            addedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                        }
                        addedPoint = hit.collider.GetComponent<PointBehaviour>();
                        if (addedPoint != null) {
                            addedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
                        }
                        selectedPoint = null;
                    }
                    else if (currentMode == 2 && hit.collider.GetComponent<PointBehaviour>())
                    {
                        DestroyPoint(hit.collider.gameObject);
                    }
                    else if (currentMode == 3 && hit.collider.GetComponent<PointBehaviour>())
                    {
                        if (hit.collider.gameObject != addedPoint.gameObject)
                        {
                            Fill(hit.collider.transform);
                        }
                    }
                    else if (currentMode == 4)
                    {
                        hit.collider.GetComponent<InteractableObject>().Clicked();
                        clickedObject = hit.collider.gameObject;
                    }
                }
            }
            else
            {
                if (currentMode == 0)
                {
                    AddPoint();
                } else if (currentMode == 1) {
                    if (addedPoint != null) {
                        addedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                        addedPoint = null;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (clickedObject != null)
            {
                clickedObject.GetComponent<InteractableObject>().UnClicked();
                clickedObject = null;
            }
        }

        if (clickedObject != null)
        {
            if (clickedObject.GetComponent<PointBehaviour>() != null)
            {
                clickedObject.GetComponent<PointBehaviour>().DrawCurves(0);
            }
            else
            {
                clickedObject.transform.parent.GetComponent<PointBehaviour>().DrawCurves(0);
            }
        }
    }

    // ======= OBJECT FUNCTIONS =======
    //Adds point
    private void AddPoint()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject p = Instantiate(point, pos, Quaternion.identity);

        selectedPoint = addedPoint;
        addedPoint = p.GetComponent<PointBehaviour>();

        addedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;

        if (selectedPoint != null && addedPoint != null)
        {
            selectedPoint.AddPoint(addedPoint.transform, false);
            addedPoint.AddPoint(selectedPoint.transform, true);
            selectedPoint.DrawCurves(0);
            selectedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }

        FindObjectOfType<CameraManager>().UpdateVisuals();
    }

    //Destroys point
    private void DestroyPoint(GameObject hit)
    {
        hit.GetComponent<PointBehaviour>().DeletePoint();

        if (hit.gameObject == selectedPoint)
        {
            selectedPoint = null;
        }
        if (hit.gameObject == selectedPoint)
        {
            addedPoint = null;
        }

    }

    //Fills
    private void Fill(Transform nextPoint)
    {
        selectedPoint = addedPoint;

        if (selectedPoint != null && addedPoint != null)
        {
            addedPoint = nextPoint.GetComponent<PointBehaviour>();
            addedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
            selectedPoint.GetComponentInChildren<SpriteRenderer>().color = Color.red;

            selectedPoint.AddPoint(addedPoint.transform, false);
            addedPoint.AddPoint(selectedPoint.transform, true);

            addedPoint.DrawCurves(0);

        }

        FindObjectOfType<CameraManager>().UpdateVisuals();
    }


    //Sets the mode
    public void CurrentMode(int i)
    {
        currentMode = i;
    }
    

}
