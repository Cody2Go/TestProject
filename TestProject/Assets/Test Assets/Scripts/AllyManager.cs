using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    #region Singleton

    public static AllyManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject[] allies;
}
