using System.Collections.Generic;

namespace CodeGenerator
{
    public class MethodModel
    {
        public string[] consumes;
        public string[] produces;
        public string[] tags;
        public string summary;
        public string operationId;
        public ParameterModel[] parameters;
        public Dictionary<string, ResponceModel> responses;
    }
}
