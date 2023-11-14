using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;


    [Header("Time Fields")]
    [SerializeField] private bool _isTimeSpeeding;
    private float _currentDeltaTime;
    private float _currentTimeSpeed;
    [SerializeField] private float _slowTimeSpeed;
    [SerializeField] private float _fastTimeSpeed;
    public bool IsTimeSpeeding { get => _isTimeSpeeding; set => _isTimeSpeeding = value; }


    [Header("Lighting Fields")]
    [SerializeField] private bool _isSunRotating;
    [SerializeField] private float _rotatingSpeed;
    [SerializeField] private Transform _directionalLightTransform;
    private DirectionalLight _directionalLight;


    [Header("BackGround Fields")]
    [SerializeField] private bool _isBackGroundMoving;
    [SerializeField] private Transform _backgroundElementsParent;
    [SerializeField] private Vector3 _backgroundBoundingBox;
    [SerializeField] private Color _boundingBoxColor;
    private float _currentBackGroundMovingSpeed;
    [SerializeField, Range(0, 750)] private float _slowBackGroundMovingSpeed;
    [SerializeField, Range(0, 750)] private float _fastBackGroundMovingSpeed;

    [SerializeField] private Transform[] _backgroundElements;


    [Header("Ship Fields")]
    [SerializeField, Range(0, 20)] private float _shipSpeed;
    [SerializeField] private Transform _leftShip;
    private Transform _baseLeftShip;
    [SerializeField] private Transform _rightShip;
    private Transform _baseRightShip;







    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        // _directionalLight = _directionalLightTransform.GetComponent<Light>();

        _baseLeftShip = Instantiate<GameObject>(new GameObject(), transform).transform;
        _baseLeftShip.SetPositionAndRotation(_leftShip.position, _leftShip.rotation);
        _baseRightShip = Instantiate<GameObject>(new GameObject(), transform).transform;
        _baseRightShip.SetPositionAndRotation(_rightShip.position, _rightShip.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        if (_isTimeSpeeding)
        {
            _currentTimeSpeed = Mathf.Lerp(_currentTimeSpeed, _fastTimeSpeed, Time.deltaTime * 0.7F);

            _currentBackGroundMovingSpeed = Mathf.Lerp(_currentBackGroundMovingSpeed, _fastBackGroundMovingSpeed, Time.deltaTime * 0.7f);
        }
        else
        {
            _currentTimeSpeed = Mathf.Lerp(_currentTimeSpeed, _slowTimeSpeed, Time.deltaTime * 5.0F);

            _currentBackGroundMovingSpeed = Mathf.Lerp(_currentBackGroundMovingSpeed, _slowBackGroundMovingSpeed, Time.deltaTime * 0.7f);
        }
        _currentDeltaTime = Time.deltaTime * _currentTimeSpeed;


        if (_isBackGroundMoving)
        {
            UpdateMovingFunction();
        }

        if(_isSunRotating)
        {
            _directionalLightTransform.Rotate(new Vector3(_rotatingSpeed * _currentDeltaTime, 0, 0));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _boundingBoxColor;
        Gizmos.DrawCube(_backgroundElementsParent.position, _backgroundBoundingBox);
    }

    void UpdateMovingFunction()
    {
        foreach (Transform t in _backgroundElements)
        {
            MovingFunction(t);
        }
    }

    private void MovingFunction(Transform t)
    {

        t.Translate(-Vector3.forward * _currentDeltaTime * _currentBackGroundMovingSpeed, Space.World);


        if (t.localPosition.z <= -(_backgroundBoundingBox.z / 2))
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, (_backgroundBoundingBox.z / 2));
        }
    }


    void MovingShipFunction()
    {

    }
}
