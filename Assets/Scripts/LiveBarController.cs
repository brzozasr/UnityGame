using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LiveBarController : MonoBehaviour
    {
        private float _liveValue;
        private Renderer _renderer;
        private GameObject _liveBar;
        private GameObject _backgroud;

        private void Awake()
        {
            Player.OnHit += UpdateBar;
        }

        private void Start()
        {
            _backgroud = transform.parent.Find("LivebarBackground").gameObject;
            _liveBar = transform.Find("Livebar").gameObject;
            _renderer = _backgroud.GetComponent<Renderer>();
            _liveValue = 1.0f;
            //Player.OnTurn += TurnBar;
        }

        private void UpdateBar(object sender, float value)
        {
            //Debug.Log(value.ToString());
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x, localScale.y, value);
            
            Color color = new Color();
            if ((1 - value) <= 0.5f)
            {
                color.r = (1 - value) * 2;
                color.g = 1;
            }
            else
            {
                color.r = 1;
                color.g = value;
            }
            
            //Debug.Log($"{color.r.ToString()} {color.g.ToString()} {color.b.ToString()}");
            _renderer.material.color = color;
        }
    }
}