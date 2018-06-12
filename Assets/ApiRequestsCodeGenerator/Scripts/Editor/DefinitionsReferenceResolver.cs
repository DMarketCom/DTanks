using Newtonsoft.Json.Serialization;

namespace CodeGenerator
{
    public class DefinitionsReferenceResolver : IReferenceResolver
    {
        void IReferenceResolver.AddReference(object context, string reference, object value)
        {
            throw new System.NotImplementedException();
        }

        string IReferenceResolver.GetReference(object context, object value)
        {
            throw new System.NotImplementedException();
        }

        bool IReferenceResolver.IsReferenced(object context, object value)
        {
            throw new System.NotImplementedException();
        }

        object IReferenceResolver.ResolveReference(object context, string reference)
        {
            PropertyModel model = new PropertyModel
            {
                refName = reference
            };
            return model;
        }
    }
}
