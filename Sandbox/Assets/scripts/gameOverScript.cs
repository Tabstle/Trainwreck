using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class gameOverScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI anomaliesText;
    [SerializeField] private GameObject Succses;
    [SerializeField] private GameObject Fail;
    [SerializeField] private GameObject LightsHint;

    [SerializeField] private Sprite dimension1Sprite;
    [SerializeField] private Sprite dimension2Sprite;



   public void Setup(int anomalies, int wahl, bool dimension1)
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        if (dimension1)
        {
            gameObject.GetComponent<Image>().sprite = dimension1Sprite;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = dimension2Sprite;
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

    public void RestartButton()
    {
        gameObject.SetActive(false);
        Succses.gameObject.SetActive(false);
        Fail.gameObject.SetActive(false);
        LightsHint.gameObject.SetActive(false);
        SceneManager.LoadScene("Game",LoadSceneMode.Single);
    }

    public void keepGoingButton()
    {
        gameObject.SetActive(false);
        Succses.gameObject.SetActive(false);
        Fail.gameObject.SetActive(false);
        LightsHint.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
