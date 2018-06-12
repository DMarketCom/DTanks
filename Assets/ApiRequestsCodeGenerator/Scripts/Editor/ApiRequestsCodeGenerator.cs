using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace CodeGenerator
{
    public enum ParameterPosition
    {
        header,
        body,
        path,
        query,
        responce
    }

    public class ApiRequestsCodeGenerator
    {
        private string apiUrl = "https://gi-dmarket-dev.devss.xyz/swagger/apidocs.json";
        

        private SwaggerModel structure;
        private List<RequestDataModel> Requests = new List<RequestDataModel>();
        private List<string> Folders = new List<string>();
        private string BasePath;
        private string TemplateFile = "/Assets/ApiRequestsCodeGenerator/Resources/RequestClassTemplate.template";
        

        private JsonParser Parcer;
        private ClassWriter Writer;

        [MenuItem("Tools/ApiRequestsCodeGenerator/Generate", false, 1)]
        public static void Generate()
        {
            new ApiRequestsCodeGenerator();
        }

        public ApiRequestsCodeGenerator()
        {
            Debug.Log("Generate");
            Init();
        }

        private void Init()
        {
            BasePath = GetPath();

            Parcer = new JsonParser(apiUrl);
            Writer = new ClassWriter(BasePath, LoadTemplate());

            Parcer.OnComplete += ProcessJson;
        }

        public string LoadTemplate()
        {
            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + TemplateFile, true);
            return reader.ReadToEnd();
        }

        public string GetPath()
        {
            var prevPath = EditorPrefs.GetString("msf.buildPath", "");
            string path = EditorUtility.SaveFolderPanel("Choose Location for requests classes", prevPath, "");

            if (!string.IsNullOrEmpty(path))
            {
                EditorPrefs.SetString("msf.buildPath", path);
            }
            return path;
        }

        public DefinitionModel GetShemeModel(string definitionsName)
        {
            DefinitionModel model = new DefinitionModel();
            definitionsName = definitionsName.Replace("#/definitions/", string.Empty);
            if (structure.definitions.ContainsKey(definitionsName))
            {
                structure.definitions.TryGetValue(definitionsName, out model);
                //TODO: add recursive getModel
            }

            return model;
        }

        private void CreateRequests()
        {
            Debug.Log("CreateRequests");
            if (structure.paths != null)
            { 
                foreach (KeyValuePair<string, Path> path in structure.paths)
                {
                    if (path.Value.get != null)
                    {
                        CreateRequest(path.Key, path.Value.get, "RequestMethod.Get", "Get");
                    }
                    if (path.Value.post != null)
                    {
                        CreateRequest(path.Key, path.Value.post, "RequestMethod.Post");
                    }
                }
            }

            Debug.Log("Generated done " + Requests.Count + " requests");
        }

        private void CreateRequest(string requestUrl, MethodModel model, string method, string classNamePrefix = "")
        {
            RequestDataModel request = new RequestDataModel(requestUrl, model, method, BasePath, classNamePrefix, GetShemeModel);

            if (Folders.IndexOf(request.Folder) == -1)
            {
                Folders.Add(request.Folder);
            }

            Requests.Add(request);

            //Debug.Log(request.ClassName);
        }

        private void CreateFolders()
        {
            Debug.Log("CreateFolders");

            string[] folders = Directory.GetDirectories(BasePath);
            foreach (string folder in folders)
            {
                FileUtil.DeleteFileOrDirectory(folder);
            }

            foreach (string folder in Folders)
            {
                Directory.CreateDirectory(folder);
            }
            Debug.Log("Created " + Folders.Count + " folders");
        }

        private void CreateFiles()
        {
            Debug.Log("CreateFiles");

            foreach(RequestDataModel Request in Requests)
            {
                Writer.CreateFile(Request);
            }

            Debug.Log("Created " + Requests.Count + " classes");
        } 

        private void ProcessJson(SwaggerModel model)
        {
            structure = model;

            CreateRequests();
            CreateFolders();
            CreateFiles();
        }

        
    }
}
