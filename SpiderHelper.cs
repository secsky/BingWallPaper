using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class SpiderHelper
    {
        public const string lastestchromeuseragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36";
        public static List<KeyValuePair<string, string>> GetRequesterByString(string strtoparse)
        {
            var heders = new  List<KeyValuePair<string, string>>();
            var lines = strtoparse.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var index = line.IndexOf(':');
                if (index==0)
                {
                    index=line.Substring(1, line.Length - 2).IndexOf(':');
                }
                var kv = new KeyValuePair<string, string>(line.Substring(0, index), line.Substring(index + 1, line.Length - index - 1).Trim());
                //hederdict[line.Substring(0, index)] = line.Substring(index + 1, line.Length - index - 1).Trim();
                heders.Add(kv);
            }
            //foreach (var kv in hederdict)
            //{
            //    Console.WriteLine(kv);
            //}
            return heders;
        }
    }
}
