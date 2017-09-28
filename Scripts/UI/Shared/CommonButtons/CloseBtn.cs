using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class CloseBtn : MonoBehaviour
	{
		public RectTransform panel;

		void Start(){
			GetComponent<Button>().onClick.AddListener(() => {
				panel.gameObject.SetActive(false);
			});
		}
	}
}

