using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public class MissileController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _velocity;

    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private VisualEffect _explosionEffect;
    void Start()
    {
        if(_explosionEffect != null)
        {
            _explosionEffect = Instantiate(_explosionEffect);
            _explosionEffect.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _target.gameObject)
        {
        
            if(_explosionEffect != null)
            {
                _explosionEffect.gameObject.SetActive(true);
                _explosionEffect.gameObject.transform.position = this.transform.position;
                Destroy(_explosionEffect.gameObject, 3);
            }


            Camera.main.TryGetComponent(out CameraController camController);

            if(camController != null) camController.Shake();

            Destroy(this.gameObject);
        }

        else return;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    
    public void FollowTarget()
    {
        if (_target != null && _rigidBody != null)
        {
            Vector3 direction = _target.transform.position - this.transform.position;
         
            this.transform.LookAt(this.transform.position + direction);

            _rigidBody.MovePosition(this.transform.position + direction * _velocity * Time.fixedDeltaTime);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
