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
        }
        else
        {
            _isOpen = true;
            StartCoroutine(OpenUpgradePanel());
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
}
