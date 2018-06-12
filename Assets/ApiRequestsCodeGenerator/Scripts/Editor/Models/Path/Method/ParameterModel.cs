using System.Collections.Generic;

namespace CodeGenerator
{
    public class ParameterModel
    {
        public string type;
        public string description;
        public string name;
        public ParameterPosition In;
        public PropertyModel schema;
        public DefinitionModel shemeModel;
        public bool required;
        public List<ParameterModel> items;
    }
}
