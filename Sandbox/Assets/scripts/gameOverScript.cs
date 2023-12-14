using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameOverScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI anomaliesText;
    [SerializeField] private GameObject Succses;
    [SerializeField] private GameObject Fail;
    [SerializeField] private GameObject LightsHint;
   public void Setup(int anomalies, int wahl, bool dimension1)
    {
        gameObject.SetActive(true);
        if (dimension1)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/EndScreens/Dimension1.jpg");
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/EndScreens/Dimension2.jpg");
        }
        switch (wahl)
        {
            case 1:
                Succses.gameObject.SetActive(true);
                break;
            case 2:
                Fail.gameObject.SetActive(true);
                anomaliesText.text = anomalies.ToString();
                break;
            case 3:
                LightsHint.gameObject.SetActive(true);
                anomaliesText.text = "Debug Mode: Lights not equal";
                break;
            default:
                break;
        }
       

    }
}
