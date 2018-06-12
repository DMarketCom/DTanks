using System.Collections.Generic;

namespace CodeGenerator
{
    public class SwaggerModel
    {
        public string swagger;
        public Info info;
        public Dictionary<string, Path> paths;
        public Dictionary<string, DefinitionModel> definitions;
    }
}
