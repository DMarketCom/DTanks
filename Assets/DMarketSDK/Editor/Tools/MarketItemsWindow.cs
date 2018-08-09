using System;
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

        [MenuItem("Tools/MarketItems Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MarketItemsEditorWindow), false, "MarketWidget Items Editor");
        }

        private void OnGUI()
        {
            DrawFileSettingsBlock();

            DrawItemsListBlock();
 
            DrawItemOperationBlock();
        }

        private void DrawFileSettingsBlock()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 20 };
            EditorGUILayout.LabelField("MarketWidget Items Editor", style, GUILayout.ExpandWidth(true), GUILayout.MinHeight(40));
            
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            
            _jsonItemsAsset = (TextAsset) EditorGUILayout.ObjectField("JSON asset", _jsonItemsAsset, typeof(TextAsset), false);
            
            if (_jsonItemsAsset != null && GUILayout.Button("Parse"))
            {
                ParseAsset(_jsonItemsAsset);
            }

            if (_jsonItemsAsset != null && GUILayout.Button("Save"))
            {
                SaveAsset(AssetDatabase.GetAssetPath(_jsonItemsAsset));
            }

            if (GUILayout.Button("Save As..."))
            {
                SaveAsset();
            }
        }

        private void DrawItemsListBlock()
        {
            GUILayout.Label("MarketWidget items list", EditorStyles.boldLabel);
            if (_itemsList == null || _itemsList.Count == 0)
            {
                EditorGUILayout.HelpBox("Items list is empty or null.", MessageType.Info);
                return;
            }
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            MarketItemData[] itemsArray = _itemsList.ToArray();
            foreach (var itemData in itemsArray)
            {
                DrawMarketItemData(itemData);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawItemOperationBlock()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add item"))
            {
                _itemsList.Add(new MarketItemData());
            }
            if (_itemsList.Count > 0)
            {
                if (GUILayout.Button("Remove last item"))
                {
                    _itemsList.Remove(_itemsList.Last());
                }
                if (GUILayout.Button("Clear list"))
                {
                    _itemsList.Clear();
                }
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
            metaData.Description = EditorGUILayout.TextField("Description", metaData.Description);
            DrawImageData(metaData.ItemImages);
            DrawTagData(metaData.Tags);
        }

        private void DrawTagData(List<string> tags)
        {
            for(var i = 0; i < tags.Count; i++)
            {
                tags[i] = EditorGUILayout.TextField("item tag", tags[i]);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("add tag"))
            {
                tags.Add(tags.Last());
            }

            if (tags.Count > 1 && GUILayout.Button("remove tag"))
            {
                tags.RemoveAt(tags.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawImageData(List<MarketItemImageData> itemImages)
        {
            foreach (var imageData in itemImages)
            {
                imageData.ImageUrl = EditorGUILayout.TextField("Image Url", imageData.ImageUrl);
                EditorGUILayout.LabelField(string.Format("image type : {0}", imageData.Type));
            }
        }

        private void ParseAsset(TextAsset asset)
        {
            try
            {
                var parsedItems = JsonConvert.DeserializeObject<List<MarketItemData>>(asset.text);
                _itemsList = parsedItems;
            }
            catch (JsonReaderException e)
            {
               ShowErrorNotification(e.Message);
                Console.WriteLine(e);
            }
        }

        private void ShowErrorNotification(string error)
        {
            ShowNotification(new GUIContent(error));
        }

        private void SaveAsset(string pathToSave = null)
        {
            if (!IsCanSaveAsset())
            {
                return;
            }

            if (string.IsNullOrEmpty(pathToSave))
            {
                pathToSave =
                    EditorUtility.SaveFilePanel("Save JSON at path", Application.dataPath, "NewItemsList", "json");
            }

            if(string.IsNullOrEmpty(pathToSave))
            {
                return;
            }

            string itemsListJSON = JsonConvert.SerializeObject(_itemsList, Formatting.Indented);
            File.WriteAllText(pathToSave, itemsListJSON);
            AssetDatabase.Refresh();
        }

        private bool IsCanSaveAsset()
        {
            foreach (var item in _itemsList)
            {
                if (string.IsNullOrEmpty(item.MetaData.ItemImages[0].ImageUrl))
                {
                    ShowErrorNotification("Image url cannot be empty");
                    return false;
                }

                if (item.MetaData.Tags.Contains(string.Empty))
                {
                    ShowErrorNotification("Tag cannot be empty");
                    return false;
                }

                if (string.IsNullOrEmpty(item.Id))
                {
                    ShowErrorNotification("Id cannot be empty");
                    return false;
                }

                if (string.IsNullOrEmpty(item.MetaData.Title))
                {
                    ShowErrorNotification("Title cannot be empty");
                    return false;
                }
            }

            return true;
        }
    }
}