using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck = null;
    [SerializeField] private LayerMask _playerMask;
    private bool _spaceKeyPressed;

    private float _horizontalInput;

    private Rigidbody _rigidbody;
    private Animator _animator;
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _spaceKeyPressed = true;
        else
            _spaceKeyPressed = false;

        _horizontalInput = Input.GetAxis("Horizontal");
        
        
    }

    private void FixedUpdate()
    {
        if (_spaceKeyPressed && Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length > 0)
        {
            _rigidbody.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
        }

        if (Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length > 0)
        {
            _animator.SetBool("jump", false);
        }
        
        if (_horizontalInput != 0)
            _animator.SetBool("walk", true);
        else
            _animator.SetBool("walk", false);
        
        if (_spaceKeyPressed)
            _animator.SetBool("jump", true);
        
        
        _rigidbody.velocity = new Vector3(_horizontalInput * 2, _rigidbody.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
        }
    }
}
