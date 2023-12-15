using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    // Fields

    [Header("Upgrades smooth")]
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

    [Header("Upgrades")]
    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private Color _upgradedColor;
    [SerializeField] private Color _toUpgradeColor;

    [SerializeField] private playerAttack _playerAttack;
    [SerializeField] Slider _upgradeSliderSp1;
    [SerializeField] Slider _upgradeSliderSp2;
    [SerializeField] Slider _upgradeSliderSp3;

    [Header("Stamina")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private float _staminaMaxValue;
    [SerializeField] private float _staminaCurrentValue;
    private float _staminaCheckValue;
    private Coroutine _staminaSlideCoroutine;
    [SerializeField] private float _staminaSlideSpeed;

    [Header("Wave")]
    [SerializeField] private int _waveCurrentValue;
    private int _waveCheckValue;
    [SerializeField] private TextMeshProUGUI _waveText;

    [Header("XP")]
    [SerializeField] private int _xpCurrentValue;
    private int _xpCheckValue;
    [SerializeField] private TextMeshProUGUI _xpText;

    [Header("Spells")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;

    [SerializeField] private Image _spell1BackgroundImage;
    [SerializeField] private Image _spell2BackgroundImage;
    [SerializeField] private Image _spell3BackgroundImage;
    [SerializeField] private GameObject _bordersSpell1;
    [SerializeField] private GameObject _bordersSpell2;
    [SerializeField] private GameObject _bordersSpell3;

    // Properties



    // Methods
    public void ChangeSpellColorState(int index)
    {
        switch (index)
        {
            case 1:
                _spell1BackgroundImage.color = _selectedColor;
                _spell2BackgroundImage.color = _unselectedColor;
                _spell3BackgroundImage.color = _unselectedColor;
                _bordersSpell1.SetActive(true);
                _bordersSpell2.SetActive(false);
                _bordersSpell3.SetActive(false);
                break;
            case 2:
                _spell1BackgroundImage.color = _unselectedColor;
                _spell2BackgroundImage.color = _selectedColor;
                _spell3BackgroundImage.color = _unselectedColor;
                _bordersSpell1.SetActive(false);
                _bordersSpell2.SetActive(true);
                _bordersSpell3.SetActive(false);
                break;
            case 3:
                _spell1BackgroundImage.color = _unselectedColor;
                _spell2BackgroundImage.color = _unselectedColor;
                _spell3BackgroundImage.color = _selectedColor;
                _bordersSpell1.SetActive(false);
                _bordersSpell2.SetActive(false);
                _bordersSpell3.SetActive(true);
                break;
        }
    }
    public void UpgradeSkillsPanelSlide()   // Manage the position and slide of upgrades Panel
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
    private void CheckStateLevels()     // Check if upgrade buttons must be in color in state acquired or not
    {
        // Ancien code qui faisait afficher tout les boutons en clair sauf les lumineux. Si on veut remmettre ça ne pas oublier de retirer les sp1Up0 etc... dans la liste upgrades buttons !!!
        //if (_playerAttack.Spell1Level != 0)
        //{
        //    if (_upgradeButtons[_playerAttack.Spell1Level - 1].GetComponent<Image>().color != _upgradedColor)
        //    {
        //        _upgradeButtons[_playerAttack.Spell1Level - 1].GetComponent<Image>().color = _upgradedColor;
        //        _upgradeSliderSp1.value = (float)_playerAttack.Spell1Level / 3f;
        //    }
        //}
        
        //if(_playerAttack.Spell2Level != 0)
        //{
        //    if (_upgradeButtons[_playerAttack.Spell2Level + 2].GetComponent<Image>().color != _upgradedColor)
        //    {
        //        _upgradeButtons[_playerAttack.Spell2Level + 2].GetComponent<Image>().color = _upgradedColor;
        //        _upgradeSliderSp2.value = (float) _playerAttack.Spell2Level / 3f;
        //    }
        //}
        
        //if (_playerAttack.Spell3Level != 0)
        //{
        //    if (_upgradeButtons[_playerAttack.Spell3Level + 5].GetComponent<Image>().color != _upgradedColor)
        //    {
        //        _upgradeButtons[_playerAttack.Spell3Level + 5].GetComponent<Image>().color = _upgradedColor;
        //        _upgradeSliderSp3.value = (float)_playerAttack.Spell3Level / 3f;
        //    }
        //}

        
        if (_upgradeButtons[_playerAttack.Spell1Level].GetComponent<Image>().color != _upgradedColor)
        {
            _upgradeButtons[_playerAttack.Spell1Level].GetComponent<Image>().color = _upgradedColor;
            _upgradeSliderSp1.value = (float)_playerAttack.Spell1Level / 3f;
            if(_playerAttack.Spell1Level > 0)
            {
                _upgradeButtons[_playerAttack.Spell1Level - 1].GetComponent<Image>().color = _toUpgradeColor;
            }
        }
        

        
        if (_upgradeButtons[_playerAttack.Spell2Level + 4].GetComponent<Image>().color != _upgradedColor)
        {
            _upgradeButtons[_playerAttack.Spell2Level + 4].GetComponent<Image>().color = _upgradedColor;
            _upgradeSliderSp2.value = (float)_playerAttack.Spell2Level / 3f;
            if(_playerAttack.Spell2Level > 0)
            {
                _upgradeButtons[_playerAttack.Spell2Level + 3].GetComponent<Image>().color = _toUpgradeColor;
            }
        }
        

        
        if (_upgradeButtons[_playerAttack.Spell3Level + 8].GetComponent<Image>().color != _upgradedColor)
        {
            _upgradeButtons[_playerAttack.Spell3Level + 8].GetComponent<Image>().color = _upgradedColor;
            _upgradeSliderSp3.value = (float)_playerAttack.Spell3Level / 3f;
            if(_playerAttack.Spell3Level > 0)
            {
                _upgradeButtons[_playerAttack.Spell3Level + 7].GetComponent<Image>().color = _toUpgradeColor;
            }
        }
        

    }

    private void CheckStaminaValue()    // Check stamina value in gameManager(i suppose it will be in GM or in player)
    {
        // If staminaCurrentValue != gmaManager.GetStaminaValue()
            // Change staminaValue
    }
    private void UpdateStaminaSlider()  // Update state of stamina slider with coroutine
    {
        if(_staminaSlideCoroutine != null)
        {
            StopCoroutine(_staminaSlideCoroutine);
        }
        _staminaSlideCoroutine = StartCoroutine(StaminaSlideCoroutine());
    }

    private void CheckXpValue()    // Check Xp value in gameManager(i suppose it will be in GM or in player)
    {
        // If _xpCurrentValue != gmaManager.GetXpValue()
            // Change xpValue
    }
    private void UpdateXpText()
    {
        _xpText.text = _xpCurrentValue.ToString();
    }
    private void CheckWaveValue()   // Check Wave value in gameManager(i suppose it will be in GM or in player)
    {
        // If _waveCurrentValue != gmaManager.GetWaveValue()
            // Change waveValue
    }
    private void UpdateWaveText()
    {
        _waveText.text = _waveCurrentValue.ToString();
    }

    private void OnValidate()
    {
        if(_staminaCheckValue != _staminaCurrentValue)
        {
            _staminaCurrentValue = Mathf.Clamp(_staminaCurrentValue, 0, _staminaMaxValue);

            _staminaCheckValue = _staminaCurrentValue;

            UpdateStaminaSlider();
        }
        if(_xpCheckValue != _xpCurrentValue)
        {
            if(_xpCurrentValue < 0)
            {
                _xpCurrentValue = 0;
            }
            _xpCheckValue = _xpCurrentValue;
            UpdateXpText();
        }
        if(_waveCheckValue != _waveCurrentValue)
        {
            if (_waveCurrentValue < 0)
            {
                _waveCurrentValue = 0;
            }
            _waveCheckValue = _waveCurrentValue;
            UpdateWaveText();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _staminaCurrentValue = _staminaMaxValue;
        _staminaCheckValue = _staminaCurrentValue;
        UpdateStaminaSlider();
        UpdateWaveText();
        UpdateXpText();
        ChangeSpellColorState(1);
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


    IEnumerator StaminaSlideCoroutine() // Coroutine to smooth stamina change visual in slider
    {
        float targetValue = _staminaCurrentValue / _staminaMaxValue;
        while (_staminaSlider.value != targetValue)
        {
            _staminaSlider.value = Mathf.Lerp(_staminaSlider.value, targetValue, Time.deltaTime * _staminaSlideSpeed);

            yield return null;
        }

        yield return null;
    }
}
