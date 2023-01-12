using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _allowShake;
    // Start is called before the first frame update

    private Vector3 _originalPosition;

    void Start()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.OnEnemyDeath += CameraShakeListener;
        }

        _originalPosition = transform.position;

        _allowShake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_allowShake)
        {
            ShakeCamera();
        }
    }

    void CameraShakeListener()
    {
        StartCoroutine(ShakeTimer(1f));
    }

    IEnumerator ShakeTimer(float timer)
    {
        _allowShake = true;
        yield return new WaitForSeconds(timer);

        transform.position = _originalPosition;
        _allowShake = false;
    }


    private void ShakeCamera()
    {
        Vector3 newPos;
        newPos.x = UnityEngine.Random.Range(-4.0f, 4.0f) + _originalPosition.x;
        newPos.y = UnityEngine.Random.Range(-4.0f, 4.0f) + _originalPosition.y;
        newPos.z = _originalPosition.z;

        this.transform.position = Vector3.MoveTowards(transform.position, newPos, 0.5f * Time.deltaTime);
    }

}
