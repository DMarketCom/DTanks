using System.Collections.Generic;

namespace CodeGenerator
{
    public class DefinitionModel
    {
        public List<string> required;
        public Dictionary<string, PropertyModel> properties;
    }
}
