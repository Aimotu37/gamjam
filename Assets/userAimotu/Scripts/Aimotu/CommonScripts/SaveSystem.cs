using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
   private static string SavePath => Path.Combine(Application.persistentDataPath, "gamesave.json");

   public static void Save(string sceneName, RoomState state)
   {
      GameSaveData data = new GameSaveData
      {
         sceneName = sceneName,
         roomStateIndex = (int)state
         
      };
      string json = JsonUtility.ToJson(data);
      File.WriteAllText(SavePath, json);
      Debug.Log($"游戏已保存至: {sceneName}, 状态: {state}");
   }

   public static GameSaveData Load()
   {
      if (!File.Exists(SavePath)) return null;
      string json = File.ReadAllText(SavePath);
      return JsonUtility.FromJson<GameSaveData>(json);
   }

   public static bool HasSaveFile() => File.Exists(SavePath);
}
