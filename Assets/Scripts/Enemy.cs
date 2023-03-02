using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    [Header("Gesture Variables")]
    [SerializeField]
    private int _numOfGestures;
    [SerializeField]
    private int _currentIndex = 0;
    [SerializeField]
    private List<GestureSO> _gestureList = new List<GestureSO>();

    [Header("Animation")]
    [SerializeField] private Animator _animator;

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

    [Header("VFX")]
    [SerializeField] private VisualEffect _hitGroundExplosion;

    [Header("Distance Indicator")]
    [SerializeField] private LineRenderer _lineRenderer;
    private LineRenderer _lineRendererActive;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private LayerMask _distanceToGroundLayerMask;

    [Header("MISC")]
    [SerializeField] private GameObject _parachute;
    [SerializeField] private Renderer _parachuteRenderer;
    [SerializeField] private float _fadeSpeed;

    [SerializeField] private List<Renderer> _childRendererList = new List<Renderer>();

    private bool _isDead = false;

    public float spawnChance
    { get { return _spawnChance; } }

    public bool allowGesturesDestroy
    { get { return _allowGesturesDestroy; } set { _allowGesturesDestroy = value; } }

    public bool isFreeFalling
    { get { return _isFreeFalling; } }

    public bool isDead
    { get { return _isDead; } }

    public List<GestureSO> gestureList
    { get { return _gestureList; } }

    public int currentIndex
    { get { return _currentIndex; } }

    // Start is called before the first frame update
    void Start()
    {
        if (AssetsGameScene.instance != null)
        {
            if (AssetsGameScene.instance.ui.canvas != null)
            {
                _canvas = AssetsGameScene.instance.ui.canvas;
            }
        }

        _currentIndex = 0;

        if (RecognizerManager.instance != null)
        {
            for (int i = 0; i < _numOfGestures; i++)
            {
                _gestureList.Add(GenerateRandom());
            }

            _gesturePanel = Instantiate(_gesturePanel, _canvas.transform);
            FollowGameObject();
            _gesturePanel.InitializePanel(_gestureList.ToArray());
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered += OnGestureTriggeredListener;
        }

        if (_animator != null) _animator.Play("Idle");
    }

    private void OnDestroy()
    {
        SafeDestroyInstatiated();

        if (GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered -= OnGestureTriggeredListener;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowGameObject();

        DetectGroundDistance();
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
                if (SceneManager.instance != null && EnemyManager.instance != null)
                {
                    if (EnemyManager.instance.allowPlayerDeath)
                    {
                        //SceneManager.instance.LoadScene("GameOver");
                        //if(GameManager.instance != null) GameManager.instance.Retry();
                        if (EnemyManager.instance.gameOverUI != null && GameManager.instance != null)
                        {
                            GameManager.instance.Pause();
                            EnemyManager.instance.gameOverUI.SetActive(true);
                        }
                    }

                    else
                    {
                        FreeFall();
                    }
                }
            }

            //FreeFall();
            //_isDead = true;
        }
    }

    private void SafeDestroyInstatiated()
    {
        if (_gesturePanel != null)
        {
            Destroy(_gesturePanel.gameObject, 5);
            _gesturePanel.gameObject.SetActive(false);
        }

        if (_lineRendererActive != null)
        {
            Destroy(_lineRendererActive.gameObject, 5);
            _lineRendererActive.gameObject.SetActive(false);
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
            _isFreeFalling = false;
            //Destroy(this.gameObject);

            Camera.main.TryGetComponent(out CameraController camController);

            if (camController != null) camController.Shake();

            StartCoroutine(BurnChildren());

            if (_hitGroundExplosion != null && _isDead)
            {
                _hitGroundExplosion = Instantiate(_hitGroundExplosion);
                _hitGroundExplosion.transform.position = this.transform.position;
                Destroy(_hitGroundExplosion.gameObject, 2);
            }
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
            if (RecognizerManager.instance != null)
            {
                RecognizerManager.instance.AllowJamming();
            }
        }
    }


    private void OnGestureTriggeredListener(GestureSO gesture)
    {
        if (_allowGesturesDestroy && _isGestureSeen && _currentIndex < _gestureList.Count)
        {
            if (gesture.gestureName == _gestureList[_currentIndex].gestureName)
            {
                if (RecognizerManager.instance != null)
                {
                    RecognizerManager.instance.hasGestureDetected = true;
                }

                _currentIndex++;
                _gesturePanel?.RemoveCurrentGesture();
            }
        }

        if (_currentIndex >= _gestureList.Count)
        {
            DebugHandler.Log("CurrentIndex is greater than _gestureList.Count");

            if (_gesturePanel != null) _gesturePanel.gameObject.SetActive(false);

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
        DebugHandler.Log("Freefalling");

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(_onKillScore);
        }

        if (_rigidBody != null && _collider != null)
        {
            _rigidBody.drag = 0;
            _collider.isTrigger = false;
        }

        if (_gesturePanel != null)
        {
            Destroy(_gesturePanel.gameObject);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered -= OnGestureTriggeredListener;
        }

        if (_parachuteRenderer != null)
        {
            StartCoroutine(BurnParachute());
        }

        _isFreeFalling = true;

        if (_animator != null) _animator.SetBool("isFreeFalling", true);
    }

    private void DetectGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _distanceToGround, _distanceToGroundLayerMask) && !_isDead)
        {
            //DebugHandler.Log("Trying to detect ground distance");
            if (hit.collider.CompareTag("KillZone"))
            {
                //DebugHandler.Log("Ground distance detected");

                if (Vector3.Distance(this.transform.position, hit.collider.transform.position) <= _distanceToGround)
                {
                    // Show the line renderer
                    if (_lineRendererActive == null)
                    {
                        _lineRendererActive = Instantiate(_lineRenderer);

                        _lineRendererActive.SetPosition(0, transform.position + Vector3.down);
                        _lineRendererActive.SetPosition(1, transform.position + Vector3.down);
                    }
                    _lineRendererActive.SetPosition(0, transform.position + Vector3.down);
                    _lineRendererActive.SetPosition(1, Vector3.MoveTowards(_lineRendererActive.GetPosition(1), hit.point, Time.deltaTime * 10f));
                }
            }
        }

        if (_isDead && _lineRendererActive != null)
        {
            Destroy(_lineRendererActive.gameObject);
        }
    }


    private IEnumerator BurnParachute()
    {
        if (_parachuteRenderer == null) yield break;

        float noiseStep = _parachuteRenderer.material.GetFloat("_Noise_Step");

        while (noiseStep > 0)
        {
            noiseStep -= _fadeSpeed * Time.deltaTime;
            _parachuteRenderer.material.SetFloat("_Noise_Step", noiseStep);

            yield return null;
        }

        _parachuteRenderer.material.SetFloat("_Noise_Step", 0f);

        if (_parachute != null && _parachute.activeSelf)
        {
            _parachute.SetActive(false);
        }
    }


    private IEnumerator BurnChildren()
    {
        if (_childRendererList == null || _childRendererList.Count == 0) yield break;

        float maxNoiseStep = 0;
        foreach (Renderer renderer in _childRendererList)
        {
            if (renderer != null)
            {
                float noiseStep = renderer.material.GetFloat("_Noise_Step");
                if (noiseStep > maxNoiseStep)
                {
                    maxNoiseStep = noiseStep;
                }
            }
        }

        while (maxNoiseStep > 0)
        {
            foreach (Renderer renderer in _childRendererList)
            {
                if (renderer != null)
                {
                    float noiseStep = renderer.material.GetFloat("_Noise_Step");
                    noiseStep -= _fadeSpeed * Time.deltaTime;
                    if (noiseStep < 0)
                    {
                        noiseStep = 0;
                    }
                    renderer.material.SetFloat("_Noise_Step", noiseStep);
                }
            }
            maxNoiseStep -= _fadeSpeed * Time.deltaTime;
            yield return null;
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        Destroy(gameObject, 10);
    }
}
