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
        [Space]
        [Range(0f, 1f)]
        public float durationUpdatingMainBar = .25f;
        [Space]
        [Range(0f, 1f)]
        public float shakingDuration = .25f;
        [Range(0f, 10f)]
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

        public void FillAmount(float amount){
            MathfLerp(mainBar.fillAmount, amount, (value) => {
                mainBar.fillAmount = value;
            }, durationUpdatingMainBar);
        }

        public void FillAmountAndShake(float amount){
            FillAmount(amount);
            Shake(shakingDuration, shakingAmount, _cachedTransform, _originalPosition);
        }
    }
}