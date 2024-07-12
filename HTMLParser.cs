using System.Text.RegularExpressions;
using System.Text;

namespace ZeroEPUB
{
    internal class HTMLParser
    {
        public string HTMLToText(string HTMLCode, bool ChangingNewlines = true)
        {
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            // Remove any JavaScript  
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
            , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            // Remove any CSS style sheets built into the website (who even does this)
            HTMLCode = Regex.Replace(HTMLCode, "<style.*?</style>", ""
            , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            // Replace special characters like &, <, >, " etc.  
            StringBuilder sbHTML = new(HTMLCode);
            // Note: There are many more special characters, these are just  
            // most common. You can add new characters in this arrays if needed  
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
        "&gt;", "&reg;", "&copy;", "&bull;", "&trade;","&#39;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "\'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }
            // Check if there are line breaks (<br>) or paragraph (<p>)  
            if (!ChangingNewlines)
            {
                sbHTML.Replace("<br>", "\n<br>");
                sbHTML.Replace("<br ", "\n<br ");
                sbHTML.Replace("<p ", "\n<p ");
                sbHTML.Replace("<p>", "\n<p>");
            }

            // return the sbHTML
            
            return sbHTML.ToString();
        }
    }
}