using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Gesture Variables")]
    [SerializeField]
    private int _numOfGestures;
    [SerializeField]
    private int _currentIndex = 0;
    [SerializeField]
    private List<GestureSO> _gestureList = new List<GestureSO>();

    [Header("Physics Variables")]
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Rigidbody _rigidBody;

    [Header("UI Variables")]
    [SerializeField]
    private Canvas _canvas;

    [Header("Gameplay Variables")]
    [SerializeField]
    private bool _allowGesturesDestroy = false;
    [SerializeField]
    private EnemyGesturePanel _gesturePanel;
    [SerializeField]
    private Transform _gesturePanelPosition;
    [SerializeField]
    private float _spawnChance;
    [SerializeField]
    private uint _onKillScore;

    [Header("Ragdoll Variables")]
    private bool _isGestureSeen = false;
    private bool _isFreeFalling = false;
    private bool _isRagdollRotating = false;
    private Vector3 _rotationRate;
    private Quaternion _deltaRotation;
    private Vector3 _rotationDestination;
    [SerializeField]
    private float _minRotationTo;
    [SerializeField]
    private float _maxRotationTo;

    private bool _isDead = false;

    public float spawnChance
    { get { return _spawnChance; } }

    public bool allowGesturesDestroy
    { get { return _allowGesturesDestroy; } set { _allowGesturesDestroy = value; } }

    public bool isFreeFalling
    { get { return _isFreeFalling; } }

    public bool isDead
    { get { return _isDead; } }

    // Start is called before the first frame update
    void Start()
    {
        if(AssetsGameScene.instance != null)
        {
            if(AssetsGameScene.instance.ui.canvas != null)
            {
                _canvas = AssetsGameScene.instance.ui.canvas;
            }
        }

        _currentIndex = 0;

        if(RecognizerManager.instance != null)
        {
            for (int i = 0; i < _numOfGestures; i++)
            {
                _gestureList.Add(GenerateRandom());
            }

            _gesturePanel = Instantiate(_gesturePanel, _canvas.transform);
            FollowGameObject();
            _gesturePanel.InitializePanel(_gestureList.ToArray());
        }

        if(GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered += OnGestureTriggeredListener;
        }
    }

    private void OnDestroy()
    { 
        if(_gesturePanel != null)
        {
            Destroy(_gesturePanel);
        }

        if(GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered -= OnGestureTriggeredListener;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowGameObject();
    }


    private void FixedUpdate()
    {
        RagDollRotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
        {
            if (!_isDead)
            {
                if (SceneManager.instance != null)
                {
                    SceneManager.instance.LoadScene("GameOver");
                }
            }

            //FreeFall();
            //_isDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_gesturePanel != null)
        {
            Destroy(_gesturePanel.gameObject);
        }

        if (collision.gameObject.CompareTag("KillZone") && _isDead)
        {
            Destroy(this.gameObject, 1f);
        }

        _isRagdollRotating = false;
        _rotationRate *= 0;


    }

    private void RagDollRotate()
    {
        if (!_isRagdollRotating && _isFreeFalling)
        {
            Vector3 randomRate;

            randomRate.x = UnityEngine.Random.Range(_minRotationTo, _maxRotationTo);
            randomRate.y = UnityEngine.Random.Range(_minRotationTo, _maxRotationTo);
            randomRate.z = UnityEngine.Random.Range(_minRotationTo, _maxRotationTo);
            _rotationRate = randomRate;

            _isRagdollRotating = true;
        }

        if (_isFreeFalling && _isRagdollRotating)
        {
            _deltaRotation = Quaternion.Euler(_rotationRate * Time.fixedDeltaTime);
            _rigidBody.MoveRotation(_rigidBody.rotation * _deltaRotation);
        }
    }

    private void FollowGameObject()
    {
        // Early exit if the gesture panel is not set
        if (_gesturePanel == null)
            return;

        // Get the position of the gesture panel in screen space
        Vector3 pos = _gesturePanelPosition.position;
        pos = Camera.main.WorldToScreenPoint(pos);
        _gesturePanel.transform.position = pos;

        // Check if the gesture panel has gone off-screen
        if (_gesturePanel.transform.position.y < Camera.main.ViewportToScreenPoint(new Vector3(0, 1, 0)).y && !_isGestureSeen)
        {
            _isGestureSeen = true;
        }
    }


    private void OnGestureTriggeredListener(GestureSO gesture)
    {
        if (_allowGesturesDestroy && _isGestureSeen && _currentIndex < _gestureList.Count)
        {
            if (gesture.gestureName == _gestureList[_currentIndex].gestureName)
            {
                if(RecognizerManager.instance != null)
                {
                    RecognizerManager.instance.nothingDetected = 0;
                }

                _currentIndex++;
                _gesturePanel?.RemoveCurrentGesture();
            }
        }

        if (_currentIndex >= _gestureList.Count)
        {
            Debug.Log("CurrentIndex is greater than _gestureList.Count");

            MissileManager.instance?.SpawnMissile(this.gameObject);
            FreeFall();
            GameManager.instance.OnEnemyDeath?.Invoke();
        }
    }


    private GestureSO GenerateRandom()
    {
        if (RecognizerManager.instance != null)
        {
            int index = UnityEngine.Random.Range(0, RecognizerManager.instance.gestureList.Count);
            return RecognizerManager.instance.gestureList[index];
        }
        return null;
    }


    private void FreeFall()
    {
        _isDead = true;
        Debug.Log("Freefalling");

        if(ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(_onKillScore);
        }

        if(_rigidBody != null && _collider != null)
        {
            _rigidBody.drag = 0;
            _collider.isTrigger = false; 
        }

        if(_gesturePanel != null)
        {
            Destroy(_gesturePanel.gameObject);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered -= OnGestureTriggeredListener;
        }

        _isFreeFalling = true;
    }
}
