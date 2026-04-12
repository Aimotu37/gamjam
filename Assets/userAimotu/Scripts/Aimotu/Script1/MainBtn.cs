using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBtn : MonoBehaviour
{
   private void Start()
   {
      if (BtnManager.Instance != null)
      {
         BtnManager.Instance.AddFloatButton(GetComponent<RectTransform>());
      }
   }
}
