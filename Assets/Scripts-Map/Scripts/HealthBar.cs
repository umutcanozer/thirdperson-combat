using System;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Camera mainCam;

    public void OnUpdateHealth(float currentHealth, float maxHealth)
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    private void Update()
    {
        //temporary solution
        if (this.CompareTag("PlayerCanvas")) return;
        healthBarImage.transform.rotation = mainCam.transform.rotation;
    }
}