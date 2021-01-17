using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MuzzleFlashController : MonoBehaviour
    {
        public GameObject MuzzleFlash;

        private GameObject _muzzle;
        private Transform _playerTransform;

        private void Start()
        {
            _playerTransform = GetComponent<Transform>();
            
            if (MuzzleFlash != null)
            {
                _muzzle = Instantiate(MuzzleFlash, transform.position, Quaternion.identity);
            }

            StartCoroutine(Flash());
        }

        private void Update()
        {
            _muzzle.transform.position = _playerTransform.position;
        }

        private IEnumerator Flash()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(_muzzle);
            Destroy(gameObject);
        }
    }
}