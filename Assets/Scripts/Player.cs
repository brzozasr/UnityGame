using System;
using System.Collections;
using System.Globalization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int livePoints;
    public float flyForce;
    public int actualLivePoints;
    public int actualLiveNumber;

    public int walkSpeed;
    public int runSpeed;
    public int resurectionDelaySec;
    public int liveNumber;

    private bool _jumpKeyPressed;
    private bool _fireKeyPressed;
    private bool _runKeyPressed;
    private bool _flyKeyPressed;
    
    public static bool menuOpened = false;

    private Animator _animator;
    private AudioManager _audioManager;
    private PlayerBulletController _playerBulletScript;
    private Rigidbody _rigidbody;
    private Rigidbody _cameraRigidBody;
    private DateTime _shotTime;
    private GameObject _arm;
    private CapsuleCollider _capsuleCollider;

    private bool _lookRight;
    private int _actualLiveNumber;
    private float _actualLivePoints;
    private float _horizontalInput;
    private int _moveSpeed;

    public static event EventHandler<float> OnHit;
    public static event EventHandler<OnPlatformEnterArgs> OnPlatformEnter;
    public static event EventHandler OnTurn;
    public static bool Dead = false;

    private Vector3 _playerBoxColliderCenter;
    private float _playerCapsuleColliderHeight;
    private Vector3 _playerStartPosition;

    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int Shot1 = Animator.StringToHash("shot");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Walk = Animator.StringToHash("walk");

    private void Awake()
    {
        OnHit = null;
        OnPlatformEnter = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Dead = false;
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerBulletScript = playerBullet.GetComponent<PlayerBulletController>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _actualLivePoints = livePoints;
        _shotTime = DateTime.Now;

        _playerBoxColliderCenter = _capsuleCollider.center;
        _playerCapsuleColliderHeight = _capsuleCollider.height;
        _playerStartPosition = transform.localPosition;

        _arm = transform.Find("Hips").Find("ArmPosition_Right").gameObject;
        
        
        // Points and lives calculation
        // Change scene index to 3 for debugging process
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Initiate start lives number
            DataStore.StartLives = liveNumber;
            Debug.Log($"DataStore.StartLives: {DataStore.StartLives.ToString()}");
            
            // Initiate start HP points
            DataStore.StartHpPoints = livePoints;
            Debug.Log($"DataStore.StartHpPoints: {DataStore.StartHpPoints.ToString()}");
            
            // Initiate lives number
            DataStore.SetCurrentLives(actualLiveNumber);
            _actualLiveNumber = actualLiveNumber;
            Debug.Log($"actualLiveNumber: {actualLiveNumber.ToString()}");
            
            // Initiate HP points
            DataStore.SetCurrentHpPoints(actualLivePoints);
            _actualLivePoints = actualLivePoints;
            Debug.Log($"actualLivePoints: {actualLivePoints.ToString()}");
        }
        else
        {
            DataStore.StartHpPoints = livePoints;
            
            // Update number of lives
            _actualLiveNumber = DataStore.Lives;
            //Debug.Log($"_actualLiveNumber: {_actualLiveNumber.ToString()}");
            
            // Update HP points
            _actualLivePoints = DataStore.HpPoints;
            //Debug.Log($"_actualLivePoints: {_actualLivePoints.ToString()}");
        }

        FirstAidKitController.OnFirstAidCollected += RecalculateHpPoints;
        
        //OnHit?.Invoke(this, _actualLivePoints / livePoints);
        
        //Debug.Log($"_actualLivePoints: {_actualLivePoints.ToString()} livePoints: {livePoints.ToString()}");
    }

    private void RecalculateHpPoints(int hpPoints)
    {
        _actualLivePoints = DataStore.AddHpPoints(hpPoints);
        //OnHit?.Invoke(this, _actualLivePoints / livePoints);
    }

    // Update is called once per frame
    void Update()
    {
        OnHit?.Invoke(this, _actualLivePoints / livePoints);
        // Update HP points
        DataStore.SetCurrentHpPoints((int) _actualLivePoints);
        // Update number of lives
        DataStore.SetCurrentLives(_actualLiveNumber);

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

        if (_jumpKeyPressed && overlapedGameObjects > 0)
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
        else if (_runKeyPressed == false)
            _animator.SetBool(Walk, false);
        else
            _animator.SetBool(Run, false);

        if (_jumpKeyPressed)
            _animator.SetBool(Jump, true);

        if (_fireKeyPressed)
        {
            _animator.SetBool(Shot1, true);
            Shot();
        }
        else
            _animator.SetBool(Shot1, false);

        CheckFallDown();
    }

    private void CheckFallDown()
    {
        if (transform.position.y < -5.0f && !Dead)
        {
            _actualLivePoints = 0;
            //OnHit?.Invoke(this, _actualLivePoints/livePoints);
            
            _animator.SetBool(Die, true);
            Dead = true;
            Debug.Log("err");
            _actualLiveNumber--;
            _capsuleCollider.height = 0.2f;
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, 0.0f, _capsuleCollider.center.z);

            StartCoroutine(Resurection());
        }
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
            gameObject.transform.SetParent(other.gameObject.transform);
        }

        if (other.gameObject.CompareTag("DoorPlatform"))
        {
            Debug.Log("On the door");
            
            if (!other.gameObject.transform.parent.gameObject.GetComponent<DoorController>().isOpened)
            {
                OnPlatformEnterArgs args = new OnPlatformEnterArgs();

                args.ChipName = other.gameObject.transform.parent.gameObject.GetComponent<DoorController>()
                    .compatibleChipName;
                args.ChipNumber = DataStore.GetItemQuantityFromInventory(args.ChipName);

                OnPlatformEnter?.Invoke(this, args);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            gameObject.transform.parent = GameObject.Find("Level").transform;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Dead && other.gameObject.CompareTag("DgoneBullet") || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Live points left: {_actualLivePoints.ToString(CultureInfo.CurrentCulture)}");
            _actualLivePoints--;
            //OnHit?.Invoke(this, _actualLivePoints / livePoints);
        }

        if (_actualLivePoints <= 0 && !Dead)
        {
            _animator.SetBool(Die, true);
            Dead = true;
            _actualLiveNumber--;
            _capsuleCollider.height = 0.2f;
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, 0.0f, _capsuleCollider.center.z);

            StartCoroutine(Resurection());
        }
    }

    private IEnumerator Resurection()
    {
        yield return new WaitForSeconds(resurectionDelaySec);

        transform.localPosition = _playerStartPosition;
        _animator.SetBool(Die, false);
        //OnHit?.Invoke(this, 1.0f);
        _actualLivePoints = livePoints;
        _capsuleCollider.height = _playerCapsuleColliderHeight;
        _capsuleCollider.center = _playerBoxColliderCenter;
        Dead = false;
    }

    private void GetKeyState()
    {
        if (!menuOpened)

        {
            if (Input.GetKeyDown(KeyCode.W))
                _jumpKeyPressed = true;
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _jumpKeyPressed = false;
                _fireKeyPressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _fireKeyPressed = false;
                _jumpKeyPressed = false;
            }
            else
            {
                _jumpKeyPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
                if (_runKeyPressed == false)
                    _runKeyPressed = true;
                else
                    _runKeyPressed = false;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                _flyKeyPressed = true;

            if (Input.GetKeyUp(KeyCode.UpArrow))
                _flyKeyPressed = false;

            /*if (Input.GetKeyDown(KeyCode.F))
            {
                Spawner.Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner.Spawner>();
                spawner.Save();
                Debug.Log("Saved");
            }*/

            _horizontalInput = Input.GetAxis("Horizontal");
        }
    }
}