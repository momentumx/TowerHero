using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaUpgradeMenu : MonoBehaviour 
{

	//public float mana;
	int houseHealth = 0;
	int wellHealth = 0;
	int startingMana = 0;
	int manaGain = 0;
	int towerHealth = 0;
	public Text manaText;

	float smCost;
	float mgCost;
	float hhCost;
	float whCost;
	float thCost;

	public Text hhButton;
	public Text whButton;
	public Text smButton;
	public Text mgButton;
	public Text thButton;

	public GameObject hh1;
	public GameObject hh2;
	public GameObject hh3;
	public GameObject hh4;
	public GameObject hh5;

	public GameObject wh1;
	public GameObject wh2;
	public GameObject wh3;
	public GameObject wh4;
	public GameObject wh5;

	public GameObject sm1;
	public GameObject sm2;
	public GameObject sm3;
	public GameObject sm4;
	public GameObject sm5;

	public GameObject mg1;
	public GameObject mg2;
	public GameObject mg3;
	public GameObject mg4;
	public GameObject mg5;

	public GameObject th1;
	public GameObject th2;
	public GameObject th3;
	public GameObject th4;
	public GameObject th5;

	// Use this for initialization
	void Start () 
	{
		if(Movement.maxMana < 200)
		{
			Movement.maxMana = 200;
		}

		mgCost = .3f*Movement.maxMana;
		smCost = .4f*Movement.maxMana;
		hhCost = .5f*Movement.maxMana;
		whCost = .6f*Movement.maxMana;
		thCost = .7f*Movement.maxMana;
		
		mgButton.text = "Upgrade (30%)";
		smButton.text = "Upgrade (40%)";
		hhButton.text = "Upgrade (50%)";
		whButton.text = "Upgrade (60%)";
		thButton.text = "Upgrade (70%)";
	
		// Get mana percentage left
		//mana = Movement.mana;
        manaText.text = ((int)((/*mana*/Movement.mana / Movement.maxMana) * 100)).ToString() + "%";

        // Set indicators for already purchased upgrades.
        if (PlayerPrefs.HasKey("HouseHealthUpgrade"))
		{
			houseHealth = PlayerPrefs.GetInt("HouseHealthUpgrade");			
		}

		if(PlayerPrefs.HasKey("WellHealthUpgrade"))
		{
			wellHealth = PlayerPrefs.GetInt("WellHealthUpgrade");			
		}

		if(PlayerPrefs.HasKey("StartingManaUpgrade"))
		{
			startingMana = PlayerPrefs.GetInt("StartingManaUpgrade");			
		}

		if(PlayerPrefs.HasKey("ManaGainUpgrade"))
		{
			manaGain = PlayerPrefs.GetInt("ManaGainUpgrade");			
		}

		if(PlayerPrefs.HasKey("TowerHealthUpgrade"))
		{
			towerHealth = PlayerPrefs.GetInt("TowerHealthUpgrade");			
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
        manaText.text = ((int)((Movement.mana / Movement.maxMana)*100)).ToString() + "%";
        // Keep indicators up to date
        switch (houseHealth) 
			{
				case 0:
					hh1.SetActive(false);
					hh2.SetActive(false);
					hh3.SetActive(false);
					hh4.SetActive(false);
					hh5.SetActive(false);
					break;
				case 1:
					hh1.SetActive(true);
					hh2.SetActive(false);
					hh3.SetActive(false);
					hh4.SetActive(false);
					hh5.SetActive(false);
					break;
				case 2:
					hh1.SetActive(true);
					hh2.SetActive(true);
					hh3.SetActive(false);
					hh4.SetActive(false);
					hh5.SetActive(false);
					break;
				case 3:
					hh1.SetActive(true);
					hh2.SetActive(true);
					hh3.SetActive(true);
					hh4.SetActive(false);
					hh5.SetActive(false);
					break;
				case 4:
					hh1.SetActive(true);
					hh2.SetActive(true);
					hh3.SetActive(true);
					hh4.SetActive(true);
					hh5.SetActive(false);
					break;
				case 5:
					hh1.SetActive(true);
					hh2.SetActive(true);
					hh3.SetActive(true);
					hh4.SetActive(true);
					hh5.SetActive(true);
					break;
			}

		switch (wellHealth) 
			{
				case 0:
					wh1.SetActive(false);
					wh2.SetActive(false);
					wh3.SetActive(false);
					wh4.SetActive(false);
					wh5.SetActive(false);
					break;
				case 1:
					wh1.SetActive(true);
					wh2.SetActive(false);
					wh3.SetActive(false);
					wh4.SetActive(false);
					wh5.SetActive(false);
					break;
				case 2:
					wh1.SetActive(true);
					wh2.SetActive(true);
					wh3.SetActive(false);
					wh4.SetActive(false);
					wh5.SetActive(false);
					break;
				case 3:
					wh1.SetActive(true);
					wh2.SetActive(true);
					wh3.SetActive(true);
					wh4.SetActive(false);
					wh5.SetActive(false);
					break;
				case 4:
					wh1.SetActive(true);
					wh2.SetActive(true);
					wh3.SetActive(true);
					wh4.SetActive(true);
					wh5.SetActive(false);
					break;
				case 5:
					wh1.SetActive(true);
					wh2.SetActive(true);
					wh3.SetActive(true);
					wh4.SetActive(true);
					wh5.SetActive(true);
					break;
			}

		switch (startingMana) 
			{
				case 0:
					sm1.SetActive(false);
					sm2.SetActive(false);
					sm3.SetActive(false);
					sm4.SetActive(false);
					sm5.SetActive(false);
					break;
				case 1:
					sm1.SetActive(true);
					sm2.SetActive(false);
					sm3.SetActive(false);
					sm4.SetActive(false);
					sm5.SetActive(false);
					break;
				case 2:
					sm1.SetActive(true);
					sm2.SetActive(true);
					sm3.SetActive(false);
					sm4.SetActive(false);
					sm5.SetActive(false);
					break;
				case 3:
					sm1.SetActive(true);
					sm2.SetActive(true);
					sm3.SetActive(true);
					sm4.SetActive(false);
					sm5.SetActive(false);
					break;
				case 4:
					sm1.SetActive(true);
					sm2.SetActive(true);
					sm3.SetActive(true);
					sm4.SetActive(true);
					sm5.SetActive(false);
					break;
				case 5:
					sm1.SetActive(true);
					sm2.SetActive(true);
					sm3.SetActive(true);
					sm4.SetActive(true);
					sm5.SetActive(true);
					break;
			}

		switch (manaGain) 
			{
				case 0:
					mg1.SetActive(false);
					mg2.SetActive(false);
					mg3.SetActive(false);
					mg4.SetActive(false);
					mg5.SetActive(false);
					break;
				case 1:
					mg1.SetActive(true);
					mg2.SetActive(false);
					mg3.SetActive(false);
					mg4.SetActive(false);
					mg5.SetActive(false);
					break;
				case 2:
					mg1.SetActive(true);
					mg2.SetActive(true);
					mg3.SetActive(false);
					mg4.SetActive(false);
					mg5.SetActive(false);
					break;
				case 3:
					mg1.SetActive(true);
					mg2.SetActive(true);
					mg3.SetActive(true);
					mg4.SetActive(false);
					mg5.SetActive(false);
					break;
				case 4:
					mg1.SetActive(true);
					mg2.SetActive(true);
					mg3.SetActive(true);
					mg4.SetActive(true);
					mg5.SetActive(false);
					break;
				case 5:
					mg1.SetActive(true);
					mg2.SetActive(true);
					mg3.SetActive(true);
					mg4.SetActive(true);
					mg5.SetActive(true);
					break;
			}

		switch (towerHealth) 
			{
				case 0:
					th1.SetActive(false);
					th2.SetActive(false);
					th3.SetActive(false);
					th4.SetActive(false);
					th5.SetActive(false);
					break;
				case 1:
					th1.SetActive(true);
					th2.SetActive(false);
					th3.SetActive(false);
					th4.SetActive(false);
					th5.SetActive(false);
					break;
				case 2:
					th1.SetActive(true);
					th2.SetActive(true);
					th3.SetActive(false);
					th4.SetActive(false);
					th5.SetActive(false);
					break;
				case 3:
					th1.SetActive(true);
					th2.SetActive(true);
					th3.SetActive(true);
					th4.SetActive(false);
					th5.SetActive(false);
					break;
				case 4:
					th1.SetActive(true);
					th2.SetActive(true);
					th3.SetActive(true);
					th4.SetActive(true);
					th5.SetActive(false);
					break;
				case 5:
					th1.SetActive(true);
					th2.SetActive(true);
					th3.SetActive(true);
					th4.SetActive(true);
					th5.SetActive(true);
					break;
			}

	}

	public void HouseHealth ()
	{	
		if (houseHealth < 5 && Movement.mana >= hhCost) 
		{
			Movement.mana -= hhCost;
			houseHealth++;
			PlayerPrefs.SetInt ("HouseHealthUpgrade", houseHealth);			
		}
	}

	public void WellHealth ()
	{
		if (wellHealth < 5 && Movement.mana >= whCost) 
		{
			Movement.mana -= whCost;
			wellHealth++;
			PlayerPrefs.SetInt ("WellHealthUpgrade", wellHealth);			
		}
	}

	public void StartingMana ()
	{
		if (startingMana < 5 && Movement.mana >= smCost) 
		{
			Movement.mana -= smCost;
			startingMana++;
			PlayerPrefs.SetInt ("StartingManaUpgrade", startingMana);			
		}		
	}

	public void ManaGain ()
	{
		if (manaGain < 5 && Movement.mana >= mgCost) 
		{
            Movement.mana -= mgCost;
			manaGain++;
			PlayerPrefs.SetInt ("ManaGainUpgrade", manaGain);			
		}
	}

	public void TowerHealth ()
	{
		if (towerHealth < 5 && Movement.mana >= thCost) 
		{
            Movement.mana -= thCost;
			towerHealth++;
			PlayerPrefs.SetInt ("TowerHealthUpgrade", towerHealth);			
		}
	}
}
