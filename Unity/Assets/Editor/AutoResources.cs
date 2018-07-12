#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class AutoResources : EditorWindow
{
    private Object imageRoot;
    private string resourcePath = "Assets/Res/UI/PokerCard/";
    private Object imageModel;
    private Transform trans;

    private List<Image> listImages;
    //private static Sprite[] images;

    private static AutoResources instance;

    [MenuItem("Tools/AutoResources")]
    static void ImageToGo()
    {
        instance = EditorWindow.GetWindow<AutoResources>();
        instance.Show();
    }

    private void OnGUI()
    {
        //imageRoot = EditorGUILayout.ObjectField(imageRoot, typeof(Transform));
        //trans = EditorGUILayout.ObjectField(trans, typeof(Transform));
        //imageModel = EditorGUILayout.ObjectField(imageModel, typeof(Object));
        this.resourcePath = EditorGUILayout.TextField("resources folder:", this.resourcePath);
        if (Selection.GetTransforms(SelectionMode.TopLevel).Length > 0)
        {
            trans = Selection.GetTransforms(SelectionMode.TopLevel)[0];
        }
        

        if (GUILayout.Button("Clone Image GameObject", GUILayout.Width(160), GUILayout.Height(28)))
        {
            CreateImageGameObject();
        }
        if (GUILayout.Button("Clone Audio GameObject", GUILayout.Width(160), GUILayout.Height(28)))
        {
            CreateAudioGameObject();
        }
    }

    void CreateImageGameObject()
    {
        if (trans != null)
        {
            if (this.resourcePath != String.Empty || this.resourcePath != "")
            {
                //Assets/Res/UI/PokerCard/
                //Sprite[] images = Resources.LoadAll<Sprite>(resourcePath);
                //Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(resourcePath) as Sprite[];
                /*Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(resourcePath) as Sprite[];
                Texture2D[] textures = AssetDatabase.LoadAllAssetsAtPath(resourcePath) as Texture2D[];
                Debug.Log(AssetDatabase.LoadAllAssetsAtPath(resourcePath).Length);
                Texture2D tex = AssetDatabase.LoadAssetAtPath(resourcePath, typeof(Texture2D)) as Texture2D;
                Sprite spi = AssetDatabase.LoadAssetAtPath(resourcePath, typeof(Sprite)) as Sprite;*/
                //Debug.Log(spi.name);
                //Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/AssetBundleResouces/Effects/textures/WaterMargin/PE1/") as Sprite[];
                //for (int i = 0; i < sprites.Length; i++)
                //{
                //    GameObject go = GameObject.Instantiate(imageModel) as GameObject;
                //    go.name = "mImg_" + sprites[i].name;
                //    Image img = go.GetComponent<Image>();
                //    img.sprite = sprites[i];
                //    img.SetNativeSize();
                //    img.transform.SetParent(trans);
                //    img.transform.localScale = Vector3.one;
                //}
                //获取指定路径下面的所有资源文件  
                if (Directory.Exists(this.resourcePath))
                {
                    DirectoryInfo direction = new DirectoryInfo(this.resourcePath);
                    FileInfo[] files = direction.GetFiles("*.png", SearchOption.AllDirectories);

                    Debug.Log(files.Length);

                    for (int i = 0; i < files.Length; i++)
                    {
                        /*if (files[i].Name.EndsWith(".meta"))
                        {
                            continue;
                        }
                        */
                        Debug.Log( "Name:" + files[i].Name );
                        //Debug.Log( "FullName:" + files[i].FullName );  
                        //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  

                        Sprite spite = AssetDatabase.LoadAssetAtPath(this.resourcePath + files[i].Name, typeof(Sprite)) as Sprite;
                        //GameObject go = GameObject.Instantiate(imageModel) as GameObject;
                        GameObject go = new GameObject();

                        go.name = "" + spite.name;
                        Image img = go.AddComponent<Image>();
                        img.sprite = spite;
                        img.SetNativeSize();
                        img.raycastTarget = false;
                        img.transform.SetParent(trans);
                        img.transform.localScale = Vector3.one;
                        img.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
    }
    void CreateAudioGameObject()
    {
        if (trans != null)
        {
            if (this.resourcePath != String.Empty || this.resourcePath != "")
            {
                
                //获取指定路径下面的所有资源文件  
                if (Directory.Exists(this.resourcePath))
                {
                    DirectoryInfo direction = new DirectoryInfo(this.resourcePath);
                    FileInfo[] files = direction.GetFiles("*.mp3", SearchOption.AllDirectories);

                    Debug.Log(files.Length);

                    for (int i = 0; i < files.Length; i++)
                    {
                        /*if (files[i].Name.EndsWith(".meta"))
                        {
                            continue;
                        }
                        */
                        Debug.Log( "Name:" + files[i].Name );
                        //Debug.Log( "FullName:" + files[i].FullName );  
                        //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  

                        AudioClip clip = AssetDatabase.LoadAssetAtPath(this.resourcePath + files[i].Name, typeof(AudioClip)) as AudioClip;
                        //GameObject go = GameObject.Instantiate(imageModel) as GameObject;
                        GameObject go = new GameObject();
                        go.transform.SetParent(this.trans);
                        go.name = "" + clip.name;
                        AudioSource sound = go.AddComponent<AudioSource>();
                        sound.clip = clip;
                        sound.loop = false;
                        sound.playOnAwake = false;
                    }
                }
            }
        }
    }
}
#endif