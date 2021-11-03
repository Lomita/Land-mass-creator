using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;                         // GameManager instance
  
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {     
    }
} 