using System;
using System.Collections;
using System.Globalization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public GameObject playerBullet;
    public GameObject gunHole;
    public GameObject muzzleFlash;
    public Transform groundCheck = null;
    public LayerMask playerMask;
    public float shotFrequency;
    public float jumpForce;
    public float livePoints;
    public float flyForce;

    public int walkSpeed;
    public int runSpeed;
    public int resurectionDelaySec;
    public int liveNumber;
    
    private bool _spaceKeyPressed;
    private bool _fireKeyPressed;
    private bool _runKeyPressed;
    private bool _dieKeyPressed;
    private bool _flyKeyPressed;

    private Animator _animator;
    private AudioManager _audioManager;
    private PlayerBulletController _playerBulletScript;
    private Rigidbody _rigidbody;
    private Rigidbody _cameraRigidBody;
    private DateTime _shotTime;
    private GameObject _arm;
    private BoxCollider _boxCollider;
    
    private bool _lookRight;
    private float _actualLivePoints;
    private float _horizontalInput;
    private int _moveSpeed;

    public static event EventHandler<float> OnHit;
    public static event EventHandler OnTurn;
    public static bool Dead = false;
    
    private Vector3 _playerBoxColliderCenter;
    private Vector3 _playerBoxColliderSize;
    private Vector3 _playerStartPosition;
    
    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int Shot1 = Animator.StringToHash("shot");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Walk = Animator.StringToHash("walk");

    private PhysicMaterial _boxColliderMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerBulletScript = playerBullet.GetComponent<PlayerBulletController>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxColliderMaterial = _boxCollider.material;
        
        _actualLivePoints = livePoints;
        _shotTime = DateTime.Now;
        
        _playerBoxColliderCenter = _boxCollider.center;
        _playerBoxColliderSize = _boxCollider.size;
        _playerStartPosition = transform.position;
        
        _arm = transform.Find("Hips").Find("ArmPosition_Right").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Read key input
        GetKeyState();
        
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

        var overlapedGameObjects = Physics.OverlapSphere(groundCheck.position, 0.1f, playerMask).Length;
        
        if (_spaceKeyPressed && overlapedGameObjects > 0)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        
        if (_flyKeyPressed)
        {
            _rigidbody.AddForce(Vector3.up * (flyForce * Time.deltaTime), ForceMode.Impulse);
        }

        var overlaps = Physics.OverlapSphere(groundCheck.position, 0.1f, playerMask);
        
        if (overlaps.Length > 0)
        {
            _animator.SetBool(Jump, false);
        }

        

        if (_horizontalInput != 0)
        {
            if (overlapedGameObjects > 0)
            {
                if (_runKeyPressed == false)
                {
                    _animator.SetBool(Walk, true);
                    _moveSpeed = walkSpeed;
                }
                else
                {
                    _animator.SetBool(Run, true);
                    _moveSpeed = runSpeed;
                }
            }
            else
            {
                if (_runKeyPressed == false)
                    _animator.SetBool(Walk, false);
                else
                    _animator.SetBool(Run, false);
            }
        }
        else
            if (_runKeyPressed == false)
                _animator.SetBool(Walk, false);
            else
                _animator.SetBool(Run, false);

        if (_spaceKeyPressed)
            _animator.SetBool(Jump, true);
        
        if (_fireKeyPressed)
        {
            _animator.SetBool(Shot1, true);
            Shot();
        }
        else
            _animator.SetBool(Shot1, false);
        
        if (_dieKeyPressed)
            _animator.SetBool(Die, false);
    }

    private void Shot()
    {
        
        if ((DateTime.Now - _shotTime).Milliseconds > (1000 / shotFrequency))
        {
            var bullet = Instantiate(playerBullet, _arm.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>()
                .AddForce(gunHole.transform.forward * _playerBulletScript.Speed, ForceMode.Impulse);
            
            _audioManager.PlaySound("PlayerShot");
            
            var muzzle = Instantiate(muzzleFlash, gunHole.transform.position, Quaternion.identity);
            muzzle.transform.SetParent(gunHole.transform);
            
            _shotTime = DateTime.Now;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_horizontalInput * _moveSpeed, _rigidbody.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bridge"))
        {
            Debug.Log("tick");
            gameObject.transform.SetParent(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            gameObject.transform.parent = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("DgoneBullet") || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Live points left: {livePoints.ToString(CultureInfo.CurrentCulture)}");
            _actualLivePoints--;
            OnHit?.Invoke(this, _actualLivePoints / livePoints);
        }

        if (_actualLivePoints <= 0)
        {
            _animator.SetBool(Die, true);
            Dead = true;
            liveNumber--;
            _boxCollider.size = new Vector3(_boxCollider.size.x, 0.0f, _boxCollider.size.z);
            _boxCollider.center = new Vector3(_boxCollider.center.x, 0.0f, _boxCollider.center.z);

            StartCoroutine(Resurection());
            Dead = false;
        }
    }

    private IEnumerator Resurection()
    {
        yield return new WaitForSeconds(resurectionDelaySec);
        
        transform.position = _playerStartPosition;
        _animator.SetBool(Die, false);
        OnHit?.Invoke(this, 1.0f);
        _actualLivePoints = livePoints;
        _boxCollider.size = _playerBoxColliderSize;
        _boxCollider.center = _playerBoxColliderCenter;
    }

    private void GetKeyState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _spaceKeyPressed = true;
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _spaceKeyPressed = false;
            _fireKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
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
    }
}
