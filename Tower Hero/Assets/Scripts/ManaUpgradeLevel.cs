using UnityEngine;
using System.Collections;

public class ManaUpgradeLevel : MonoBehaviour 
{

	void Start()
	{
		Movement.maxMana = PlayerPrefs.GetInt("StartingManaUpgrade")*25 +200;
		Movement.mana = Movement.maxMana;

		foreach (var BuildScript in Movement.buildings) {
			if(BuildScript.tag == "Houses"){
				BuildScript.HP += 20*PlayerPrefs.GetInt ("HouseHealthUpgrade");
				BuildScript.currhp = BuildScript.HP;
			}
			if(BuildScript.tag == "Mana"){
				BuildScript.HP += 20*PlayerPrefs.GetInt ("WellHealthUpgrade");
				BuildScript.currhp = BuildScript.HP;
			}
			if(BuildScript.tag == "Wagon")
				BuildScript.GetComponent<WagonScript>().manaGain += 20*PlayerPrefs.GetInt ("ManaGainUpgrade");

			if(BuildScript.tag == "Gate"){
				BuildScript.HP += 20*PlayerPrefs.GetInt ("TowerHealthUpgrade");
				BuildScript.currhp = BuildScript.HP;
			}
		}
	}

	/*public void Upgrade ()
	{
		
		// Get upgrade levels and upgrade things
		/*if (PlayerPrefs.HasKey ("HouseHealthUpgrade")) 
		{
						// Upgrade house health
			/*switch (houseHealth) 
			{
				case 0:
					break;
				case 1:
					foreach(BuildingScript house in houses)
					{
						house.HP = 55;
					}
					break;
				case 2:
					foreach(BuildingScript house in houses)
					{
						house.HP = 60;
					}
					break;
				case 3:
					foreach(BuildingScript house in houses)
					{
						house.HP = 65;
					}
					break;
				case 4:
					foreach(BuildingScript house in houses)
					{
						house.HP = 70;
					}
					break;
				case 5:
					foreach(BuildingScript house in houses)
					{
						house.HP = 75;
					}
					break;
			}
		}

		if(PlayerPrefs.HasKey("WellHealthUpgrade"))
		{
			// Upgrade well health
			switch (wellHealth) 
			{
				case 0:
					break;
				case 1:
					foreach(BuildingScript well in wells)
					{
						well.HP = 35;
					}
					break;
				case 2:
					foreach(BuildingScript well in wells)
					{
						well.HP = 40;
					}
					break;
				case 3:
					foreach(BuildingScript well in wells)
					{
						well.HP = 45;
					}
					break;
				case 4:
					foreach(BuildingScript well in wells)
					{
						well.HP = 50;
					}
					break;
				case 5:
					foreach(BuildingScript well in wells)
					{
						well.HP = 55;
					}
					break;
			}
		}

		if(PlayerPrefs.HasKey("StartingManaUpgrade"))
		{
			// Upgrade starting mana
			switch (startingMana) 
			{
				case 0:
					break;
				case 1:
					Movement.mana = 1100;
					break;
				case 2:
					Movement.mana = 1200;
					break;
				case 3:
					Movement.mana = 1300;
					break;
				case 4:
					Movement.mana = 1400;
					break;
				case 5:
					Movement.mana = 1500;
					break;
			}
		}

		if(PlayerPrefs.HasKey("ManaGainUpgrade"))
		{
			// Upgrade mana gain
			switch (manaGain) 
			{
				case 0:
					break;
				case 1:
					foreach(WagonScript wagon in wagons)
					{
						wagon.manaGain = 60;
					}
					break;
				case 2:
					foreach(WagonScript wagon in wagons)
					{
						wagon.manaGain = 70;
					}
					break;
				case 3:
					foreach(WagonScript wagon in wagons)
					{
						wagon.manaGain = 80;
					}
					break;
				case 4:
					foreach(WagonScript wagon in wagons)
					{
						wagon.manaGain = 90;
					}
					break;
				case 5:
					foreach(WagonScript wagon in wagons)
					{
						wagon.manaGain = 100;
					}
					break;
			}
		}

		if(PlayerPrefs.HasKey("TowerHealthUpgrade"))
		{
			// Upgrade tower health
			switch (towerHealth) 
			{
				case 0:
					break;
				case 1:
					tower.HP = 125;
					break;
				case 2:
					tower.HP = 150;
					break;
				case 3:
					tower.HP = 175;
					break;
				case 4:
					tower.HP = 200;
					break;
				case 5:
					tower.HP = 225;
					break;
			}
		}
	}*/
}
