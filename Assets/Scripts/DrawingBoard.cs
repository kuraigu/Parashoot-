using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingBoard : MonoBehaviour
{
    private bool _isHeld;
    [SerializeField]
    private LineDrawer _lineDrawerReference;

    [SerializeField]
    private float _timeToFade;

    private LineDrawer _lineDrawer;

    Vector2 _initialPosition;
    Vector2 _finalPosition;
    Vector2 _direction;
    

    // Start is called before the first frame update
    void Start()
    {
        _isHeld = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            MouseControl();
        }

        else
        {
            TouchControl();
        }
    }


    void MouseControl()
    {
        if(Input.GetMouseButton(0))
        {
            if(!_isHeld)
            {
                _isHeld = true;
                if (_lineDrawer != null) Destroy(_lineDrawer, _timeToFade);
                _lineDrawer = Instantiate(_lineDrawerReference);
                _initialPosition = Input.mousePosition;
                _lineDrawer.transform.position = new Vector3(_lineDrawer.transform.position.x, _lineDrawer.transform.position.y + 1, 0f);
            }
        }

        if (_isHeld)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -(Camera.main.transform.position.z);

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _lineDrawer.UpdateLine(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(_lineDrawer.gameObject, _timeToFade);
            _isHeld = false;
            _finalPosition = Input.mousePosition;

            _direction = _finalPosition - _initialPosition;

            if(RecognizerManager.instance != null)
            {
                RecognizerManager.instance.CheckAllGestures(_lineDrawer.pointList);
            }


            Debug.Log("No Longer held");
        }    
    }


    void TouchControl()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!_isHeld)
            {
                _isHeld = true;
                if (_lineDrawer != null) Destroy(_lineDrawer, _timeToFade);
                _lineDrawer = Instantiate(_lineDrawerReference);
                _initialPosition = Input.mousePosition;
                _lineDrawer.transform.position = new Vector3(_lineDrawer.transform.position.x, _lineDrawer.transform.position.y + 1, 0f);
            }
        }

        if (_isHeld)
        {
            Vector3 mousePosition = Input.GetTouch(0).position;
            mousePosition.z = -(Camera.main.transform.position.z);

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _lineDrawer.UpdateLine(mousePosition);
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Destroy(_lineDrawer.gameObject, _timeToFade);
            _isHeld = false;
            _finalPosition = Input.mousePosition;

            _direction = _finalPosition - _initialPosition;

            if (RecognizerManager.instance != null)
            {
                RecognizerManager.instance.CheckAllGestures(_lineDrawer.pointList);
            }


            Debug.Log("No Longer held");
        }
    }
}
