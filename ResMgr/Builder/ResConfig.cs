using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ResConfig", menuName = "ResMgr/Create", order = 1)]
public class ResConfig : ScriptableObject
{
    [SerializeField]
    public List<ResPackage> AssetsList;
    [SerializeField]
    public List<ResPackage> PrefabList;
}

[System.Serializable]
public class ResPackage
{
    [SerializeField]
    public string Name = string.Empty;
    [SerializeField]
    public string Path = string.Empty;
}