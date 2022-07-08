using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] childs = null;

    private void OnEnable()
    {
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i] != null)
                childs[i].SetActive(true);
        }
    }


    private void OnDisable()
    {
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i] != null)
                childs[i].SetActive(false);
        }
    }
}
