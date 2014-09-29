using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace TcgplayerApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // string[] stuff = GetAllSets();
            // DataTable table = GetSetCards(stuff[0]);

            // DataTableToDataGridView(table, dataGridView1);

            // string[] stuff1 = GetAllEvents();

            LoadPrimes();
        }

        public string[] GetAllSets()
        {
            string link = "http://yugioh.tcgplayer.com/all_yugioh_sets.asp";
            string source = GetHtml(link);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            HtmlNode table = doc.GetElementbyId("main_page_content");
            string table_string = "";

            foreach (HtmlNode row in table.SelectNodes("tr"))
            {
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    table_string = cell.InnerText.Trim();
                }
            }

            return ParseString("All", "Name:", table_string);
        }

        public DataTable GetSetCards(string set)
        {
            DataTable result = null;
            string table_string = "";
            string link = "http://yugioh.tcgplayer.com/db/search_result.asp?SetName=" + set;
            string source = "";
            try
            {
                source = GetHtml(link);

            }
            catch
            {
                MessageBox.Show("Webpage Error");
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            HtmlNode table1 = doc.GetElementbyId("main_page_content");

            if (table1 == null)
            {
                return result;
            }

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

            table_string = table_string.Replace("&nbsp;", String.Empty);
            DataTable real_table = new DataTable();

            string[] parsed_string = ParseString(null, "<!--JavaScript Tag", table_string);

            result = new DataTable();

            for (int i = 0; i < 6; i++)
            {
                result.Columns.Add(parsed_string[i], typeof(string));
            }

            string[] table_row = new string[6];
            for (int r = 1; r < (parsed_string.Length + 1) / 6; r++)
            {
                for (int i = 0; i < 6; i++)
                {
                    table_row[i] = parsed_string[r * 6 + i];
                }

                result.Rows.Add(table_row);
            }

            return result;
        }

        /*
        public string[] GetAllEvents()
        {
            string link = "http://yugioh.tcgplayer.com/db/deck_search.asp";
            string source = GetHtml(link);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            HtmlNode table = doc.GetElementbyName("Location");
            string table_string = "";

            foreach (HtmlNode row in table.SelectNodes("tr"))
            {
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    table_string = cell.InnerText.Trim();
                }
            }

            table_string = "";

            return ParseString("All", "Name:", table_string);
        }
        */

        public double SetWorth(DataTable set, string[] rarities, int[] rarity_odds, int cards_per_pack)
        {
            // Assume rarities and rarity_odds is sorted
            
            long int_result = 0;
            double result = 0d;
            int rarity_column = 2;
            int price_column = 4;
            int[] rarity_num = new int[rarities.Length];
            int common_rarity = -1;
            int counter = 0;

            // Sort
            set = Resort(set, "Rarity", "ASC");

            // Get Number of Each Rarity
            counter = 0;
            for (int i = 0; i < rarity_num.Length; i++)
            {
                rarity_num[i] = 0;

                // string temp = set.Rows[counter][rarity_column].ToString();

                while (counter < set.Rows.Count
                    && set.Rows[counter][rarity_column].ToString() == rarities[i])
                {
                    rarity_num[i]++;
                    counter++;
                }
            }

            int pack_num = 1;
            int[] rarity_possiblities = new int[rarity_odds.Length];

            for (int a = 0; a < rarity_odds.Length; a++)
            {
                rarity_possiblities[a] = rarity_odds[a] * rarity_num[a];

                if (rarity_num[a] == common_rarity) rarity_possiblities[a] = 0;
            }
            pack_num = LCM(rarity_possiblities);


            counter = 0;
            int card_count = 0;
            for (int i = 0; i < rarity_num.Length; i++)
            {
                if (rarity_odds[i] == common_rarity)
                {
                    while (counter < set.Rows.Count
                    && set.Rows[counter][rarity_column].ToString() == rarities[i])
                    {
                        counter++;
                    }

                    continue;
                }
                
                while (counter < set.Rows.Count
                    && set.Rows[counter][rarity_column].ToString() == rarities[i])
                {
                    card_count += pack_num / rarity_possiblities[i];

                    string entry = set.Rows[counter][price_column].ToString()
                        .Replace("$", String.Empty).Replace(".", String.Empty);

                    if (entry != "N/A")
                    {
                        int_result += Convert.ToInt64(entry)
                            * pack_num / rarity_possiblities[i];

                        // Output.Text += set.Rows[counter][0].ToString() + " gives "
                        //     + entry.ToString() + " * " + (pack_num / rarity_possiblities[i]).ToString()
                        //     + " = " + (Convert.ToInt64(entry) * pack_num / rarity_possiblities[i]).ToString() + "\n";
                    }
                    counter++;
                }
            }

            Output.Text += "Rare cards give " + (Convert.ToDouble(int_result / pack_num) / 100d).ToString() + "\n";

            // Commons
            counter = 0;
            for (int i = 0; i < rarity_num.Length; i++)
            {
                if (rarity_odds[i] == common_rarity)
                {
                    int common_sum = 0;

                    while (counter < set.Rows.Count
                    && set.Rows[counter][rarity_column].ToString() == rarities[i])
                    {
                        string entry = set.Rows[counter][price_column].ToString()
                        .Replace("$", String.Empty).Replace(".", String.Empty);

                        if (entry != "N/A")
                        {
                            common_sum += Convert.ToInt32(entry);
                        }
                        counter++;
                    }

                    // Output.Text += "Commons give " + (common_sum / rarity_num[i]).ToString() 
                    //     + " * " + (cards_per_pack * pack_num - card_count).ToString() + "\n";

                    int_result += Convert.ToInt64(cards_per_pack * pack_num - card_count) * common_sum / rarity_num[i];
                }
                else
                {
                    while (counter < set.Rows.Count
                    && set.Rows[counter][rarity_column].ToString() == rarities[i])
                    {
                        counter++;
                    }
                }
            }

            int_result /= pack_num;

            result = Convert.ToDouble(int_result);
            result /= 100d;

            return result;
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

        public DataTable Resort(DataTable dt, string colName, string direction)
        {
            DataTable dtOut = null;
            dt.DefaultView.Sort = colName + " " + direction;
            dtOut = dt.DefaultView.ToTable();
            return dtOut;
        }

        public int LCM(int[] array)
        {
            int result = 1;
            int max_prime_degree = 0;
            bool all_one = true;
            int current_prime = 0;

            int[] array_copy = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array_copy[i] = array[i];
            }
            
            for (int i = 0; i < array_copy.Length; i++)
            {
                if (array_copy[i] < 1)
                {
                    array_copy[i] = 1;
                }
                else if (array_copy[i] != 1)
                {
                    all_one = false;
                }
            }

            current_prime = 0;
            while (!all_one)
            {
                all_one = true;
                max_prime_degree = 0;

                for (int i = 0; i < array_copy.Length; i++)
                {
                    int prime_degree_counter = 0;
                    while (array_copy[i] % primes[current_prime] == 0)
                    {
                        array_copy[i] /= primes[current_prime];
                        prime_degree_counter++;
                    }

                    if (prime_degree_counter > max_prime_degree)
                    {
                        max_prime_degree = prime_degree_counter;
                    }

                    if (array_copy[i] != 1)
                    {
                        all_one = false;
                    }
                }

                result *= Convert.ToInt32(Math.Pow(primes[current_prime], max_prime_degree));
                current_prime++;

                if (current_prime >= primes.Length)
                {
                    LoadPrimes();
                }
            }

            return result;
        }

        int[] primes = null;
        public void LoadPrimes()
        {
            if (primes == null)
            {
                primes = new int[32]
                { 2, 3, 5, 7, 11, 
                13, 17, 19, 23, 29, 
                31, 37, 41, 43, 47, 
                53, 59, 61, 67, 71, 
                73, 79, 83, 89, 97,
                101, 103, 107, 109, 113,
                127, 131 };
            }
            else
            {
                string uh_oh = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] rarities = { "C", "GR", "R", "SCR", "SP", "SR", "UR", "UTR" };
            int[] rarity_odds = { -1, 120, 1, 23, 3, 5, 12, 24 };

            string[] sets = { "Duelist Alliance", "Legacy of the Valiant", "Shadow Specters",
                            "Judgment of the Light", "Starstrike Blast"};
            string[] sets1 = { "The Legend of Blue Eyes White Dragon", "Metal Raiders", "Magic Ruler" };
            
            double result;

            for (int i = 0; i < sets1.Length; i++)
            {
                // result = SetWorth(GetSetCards(sets1[i]), rarities, rarity_odds, 9);
                // Output.Text += sets1[i] + " - " + result.ToString() + "\n";
            }

            string[] rarities1 = { "C", "R", "SCR", "SR", "UR" };
            int[] rarity_odds1 = { -1, 1, 1, 1, 1 };

            result = SetWorth(GetSetCards("2014 Mega-Tin Mega-Pack"), rarities1, rarity_odds1, 16);
            Output.Text += "2014 Mega-Tin Mega-Pack - " + result.ToString() + "\n";
        }

        
        
    }
}
