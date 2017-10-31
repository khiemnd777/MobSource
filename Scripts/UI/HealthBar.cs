using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
    public class HealthBar : MobBehaviour
    {
        public bool showLabel = true;
        public string labelFormat = "{0}/{1}";
        [Space]
        [Range(0f, 1f)]
        public float durationUpdatingMainBar = .25f;
        [Range(0f, 1f)]
        public float durationWaitUntilSubBar = .5f;
        [Range(0f, 1f)]
        public float durationWaitUntilMainBar = .5f;
        [Space]
        [Range(0f, 1f)]
        public float shakingDuration = .25f;
        [Range(0f, 10f)]
        public float shakingAmount = .2f;

        Vector3 _originalPosition;
        Transform _cachedTransform;
        Image _mainBar;
        Image _subBar;
        Image _addBar;
        Text _label;

        void Awake(){
            _mainBar = GetChildMonoComponent<Image>("HealthBarBG/MainBar");
            _subBar = GetChildMonoComponent<Image>("HealthBarBG/SubBar");
            _addBar = GetChildMonoComponent<Image>("HealthBarBG/AddBar");
            _label = GetChildMonoComponent<Text>("Label");
            _cachedTransform = transform;
            _originalPosition = _cachedTransform.localPosition;
        }

        void Update(){
            _label.gameObject.SetActive(showLabel);
        }

        public void SetLabel(params object[] contents){
            _label.text = string.Format(labelFormat, contents);
        }

        public void Subtract(float amount, bool hasEffect = true){
            if(hasEffect){
                StartCoroutine(Subtracting(amount, _mainBar, _subBar, _cachedTransform, _originalPosition));
            } else {
                _mainBar.fillAmount = amount;
                _addBar.fillAmount = amount;
                _subBar.fillAmount = amount;
            }
        }

        public void Add(float amount, bool hasEffect = true){
            if(hasEffect){
                StartCoroutine(Adding(amount, _mainBar, _addBar, _cachedTransform, _originalPosition));
            } else {
                _mainBar.fillAmount = amount;
                _addBar.fillAmount = amount;
                _subBar.fillAmount = amount;
            }
        }

        IEnumerator Subtracting(float amount, Image mainBar, Image subBar, Transform target, Vector3 originalPosition){
            yield return null;
            MathfLerp(mainBar.fillAmount, amount, (value) => {
                    mainBar.fillAmount = value;
                    _addBar.fillAmount = value;
                }, durationUpdatingMainBar);
            Shake(shakingDuration, shakingAmount, target, originalPosition);
            yield return new WaitForSeconds(durationWaitUntilSubBar);
            MathfLerp(subBar.fillAmount, amount, (value) => {
                subBar.fillAmount = value;
            }, durationUpdatingMainBar);
        }

        IEnumerator Adding(float amount, Image mainBar, Image addBar, Transform target, Vector3 originalPosition){
            yield return null;
            MathfLerp(addBar.fillAmount, amount, (value) => {
                addBar.fillAmount = value;
            }, durationUpdatingMainBar);
            yield return new WaitForSeconds(durationWaitUntilMainBar);
            MathfLerp(mainBar.fillAmount, amount, (value) => {
                    mainBar.fillAmount = value;
                    _subBar.fillAmount = value;
                }, durationUpdatingMainBar);
        }
    }
}