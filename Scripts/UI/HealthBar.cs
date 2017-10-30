using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
    public class HealthBar : MobBehaviour
    {
        public const string EVENT_UPDATE_BAR = "event-hp-update-amount";

        public Color color;
        public Image backgroundBar;
        public Image mainBar;
        public float durationUpdatingMainBar = .25f;
        [Space]
        public float shakingDuration = .25f;
        public float shakingAmount = .2f;

        Vector3 _originalPosition;
        Transform _cachedTransform;

        void Start(){
            _cachedTransform = transform;
            _originalPosition = _cachedTransform.localPosition;

            mainBar.color = color;
            
            EventManager.StartListening(EVENT_UPDATE_BAR, new Action<float, int>((amount, id) =>{
                if(GetInstanceID() != id)
                    return;
                MathfLerp(mainBar.fillAmount, amount, (value) => {
                    mainBar.fillAmount = value;
                }, durationUpdatingMainBar);
                Shake(shakingDuration, shakingAmount, _cachedTransform, _originalPosition);
            }));
        }
    }
}