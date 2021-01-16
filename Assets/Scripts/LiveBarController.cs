using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LiveBarController : MonoBehaviour
    {
        private float _liveValue;
        private Renderer _renderer;
        private RectTransform _rectTransform;
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _renderer = GetComponent<Renderer>();
            _liveValue = 1.0f;
            Player.OnHit += UpdateBar;
        }

        private void UpdateBar(object sender, float value)
        {
            Debug.Log(value.ToString());
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x, localScale.y, value);
        }
    }
}