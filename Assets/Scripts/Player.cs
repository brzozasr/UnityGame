using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck = null;
    [SerializeField] private LayerMask _playerMask;
    private bool _spaceKeyPressed;
    private bool _fKeyPressed;
    private bool _rKeyPressed = false;

    private float _horizontalInput;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool _lookRight;
    private Rigidbody _cameraRigidBody;
    [SerializeField] private int WalkSpeed;
    [SerializeField] private int RunSpeed;
    private int MoveSpeed;

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
        else if (Input.GetKeyDown(KeyCode.F))
        {
            _spaceKeyPressed = false;
            _fKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _fKeyPressed = false;
            _spaceKeyPressed = false;
        }
        else
        {
            _spaceKeyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            if (_rKeyPressed == false)
                _rKeyPressed = true;
            else
                _rKeyPressed = false;
        
        
        _horizontalInput = Input.GetAxis("Horizontal");
        
        if (_horizontalInput < 0 && !_lookRight)
        {
            transform.Rotate(Vector3.up, 180.0f);
            _lookRight = true;
        }
        else if (_horizontalInput > 0 && _lookRight)
        {
            transform.Rotate(Vector3.up, 180.0f);
            _lookRight = false;
        }
        
        if (_spaceKeyPressed && Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length > 0)
        {
            _rigidbody.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
        }

        if (Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length > 0)
        {
            _animator.SetBool("jump", false);
        }
        
        if (_horizontalInput != 0)
        {
            if (_rKeyPressed == false)
            {
                _animator.SetBool("walk", true);
                MoveSpeed = WalkSpeed;
            }
            else
            {
                _animator.SetBool("run", true);
                MoveSpeed = RunSpeed;
            }
        }
        else
            if (_rKeyPressed == false)
                _animator.SetBool("walk", false);
            else
                _animator.SetBool("run", false);

        if (_spaceKeyPressed)
            _animator.SetBool("jump", true);
        
        if (_fKeyPressed)
            _animator.SetBool("shot", true);
        else
            _animator.SetBool("shot", false);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_horizontalInput * MoveSpeed, _rigidbody.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
        }
    }
}
