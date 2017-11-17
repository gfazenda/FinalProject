using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextFileReader: MonoBehaviour
{



    public Text t;
    


 ////   [MenuItem("Tools/Write file")]
 //   static void WriteString()
 //   {
 //       string path = "Assets/Resources/testfile.txt";

 //       //Write some text to the test.txt file
 //       StreamWriter writer = new StreamWriter(path, true);
 //       writer.WriteLine("Test");
 //       writer.Close();

 //       //Re-import the file to update the reference in the editor
 //      // AssetDatabase.ImportAsset(path);
 //       TextAsset asset = (TextAsset)Resources.Load("test");

 //       //Print the text from the file
 //       Debug.Log(asset.text);
 //   }

 // //  [MenuItem("Tools/Read file")]
 //   static void ReadString()
 //   {
 //       string path = "Assets/Resources/test.txt";

 //       //Read the text from directly from the test.txt file
 //       StreamReader reader = new StreamReader(path);
 //       Debug.Log(reader.ReadToEnd());
 //       reader.Close();
 //   }

    public Vector2 ReadCurrentMapSize()
    {
        LoadFile("Level" + GameManager.Instance.currentLevel + ".txt");
        return Vector2.zero;
    }

    public LevelInformation LoadFile(string file)
    {
        string thing = null;
        string path = Path.Combine(Application.streamingAssetsPath, "Levels\\");
        path += file;
        //path = Path.Combine(path, "\\Level1.txt");
        if (Application.platform == RuntimePlatform.Android)
        {
            path = "jar:file://" + Application.dataPath + "!/assets/Levels/" + file;
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
          //  t.text = wwwfile.text;
            thing = wwwfile.text;
        }else
        {
            thing = File.ReadAllText(path);
            thing.Replace("\\n", "");
            thing.Trim();
            
         //   t.text = thing[0] + "" + thing[1];
            
        }
        string[] lines = thing.Split('\n');

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
        
        Debug.Log(path);
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