using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Give player upgrades 
    // Make upgrades spawnable in game
    // Communicate between scenes
    // Handle currency

    int playerCurrency;
    int upgradeCost;

    bool laserUpgrade = false;
    bool flameThrowerUpgrade = false;

    bool damageUpgrade = false;
    bool damage2Upgrade = false;

    bool shieldUpgrade = false;

    bool speedUpgrade = false;

    private void Start()
    {
        PlayerPrefs.GetInt("LaserUpgrade");
        PlayerPrefs.GetInt("FlameThrowerUpgrade");
        PlayerPrefs.GetInt("DamageUpgrade");
        PlayerPrefs.GetInt("Damage2Upgrade");
        PlayerPrefs.GetInt("ShieldUpgrade");
        PlayerPrefs.GetInt("SpeedUpgrade");

        playerCurrency = PlayerPrefs.GetInt("PlayerCurrency");


    }

    public void BuyingLaserUpgrade() 
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            laserUpgrade = true;

            PlayerPrefs.SetInt("LaserUpgrade", 1);
        }
    }

    public void BuyingFlameThrowerUpgrade()
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            flameThrowerUpgrade = true;

            PlayerPrefs.SetInt("FlameThrowerUpgrade", 1);
        }
    }

    public void BuyingDamageUpgrade()
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            damageUpgrade = true;

            PlayerPrefs.SetInt("DamageUpgrade", 1);
        }
    }
    public void BuyingDamage2Upgrade()
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            damage2Upgrade = true;

            PlayerPrefs.SetInt("Damage2Upgrade", 1);
        }
    }
    public void BuyingShieldUpgrade()
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            shieldUpgrade = true;

            PlayerPrefs.SetInt("ShieldUpgrade", 1);
        }
    }

    public void BuyingSpeedUpgrade()
    {
        if (playerCurrency >= upgradeCost)
        {
            playerCurrency -= upgradeCost;
            speedUpgrade = true;

            PlayerPrefs.SetInt("SpeedUpgrade", 1);
        }
    }


}
