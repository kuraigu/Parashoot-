using UnityEngine;

public class SimulateParachute : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _swayFrequency = 0.5f;
    [SerializeField] private float _swayAmplitude = 10f;
    [SerializeField] private float _maxSwayAngle = 30f;

    private float _initialRotation;
    private float _swayTime;

    private void Start()
    {
        _initialRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        if (_enemy != null)
        {
            if (!_enemy.isDead)
            {
                // Calculate the new sway angle based on a sine wave
                _swayTime += Time.deltaTime;
                float swayAngle = Mathf.Sin(_swayTime * _swayFrequency) * _swayAmplitude;

                // Limit the sway angle to the maximum allowed value
                swayAngle = Mathf.Clamp(swayAngle, -_maxSwayAngle, _maxSwayAngle);

                // Set the new rotation of the parachute
                transform.eulerAngles = new Vector3(0, 0, _initialRotation + swayAngle);
            }
        }
    }
}
