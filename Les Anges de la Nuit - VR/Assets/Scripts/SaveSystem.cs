using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class SaveSystem
{
	public static void SavePlayer()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/data.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		List<PlayerController> players = new List<PlayerController>();
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
			players.Add(gameObject.GetComponent<PlayerController>());

		PlayerData data = new PlayerData(players);

		formatter.Serialize(stream, data);
		
		stream.Close();
	}

	public static PlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + "/data.save";
		
		if (!File.Exists(path)) return null;
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		if (stream.Length == 0) return null;
		
		PlayerData data = formatter.Deserialize(stream) as PlayerData;
		stream.Close();

		data.ReloadDico();
		return data;
	}
}