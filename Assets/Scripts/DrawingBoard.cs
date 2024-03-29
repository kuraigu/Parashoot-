using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading;
using System.Threading.Tasks;
public class DrawingBoard : MonoBehaviour
{
    private bool _isHeld;
    [SerializeField]
    private LineDrawer _lineDrawerReference;

    [SerializeField]
    private float _timeToFade;

    [SerializeField]
    private float _holdTimeThreshold = 1f;

    private Dictionary<int, float> _holdTimeTrackerDict = new Dictionary<int, float>();

    private Dictionary<int, LineDrawer> _lineDrawerDict = new Dictionary<int, LineDrawer>();

    //private LineDrawer _lineDrawer;

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
        if (SystemInfo.deviceType == DeviceType.Desktop)
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
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!_isHeld)
                {
                    _isHeld = true;

                    // Destroy the previous line drawer if there is one
                    if (_lineDrawerDict.Count > 0)
                    {
                        int lastIndex = _lineDrawerDict.Count - 1;
                        Destroy(_lineDrawerDict[lastIndex].gameObject, _timeToFade);
                        _lineDrawerDict.Remove(lastIndex);
                    }

                    // Create a new line drawer and add it to the dictionary
                    int newIndex = _lineDrawerDict.Count;
                    _lineDrawerDict[newIndex] = Instantiate(_lineDrawerReference);
                    _initialPosition = Input.mousePosition;
                    _lineDrawerDict[newIndex].transform.position = new Vector3(_lineDrawerDict[newIndex].transform.position.x, _lineDrawerDict[newIndex].transform.position.y + 1, 0f);
                }
            }

            if (_isHeld)
            {

                if (_lineDrawerDict.Count > 0)
                {
                    int lastIndex = _lineDrawerDict.Count - 1;
                    if (_lineDrawerDict[lastIndex] != null)
                    {
                        Vector3 mousePosition = Input.mousePosition;
                        mousePosition.z = -(Camera.main.transform.position.z);

                        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                        _lineDrawerDict[lastIndex].UpdateLine(mousePosition);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_lineDrawerDict.Count > 0)
            {
                int lastIndex = _lineDrawerDict.Count - 1;
                if (_lineDrawerDict[lastIndex] != null)
                {

                    if (RecognizerManager.instance != null)
                    {
                        //RecognizerManager.instance.CheckAllGestures(_lineDrawerDict[lastIndex].pointList);
                        RecognizerManager.instance.OnCheckAllGestures?.Invoke(_lineDrawerDict[lastIndex].pointList);
                    }


                    Destroy(_lineDrawerDict[lastIndex].gameObject, _timeToFade);
                    _lineDrawerDict.Remove(lastIndex);
                    _isHeld = false;
                    _finalPosition = Input.mousePosition;
                    _direction = _finalPosition - _initialPosition;
                }
            }

            DebugHandler.Log("No Longer held");
        }
    }


    private void TouchControl()
    {
        int numOfTouches = Input.touchCount;

        for (int i = 0; i < numOfTouches; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    continue;
                }

                // Create a new line drawer and add it to the dictionary for this touch
                _lineDrawerDict[touch.fingerId] = Instantiate(_lineDrawerReference);
                _lineDrawerDict[touch.fingerId].transform.position = new Vector3(_lineDrawerDict[touch.fingerId].transform.position.x, _lineDrawerDict[touch.fingerId].transform.position.y + 1, 0f);

                _initialPosition = touch.position;
            }

            if (_lineDrawerDict.TryGetValue(touch.fingerId, out LineDrawer lineDrawer))
            {
                // Update the line drawer associated with this touch
                if (lineDrawer != null)
                {
                    Vector3 touchPosition = touch.position;
                    touchPosition.z = -(Camera.main.transform.position.z);
                    touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                    lineDrawer.UpdateLine(touchPosition);

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (RecognizerManager.instance != null)
                        {
                            //RecognizerManager.instance.CheckAllGestures(lineDrawer.pointList);
                            RecognizerManager.instance.OnCheckAllGestures?.Invoke(lineDrawer.pointList);
                        }

                        // Destroy the line drawer associated with this touch
                        Destroy(lineDrawer.gameObject, _timeToFade);
                        _lineDrawerDict.Remove(touch.fingerId);
                        Debug.Log("No Longer held");
                    }
                }
            }
        }
    }
}
