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
    private Light _directionalLight;

    [SerializeField] private List<Light> _lights;
    private float _currentLightIntensity;
    private float _angleValue;



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
    [SerializeField, Range(0, 60)] private float _shipMovingSpeed;
    [SerializeField, Range(0, 20)] private float _shipRotatingSpeed;

    [SerializeField] private ShipPatrolStruct[] _shipsArray;


    [System.Serializable]
    public struct ShipPatrolStruct
    {
        public Transform _shipTransform;
        public bool _finishedPatrol;
        public bool _startPatrol;
        public int _currentPatrolPoint;

        [HideInInspector] public Transform _baseShipPoint;
        public Transform[] _startAnimationPoints;
        public Transform[] _endAnimationPoints;

    }




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
        _directionalLight = _directionalLightTransform.GetComponent<Light>();

        for(int i = 0; i < _shipsArray.Length; i++)
        {
            _shipsArray[i]._baseShipPoint = Instantiate<GameObject>(new GameObject(), transform).transform;
            _shipsArray[i]._baseShipPoint.SetPositionAndRotation(_shipsArray[i]._shipTransform.position, _shipsArray[i]._shipTransform.rotation);
        }

       /*Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        Color[] colorsArray = texture.GetPixels(texture.width / 2, texture.height / 2, 5, 5);
        foreach (Color color in colorsArray)
        {
            
        }*/


        
    }

    // Update is called once per frame
    void Update()
    {
        //angleValue = Vector3.Angle(Vector3.up, _directionalLightTransform.rotation.eulerAngles);
        //angleValue = Quaternion.Angle(Quaternion.LookRotation(Vector3.up), _directionalLightTransform.localRotation);

        _angleValue = Quaternion.Angle(Quaternion.LookRotation(Vector3.up), _directionalLightTransform.rotation);
        if(_angleValue >= 110)
        {
            Debug.Log("Jour");
            _currentLightIntensity = Mathf.Lerp(_currentLightIntensity, 0, Time.deltaTime);

        }
        else
        {
            Debug.Log("Nuit");
            _currentLightIntensity = Mathf.Lerp(_currentLightIntensity, 25.0f, Time.deltaTime);
        }


        if (_angleValue >= 90)
        {
            Debug.Log("Jour");
            _directionalLight.intensity = Mathf.Lerp(_directionalLight.intensity, 1, Time.deltaTime);

        }
        else
        {
            Debug.Log("Nuit");
            _directionalLight.intensity = Mathf.Lerp(_directionalLight.intensity, 0, Time.deltaTime);
        }

        foreach (Light light in _lights)
        {
            light.intensity = _currentLightIntensity;
        }



        if (_isTimeSpeeding)
        {
            UpdatingShipFunction();

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


    void UpdatingShipFunction()
    {

        for (int i = 0; i < _shipsArray.Length; i++)
        {

            if (!_shipsArray[i]._finishedPatrol)
            {
                if (_shipsArray[i]._startAnimationPoints.Length != 0 && _shipsArray[i]._startPatrol)
                {

                    Vector3 targetPosition = _shipsArray[i]._startAnimationPoints[_shipsArray[i]._currentPatrolPoint].transform.position;
                    Vector3 targetPositionRounded = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));

                    Vector3 shipPosition = _shipsArray[i]._shipTransform.position;
                    Vector3 shipPositionRounded = new Vector3(Mathf.Round(shipPosition.x), Mathf.Round(shipPosition.y), Mathf.Round(shipPosition.z));


                    if (targetPositionRounded == shipPositionRounded)
                    {
                        if (_shipsArray[i]._currentPatrolPoint == _shipsArray[i]._startAnimationPoints.Length - 1)
                        {
                            _shipsArray[i]._startPatrol = false;
                            _shipsArray[i]._currentPatrolPoint = 0;
                            _shipsArray[i]._shipTransform.SetPositionAndRotation(_shipsArray[i]._endAnimationPoints[0].position, _shipsArray[i]._endAnimationPoints[0].rotation);
                        }
                        else
                        {
                            _shipsArray[i]._currentPatrolPoint++;
                        }

                    }

                    TranslateShipTransform(_shipsArray[i]._shipTransform, _shipsArray[i]._startAnimationPoints[_shipsArray[i]._currentPatrolPoint].transform);

                }


                if (_shipsArray[i]._endAnimationPoints.Length != 0 && !_shipsArray[i]._startPatrol)
                {

                    Vector3 targetPosition = _shipsArray[i]._endAnimationPoints[_shipsArray[i]._currentPatrolPoint].transform.position;
                    Vector3 targetPositionRounded = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));

                    Vector3 shipPosition = _shipsArray[i]._shipTransform.position;
                    Vector3 shipPositionRounded = new Vector3(Mathf.Round(shipPosition.x), Mathf.Round(shipPosition.y), Mathf.Round(shipPosition.z));


                    if (targetPositionRounded == shipPositionRounded)
                    {
                        if (_shipsArray[i]._currentPatrolPoint == _shipsArray[i]._endAnimationPoints.Length - 1)
                        {
                            _shipsArray[i]._finishedPatrol = true;
                        }
                        else
                        {
                            _shipsArray[i]._currentPatrolPoint++;
                        }
                    }

                    EndPatrol(_shipsArray[i]._shipTransform, _shipsArray[i]._endAnimationPoints[_shipsArray[i]._currentPatrolPoint].transform);
                    

/*
                    if (_shipsArray[i]._baseShipPoint.position == _shipsArray[i]._shipTransform.position)
                    {
                        
                        _shipsArray[i]._finishedPatrol = true;
                        
                    }
                    
                    EndPatrol(_shipsArray[i]._shipTransform, _shipsArray[i]._baseShipPoint);*/
                }
            }
            
        }
    }

    public void ResetAllShips()
    {
        for(int i = 0; i < _shipsArray.Length; i++)
        {
            _shipsArray[i]._finishedPatrol = false;
            _shipsArray[i]._currentPatrolPoint = 0;
            _shipsArray[i]._startPatrol = true;
        }
    }


    public void TranslateShipTransform(Transform ship, Transform target)
    {

        Vector3 relativePos = target.position - ship.position;

        Quaternion newRotation = Quaternion.LookRotation(relativePos, ship.up);
        
        ship.rotation = Quaternion.Lerp(ship.rotation, newRotation, Time.deltaTime * _shipRotatingSpeed);

        ship.position = Vector3.Lerp(ship.position, target.position, Time.deltaTime * _shipMovingSpeed);
    }


    public void EndPatrol(Transform ship, Transform target)
    {

        Vector3 relativePos = target.position - ship.position;

        Quaternion newRotation = Quaternion.LookRotation(relativePos, ship.up);

        //ship.rotation = Quaternion.Lerp(ship.rotation, newRotation, Time.deltaTime * _shipRotatingSpeed);
        ship.rotation = Quaternion.Lerp(ship.rotation, target.rotation, Time.deltaTime * _shipRotatingSpeed);

        ship.position = Vector3.Lerp(ship.position, target.position, Time.deltaTime);
    }

    public bool AllShipsPatrolTurnEnded()
    {
        foreach (var ship in _shipsArray)
        {
            if (ship._finishedPatrol)
            {
                continue;
            }
            else
            {
                return false;
            }
            
        }

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _boundingBoxColor;
        Gizmos.DrawCube(_backgroundElementsParent.position, _backgroundBoundingBox);




        Gizmos.color = Color.green;
        for(int i = 0; i < _shipsArray.Length; i++)
        {

            if (_shipsArray[i]._startAnimationPoints.Length != 0)
            {
                for(int x = 0; x < _shipsArray[i]._startAnimationPoints.Length; x++)
                {
                    if (x != _shipsArray[i]._startAnimationPoints.Length - 1)
                    {
                        Gizmos.DrawSphere(_shipsArray[i]._startAnimationPoints[x].transform.position, 0.85f);
                        Gizmos.DrawLine(_shipsArray[i]._startAnimationPoints[x].transform.position, _shipsArray[i]._startAnimationPoints[x + 1].transform.position);
                    }
                    else
                    {
                        Gizmos.DrawSphere(_shipsArray[i]._startAnimationPoints[x].transform.position, 0.85f);
                        Gizmos.DrawLine(_shipsArray[i]._startAnimationPoints[x].transform.position, _shipsArray[i]._startAnimationPoints[0].transform.position);
                    }
                }
            }


            if (_shipsArray[i]._endAnimationPoints.Length != 0)
            {
                for (int x = 0; x < _shipsArray[i]._endAnimationPoints.Length; x++)
                {
                    if (x != _shipsArray[i]._endAnimationPoints.Length - 1)
                    {
                        Gizmos.DrawSphere(_shipsArray[i]._endAnimationPoints[x].transform.position, 0.85f);
                        Gizmos.DrawLine(_shipsArray[i]._endAnimationPoints[x].transform.position, _shipsArray[i]._endAnimationPoints[x + 1].transform.position);
                    }
                    else
                    {
                        Gizmos.DrawSphere(_shipsArray[i]._endAnimationPoints[x].transform.position, 0.85f);
                        //Gizmos.DrawLine(_shipsArray[i]._endAnimationPoints[x].transform.position, _shipsArray[i]._endAnimationPoints[0].transform.position);
                    }
                }
            }
        }
    }
}
