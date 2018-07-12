using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SetTexture : EditorWindow
{
    enum MaxSize
    {
        Automatic = 9,
        Size_32 = 32,
        Size_64 = 64,
        Size_128 = 128,
        Size_256 = 256,
        Size_512 = 512,
        Size_1024 = 1024,
        Size_2048 = 2048,
        Size_4096 = 4096,
        Size_8192 = 8192,

    }

    enum Platform
    {
        Android,
        iPhone,
        Standalone
    }

    // ----------------------------------------------------------------------------  
    TextureImporterType textureType = TextureImporterType.Sprite;
    TextureImporterFormat textureFormat = TextureImporterFormat.Automatic;
    MaxSize textureSize = MaxSize.Automatic;
    TextureImporterCompression textureCompression = TextureImporterCompression.Uncompressed;
    //private BuildTarget target = BuildTarget.Android;

    bool ifAllowsAlphaSplitting = true;
    bool ifMipmapEnabled = false;
    bool over_ride = false;

    float secs = 10.0f;
    double startVal = 0;
    float progress = 0f;
    private string texturePath = "Assets/Res/UI/";
    private Platform platform = Platform.iPhone;
    private int num = 0;

    static SetTexture window;
    [@MenuItem("Tools/Texture Settings")]
    private static void Init()
    {
        Rect wr = new Rect(0, 0, 200, 200);
        window = (SetTexture)EditorWindow.GetWindowWithRect(typeof(SetTexture), wr, false, "图片格式设置");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("设置UI文件夹路径，Assets/Res/UI/ 为默认值", MessageType.Info);
        this.texturePath = EditorGUILayout.TextField("resources folder:", this.texturePath);
        EditorGUILayout.Space();

        this.platform = (Platform)EditorGUILayout.EnumPopup("平台:", this.platform);
        textureType = (TextureImporterType)EditorGUILayout.EnumPopup("类型:", textureType);
        textureFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("格式:", textureFormat);
        textureSize = (MaxSize)EditorGUILayout.EnumPopup("尺寸:", textureSize);
        textureCompression = (TextureImporterCompression)EditorGUILayout.EnumPopup("压缩:", textureCompression);
        ifAllowsAlphaSplitting = EditorGUILayout.Toggle("是否允许透明分离:", ifAllowsAlphaSplitting);
        ifMipmapEnabled = EditorGUILayout.Toggle("是否允许Mipmap:", ifMipmapEnabled);
        over_ride = EditorGUILayout.Toggle("是否允许Override:", this.over_ride);

        EditorGUILayout.Space();

        if (GUILayout.Button("设置"))
        {
            TextureImporterPlatformSettings t = new TextureImporterPlatformSettings();

            t.allowsAlphaSplitting = ifAllowsAlphaSplitting;
            t.overridden = over_ride;
            t.format = textureFormat;
            t.maxTextureSize = (int)textureSize;
            t.textureCompression = textureCompression;
            
            this.ChangeTextureFormatSettings(t);
        }

    }

    private int count = 0;
    private int index = 0;

    /// <summary>
    /// Find all pictures in this root folder, now support 4 levels folder
    /// </summary>
    /// <param name="_t"></param>
    private void ChangeTextureFormatSettings(TextureImporterPlatformSettings _t)
    {
        Debug.Log($"--Level 1--{this.platform.ToString()}");
        DirectoryInfo direction = new DirectoryInfo(this.texturePath);
        FileInfo[] files = direction.GetFiles("*.png", SearchOption.AllDirectories);
        this.count = files.Length;
        SetOneFolderTexture(this.texturePath, _t);
        string[] folders = Directory.GetFileSystemEntries(this.texturePath);
        for (int i = 0; i < folders.Length; i++)
        {
            if (folders[i].EndsWith(".meta"))
            {
                continue;
            }
            if (folders[i].EndsWith(".png"))
            {
                continue;
            }
            if (folders[i].EndsWith(".jpg"))
            {
                continue;
            }
            folders[i] = folders[i].Replace(@"\", "/");
            Debug.Log($"--Level 2--{folders[i]}");
            SetOneFolderTexture(folders[i], _t);

            string[] folders2 = Directory.GetFileSystemEntries(folders[i]);
            for (int j = 0; j < folders2.Length; j++)
            {
                if (folders2[j].EndsWith(".meta"))
                {
                    continue;
                }
                if (folders2[j].EndsWith(".png"))
                {
                    continue;
                }
                if (folders2[j].EndsWith(".jpg"))
                {
                    continue;
                }
                folders2[j] = folders2[j].Replace(@"\", "/");
                Debug.Log($"--Level 3--{folders2[j]}");
                SetOneFolderTexture(folders2[j], _t);

                string[] folders3 = Directory.GetFileSystemEntries(folders2[j]);
                for (int k = 0; k < folders3.Length; k++)
                {
                    if (folders3[k].EndsWith(".meta"))
                    {
                        continue;
                    }
                    if (folders3[k].EndsWith(".png"))
                    {
                        continue;
                    }
                    if (folders3[k].EndsWith(".jpg"))
                    {
                        continue;
                    }
                    folders3[k] = folders3[k].Replace(@"\", "/");
                    Debug.Log($"--Level 4--{folders3[k]}");
                    SetOneFolderTexture(folders3[k], _t);
                }
            }
        }
        if (Directory.Exists(this.texturePath))
        {
            /*DirectoryInfo direction = new DirectoryInfo(this.texturePath);
            FileInfo[] files = direction.GetFiles("*.png", SearchOption.AllDirectories);

            Debug.Log(files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                Debug.Log("Name:" + files[i].Name);
                Debug.Log("Path:" + texturePath + files[i].Name);
                TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath + files[i].Name) as TextureImporter;
                textureImporter.textureType = textureType;
                textureImporter.SetPlatformTextureSettings(this.target.ToString(), _t.maxTextureSize, _t.format, _t.allowsAlphaSplitting);

                ShowProgress((float)i / (float)files.Length, files.Length, i);
                //i++;
                AssetDatabase.ImportAsset(texturePath + files[i].Name);
            }*/
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        this.count = 0;
        this.index = 0;
        //textures = null;
    }

    private void SetOneFolderTexture(string folder, TextureImporterPlatformSettings _t)
    {
        string[] files = Directory.GetFileSystemEntries(folder);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".png"))
            {
                files[i] = files[i].Replace(@"\", "/");
                /*files[i] = files[i].Replace(@".png", "");*/
                //Debug.Log($"---{files[i]}");
                index++;
                TextureImporter textureImporter = AssetImporter.GetAtPath(files[i]) as TextureImporter;
                textureImporter.textureType = textureType;
                textureImporter.spritePackingTag = files[i].Split('/')[3];
                textureImporter.maxTextureSize = this.GetMaxSize(AssetDatabase.LoadAssetAtPath<Texture2D>(files[i])); 
                textureImporter.SetPlatformTextureSettings(this.platform.ToString(), textureImporter.maxTextureSize, _t.format, _t.allowsAlphaSplitting);
                ShowProgress((float)this.index / (float)count, this.count, index, files[i]);
                AssetDatabase.ImportAsset(files[i]);
            }

        }
    }

    private int GetMaxSize(Texture2D texture)
    {
        float width = (float)texture.width;
        float height = (float)texture.height;
        if (width >= height)
        {
            
        }
        if ((double)width > (double)height)
        {
            this.num = 0;
            while ((double)width / 2.0 >= 1.0)
            {
                width /= 2f;
                ++this.num;
            }
        }
        else
        {
            this.num = 0;
            while ((double)height / 2.0 >= 1.0)
            {
                height /= 2f;
                ++this.num;
            }
        }
        return (int)Mathf.Pow(2, this.num + 1);
        /*if (texture.width >= texture.height)
        {
            return GetMaxSize(texture.width);
        }
        else
        {
            return GetMaxSize(texture.height);
        }*/
    }

    private int GetMaxSize(int size)
    {
        if (size <= 32)
        {
            size = 32;
        }
        else if(size <= 64)
        {
            size = 64;
        }
        else if(size <= 128)
        {
            size = 128;
        }
        else if(size <= 256)
        {
            size = 256;
        }
        else if(size <= 512)
        {
            size = 512;
        }
        else if(size <= 1024)
        {
            size = 1024;
        }
        else
        {
            size = 2048;
        }
        return size;
    }

    public void ShowProgress(float val, int total, int cur, string fileName)
    {
        EditorUtility.DisplayProgressBar($"设置图片中  {fileName}  ...", string.Format("请稍等({0}/{1}) ", cur, total), val);
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}