using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingBoard : MonoBehaviour
{
    private bool _isHeld;
    [SerializeField]
    private LineDrawer _lineDrawerReference;
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
        MouseControl();
    }


    void MouseControl()
    {
        if(Input.GetMouseButton(0))
        {
            if(!_isHeld)
            {
                _isHeld = true;
                if (_lineDrawer != null) Destroy(_lineDrawer, 0.3f);
                _lineDrawer = Instantiate(_lineDrawerReference);
                _initialPosition = Input.mousePosition;
                _lineDrawer.transform.position = new Vector3(_lineDrawer.transform.position.x, _lineDrawer.transform.position.y, -1f);
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
            Destroy(_lineDrawer.gameObject, 0.3f);
            _isHeld = false;
            _finalPosition = Input.mousePosition;

            _direction = _finalPosition - _initialPosition;


            if(_lineDrawer.pointList.Count > 2)
            {
                if (RecognizerManager.instance != null)
                {
                    RecognizerManager.instance.CheckAllGestures(_direction.normalized);
                }
            }
           

            Debug.Log("No Longer held");
        }
    }
}
