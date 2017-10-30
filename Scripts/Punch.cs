using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
    public class Punch : MobBehaviour
    {
		public HealthBar healthBar;
		Button button;

		float health = 250f;
		float startHealth;
        void Start()
        {
			startHealth = health;
			healthBar.SetLabel(health, startHealth);
			healthBar.FillAmount(health / startHealth, false);
			GetComponent<Button>().onClick.AddListener(() => {
				health -= Mathf.FloorToInt(Random.Range(7f, 85f));
				if(health <= 0f)
					health = 0f;
				healthBar.SetLabel(health, startHealth);
				healthBar.FillAmount(health / startHealth);
			});
        }
    }
}