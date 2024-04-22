using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHide_Show : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameObject.activeSelf == true)
            {
                gameObject.SetActive(false);

            }
            else
            {
                gameObject.SetActive(true);
            }

        }
    }
}
