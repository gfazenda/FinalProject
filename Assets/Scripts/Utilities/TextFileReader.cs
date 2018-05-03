using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
public class TextFileReader: MonoBehaviour
{



    void Start()
    {
        //WriteString();
        //ReadCurrentLevel();
    }
    public Vector2 ReadCurrentMapSize()
    {
        LoadFile("Level" + GameManager.Instance.currentLevel + ".txt");
        return Vector2.zero;
    }

    public void SaveLevel(int level)
    {

    //    StreamWriter file;
    //      string fileName="CurrentLevel.txt";
    ////  https://forum.unity.com/threads/resolved-cant-write-in-application-persistentdatapath-no-error-sent.492203/
    //     string path = Path.Combine(Application.streamingAssetsPath, fileName);
    //    //path = Path.Combine(path, "\\Level1.txt");
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        path = Application.persistentDataPath + "/" + fileName;
    //       // path = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
    //    }
 
    //        if (!File.Exists(path))
    //        {
    //            file = File.CreateText(path);
    //            Debug.Log("file not here");
    //            file.WriteLine(level.ToString());
    //            file.Close();
    //        }
    //        else
    //        {
    //            Debug.Log("file issss here");
    //        File.WriteAllText(path, level.ToString());
               
    //        }
            PlayerPrefs.SetInt(Prefs.Level, level);
 
        //    Debug.Log(path);
        // catch (Exception e)
        // {
        // }
        // string fileName="CurrentLevel.txt";
        // string path = Path.Combine(Application.streamingAssetsPath, fileName);
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     path = Application.persistentDataPath + @"/PlayerInfo/" + fileName;
        // }
        // TextWriter tw = new StreamWriter(path);
        // tw.Write("2");
        // tw.Close();
    }

    public int ReadCurrentLevel(){
        //int currLevel = -1;
        //string fileName="CurrentLevel.txt";
        //string fileString = null;
        //string path = Path.Combine(Application.streamingAssetsPath, fileName);
        ////path = Path.Combine(path, "\\Level1.txt");
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    path = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        //    Debug.Log(path);
        //    WWW wwwfile = new WWW(path);
        //    while (!wwwfile.isDone) { }
        //  //  t.text = wwwfile.text;
        //    fileString = wwwfile.text;
        //}else
        //{
        //    fileString = File.ReadAllText(path);
        //}

        //// if(fileString != null){
        ////     currLevel = int.Parse(fileString);
        //// }
        //currLevel = System.Convert.ToInt32(fileString);
        //Debug.Log("current level isssssss " + currLevel.ToString());
        //return currLevel;
        return PlayerPrefs.GetInt(Prefs.Level, 1);
    }



    public LevelInformation LoadFile(string file)
    {
        string fileString = null;
        string path = Path.Combine(Application.streamingAssetsPath, "Levels\\");
        path += file;
        //path = Path.Combine(path, "\\Level1.txt");
        if (Application.platform == RuntimePlatform.Android)
        {
            path = "jar:file://" + Application.dataPath + "!/assets/Levels/" + file;
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
          //  t.text = wwwfile.text;
            fileString = wwwfile.text;
        }else
        {
            fileString = File.ReadAllText(path);
            fileString.Replace("\\n", "");
            fileString.Trim();
            
         //   t.text = thing[0] + "" + thing[1];
            
        }
        string[] lines = fileString.Split('\n');

        LevelInformation level = new LevelInformation();
        level.mapSize = new Vector2(float.Parse(lines[0]), float.Parse(lines[1]));
 
        level.objects = new int[(int)level.mapSize.x, (int)level.mapSize.y];
        int index = 2;
        for (int i = 0; i < level.mapSize.y; i++)
        {
            string[] elements = lines[index].Split(' ');
            for (int j = 0; j < level.mapSize.x; j++)
            {       
                level.objects[j,i] = int.Parse(elements[j]);
              //  Debug.Log(level.objects[i, j]);
            }
            index++;
        }


        //File.WriteAllText(path, JsonUtility.ToJson(level));

        //var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "alphabet.t");
        //File.WriteAllBytes(filepath, wwwfile.bytes);
        
      //  Debug.Log(path);
        return level;
        //Debug.Log(thing);
    }

 
    //public string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile");
    //public string result = "";
    //IEnumerator Example()
    //{
    //    if (filePath.Contains("://"))
    //    {
    //        Networking.UnityWebRequest www = Networking.UnityWebRequest.Get(filePath);
    //        yield return www.SendWebRequest();
    //        result = www.downloadHandler.text;
    //    }
    //    else
    //        result = System.IO.File.ReadAllText(filePath);
    //}


}



[System.Serializable]
public class LevelInformation
{
    public Vector2 mapSize;
    public int[,] objects;
}