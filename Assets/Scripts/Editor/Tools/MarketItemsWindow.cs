using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DMarketSDK.Editor
{
    public sealed class MarketItemsEditorWindow : EditorWindow
    {
        private List<MarketItemData> _itemsList = new List<MarketItemData>();
        private Vector2 _scrollPosition = Vector2.zero;
        private TextAsset _jsonItemsAsset;

        [MenuItem("DMarket/MarketItems Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MarketItemsEditorWindow), false, "Market Items Editor");
        }

        private void OnGUI()
        {
            DrawTopBlock();

            GUILayout.Label("Market items list", EditorStyles.boldLabel);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            DrawItemsListBlock();
            EditorGUILayout.EndScrollView();

            DrawBottomBlock();
        }

        private void DrawTopBlock()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 20 };
            EditorGUILayout.LabelField("Market Items Editor", style, GUILayout.ExpandWidth(true), GUILayout.MinHeight(40));
            
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            _jsonItemsAsset = (TextAsset) EditorGUILayout.ObjectField("JSON asset", _jsonItemsAsset, typeof(TextAsset), false);

            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Parse JSON asset"))
            {
                ParseJSONAsset(_jsonItemsAsset);
            }

            if (GUILayout.Button("Save JSON asset"))
            {
                SaveJSONAsset();
            }
        }

        private void DrawItemsListBlock()
        {
            if(_itemsList == null || _itemsList.Count == 0)
            {
                EditorGUILayout.HelpBox("Items list is empty or null.", MessageType.Info);
                return;
            }

            MarketItemData[] itemsArray = _itemsList.ToArray(); // Create array for edit collection in foreach.
            foreach (var itemData in itemsArray)
            {
                DrawMarketItemData(itemData);
            }
        }

        private void DrawBottomBlock()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add item"))
            {
                _itemsList.Add(new MarketItemData());
            }

            if (GUILayout.Button("Remove last item"))
            {
                if (_itemsList.Count > 0)
                {
                    _itemsList.Remove(_itemsList.Last());
                }
            }

            if (GUILayout.Button("Clear list"))
            {
                _itemsList.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMarketItemData(MarketItemData itemData)
        {
            Rect rect = EditorGUILayout.BeginVertical();

            GUI.Box(rect, GUIContent.none);

            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(30));

            GUILayout.Label(string.Format("ItemId: {0}", itemData.Id), EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                _itemsList.Remove(itemData);
            }

            EditorGUILayout.EndHorizontal();

            itemData.Id = EditorGUILayout.TextField("Id", itemData.Id);
            DrawItemMetaData(itemData.MetaData);

            EditorGUILayout.EndVertical();
        }

        private void DrawItemMetaData(MarketItemMetaData metaData)
        {
            metaData.Title = EditorGUILayout.TextField("Title", metaData.Title);
            metaData.GameId = EditorGUILayout.TextField("Game Id", metaData.GameId);
            metaData.Category = EditorGUILayout.TextField("Category", metaData.Category);
            metaData.Description = EditorGUILayout.TextField("Description", metaData.Description);

            List<MarketItemImageData> imagesList = metaData.ItemImages;

            foreach(var imageData in imagesList)
            {
                DrawItemImageData(imageData);
            }

            if (GUILayout.Button("Add image data"))
            {
                imagesList.Add(new MarketItemImageData());
            }

            if (imagesList.Count > 0 && GUILayout.Button("Remove image data"))
            {
                imagesList.Remove(metaData.ItemImages.Last());
            }
        }

        private void DrawItemImageData(MarketItemImageData imageData)
        {
            Rect rect = EditorGUILayout.BeginVertical();

            GUI.Box(rect, GUIContent.none);
            imageData.ImageUrl = EditorGUILayout.TextField("Image Url", imageData.ImageUrl);
            imageData.Type = EditorGUILayout.TextField("Type", imageData.Type);

            EditorGUILayout.EndVertical();
        }

        private void ParseJSONAsset(TextAsset asset)
        {
            List<MarketItemData> parsedItems = JsonConvert.DeserializeObject<List<MarketItemData>>(asset.text);
            _itemsList = parsedItems;
        }

        private void SaveJSONAsset()
        {
            string pathToSave = EditorUtility.SaveFilePanel("Save JSON at path", Application.dataPath, "NewItemsList", ".json");

            if(string.IsNullOrEmpty(pathToSave))
            {
                return;
            }

            string itemsListJSON = JsonConvert.SerializeObject(_itemsList, Formatting.Indented);
            File.WriteAllText(pathToSave, itemsListJSON);

            AssetDatabase.Refresh();
        }
    }
}