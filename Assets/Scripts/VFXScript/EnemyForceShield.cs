using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyForceShield : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    [SerializeField]
    private VisualEffect _forceFieldVFX;

    [SerializeField]
    private float _dissolveRate;

    [SerializeField]
    private bool _allowDissolve;

    // Start is called before the first frame update
    void Start()
    {
        _allowDissolve = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy != null)
        {
            if (_enemy.currentIndex >= _enemy.gestureList.Count - 2)
            {
                _allowDissolve = true;
            }
        }

        if (_allowDissolve)
        {
            Dissolve();
        }
    }

    private void Dissolve()
    {
        if (_forceFieldVFX != null)
        {
            float currentStep = _forceFieldVFX.GetFloat("Step");

            if(currentStep > 0)
            {
                currentStep -= _dissolveRate * Time.deltaTime;
                _forceFieldVFX.SetFloat("Step", currentStep);
            }

            else 
            {
                _forceFieldVFX.SetFloat("Step", 0);
                _forceFieldVFX.Stop();
            }
        }
    }

}
