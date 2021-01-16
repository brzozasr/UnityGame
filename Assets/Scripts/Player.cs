using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck = null;
    [SerializeField] private LayerMask _playerMask;
    public GameObject _playerBullet;
    public GameObject _gunHole;
    public float _shotFrequency;
    
    private bool _spaceKeyPressed;
    private bool _fireKeyPressed;
    private bool _runKeyPressed = false;

    private float _horizontalInput;
    private PlayerBulletController _playerBulletScript;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool _lookRight;
    private Rigidbody _cameraRigidBody;
    [SerializeField] private int WalkSpeed;
    [SerializeField] private int RunSpeed;
    private int MoveSpeed;
    private DateTime _shotTime;
    private GameObject _arm;

    // Start is called before the first frame update
    void Start()
    {
        _shotTime = DateTime.Now;
        _playerBulletScript = _playerBullet.GetComponent<PlayerBulletController>();

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _arm = transform.Find("Hips").Find("ArmPosition_Right").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _spaceKeyPressed = true;
        else if (Input.GetKeyDown(KeyCode.F))
        {
            _spaceKeyPressed = false;
            _fireKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _fireKeyPressed = false;
            _spaceKeyPressed = false;
        }
        else
        {
            _spaceKeyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            if (_runKeyPressed == false)
                _runKeyPressed = true;
            else
                _runKeyPressed = false;
        
        
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
            if (_runKeyPressed == false)
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
            if (_runKeyPressed == false)
                _animator.SetBool("walk", false);
            else
                _animator.SetBool("run", false);

        if (_spaceKeyPressed)
            _animator.SetBool("jump", true);
        
        if (_fireKeyPressed)
        {
            _animator.SetBool("shot", true);
            Shot();
        }
        
        else
            _animator.SetBool("shot", false);
    }

    private void Shot()
    {
        
        if ((DateTime.Now - _shotTime).Milliseconds > (1000 / _shotFrequency))
        {
            var bullet = Instantiate(_playerBullet, _arm.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>()
                .AddForce(_gunHole.transform.forward * _playerBulletScript.Speed, ForceMode.Impulse);
            
            _shotTime = DateTime.Now;
        }
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
