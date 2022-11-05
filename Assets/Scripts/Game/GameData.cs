using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


// Branched off of https://github.com/cmilr/Unity2D-Components/blob/master/Game/GameData.cs
public class GameData
{
	public int CurrentScore		{ get; set; }
	public int Lives			{ get; set; }
	public int CurrentLevel		{ get; set; }

	void Awake()
	{
		CurrentScore	= 0;
		Lives			= 3;
		CurrentLevel	= 1;
	}

	public void Save()
	{
		var bf			= new BinaryFormatter();
		FileStream file	= File.Create(Application.persistentDataPath + "/GameData.dat");
		var container	= new GameDataContainer();

		container.currentScore		= CurrentScore;
		container.lives				= Lives;
		container.currentLevel		= CurrentLevel;

		bf.Serialize(file, container);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
		{
			var bf			= new BinaryFormatter();
			FileStream file	= File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
			var container	= (GameDataContainer)bf.Deserialize(file);
			file.Close();

			CurrentScore	= container.currentScore;
			Lives			= container.lives;
			CurrentLevel	= container.currentLevel;
		}
	}
}




[Serializable]
class GameDataContainer
{
	public int currentScore;
	public int lives;
	public int currentLevel;
}

