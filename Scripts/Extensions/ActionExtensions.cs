using System;

namespace Mob
{
	public static class ActionExtensions
	{
		public static Action<object> Convert<T>(this Action<T> act){
			if (act == null)
				return null;
			return new Action<object> (o => act ((T)o));
		}
	}
}

