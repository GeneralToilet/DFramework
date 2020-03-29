using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(ResConfig))]
public class ResBuilder : Editor
{
    //资源配置表
    private ResConfig config;


    private ReorderableList assetsList;
    private ReorderableList prefabList;

    private Dictionary<string, string> prefabDic;
    private Dictionary<string, string> assetsDic;

    private void Awake()
    {
        //初始化配置表
        config = this.target as ResConfig;

        if (config.AssetsList == null) config.AssetsList = new List<ResPackage>();
        if (config.PrefabList == null) config.PrefabList = new List<ResPackage>();
    }

    public void OnEnable()
    {
        assetsList = new ReorderableList(serializedObject, serializedObject.FindProperty("AssetsList"));
        prefabList = new ReorderableList(serializedObject, serializedObject.FindProperty("PrefabList"));

        assetsList.drawHeaderCallback = (Rect rect) =>
         {
             EditorGUI.LabelField(rect, "AssetsPackage");
         };

        assetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (index == 0)
                assetsDic = new Dictionary<string, string>();

            float x = rect.x;
            float offset = (rect.width - 100) * .5f;
            rect.width = 60;
            rect.x += 20;
            EditorGUI.LabelField(rect, "ABName : ");

            rect.x += rect.width;
            rect.width = offset;
            SerializedProperty name = assetsList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Name");
            EditorGUI.LabelField(rect, name.stringValue);

            rect.x += rect.width;
            rect.width = 40;
            EditorGUI.LabelField(rect, "Path");

            rect.x += rect.width;
            rect.width = offset;
            SerializedProperty path = assetsList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Path");
            DefaultAsset asset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(path.stringValue);
            asset = EditorGUI.ObjectField(rect, asset, typeof(DefaultAsset)) as DefaultAsset;
            path.stringValue = AssetDatabase.GetAssetPath(asset);

            if (asset != null)
            {
                name.stringValue = path.stringValue.Replace('/', '_');
                name.stringValue = name.stringValue.Split('.')[0].ToLower();
            }



            rect.x = x;
            rect.width = 20;

            if (path.stringValue == string.Empty)
            {
                Debug.LogError(string.Format("ResBuilder :AssetsPackage 路径不能为空！"));
                EditorGUI.HelpBox(rect, "", MessageType.Error);
                return;
            }

            foreach (string key in assetsDic.Keys)
            {

                if (key == string.Empty)
                    continue;

                if (path.stringValue.Contains(key) || key.Contains(path.stringValue)) 
                {
                    Debug.LogError(string.Format("ResBuilder : 存在相互包含的两个路径，请检查ab包 {0} 和 {1}", assetsDic[key], name.stringValue));
                    EditorGUI.HelpBox(rect, "", MessageType.Error);
                    return;
                }
            }

            if (path.stringValue != string.Empty)
                assetsDic.Add(path.stringValue, name.stringValue);

        };


        prefabList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "PrefabPackage");
        };
        prefabList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (index == 0)
                prefabDic = new Dictionary<string, string>();

            float x = rect.x;
            float offset = (rect.width - 100) * .5f;
            rect.width = 60;
            rect.x += 20;
            EditorGUI.LabelField(rect, "ABName : ");

            rect.x += rect.width;
            rect.width = offset;
            SerializedProperty name = prefabList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Name");
            EditorGUI.LabelField(rect, name.stringValue);

            rect.x += rect.width;
            rect.width = 40;
            EditorGUI.LabelField(rect, "Path");

            rect.x += rect.width;
            rect.width = offset - 20;
            SerializedProperty path = prefabList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Path");
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path.stringValue);
            asset = EditorGUI.ObjectField(rect, asset, typeof(GameObject)) as GameObject;
            path.stringValue = AssetDatabase.GetAssetPath(asset);

            if (asset != null)
            {
                name.stringValue = path.stringValue.Replace('/', '_');
                name.stringValue = name.stringValue.Split('.')[0].ToLower();
            }


            rect.x = x;
            rect.width = 20;

            if (path.stringValue == string.Empty)
            {
                Debug.LogError(string.Format("ResBuilder :PrefabPackage 路径不能为空！"));
                EditorGUI.HelpBox(rect, "", MessageType.Error);
                return;
            }

            if (prefabDic.ContainsKey(path.stringValue))
            {
                EditorGUI.HelpBox(rect, "", MessageType.Error);
                Debug.LogError("ResBuilder : 存在相同路径的资源，请检查ab包 : " + name.stringValue);
            }

            foreach (string key in assetsDic.Keys)
            {

                if (key == string.Empty)
                    continue;

                if (path.stringValue.Contains(key) || key.Contains(path.stringValue))
                {
                    Debug.LogError(string.Format("ResBuilder : 资源包中存在此预设，请检查资源包 {0}", key.ToLower().Replace('/', '_')));
                    EditorGUI.HelpBox(rect, "", MessageType.Error);
                    return;
                }
            }

            EditorGUI.HelpBox(rect, "", MessageType.None);
            prefabDic.Add(path.stringValue, name.stringValue);
        };
    }


    public override void OnInspectorGUI()
    {


        serializedObject.Update();
        assetsList.DoLayoutList();
        prefabList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();


        if(GUILayout.Button("Build"))
        {
            Build();
        }
    }

    private void Build()
    {
            
    }


}


[CustomPropertyDrawer(typeof(ResDrawer))]
public class ResDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }
}