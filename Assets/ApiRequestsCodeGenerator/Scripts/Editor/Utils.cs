using System.Text.RegularExpressions;

namespace CodeGenerator
{
    public static class Utils
    {
        public static string GetFileName(string url)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(url)) {
                string[] urls = url.Split('/');
                if (urls.Length > 0) {
                    path += Utils.UppercaseFirst(urls[1]);
                }
            }
            return path;
        }

        public static string GetNamespace(string url)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                string[] urls = url.Split('/');
                if (urls.Length > 0)
                {
                    urls[1] = urls[1].Replace('-', ' ');
                    path += Utils.UppercaseFirst(urls[1]);
                    urls[1] = urls[1].Replace(" ", string.Empty);
                }
            }
            return path;
        }

        public static string GetClassName(string folder, string url)
        {
            string Name = url;
            string[] RemoveStrings = new string[] {
                "/" + folder.ToLower() + "/",
                "dmarket/"
            };

            foreach (string RemoveTemplate in RemoveStrings)
            {
                Name = Name.Replace(RemoveTemplate, string.Empty);
            }

            Name = Name.Replace('/', ' ').Replace('-', ' ');
            //find params in url
            Name = Regex.Replace(Name, @"{(.*)}", string.Empty);
            Name = Utils.UppercaseFirst(Name);

            return Name;
        }


        public static string UppercaseFirst(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string[] words = value.Split(' ');

            if (words.Length <= 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < words.Length; i++)
            {
                if (string.IsNullOrEmpty(words[i]))
                {
                    continue;
                }
                words[i] = words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);

            }

            return string.Join(string.Empty, words);
        }
    }
}
