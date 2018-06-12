using System.IO;

namespace CodeGenerator
{
    public class ClassWriter
    {
        private string _path;
        private string _template;

        public ClassWriter(string path, string template)
        {
            _path = path;
            _template = template;
        }

        public void CreateFile(RequestDataModel Request)
        {
            string FileName = Request.Folder + "/" + Request.ClassName + ".cs";

            File.WriteAllText(FileName, ApplyDataOfRequest(Request));
        }

        private string ApplyDataOfRequest(RequestDataModel Request)
        {
            string ClassData = _template;
            ParamsReplacer Replacer = new ParamsReplacer(_template, Request);
            return Replacer.Process();
        }
    }
}

