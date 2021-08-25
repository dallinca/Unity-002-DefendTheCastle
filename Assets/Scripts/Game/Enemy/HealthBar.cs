using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject Damage;
    public GameObject Health;
    public TextMesh HealthText;

    public int health;
    public int maxHealth;
    public int damage;
    public float HealthPercentage;
    public float DamagePercentage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int health, int maxHealth) {
        this.health = health;
        this.maxHealth = maxHealth;
        damage = maxHealth - health;

        float fHealth = (float)health;
        float fMaxHealth = (float)maxHealth;

        float fPercent = (fHealth / fMaxHealth);
        if (fPercent < 0) {
            HealthPercentage = 0;
        } else if (fPercent > 1) {
            HealthPercentage = 1;
        } else {
            HealthPercentage = fPercent;
        }

        DamagePercentage = 1 - HealthPercentage;

        UpdateView();
    }

    private void UpdateView() {
        // Bail Early if no components present
        if (!Damage || !Health) {
            return;
        }

        // Update Damage Bar
        Damage.transform.localScale = new Vector3(Damage.transform.localScale.x, Damage.transform.localScale.y, DamagePercentage);
        Damage.transform.localPosition = new Vector3(Damage.transform.localPosition.x, Damage.transform.localPosition.y, HealthPercentage / 2);

        // Update Health Bar
        Health.transform.localScale = new Vector3(Health.transform.localScale.x, Health.transform.localScale.y, HealthPercentage);
        Health.transform.localPosition = new Vector3(Health.transform.localPosition.x, Health.transform.localPosition.y, -(DamagePercentage / 2));

        // Update Health Text
        if (HealthText) {
            HealthText.text = health + "/" + maxHealth;
        }
    }
}
