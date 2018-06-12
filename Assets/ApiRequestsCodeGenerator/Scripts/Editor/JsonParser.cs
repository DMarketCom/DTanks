using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CodeGenerator
{
    public class JsonParser
    {
        public Action<SwaggerModel> OnComplete;
        public SwaggerModel structure;

        private WWW www;

        public JsonParser(string apiUrl)
        {
            LoadJson(apiUrl);
        }

        private void LoadJson(string url)
        {
            www = new WWW(url);
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                EditorApplication.update = Update;
            else
                WaitForDownload();
#else
         WaitForDownload();
#endif
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (www.isDone)
            {
                EditorApplication.update = null;
                LoadCompleted();
            }
        }
#endif

        private IEnumerator WaitForDownload()
        {
            yield return www;
            LoadCompleted();
        }

        private void LoadCompleted()
        {
            if (OnComplete != null)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.ReferenceResolverProvider = ReferenceResolverProvider;
                structure = JsonConvert.DeserializeObject<SwaggerModel>(www.text, setting);
                OnComplete.Invoke(structure);
            }
        }

        public IReferenceResolver ReferenceResolverProvider()
        {
            return new DefinitionsReferenceResolver();
        }
    }
}
