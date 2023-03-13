using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthWithUI : Health
{
    public Slider healthBar;
    public Image healthIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
        }
    }
    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthBar)
        {
            healthBar.value = currentHealth;
        }
        else if (healthIcon)
        {
            healthIcon.fillAmount = currentHealth / maxHealth;
        }
    }
}
