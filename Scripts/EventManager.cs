using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class EventManager : MonoBehaviour {

		private Dictionary <string, Delegate> eventDictionary;

		private static EventManager eventManager;

		public static EventManager instance
		{
			get
			{
				if (!eventManager)
				{
					eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

					if (!eventManager)
					{
						Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
					}
					else
					{
						eventManager.Init (); 
					}
				}

				return eventManager;
			}
		}

		void Init ()
		{
			if (eventDictionary == null)
			{
				eventDictionary = new Dictionary<string, Delegate>();
			}
		}

		public static void StartListening (string eventName, Delegate listener)
		{
			Delegate thisEvent = null;

			if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
			{
				instance.eventDictionary [eventName] = Delegate.Combine(instance.eventDictionary [eventName], listener);
			} 
			else
			{
				instance.eventDictionary.Add (eventName, listener);
			}
		}

		public static void StopListening (string eventName, Delegate listener)
		{
			if (eventManager == null) return;
			Delegate thisEvent = null;
			if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
				instance.eventDictionary [eventName] = Delegate.Remove(instance.eventDictionary [eventName], listener);
			}
		}

		public static void TriggerEvent (string eventName, object args = null)
		{
			Delegate thisEvent = null;
			if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
			{
				if (thisEvent.IsNull ())
					return;
				var invocataionList = thisEvent.GetInvocationList ();
				foreach (var evt in invocataionList) {
					var target = evt.Target;
					if (target.IsNull()) {
						StopListening (eventName, evt);
					} else {
						var methodInfo = evt.Method;
						if (args == null) {
							methodInfo.Invoke (target, null);
							continue;
						}
						var paramList = methodInfo.GetParameters ();
						var type = args.GetType ();
						var __a = new object[paramList.Length];
						for (int index = 0; index < paramList.Length; index++)
						{
							var parameter = paramList[index];
							var name = parameter.Name;
							// Get the value from obj
							var property = type.GetProperty(name);
							var value = property.GetValue(args, null);
							__a[index] = value;
						}
						methodInfo.Invoke (target, __a);
						// I think Invoke is better than DynamicInvoke about performance
//						thisEvent.DynamicInvoke (__a);
					}
				}
			}
		}
	}	
}