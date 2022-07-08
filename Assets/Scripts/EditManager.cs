using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EditManager : MonoBehaviour
{
    [SerializeField]
    private Transform propertiesHolder = null;

    [SerializeField] private GameObject nonEditableText = null;
    //[SerializeField] private GameObject editableText = null;

    // ======= UNITY FUNCTIONS =======
    //Runned Every Frame
    private void Update() {
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
                    if (hit.collider.GetComponent<PointBehaviour>()) {
                        
                        float l = hit.collider.GetComponent<PointBehaviour>().GetLength();
                        Debug.Log(l);
                        GameObject lengthText = Instantiate(nonEditableText); 
                        lengthText.transform.SetParent(propertiesHolder);
                        lengthText.transform.GetChild(1).GetComponent<TMP_InputField>().text = (Mathf.Round(l*100)/100).ToString() + " m";
                    }
                    Debug.Log("Interactable");
                }
            }
            else
            {

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }

    }
}
