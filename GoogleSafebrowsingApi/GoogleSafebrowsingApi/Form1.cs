using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace GoogleSafebrowsingApi
{
    public partial class Form1 : Form
    {
        int len = 0;
        
        public Form1()
        {
            InitializeComponent();
            len = ("Of the 43 pages we tested on the site over the past 90 days, 0 page(s) resulted "
            + "in malicious software being downloaded and installed without user consent. The last time"
            + "Google visited this site was on 2014-09-04, and suspicious content was never found on this"
            + "site within the past 90 days.\n\nThis site was hosted on 2 network(s) including AS15169 (GOOGLE), AS36040 (YOUTUBE).").Length;
        }

        public bool SafeWebsite(string url)
        {
            bool result = true;
            string link = "http://www.google.com/safebrowsing/diagnostic?site=" + url;
            string source = GetHtml(link).Replace("\r", String.Empty);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            string suspicious_address = "doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[2]";
            string happen_address = "doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[4]";
            string intermediary_address = "doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[6]";
            string malware_host_address = "doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[8]";

            result = result && (StringToNode(suspicious_address, doc).InnerText.Trim() 
                == "This site is not currently listed as suspicious.");
            result = result && (InString(StringToNode(happen_address, doc).InnerText.Trim(), 
                "suspicious content was never found on this site within the past 90 days.") != -1)
                && (InString(StringToNode(happen_address, doc).InnerText,
                "past 90 days, 0 page(s) resulted in") != -1);
            result = result && (InString(StringToNode(intermediary_address, doc).InnerText.Trim(), 
                "did not appear to function as an intermediary for the infection of any sites.") != -1);
            result = result && (StringToNode(malware_host_address, doc).InnerText.Trim() 
                == "No, this site has not hosted malicious software over the past 90 days.");

            return result;
        }
        
        #region Text Functions

        public string[] ParseString(string beginning, string ending, string text)
        {
            string[] result = null;

            text = text.Replace("\t", String.Empty);
            text = text.Replace("\r", String.Empty);

            while (TimesInString(text, "\n\n") > 0)
            {
                text = text.Replace("\n\n", "\n");
            }

            // beginning
            if (beginning != null)
            {
                if (InString(text, beginning) == -1)
                {
                    MessageBox.Show("Beginning Error");
                }
                text = text.Substring(InString(text, beginning) + beginning.Length,
                    text.Length - (InString(text, beginning) + beginning.Length));
            }

            // ending
            if (ending != null)
            {
                if (InString(text, ending) == -1)
                {
                    MessageBox.Show("Ending Error");
                }
                text = text.Substring(0, InString(text, ending));
            }

            text = text.Trim();


            result = StringToArray(text);

            return result;
        }

        int TimesInString(string text, string substring)
        {
            int result = 0;
            int counter = 0;

            while (counter + substring.Length <= text.Length)
            {
                if (text.Substring(counter, substring.Length) == substring)
                {
                    result++;
                    counter += substring.Length;
                }
                else
                {
                    counter++;
                }

            }

            return result;
        }

        string[] StringToArray(string text)
        {
            string[] result = new string[TimesInString(text, "\n") + 1];

            for (int i = 0; i < result.Length - 1; i++)
            {
                result[i] = text.Substring(0, InString(text, "\n")).Trim();
                text = text.Substring(InString(text, "\n"), text.Length - InString(text, "\n"));

                if (text.Length > 1)
                {
                    text = text.Substring(1, text.Length - 1);
                }
            }
            result[result.Length - 1] = text.Trim();

            return result;
        }

        public int InString(string text, string substring)
        {
            // gives the first position of a desired substring
            int position = -1;

            for (int a = 0; a <= text.Length - substring.Length; a++)
            {
                if (text.Substring(a, substring.Length) == substring && position == -1) position = a;
            }

            return position;
        }

        #endregion
        
        private string GetHtml(string url)
        {
            // Assemble the search query to send to the server.
            // Grab the HTML source code via a web request.
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

        public HtmlNode StringToNode(string string_node, HtmlAgilityPack.HtmlDocument doc)
        {
            HtmlNode result = doc.DocumentNode;
            
            if (string_node.Length > 2 && string_node.Substring(0, 3) == "doc")
            {
                string_node = string_node.Substring(3, string_node.Length - 3);

                while (string_node.Length != 0)
                {
                    if (string_node.Length >= 14 && string_node.Substring(0, 12) == ".ChildNodes[")
                    {
                        string_node = string_node.Substring(12, string_node.Length - 12);

                        try
                        {
                            result = result.ChildNodes[Convert.ToInt32(string_node.Substring(0, InString(string_node, "]")))];
                        }
                        catch
                        {
                            MessageBox.Show("Parsing Error: " + string_node);
                        }
                        string_node = string_node.Substring(InString(string_node, "]") + 1, string_node.Length - (InString(string_node, "]") + 1));
                    }
                    else
                    {
                        MessageBox.Show("ChildNodes Error: " + string_node);
                    }
                }
            }
            else
            {
                MessageBox.Show("doc Error: " + string_node);
            }

            return result;
        }

        public void SearchNodes(string text, HtmlNode parent, string location)
        {
            if (InString(parent.InnerText.ToLower(), text.ToLower()) != -1 && parent.InnerText.Length < 2 * len)
            {
                Output.Text += "phrase found:\n" + parent.InnerText + "\n" + location + "\n\n";
            }

            // Output.Text += parent.InnerText + "\n";

            /*
            foreach (HtmlNode child in parent.ChildNodes)
            {
                SearchNodes(text, child);
            }
            */

            for (int i = 0; i < parent.ChildNodes.Count; i++)
            {
                SearchNodes(text, parent.ChildNodes[i], location + ".ChildNodes[" + i.ToString() + "]");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool stuff = SafeWebsite("google.com/safebrowsing");
            MessageBox.Show(stuff.ToString());
        }

    }
}
