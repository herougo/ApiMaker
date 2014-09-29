using HtmlAgilityPack;
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

namespace API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] links = new string[14];
            FileStream fs;
            StreamWriter sw;
            string table_string;
            string[] sets;

            #region Links Values

            links[0] = "http://tvonline.bell.ca/tvonline/servlet/CommandServlet?command=flow&processid=165&simpleSearchKeyWord=agents&requestedPage=0&requestedSortMethod=0&requestedSearchMethod=0&filterProgramMethod=0&filterProgramType=0";
            links[1] = "http://tvonline.bell.ca/tvonline/servlet/CommandServlet?command=flow&processid=167&simpleSearchKeyWord=agents&requestedPage=0&requestedSortMethod=3&requestedSearchMethod=0&filterProgramMethod=0&filterProgramType=0&simpleSearchSelectedResultTitle=The%20Avengers&simpleSearchSelectedResultEpisodeTitle=The%20Avengers&occurences=1&prevRequestedPageNum=0&prevRequestedSortMethod=0";
            links[2] = "http://yugioh.tcgplayer.com/db/deck.asp?deck_id=99372";
            links[3] = "http://yugioh.tcgplayer.com/db/deck_search.asp";
            links[4] = "http://yugioh.tcgplayer.com/all_yugioh_sets.asp";
            links[5] = "http://yugioh.tcgplayer.com/db/search_result.asp?SetName=Structure+Deck%3A+Realm+of+Light";
            links[6] = "http://yugioh.tcgplayer.com/db/deck_search_result.asp?Location=YCS%20-%202013-12-01%20Turin%20Italy";
            links[7] = "http://en.wikipedia.org/wiki/Billy_Talent_discography";
            links[8] = "http://www.billboard.com/charts/2014-07-19/hot-100";
            links[9] = "http://www.billboard.com/charts/2014-07-19/hot-mainstream-rock-tracks";
            links[10] = "http://www.billboard.com/charts/2014-07-19/hard-rock-albums";
            links[11] = "http://www.billboard.com/charts/2014-07-19/alternative-songs";
            links[12] = "http://www.imdb.com/title/tt1646971/";
            links[13] = "http://www.imdb.com/movies-coming-soon/2014-07/";

            #endregion

            string source;

            string path = AppDomain.CurrentDomain.BaseDirectory + "legend.txt";

            #region Write Legend

            if (!(File.Exists(path)))
            {

                fs = new FileStream(path, FileMode.Create);
                sw = new StreamWriter(fs);

                sw.Write("1 - " + links[0]);
                for (int i = 1; i < links.Length; i++)
                {
                    sw.WriteLine("\n" + (i + 1).ToString() + " - " + links[i]);
                }


                sw.Close();
                fs.Close();

            }

            #endregion

            #region Download Source Code

            for (int i = 0; i < links.Length; i++)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + i.ToString() + ".txt";

                if (!(File.Exists(path)))
                {
                    fs = new FileStream(path, FileMode.Create);
                    sw = new StreamWriter(fs);

                    sw.Write(GetHtml(links[i]));

                    sw.Close();
                    fs.Close();
                }
            }

            #endregion

            

            // Obtain the HTML source code.
            // source = GetHtml(links[5]);

            fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "4.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            source = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            // Output.Text += source;
                        
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            // Establish a list of HtmlNodes to hold the rows of the results table.
            // List<HtmlNode> trs = new List<HtmlNode>();

            #region Look Through Tables

            
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                // Output.Text += "Found: " + table.Id + "\n\n";

                if (table.Id != null && table.Id.Trim() != "")
                {
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {

                        // Output.Text += "row ------------------------------- \n"
                        //     + "------------------------------------\n"
                        //     + "------------------------------------\n";
                        

                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            // Output.Text += "cell: " + cell.InnerText.Trim() + " - ";
                        }

                        /*
                        foreach (HtmlNode cell in row.SelectNodes("a"))
                        {
                            Output.Text += "cell: " + cell.InnerText.Trim() + " - ";
                        }
                        */

                        // Output.Text += "\n";
                    }
                }

                // Output.Text += "\n\n";
            }
            

            #endregion

            HtmlNode table2 = doc.GetElementbyId("main_page_content");
            table_string = "";

            foreach (HtmlNode row in table2.SelectNodes("tr"))
            {
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    table_string = cell.InnerText.Trim();
                }
            }

            if (InString(table_string, "Name:") == -1)
            {
                MessageBox.Show("Error");
            }
            table_string = table_string.Replace("\t", String.Empty);
            table_string = table_string.Replace("\r", String.Empty);

            while (TimesInString(table_string, "\n\n") > 0)
            {
                table_string = table_string.Replace("\n\n", "\n");
            }

            table_string = table_string.Substring(InString(table_string, "All") + 3, table_string.Length - (InString(table_string, "All") + 3));
            table_string = table_string.Substring(0, InString(table_string, "Name:"));
            table_string = table_string.Trim();
            
            sets = StringToArray(table_string);



            fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "5.txt", FileMode.Open);
            sr = new StreamReader(fs);

            source = sr.ReadToEnd();
            doc.LoadHtml(source);

            sr.Close();
            fs.Close();

            // Grab the results table.  Bell was nice enough to give their results
            // table a unique id tag, so we can easily pluck it from the HTML
            // source code.
            HtmlNode table1 = doc.GetElementbyId("main_page_content");
            
            // If we can't get the table, then there were no results returned, or there
            // was an error, so alert the user there are no results and then quit
            // this function.
            if (table1 == null)
            {
                return;
            }

            table_string = "";

            foreach (HtmlNode row in table1.SelectNodes("tr"))
            {
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    table_string = cell.InnerText.Trim();
                }
            }

            // beginning to "<!--JavaScript Tag"
            // &nbsp; &nbsp;
            // &nbsp;

            if (InString(table_string, "<!--JavaScript Tag") == -1)
            {
                MessageBox.Show("Error");
            }
            table_string = table_string.Substring(0, InString(table_string, "<!--JavaScript Tag"));
            table_string = table_string.Replace("&nbsp;", String.Empty);
            table_string = table_string.Replace("\r", String.Empty);

            Output.Text = table_string;

            DataTable real_table = new DataTable();
            int next_char = 0;
            

            for (int i = 0; i < 6; i++)
            {
                next_char = NextWhitespace(table_string);
                real_table.Columns.Add(table_string.Substring(0, next_char), typeof(string));
                table_string = table_string.Substring(next_char, table_string.Length - next_char);

                next_char = NextNonWhitespace(table_string);
                table_string = table_string.Substring(next_char, table_string.Length - next_char);
            }

            string[] table_row = new string[6];
            while (NextWhitespace(table_string) != -1 
                && NextNonWhitespace(table_string) != -1)
            {
                for (int i = 0; i < 6; i++)
                {
                    next_char = NextWhitespace(table_string);
                    table_row[i] = table_string.Substring(0, next_char);
                    table_string = table_string.Substring(next_char, table_string.Length - next_char);

                    next_char = NextNonWhitespace(table_string);
                    if (next_char != -1)
                    {
                        table_string = table_string.Substring(next_char, table_string.Length - next_char);
                    }
                }
                real_table.Rows.Add(table_row);
            }

            DataTableToDataGridView(real_table, dataGridView1);

            MessageBox.Show(SumColumn(real_table, 3).ToString()
                + " - " + SumColumn(real_table, 4).ToString()
                + " - " + SumColumn(real_table, 5).ToString());

            int stuff = 0;
            
        }

        public double SumColumn(DataTable t, int column)
        {
            double sum = 0d;
            string entry;

            for (int i = 0; i < t.Rows.Count; i++)
            {
                entry = t.Rows[i][column].ToString();
                entry = entry.Replace("$", String.Empty);

                sum += Convert.ToDouble(entry);
            }

            return sum;
        }

        public void DataTableToDataGridView(DataTable t, DataGridView dgv)
        {
            dgv.Columns.Clear();

            for (int i = 0; i < t.Columns.Count; i++)
            {
                dgv.Columns.Add(t.Columns[i].ColumnName, t.Columns[i].ColumnName);
            }

            foreach (DataRow row in t.Rows)
            {
                string[] string_row = new string[t.Columns.Count];

                for (int c = 0; c < string_row.Length; c++)
                {
                    string_row[c] = row[c].ToString();
                }

                dgv.Rows.Add(string_row);
            }

            dgv.Refresh();
        }

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

        public int NextNonWhitespace(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) != "\n" && text.Substring(i, 1) != "\t")
                {
                    return i;
                }
            }

            return -1;
        }

        public int NextWhitespace(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == "\n" || text.Substring(i, 1) == "\t")
                {
                    return i;
                }
            }

            return -1;
        }

        int TimesInString(string text, string substring) {
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

        public void SearchNodes(string text, HtmlNode parent, string location)
        {
            if (InString(parent.InnerText.ToLower(), text.ToLower()) != -1 && parent.InnerText.Length < 50)
            {
                 Output.Text += "phrase found:\n" + parent.InnerText + "\n" + location + "\n";
            }

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
    
    }
}
