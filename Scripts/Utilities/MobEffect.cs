using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mob
{
    public static class MobEffect
    {
        public static IEnumerator Shaking(float duration, float amount, Transform target, Vector3 originalPosition){
            float endTime = Time.time + duration;
            while (Time.time < endTime) {
                target.localPosition = originalPosition + Random.insideUnitSphere * amount;
                duration -= Time.deltaTime;
                yield return null;
            }
            target.localPosition = originalPosition;
        }
    }
}