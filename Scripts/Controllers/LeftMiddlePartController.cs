using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mob
{
	public class LeftMiddlePartController : MobBehaviour
	{
		Touch initTouch;
		bool swiped;

		void Update(){
			if (Input.touchCount > 0) {
				Debug.Log (1);
				foreach (var t in Input.touches) {
					if (t.phase == TouchPhase.Began) {
						initTouch = t;
					} else if (t.phase == TouchPhase.Moved && !swiped) {
						var xMoved = initTouch.position.x - t.position.x;
						var yMoved = initTouch.position.y - t.position.y;
						var distance = Mathf.Sqrt (xMoved * xMoved + yMoved * yMoved);
						var isHorizontalSwiping = Mathf.Abs (xMoved) > Mathf.Abs (yMoved);
						Debug.Log (distance);
						if (distance > 50f) {
							
						}
					} else if (t.phase == TouchPhase.Ended) {
						initTouch = new Touch ();
						swiped = false;
					}
				}
			}
		}
	}	
}
