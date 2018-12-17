using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Memory
{
  public class Loader : MonoBehaviour
  {
    void Awake()
    {
      string gmPath = "Assets/Prefabs/GameManager.prefab";
      UnityEngine.Object gameManager = AssetDatabase.LoadAssetAtPath(gmPath, typeof(GameObject));

      if (GameManager.Instance == null)
        Instantiate(gameManager);
    }
  }
}
