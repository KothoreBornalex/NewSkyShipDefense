using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    // Fields
    [SerializeField] private RectTransform _upgradeSkillsPanel;

    [SerializeField] private RectTransform _openPosition;
    [SerializeField] private RectTransform _closedPosition;

    [SerializeField] private RectTransform _upgradesOpenPosition;
    [SerializeField] private RectTransform _upgradesClosePosition;

    [SerializeField] private RectTransform _upgradesPack;

    private bool _isOpen;

    [SerializeField] private float _slideSpeed;

    [SerializeField] private RectTransform _openCloseIcon;
    [SerializeField] private float _rotationSpeed;


    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private Color _upgradedColor;

    [SerializeField] private playerAttack _playerAttack;


    // Properties



    // Methods
    public void UpgradeSkillsPanelSlide()
    {
        if( _isOpen)
        {
            _isOpen = false;
            StartCoroutine(CloseUpgradePanel());
            if(_openCloseIcon != null)
            {
                StartCoroutine(RotateCloseIcon());  // Rotate icon to arrow point top
            }
            else
            {
                Debug.LogWarning("Forgot to drag the icon ui button close/open so it won't rotate !!!");
            }
        }
        else
        {
            _isOpen = true;
            StartCoroutine(OpenUpgradePanel());
            if(_openCloseIcon != null)
            {
                StartCoroutine(RotateOpenIcon());   // Rotate icon to arrow point down
            }
            else
            {
                Debug.LogWarning("Forgot to drag the icon ui button close/open so it won't rotate !!!");
            }
        }
    }

    private void CheckStateLevels()
    {
        if (_playerAttack.Spell1Level != 0)
        {
            if (_upgradeButtons[_playerAttack.Spell1Level - 1].GetComponent<Image>().color != _upgradedColor)
            {
                _upgradeButtons[_playerAttack.Spell1Level - 1].GetComponent<Image>().color = _upgradedColor;
            }
        }
        
        if(_playerAttack.Spell2Level != 0)
        {
            if (_upgradeButtons[_playerAttack.Spell2Level + 2].GetComponent<Image>().color != _upgradedColor)
            {
                _upgradeButtons[_playerAttack.Spell2Level + 2].GetComponent<Image>().color = _upgradedColor;
            }
        }
        
        if (_playerAttack.Spell3Level != 0)
        {
            if (_upgradeButtons[_playerAttack.Spell3Level + 5].GetComponent<Image>().color != _upgradedColor)
            {
                _upgradeButtons[_playerAttack.Spell3Level + 5].GetComponent<Image>().color = _upgradedColor;
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckStateLevels();
    }

    IEnumerator OpenUpgradePanel()
    {
        Vector3 positionTemp = _upgradeSkillsPanel.position;

        Vector3 upPositionTemp = _upgradesPack.position;
        
        while ((_upgradeSkillsPanel.position.y != _openPosition.position.y) && (_upgradesPack.position != _upgradesOpenPosition.position) && _isOpen)
        {

            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _openPosition.position.y, Time.deltaTime * _slideSpeed);

            upPositionTemp.y = Mathf.Lerp(_upgradesPack.position.y, _upgradesOpenPosition.position.y, Time.deltaTime* _slideSpeed * 4f);

            _upgradeSkillsPanel.position = positionTemp;

            _upgradesPack.position = upPositionTemp;

            yield return null;
        }

        yield return null;
    }
    IEnumerator CloseUpgradePanel()
    {
        Vector3 positionTemp = _upgradeSkillsPanel.position;

        Vector3 upPositionTemp = _upgradesPack.position;

        while ((_upgradeSkillsPanel.position.y != _closedPosition.position.y) && (_upgradesPack.position != _upgradesClosePosition.position) && !_isOpen)
        {
            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _closedPosition.position.y, Time.deltaTime * _slideSpeed);

            upPositionTemp.y = Mathf.Lerp(_upgradesPack.position.y, _upgradesClosePosition.position.y, Time.deltaTime * _slideSpeed * 4f);

            _upgradeSkillsPanel.position = positionTemp;

            _upgradesPack.position = upPositionTemp;

            yield return null;
        }

        yield return null;
    }


    IEnumerator RotateOpenIcon()
    {
        Vector3 rotationTemp = _openCloseIcon.rotation.eulerAngles;

        Vector3 rotationOpened = new Vector3(0, 0, 0);  // fleche pointant vers bas

        while (_openCloseIcon.rotation.eulerAngles.z != rotationOpened.z && _isOpen)
        {
            rotationTemp.z = Mathf.Lerp(_openCloseIcon.rotation.eulerAngles.z, rotationOpened.z, Time.deltaTime * _rotationSpeed);

            _openCloseIcon.rotation = Quaternion.Euler(rotationTemp);

            yield return null;
        }

        yield return null;
    }
    IEnumerator RotateCloseIcon()
    {
        Vector3 rotationTemp = _openCloseIcon.rotation.eulerAngles;

        Vector3 rotationClosed = new Vector3(0, 0, 180);  // fleche pointant vers haut

        while (_openCloseIcon.rotation.eulerAngles.z != rotationClosed.z && !_isOpen)
        {
            rotationTemp.z = Mathf.Lerp(_openCloseIcon.rotation.eulerAngles.z, rotationClosed.z, Time.deltaTime * _rotationSpeed);

            _openCloseIcon.rotation = Quaternion.Euler(rotationTemp);

            yield return null;
        }

        yield return null;
    }
}
