using UnityEngine;
using System.Collections;

namespace Mob
{
	public class SlashLine : MobBehaviour
	{
		public Transform target;
		public float maxDistance = 10f;
		public float speed = 0.1f;

		Transform _cachedTransform;
		Vector2 destination;

		void Start(){
			_cachedTransform = transform;
			_cachedTransform.parent = target.parent;
			// to calculate position of slash-line spawning
			_cachedTransform.position = Spawn ();
			destination = target.position + (target.position - _cachedTransform.position);
			Destroy (gameObject, 2f);
		}

		Vector2 Spawn(){
			while (true) {
				// to find out appropriated position
				var p = Random.insideUnitCircle * maxDistance + (Vector2)target.position;
				if (Vector2.Distance (p, (Vector2)target.position) - Random.Range (maxDistance * 0.875f, maxDistance) < 0f) {
					continue;
				}
				return p;
			}
		}

		void FixedUpdate(){
			_cachedTransform.position = Vector2.Lerp (_cachedTransform.position, destination, speed);
			speed *= 1.1f;
		}
	}
}

