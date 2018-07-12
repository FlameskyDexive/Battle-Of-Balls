using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.UI;


public class BuildAnimation : EditorWindow
{

    //生成出的Prefab的路径
    private static string PrefabPath = "Assets/ExtraModule/UIEffect/Prefabs";
    //生成出的AnimationController的路径
    private static string AnimationControllerPath = "Assets/ExtraModule/UIEffect/Animations";
    //生成出的Animation的路径
    private static string AnimationPath = "Assets/ExtraModule/UIEffect/Animations";
    //美术给的原始图片路径
    //private static string ImagePath = Application.dataPath + "/Raw";

    private string imagePath = "Res/AnimationUI";

    private static BuildAnimation instance;
    [MenuItem("Tools/2D Animation Maker")]
    static void ImageToGo()
    {
        instance = EditorWindow.GetWindow<BuildAnimation>();
        instance.Show();
    }

    private void OnGUI()
    {
        //imageRoot = EditorGUILayout.ObjectField(imageRoot, typeof(Transform));
        //trans = EditorGUILayout.ObjectField(trans, typeof(Transform));
        imagePath = EditorGUILayout.TextField("image folder:", imagePath);
        if (GUILayout.Button("Build Animation And Prefab", GUILayout.Width(180), GUILayout.Height(28)))
        {
            BuildAniamtion(imagePath);
        }
    }

    void BuildAniamtion(string path)
    {
        path = Application.dataPath + "/" + path;
        DirectoryInfo raw = new DirectoryInfo(path);
        foreach (DirectoryInfo dictorys in raw.GetDirectories())
        {
            List<AnimationClip> clips = new List<AnimationClip>();
            foreach (DirectoryInfo dictoryAnimations in dictorys.GetDirectories())
            {
                //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
                clips.Add(BuildAnimationClip(dictoryAnimations));
            }
            //把所有的动画文件生成在一个AnimationController里
            UnityEditor.Animations.AnimatorController controller = BuildAnimationController(clips, dictorys.Name);
            //最后生成程序用的Prefab文件
            BuildPrefab(dictorys, controller);
        }
    }


    static AnimationClip BuildAnimationClip(DirectoryInfo dictorys)
    {
        string animationName = dictorys.Name;
        //查找所有图片，因为我找的测试动画是.jpg 
        FileInfo[] images = dictorys.GetFiles("*.png");
        AnimationClip clip = new AnimationClip();
        //AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);
        
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        //curveBinding.type = typeof(SpriteRenderer);
        curveBinding.type = typeof(Image);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[images.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float frameTime = 1 / 10f;
        for (int i = 0; i < images.Length; i++)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i].FullName));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        //动画帧率，30比较合适
        clip.frameRate = 30;

        //有些动画我希望天生它就动画循环
        if (animationName.IndexOf("idle") >= 0)
        {
            //设置idle文件为循环动画
            SerializedObject serializedClip = new SerializedObject(clip);
            AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
            clipSettings.loopTime = true;
            serializedClip.ApplyModifiedProperties();
        }
        string parentName = System.IO.Directory.GetParent(dictorys.FullName).Name;
        System.IO.Directory.CreateDirectory(AnimationPath + "/" + parentName);
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, AnimationPath + "/" + parentName + "/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        return clip;
    }

    static AnimatorController BuildAnimationController(List<AnimationClip> clips, string name)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(AnimationControllerPath + "/" + name + ".controller");
        var layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        foreach (AnimationClip newClip in clips)
        {
            UnityEditor.Animations.AnimatorState state = sm.AddState(newClip.name);
            //state.SetAnimationClip(newClip, layer);
            state.motion = newClip;
            sm.AddAnyStateTransition(state);
            //UnityEditor.Animations.AnimatorTransition trans = sm.AddAnyStateTransition(state);
            //trans.RemoveCondition(0);
        }
        AssetDatabase.SaveAssets();
        return animatorController;
    }

    static void BuildPrefab(DirectoryInfo dictorys, UnityEditor.Animations.AnimatorController animatorCountorller)
    {
        //生成Prefab 添加一张预览用的Sprite
        FileInfo images = dictorys.GetDirectories()[0].GetFiles("*.png")[0];
        GameObject go = new GameObject();
        go.name = dictorys.Name;
        //SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
        Image img = go.AddComponent<Image>();
        img.raycastTarget = false;
        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));
        img.SetNativeSize();

        //spriteRender.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));
        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorCountorller;
        PrefabUtility.CreatePrefab(PrefabPath + "/" + go.name + ".prefab", go);
        DestroyImmediate(go);
    }


    public static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets\\"));
        else
            return path.Substring(path.IndexOf("Assets/"));
    }


    class AnimationClipSettings
    {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }

}