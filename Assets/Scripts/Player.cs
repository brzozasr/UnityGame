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
    public float _jumpForce;
    public Animation _dieAnim;
    [SerializeField] private float _livePoints;
    [SerializeField] private float _flyForce;
    private float _actualLivePoints;
    
    private bool _spaceKeyPressed;
    private bool _fireKeyPressed;
    private bool _runKeyPressed = false;
    private bool _dieKeyPressed;
    private bool _flyKeyPressed;

    private float _horizontalInput;
    private PlayerBulletController _playerBulletScript;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool _lookRight;
    private Rigidbody _cameraRigidBody;
    [SerializeField] private int WalkSpeed;
    [SerializeField] private int RunSpeed;
    [SerializeField] private int ResurectionDelaySec;
    [SerializeField] private ParticleSystem MuzzleFlash;
    private int MoveSpeed;
    private DateTime _shotTime;
    private GameObject _arm;
    private BoxCollider _boxCollider;
    public static event EventHandler<float> OnHit;
    public static event EventHandler OnTurn;
    public static bool Dead = false;
    private Vector3 _playerBoxColliderCenter;
    private Vector3 _playerBoxColliderSize;
    private Vector3 _playerStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        _actualLivePoints = _livePoints;
        _shotTime = DateTime.Now;
        _playerBulletScript = _playerBullet.GetComponent<PlayerBulletController>();
        _boxCollider = GetComponent<BoxCollider>();
        _playerBoxColliderCenter = _boxCollider.center;
        _playerBoxColliderSize = _boxCollider.size;
        _playerStartPosition = transform.position;

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

        if (Input.GetKeyDown(KeyCode.D))
            _dieKeyPressed = true;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            _flyKeyPressed = true;
        
        if (Input.GetKeyUp(KeyCode.UpArrow))
            _flyKeyPressed = false;
        
        
        _horizontalInput = Input.GetAxis("Horizontal");
        
        if (_horizontalInput < 0 && !_lookRight)
        {
            transform.Rotate(Vector3.up, 180.0f);
            _lookRight = true;
            OnTurn?.Invoke(this, EventArgs.Empty);
        }
        else if (_horizontalInput > 0 && _lookRight)
        {
            transform.Rotate(Vector3.up, 180.0f);
            _lookRight = false;
            OnTurn?.Invoke(this, EventArgs.Empty);
        }

        var overlapedGameObjects = Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length;
        
        if (_spaceKeyPressed && overlapedGameObjects > 0)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }
        
        if (_flyKeyPressed)
        {
            Debug.Log("fly");
            _rigidbody.AddForce(Vector3.up * (_flyForce * Time.deltaTime), ForceMode.Impulse);
        }

        if (Physics.OverlapSphere(_groundCheck.position, 0.1f, _playerMask).Length > 0)
        {
            _animator.SetBool("jump", false);
        }
        
        if (_horizontalInput != 0)
        {
            if (overlapedGameObjects > 0)
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
            {
                if (_runKeyPressed == false)
                    _animator.SetBool("walk", false);
                else
                    _animator.SetBool("run", false);
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
        
        if (_dieKeyPressed)
            _animator.SetBool("die", false);
    }

    private void Shot()
    {
        
        if ((DateTime.Now - _shotTime).Milliseconds > (1000 / _shotFrequency))
        {
            var bullet = Instantiate(_playerBullet, _arm.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>()
                .AddForce(_gunHole.transform.forward * _playerBulletScript.Speed, ForceMode.Impulse);
            
            MuzzleFlash.Play();

            //var muzzle = Instantiate(MuzzleFlash, _gunHole.transform.position, Quaternion.identity);
            
            
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
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("DgoneBullet"))
        {
            Debug.Log($"Live points left: {_livePoints.ToString()}");
            _actualLivePoints--;
            OnHit?.Invoke(this, _actualLivePoints / _livePoints);
        }

        if (_actualLivePoints <= 0)
        {
            _animator.SetBool("die", true);
            Dead = true;
            _boxCollider.size = new Vector3(_boxCollider.size.x, 0.0f, _boxCollider.size.z);
            _boxCollider.center = new Vector3(_boxCollider.center.x, 0.0f, _boxCollider.center.z);

            StartCoroutine(Resurection());
            Dead = false;
        }
    }

    private IEnumerator Resurection()
    {
        yield return new WaitForSeconds(ResurectionDelaySec);
        
        transform.position = _playerStartPosition;
        _animator.SetBool("die", false);
        OnHit?.Invoke(this, 1.0f);
        _actualLivePoints = _livePoints;
        _boxCollider.size = _playerBoxColliderSize;
        _boxCollider.center = _playerBoxColliderCenter;
    }
}
