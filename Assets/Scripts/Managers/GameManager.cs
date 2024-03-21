using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    // Events
    public UnityEvent<bool> OnLevelEndArrival;
    public UnityEvent<int> OnHPValueChanged;
    public UnityEvent<int> OnCurrencyValueChanged;
    public UnityEvent<int> OnSilkBundleValueChanged;

    // pause state
    bool _isPaused = false;
    public bool isPaused
    {
        get => _isPaused;
        set => _isPaused = value;
    }

    // end of level state
    bool _isAtEndOfLevel = false;
    public bool isAtEndOfLevel
    {
        get => _isAtEndOfLevel;
        set
        {
            _isAtEndOfLevel = value;
            OnLevelEndArrival?.Invoke(_isAtEndOfLevel);
            Debug.Log("_isAtEndOfLevel: " + _isAtEndOfLevel);
        }
    }

    // PLAYER ATTRIBUTES
    // Health 
    [SerializeField] int maxHP = 5;
    private int _currentHP = 5;
    public int currentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = value;

            _currentHP = Mathf.Clamp(_currentHP, 0, maxHP);

            OnHPValueChanged?.Invoke(_currentHP);

            if (_currentHP <= 0)
            {
                Debug.Log("You dead");
            }
            //Debug.Log("HP has been set to: " + _currentHP.ToString());
        }
    }

    // Currency
    [SerializeField] int maxCurrency = 999999;
    private int _currentCurrency = 0;
    public int currentCurrency
    {
        get { return _currentCurrency; }
        set
        {
            _currentCurrency = value;
            if (_currentCurrency > maxCurrency)
                _currentCurrency = maxCurrency;
            OnCurrencyValueChanged?.Invoke(_currentCurrency);

            Debug.Log("Currency has been set to: " + _currentCurrency.ToString());
        }
    }

    // Silk 
    [SerializeField] int maxSilkBundles = 99;
    private int _currentSilkBundles = 0;
    public int currentSilkBundles
    {
        get { return _currentSilkBundles; }
        set
        {
            _currentSilkBundles = value;
            if (_currentSilkBundles > maxSilkBundles)
                _currentSilkBundles = maxSilkBundles;
            OnSilkBundleValueChanged?.Invoke(_currentSilkBundles);

            Debug.Log("Silk Bundles has been set to: " + _currentSilkBundles.ToString());
        }
    }


    protected override void Awake()
    {
        base.Awake();
    }

    public void TestGameManager()
    {
        Debug.Log("GameManager test function");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
