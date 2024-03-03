using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Validators.CoreValidators
{
    public class HtmlDecoder
    {
        public  string DecodeHtml(string html)
        {
            return html.Replace("&lt;", "<")
                       .Replace("&gt;", ">")
                       .Replace("&amp;", "&")
                       .Replace("&quot;", "\"")
                       .Replace("&#39;", "'")
                       .Replace("&apos;", "'");
        }

    }
}
