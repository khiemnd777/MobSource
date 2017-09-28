using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AffectModule : RaceModule
	{
		#region Affect functions

		List<Affect> _affects = new List<Affect>();

		public Affect[] affects {
			get {
				return _affects.ToArray();
			}
		}

		public void AddAffect (Affect affect)
		{
			if (_affects == null) {
				_affects = new List<Affect> ();
			}
			_affects.Add (affect);
		}

		public int RemoveAffect(Predicate<Affect> match){
			return _affects.RemoveAll(match);
		}

		public bool HasAffect<T>() where T : Affect{
			return _affects.Any (x => x.GetType ().IsEqual<T> ());
		}

		public T[] GetAffects<T>(Action<T> predicate = null) where T: Affect{
			T[] result = _affects
				.Where(x => x.GetType().IsEqual<T>()
					&& !x.IsNull())
				.Cast<T>()
				.ToArray();
			foreach (var r in result) {
				predicate.Invoke (r);
			}
			return result;
		}

		public T[] GetSubAffects<T>(Action<T> predicate = null){
			if (_affects == null)
				return null;
			var result = _affects
				.Where (x => typeof(T).IsAssignableFrom (x.GetType ())
					&& !x.IsNull())
				.Cast<T> ()
				.ToArray();
			if (result.Length > 0 && predicate != null) {
				foreach (var r in result) {
					predicate.Invoke (r);
				}
			}
			return result;
		}

		public Affect[] GetSubAffects(Type type, Action<Affect> predicate = null){
			if (_affects == null)
				return null;
			var result = _affects
				.Where (x => type.IsAssignableFrom (x.GetType ()))
				.ToArray();
			if (result.Length > 0 && predicate != null) {
				foreach (var r in result) {
					predicate.Invoke (r);
				}
			}
			return result;
		}

		public bool HasSubAffect<T>(){
			return _affects.Any (x => typeof(T).IsAssignableFrom (x.GetType ()));
		}

		public void RefreshAffect ()
		{
			if (_affects == null)
				return;
			_affects.RemoveAll (x => x == null);
		}

		IEnumerator RefreshingAffect(){
			if (gameObject == null)
				yield return null;
			while (true) {
				RefreshAffect ();
				yield return null;
			}
		}

		void Start(){
			StartCoroutine (RefreshingAffect());
		}

		#endregion
	}
}

