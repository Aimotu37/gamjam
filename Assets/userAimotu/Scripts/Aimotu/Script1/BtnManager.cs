using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
   public static BtnManager Instance;

   public float floatSpeed = 2f;
   public float floatRange = 2.5f;
   
   private List<RectTransform> floatButtons = new List<RectTransform>();
   private List<Vector3> originalPositions = new List<Vector3>();


   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Update()
   {
      for (int i = 0; i < floatButtons.Count; i++)
      {
         if(floatButtons[i] == null) continue;
         float y = originalPositions[i].y + Mathf.Sin(Time.time * floatSpeed) * floatRange;
         floatButtons[i].localPosition = new Vector3(originalPositions[i].x, y, originalPositions[i].z);
      }
   }

   public void AddFloatButton(RectTransform btn)
   {
      if(btn == null || floatButtons.Contains(btn)) return;
      
      floatButtons.Add(btn);
      originalPositions.Add(btn.localPosition);
   }
}
