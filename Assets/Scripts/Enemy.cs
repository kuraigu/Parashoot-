using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _numOfGestures;
    [SerializeField]
    private int _currentIndex;
    [SerializeField]
    private List<GestureSO> _gestureList = new List<GestureSO>();

    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private bool _allowGesturesDestroy;

    [SerializeField]
    private EnemyGesturePanel _gesturePanel;

    [SerializeField]
    private Transform _gesturePanelPosition;

    [SerializeField]
    private float _spawnChance;

    [SerializeField]
    private uint _onKillScore;

    private bool _isFreeFalling = false;

    private bool _isRagdollRotating = false;

    private Vector3 _rotationRate;
    private Quaternion _deltaRotation;
    private Vector3 _rotationDestination;

    [SerializeField]
    private float _minRotationTo;
    [SerializeField]
    private float _maxRotationTo;

    public float spawnChance
    { get { return _spawnChance; } }


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

        _allowGesturesDestroy = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mortality")) _allowGesturesDestroy = true;

        if (other.CompareTag("KillZone")) FreeFall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnEnemyDeath?.Invoke();
        }

        if(_gesturePanel != null)
        {
            Destroy(_gesturePanel.gameObject);
        }

        if (collision.gameObject.CompareTag("KillZone")) Destroy(this.gameObject, 1f);

        _isRagdollRotating = false; _isFreeFalling = false;
        _rotationRate *= 0;
    }

    private void FollowGameObject()
    {
        if(_gesturePanel != null)
        {
            Vector3 pos = _gesturePanelPosition.position;
            pos = Camera.main.WorldToScreenPoint(pos);
            _gesturePanel.transform.position = pos;
        }
    }


    private void OnGestureTriggeredListener(GestureSO gesture)
    {
        if(_allowGesturesDestroy && _currentIndex < _gestureList.Count)
        {
            if (gesture.gestureName == _gestureList[_currentIndex].gestureName && _gesturePanel != null)
            {
                _currentIndex++;
                _gesturePanel.RemoveCurrentGesture();

                if (_currentIndex > _gestureList.Count - 1)
                {
                    FreeFall();

                    if(MissileManager.instance != null)
                    {
                        MissileManager.instance.SpawnMissile(this.gameObject);
                    }
                }
            }
        }
    }


    private GestureSO GenerateRandom()
    {
        int index = UnityEngine.Random.Range(0, RecognizerManager.instance.gestureList.Count);

        if (RecognizerManager.instance != null)
        {
           return RecognizerManager.instance.gestureList[index];
        }

        return null;
    }


    private void FreeFall()
    {
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
