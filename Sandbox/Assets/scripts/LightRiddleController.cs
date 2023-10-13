using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRiddleController : MonoBehaviour
{

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    [SerializeField] private bool disableBtn1 = false;
    [SerializeField] private bool disableBtn2 = false;
    [SerializeField] private bool disableBtn3 = false;

    [SerializeField] private bool light1Active = false;
    [SerializeField] private bool light2Active = false;
    [SerializeField] private bool light3Active = false;
    [SerializeField] private bool light4Active = false;
    
    public void pressedButton(GameObject o)
    {
        if (o.Equals(button1) && !disableBtn1)
        {
            light1.SetActive(!light1.activeSelf);
            light2.SetActive(!light2.activeSelf);
            light4.SetActive(!light4.activeSelf);
        }
        else if (o.Equals(button2) && !disableBtn2)
        {
            light1.SetActive(!light1.activeSelf);
            light3.SetActive(!light3.activeSelf);
        }
        else if (o.Equals(button3) && !disableBtn3)
        {
            light2.SetActive(!light2.activeSelf);
            light4.SetActive(!light4.activeSelf);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        light1.SetActive(light1Active);
        light2.SetActive(light2Active);
        light3.SetActive(light3Active);
        light4.SetActive(light4Active);
    }

    public bool getState(GameObject o)
    {
        if (o.Equals(button1))
        {
            return disableBtn1;
        }else if (o.Equals(button2))
        {
            return disableBtn2;
        }else if (o.Equals(button3))
        {
            return disableBtn3;
        }
        return false;
    }
}

