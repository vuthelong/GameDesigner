using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject Inventory;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputkeyboard();
    }

    private void Inputkeyboard()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

        }
        else if (Input.GetKey(KeyCode.M))
        {

        }
        else if (Input.GetKey(KeyCode.I))
        {

        }
        else if (!Input.GetKey(KeyCode.E))
        {

        }
    }
}
