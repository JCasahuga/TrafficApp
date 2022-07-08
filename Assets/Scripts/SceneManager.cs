using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] managers = null;

    public void SetMode(int e)
    {
        for (int i = 0; i < managers.Length; i++)
        {
            if (i != e)
            {
                managers[i].SetActive(false);
            }
            else
            {
                managers[i].SetActive(true);
            }
        }
    }

}
