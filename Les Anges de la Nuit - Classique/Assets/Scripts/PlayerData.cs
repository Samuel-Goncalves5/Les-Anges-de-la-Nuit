[System.Serializable]
public class PlayerData
{
    public string[] general;
    public string[] elea;
    public bool eleaInitialized;
    public string[] emma;
    public bool emmaInitialized;
    public string[] elena;
    public bool elenaInitialized;
    public string[] eva;
    public bool evaInitialized;

    public PlayerData(string roomName, string nickName)
    {
        general    = new string[2];
        general[0] = roomName;
        general[1] = nickName;
        
        elea  = new string[8];
        emma  = new string[8];
        elena = new string[8];
        eva   = new string[8];

        eleaInitialized = false;
        emmaInitialized = false;
        elenaInitialized = false;
        evaInitialized = false;
    }
}