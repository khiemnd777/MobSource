using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class MobUtility
    {
        public static IEnumerator WaitInWhile(float timeInSecond, Func<bool> funcInWhile, Action timeOut = null){
            var remainingTime = timeInSecond;
            var floorTime = Mathf.FloorToInt(remainingTime);
            var result = false;
            while (remainingTime > 0)
            {
                yield return null;
                remainingTime -= Time.deltaTime;
                var newFloorTime = Mathf.FloorToInt(remainingTime);
                if (newFloorTime != floorTime)
                {
                    floorTime = newFloorTime;
                    result = funcInWhile.Invoke();
                    if(result)
                        break;
                }
            }
            if(!result && timeOut != null){
                timeOut.Invoke();
            }
        }
    }
}