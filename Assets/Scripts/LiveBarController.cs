using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LiveBarController : MonoBehaviour
    {
        private float _liveValue;
        private Renderer _renderer;
        private RectTransform _rectTransform;
        private GameObject _liveBar;
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _liveBar = transform.Find("Livebar").gameObject;
            _renderer = _liveBar.GetComponent<Renderer>();
            _liveValue = 1.0f;
            Player.OnHit += UpdateBar;
            //Player.OnTurn += TurnBar;
        }

        private void UpdateBar(object sender, float value)
        {
            Debug.Log(value.ToString());
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x, localScale.y, value);

            Color color = new Color();
            if ((1 - value) <= 0.5f)
            {
                color.r = 1 - value;
                color.g = 1;
            }
            else
            {
                color.r = 1;
                color.g = value;
            }
            
            Debug.Log($"{color.r.ToString()} {color.g.ToString()} {color.b.ToString()}");
            _renderer.material.color = color;
        }

        private void TurnBar(object sender, EventArgs args)
        {
            transform.Rotate(Vector3.up, 180.0f);
            //transform.rotation = Quaternion. (transform.position, Vector3.up);
        }
    }
}