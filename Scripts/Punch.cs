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

		float firstAmount = 1;
        void Start()
        {
			healthBar.FillAmount(firstAmount);
			GetComponent<Button>().onClick.AddListener(() => {
				firstAmount -= .1f;
				if(firstAmount <= 0f)
					firstAmount = 1f;
				healthBar.FillAmountAndShake(firstAmount);
			});
        }
    }
}