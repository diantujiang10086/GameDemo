using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class AtlasAnimationDataEditor : EditorWindow
{
    [MenuItem("Tools/AtlasAnimationData")]
    static void Run()
    {
        GetWindow<AtlasAnimationDataEditor>().Show();
    }

    private bool isImport = true;
    private Vector2 scrollPos;
    private Texture2D atlasTexture;
    private AtlasAnimationData selectedAsset;
    private List<Animation2D> animations = new List<Animation2D>();

    private void OnEnable()
    {
        isImport = true;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        var _selectedAsset = EditorGUILayout.ObjectField("AtlasAnimationData Asset", selectedAsset, typeof(AtlasAnimationData), false) as AtlasAnimationData;
        if (EditorGUI.EndChangeCheck())
        {
            if (_selectedAsset != null && _selectedAsset != selectedAsset)
            {
                selectedAsset = _selectedAsset;
                isImport = true;
            }
        }

        if (isImport)
        {
            ImportFromAsset(selectedAsset);
            isImport = false;
        }

        atlasTexture = EditorGUILayout.ObjectField("atlas Texture",atlasTexture, typeof(Texture2D),true) as Texture2D;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < animations.Count; i++)
        {
            var animation = animations[i];
            EditorGUILayout.BeginVertical("box");

            animation.isExpanded = EditorGUILayout.Foldout(animation.isExpanded, $"Animation {i + 1}", true);

            if (animation.isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                animation.isLoop = EditorGUILayout.Toggle("Loop", animation.isLoop);
                animation.frameRate = EditorGUILayout.IntField("Frame Rate", animation.frameRate);
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    animations.RemoveAt(i);
                    i--;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    continue;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Sprites", EditorStyles.boldLabel);

                for (int j = 0; j < animation.sprites.Count; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    animation.sprites[j] = EditorGUILayout.ObjectField(animation.sprites[j], typeof(Sprite), false) as Sprite;
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        animation.sprites.RemoveAt(j);
                        j--;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Sprite"))
                {
                    animation.sprites.Add(null);
                }

                SpritesDropArea(animation);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Animation"))
        {
            animations.Add(new Animation2D() { isLoop = true, frameRate = 12, sprites = new List<Sprite>() });
        }

        if (GUILayout.Button("Save"))
        {
            SaveAtlasAnimationData();
        }
        EditorGUILayout.EndVertical();
    }

    private void ImportFromAsset(AtlasAnimationData asset)
    {
        if (asset == null)
            return;

        animations.Clear();

        if (asset.atlasAnimations == null) return;

        foreach (var anim in asset.atlasAnimations)
        {
            var anim2D = new Animation2D
            {
                isLoop = anim.isLoop,
                frameRate = anim.frameRate,
                sprites = new List<Sprite>(anim.sprites), 
                isExpanded = true
            };
            animations.Add(anim2D);

            if(anim.sprites.Length > 0)
            {
                atlasTexture = anim.sprites[0].texture;
            }
        }
    }

    private void SpritesDropArea(Animation2D animation)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Drag Sprites Here:", EditorStyles.helpBox);

        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Sprites Here", EditorStyles.centeredGreyMiniLabel);

        Event evt = Event.current;
        if (dropArea.Contains(evt.mousePosition))
        {
            if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        if (obj is Sprite sprite)
                        {
                            animation.sprites.Add(sprite);
                        }
                        else if (obj is Texture2D texture)
                        {
                            string path = AssetDatabase.GetAssetPath(texture);
                            var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                            foreach (var asset in assets)
                            {
                                if (asset is Sprite innerSprite)
                                {
                                    animation.sprites.Add(innerSprite);
                                }
                            }
                        }
                    }

                    evt.Use();
                }
            }
        }

    }

    private void SaveAtlasAnimationData()
    {
        if (atlasTexture == null || animations.Count == 0)
        {
            Debug.LogWarning("No texture selected or animations are empty.");
            return;
        }

        var path = AssetDatabase.GetAssetPath(atlasTexture);
        var savePath = EditorUtility.SaveFilePanel("save animation", Path.GetDirectoryName(path), atlasTexture.name, "asset");

        if (string.IsNullOrEmpty(savePath)) return;

        savePath = savePath.Replace(Application.dataPath, "Assets");

        var asset = AssetDatabase.LoadAssetAtPath<AtlasAnimationData>(savePath);
        bool isCreate = false;

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<AtlasAnimationData>();
            isCreate = true;
        }

        var atlasAnimations = new AtlasAnimation[animations.Count];
        for (int i = 0; i < animations.Count; i++)
        {
            var animation2D = animations[i];
            int count = animation2D.sprites.Count;
            var animData = new AtlasAnimation
            {
                isLoop = animation2D.isLoop,
                frameRate = animation2D.frameRate,
                sprites = animation2D.sprites.ToArray(),
                tiles = new float4[count],
                sizes = new float2[count]
            };

            for (int j = 0; j < count; j++)
            {
                var sprite = animation2D.sprites[j];
                animData.tiles[j] = GetTile(atlasTexture.width, atlasTexture.height, sprite.rect);
                animData.sizes[j] = new float2(sprite.rect.width / 100f, sprite.rect.height / 100f);
            }

            atlasAnimations[i] = animData;
        }

        asset.atlasAnimations = atlasAnimations;

        if (isCreate)
            AssetDatabase.CreateAsset(asset, savePath);
        else
            EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        selectedAsset = asset;
    }

    float4 GetTile(int atlasWidth, int atlasHeight, Rect rect)
    {
        return new float4(rect.width / atlasWidth, rect.height / atlasHeight, rect.x / atlasWidth, rect.y / atlasHeight);
    }

    class Animation2D
    {
        public bool isLoop;
        public int frameRate;
        public bool isExpanded = true;
        public List<Sprite> sprites;
    }
}
