using System;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using Photon.Realtime;

[Serializable]
public static class SaveSystem
{
	public static PlayerData Sauvegarde;

	// PARTIE SAUVEGARDE -
	
	public static void SaveGame()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/data.save";
		FileStream stream = new FileStream(path, FileMode.Create);
		var v = ActualData();
		if (v is null) return;
		formatter.Serialize(stream, v);
		
		stream.Close();
	}

	private static PlayerData ActualData()
	{
		if (Sauvegarde is null)
			Sauvegarde = new PlayerData(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.LocalPlayer.NickName);
		
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			GameObject perso = (GameObject) player.CustomProperties["Personnage"];

			if (perso is null) return null;
			
			Vector3 p = perso.transform.position;
			float[] position = new float[3];
			position[0] = p.x; position[1] = p.y; position[2] = p.z;
			
			Quaternion r = perso.transform.rotation;
			float[] rotation = new float[4];
			rotation[0] = r.x; rotation[1] = r.y;
			rotation[2] = r.z; rotation[3] = r.w;

			Save(Sauvegarde, (string) player.CustomProperties["Character"], position, rotation);
		}
		
		return Sauvegarde;
	}

	public static string[] GetInfos(PlayerData data, string name)
	{
		switch (name)
		{
			case "Elea"  : return data.elea;
			case "Emma"  : return data.emma;
			case "Elena" : return data.elena;
			case "Eva"   : return data.eva;
		}
        
		throw new ArgumentException();
	}
	
	private static void Initialize(PlayerData data, string name)
	{
		switch (name)
		{
			case "Elea"  : data.eleaInitialized = true; return;
			case "Emma"  : data.emmaInitialized = true; return;
			case "Elena" : data.elenaInitialized = true; return;
			case "Eva"   : data.evaInitialized = true; return;
		}
		throw new ArgumentException();
	}

	public static bool IsInitialized(PlayerData data, string name)
	{
		switch (name)
		{
			case "Elea"  : return data.eleaInitialized;
			case "Emma"  : return data.emmaInitialized;
			case "Elena" : return data.elenaInitialized;
			case "Eva"   : return data.evaInitialized;
		}
		throw new ArgumentException();
	}
	
	private static void Save(PlayerData data, string name, float[] position, float[] rotation)
	{
		Save(GetInfos(data, name), position, rotation);
		Initialize(data, name);
	}

	private static void Save(string[] name, float[] position, float[] rotation)
	{
		name[0] = position[0].ToString(CultureInfo.InvariantCulture);
		name[1] = position[1].ToString(CultureInfo.InvariantCulture);
		name[2] = position[2].ToString(CultureInfo.InvariantCulture);
        
		name[3] = rotation[0].ToString(CultureInfo.InvariantCulture);
		name[4] = rotation[1].ToString(CultureInfo.InvariantCulture);
		name[5] = rotation[2].ToString(CultureInfo.InvariantCulture);
		name[6] = rotation[3].ToString(CultureInfo.InvariantCulture);
	}
	
	// -----------------------------------------------
	
	// PARTIE LECTURE -
	
	public static PlayerData LoadPlayer(bool firstTime = false)
	{
		string path = Application.persistentDataPath + "/data.save";
		
		if (!File.Exists(path)) return null;
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		if (stream.Length == 0) return null;

		if (firstTime)
		{
			PlayerData temp = formatter.Deserialize(stream) as PlayerData;
			stream.Close();
			return temp;
		}
		
		Sauvegarde = formatter.Deserialize(stream) as PlayerData;
		stream.Close();
		return Sauvegarde;
	}
	
	// -----------------------------------------------
}