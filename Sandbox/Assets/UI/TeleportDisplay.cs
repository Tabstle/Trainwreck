using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class TeleportDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int teleportCooldown = 5;
    private int currentCooldown = 0;
    [SerializeField] private TextMeshProUGUI textMeshProComponent;


    // Update is called once per frame
    void Update()
    {
        if (currentCooldown > 0)
        {
            textMeshProComponent.text = currentCooldown.ToString() + " sec";
        }
        else
        {
            textMeshProComponent.text = "Ready";
        }

    }

    public void startCooldown()
    {
        currentCooldown = teleportCooldown;
        Task.Run(async () =>
        {
            while (currentCooldown > 0)
            {
                await Task.Delay(1000);
                currentCooldown--;
            }
        });
    }

    public bool isOnCooldonw()
    {
        return currentCooldown > 0;
    }
}


