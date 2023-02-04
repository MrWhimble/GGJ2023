using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    [SerializeField] private string moneyPrefName;
    //[SerializeField] private PrefData[] prefs;

    [SerializeField] private PrefData[] weaponPrefs;
    [SerializeField] private PrefData speedPref;
    [SerializeField] private PrefData damage1Pref;
    [SerializeField] private PrefData damage2Pref;
    [SerializeField] private PrefData shieldPref;

    private int _money;

    private void LoadShopPrefs()
    {
        _money = PlayerPrefs.GetInt(moneyPrefName, 0);

        if (weaponPrefs is {Length: > 1})
        {
            for (int i = 1; i < weaponPrefs.Length; i++)
            {
                weaponPrefs[i].state = PlayerPrefs.GetInt(weaponPrefs[i].name, 0) != 0;
            }
        }
        speedPref.state = PlayerPrefs.GetInt(speedPref.name, 0) != 0;
        damage1Pref.state = PlayerPrefs.GetInt(damage1Pref.name, 0) != 0;
        damage2Pref.state = PlayerPrefs.GetInt(damage2Pref.name, 0) != 0;
        shieldPref.state = PlayerPrefs.GetInt(shieldPref.name, 0) != 0;
    }
}

[System.Serializable]
public class PrefData
{
    public string name;
    public bool state;
    public GameObject prefab;
}