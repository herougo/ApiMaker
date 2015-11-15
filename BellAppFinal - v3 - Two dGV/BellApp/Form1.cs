using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;
using HtmlAgilityPack;

namespace BellApp
{
    public partial class Form1 : Form
    {
        string ExactSavePath = AppDomain.CurrentDomain.BaseDirectory + "MoviesExact.txt";
        string PhraseSavePath = AppDomain.CurrentDomain.BaseDirectory + "MoviesPhrase.txt";
        string OtherSavePath = AppDomain.CurrentDomain.BaseDirectory + "MoviesOther.txt";
        string PrevDayPath = AppDomain.CurrentDomain.BaseDirectory + "PrevDay.txt";
        string SearchResultsPath = AppDomain.CurrentDomain.BaseDirectory + "SearchResults.txt";
        string FalseResultsPath = AppDomain.CurrentDomain.BaseDirectory + "FalseResults.txt";
        string RecordedMoviesPath = AppDomain.CurrentDomain.BaseDirectory + "RecordedMovies.txt";
        string MissingChannelsPath = AppDomain.CurrentDomain.BaseDirectory + "MissingChannels.txt";

        int[] missingChannels = new int[100];
        string[] movieTitle = new string[500];
        string[,] SearchResults = new string[500, 5];
        string[] falseResult = new string[500];
        int counter = 0;

        public Form1()
        {
            InitializeComponent();

            // Load previous search results
            ResetSearchResults();
            LoadChart();

            // Load previous day
            UpdateDate();

            // Load saved exact titles
            FileStream fs = new FileStream(ExactSavePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            tbSearchExact.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            // Load phrase titles
            fs = new FileStream(PhraseSavePath, FileMode.Open);
            sr = new StreamReader(fs);

            tbSearchPhrase.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            // Load Other titles
            fs = new FileStream(OtherSavePath, FileMode.Open);
            sr = new StreamReader(fs);

            tbSearchOther.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            // Load False Results
            for (int a = 0; a < 500; a++)
            {
                falseResult[a] = "";
            }

            counter = 0;
            
            fs = new FileStream(FalseResultsPath, FileMode.Open);
            sr = new StreamReader(fs);

            while (sr.Peek() != -1)
            {
                falseResult[counter] = sr.ReadLine();
                counter++;
            }

            sr.Close();
            fs.Close();

            // Sort the data
            this.dGVOutput.Sort(this.Column3, ListSortDirection.Ascending);
            this.dGVOutputMC.Sort(this.Column8, ListSortDirection.Ascending);
        }

        // Queries the Bell server for the HTML source code.  Accepts a query
        // (a show title) and returns the full HTML code as a string.
        private string GetHtml(string query)
        {
            // Assemble the search query to send to the server.
            string url = "http://tvonline.bell.ca/tvonline/servlet/CommandServlet?command=flow&processid=165&simpleSearchKeyWord=" 
                + query
                + "&requestedPage=0&requestedSortMethod=0&requestedSearchMethod=0&filterProgramMethod=0&filterProgramType=0";

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

        public bool SearchString(string text, string substring)
        {
            // searches text string for the subtring string and is true if it is found
            bool inString = false;
            for (int a = 0; a <= (text.Length - substring.Length); a++)
            {
                if (text.Substring(a, substring.Length) == substring) inString = true;
            }
            return inString;
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

        public void ResetSearchResults()
        {
            for (int a = 0; a < 500; a++)
            {
                for (int b = 0; b < 5; b++)
                {
                    SearchResults[a, b] = "";
                }
            }
        }

        public void SearchExact()
        {
            string source;

            // Obtain the HTML source code.
            source = GetHtml(movieTitle[counter]);

            // Declare a new HtmlDocument.  We will use this to traverse the 
            // document structure to search for important data.  HtmlAgilityPack
            // is an opensource, third-party library downloaded from...
            // http://htmlagilitypack.codeplex.com/  A good programmer never
            // re-invents the wheel (unless they absolutely must).
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            // Load our HTML code into the HtmlDocument, so we can traverse the
            // code more easily.
            doc.LoadHtml(source);

            // Establish a list of HtmlNodes to hold the rows of the results table.
            List<HtmlNode> trs = new List<HtmlNode>();

            // Grab the results table.  Bell was nice enough to give their results
            // table a unique id tag, so we can easily pluck it from the HTML
            // source code.
            HtmlNode table = doc.GetElementbyId("guideSResults");


            // If we can't get the table, then there were no results returned, or there
            // was an error, so alert the user there are no results and then quit
            // this function.
            if (table == null)
            {
                return;
            }

            // Loop through all the children of the table and add children to the
            // row list if it is actually an HTML element.  Half the children are
            // HTML elements (<tr>) and half are text nodes, alternating.  This is 
            // just the way HTML documents are laid out.  We want to ignore the
            // text nodes and just get the nodes which are the <tr>'s.
            foreach (HtmlNode child in table.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element) trs.Add(child);
            }

            // Loop through all the rows in the list (except the first one) and extract
            // the data we are looking for.  The first row contains columns headers and
            // graphics, so its not useful to us.
            for (int i = 1; i < trs.Count; i++)
            {
                string title = "";
                string category = "";
                string date = "";
                string time = "";
                string channel = "";

                // Attempt to read the child nodes.  If there is an error catch it so we
                // don't crash.  Instead we will just write empty strings to the
                // output grid for some shows.
                try
                {
                    // The title is found in the second child node of the second
                    // child node of the <tr> element in the <table>.
                    title = trs[i].ChildNodes[1].ChildNodes[1].InnerText.Trim();

                    // These operate the same as the 'title' extraction method
                    // above.  To set these statements up you have to look at
                    // the HTML source code and figure out where all the 
                    // desired child nodes are in the document structure.
                    // I am going up by two and sticking to the odd
                    // numbers because those are the element nodes.  For
                    // this HTML document the even nodes are the text nodes.
                    category = trs[i].ChildNodes[3].InnerText.Trim();
                    date = trs[i].ChildNodes[5].InnerText.Trim();
                    time = trs[i].ChildNodes[7].InnerText.Trim();
                    channel = trs[i].ChildNodes[9].InnerText.Trim();
                }
                catch
                {
                    // Catch all errors, but we don't need to actually do
                    // anything with them.
                }
                
                // Verifies search result
                if (title.ToUpper() == movieTitle[counter].ToUpper() && date != "" && CheckFalseResults(title))
                {
                    // Edit date
                    date = EditDate(date);
                    
                    // Write the data we found to the output datagrid.
                    AddRow(title, category, date, time, channel);

                    // Records it
                    FileStream fs = new FileStream(SearchResultsPath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(title);
                    sw.WriteLine(category);
                    sw.WriteLine(date);
                    sw.WriteLine(time);
                    sw.WriteLine(channel);

                    sw.Close();
                    fs.Close();
                }

                // NEW
            }
        }

        public void SearchPhrase()
        {
            string source;

            // Obtain the HTML source code.
            source = GetHtml(movieTitle[counter]);

            // Declare a new HtmlDocument.  We will use this to traverse the 
            // document structure to search for important data.  HtmlAgilityPack
            // is an opensource, third-party library downloaded from...
            // http://htmlagilitypack.codeplex.com/  A good programmer never
            // re-invents the wheel (unless they absolutely must).
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            // Load our HTML code into the HtmlDocument, so we can traverse the
            // code more easily.
            doc.LoadHtml(source);

            // Establish a list of HtmlNodes to hold the rows of the results table.
            List<HtmlNode> trs = new List<HtmlNode>();

            // Grab the results table.  Bell was nice enough to give their results
            // table a unique id tag, so we can easily pluck it from the HTML
            // source code.
            HtmlNode table = doc.GetElementbyId("guideSResults");


            // If we can't get the table, then there were no results returned, or there
            // was an error, so alert the user there are no results and then quit
            // this function.
            if (table == null)
            {
                return;
            }

            // Loop through all the children of the table and add children to the
            // row list if it is actually an HTML element.  Half the children are
            // HTML elements (<tr>) and half are text nodes, alternating.  This is 
            // just the way HTML documents are laid out.  We want to ignore the
            // text nodes and just get the nodes which are the <tr>'s.
            foreach (HtmlNode child in table.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element) trs.Add(child);
            }

            // Loop through all the rows in the list (except the first one) and extract
            // the data we are looking for.  The first row contains columns headers and
            // graphics, so its not useful to us.
            for (int i = 1; i < trs.Count; i++)
            {
                string title = "";
                string category = "";
                string date = "";
                string time = "";
                string channel = "";

                // Attempt to read the child nodes.  If there is an error catch it so we
                // don't crash.  Instead we will just write empty strings to the
                // output grid for some shows.
                try
                {
                    // The title is found in the second child node of the second
                    // child node of the <tr> element in the <table>.
                    title = trs[i].ChildNodes[1].ChildNodes[1].InnerText.Trim();

                    // These operate the same as the 'title' extraction method
                    // above.  To set these statements up you have to look at
                    // the HTML source code and figure out where all the 
                    // desired child nodes are in the document structure.
                    // I am going up by two and sticking to the odd
                    // numbers because those are the element nodes.  For
                    // this HTML document the even nodes are the text nodes.
                    category = trs[i].ChildNodes[3].InnerText.Trim();
                    date = trs[i].ChildNodes[5].InnerText.Trim();
                    time = trs[i].ChildNodes[7].InnerText.Trim();
                    channel = trs[i].ChildNodes[9].InnerText.Trim();
                }
                catch
                {
                    // Catch all errors, but we don't need to actually do
                    // anything with them.
                }

                // NEW
                // Verifies search result
                if (SearchString(title.ToUpper(), movieTitle[counter].ToUpper()) && date != "" && CheckFalseResults(title))
                {
                    // Edit date
                    date = EditDate(date);
                    
                    // Write the data we found to the output datagrid.
                    AddRow(title, category, date, time, channel);

                    // Records it
                    FileStream fs = new FileStream(SearchResultsPath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(title);
                    sw.WriteLine(category);
                    sw.WriteLine(date);
                    sw.WriteLine(time);
                    sw.WriteLine(channel);

                    sw.Close();
                    fs.Close();
                }

                // NEW
            }
        }

        public void SearchOther()
        {
            string source;

            // Obtain the HTML source code.
            source = GetHtml(movieTitle[counter]);

            // Declare a new HtmlDocument.  We will use this to traverse the 
            // document structure to search for important data.  HtmlAgilityPack
            // is an opensource, third-party library downloaded from...
            // http://htmlagilitypack.codeplex.com/  A good programmer never
            // re-invents the wheel (unless they absolutely must).
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            // Load our HTML code into the HtmlDocument, so we can traverse the
            // code more easily.
            doc.LoadHtml(source);

            // Establish a list of HtmlNodes to hold the rows of the results table.
            List<HtmlNode> trs = new List<HtmlNode>();

            // Grab the results table.  Bell was nice enough to give their results
            // table a unique id tag, so we can easily pluck it from the HTML
            // source code.
            HtmlNode table = doc.GetElementbyId("guideSResults");


            // If we can't get the table, then there were no results returned, or there
            // was an error, so alert the user there are no results and then quit
            // this function.
            if (table == null)
            {
                return;
            }

            // Loop through all the children of the table and add children to the
            // row list if it is actually an HTML element.  Half the children are
            // HTML elements (<tr>) and half are text nodes, alternating.  This is 
            // just the way HTML documents are laid out.  We want to ignore the
            // text nodes and just get the nodes which are the <tr>'s.
            foreach (HtmlNode child in table.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element) trs.Add(child);
            }

            // Loop through all the rows in the list (except the first one) and extract
            // the data we are looking for.  The first row contains columns headers and
            // graphics, so its not useful to us.
            for (int i = 1; i < trs.Count; i++)
            {
                string title = "";
                string category = "";
                string date = "";
                string time = "";
                string channel = "";

                // Attempt to read the child nodes.  If there is an error catch it so we
                // don't crash.  Instead we will just write empty strings to the
                // output grid for some shows.
                try
                {
                    // The title is found in the second child node of the second
                    // child node of the <tr> element in the <table>.
                    title = trs[i].ChildNodes[1].ChildNodes[1].InnerText.Trim();

                    // These operate the same as the 'title' extraction method
                    // above.  To set these statements up you have to look at
                    // the HTML source code and figure out where all the 
                    // desired child nodes are in the document structure.
                    // I am going up by two and sticking to the odd
                    // numbers because those are the element nodes.  For
                    // this HTML document the even nodes are the text nodes.
                    category = trs[i].ChildNodes[3].InnerText.Trim();
                    date = trs[i].ChildNodes[5].InnerText.Trim();
                    time = trs[i].ChildNodes[7].InnerText.Trim();
                    channel = trs[i].ChildNodes[9].InnerText.Trim();
                }
                catch
                {
                    // Catch all errors, but we don't need to actually do
                    // anything with them.
                }

                if (date != "" && CheckFalseResults(title))
                {
                    // Edit date
                    date = EditDate(date);
                                        
                    AddRow(title, category, date, time, channel);

                    // Records it
                    FileStream fs = new FileStream(SearchResultsPath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(title);
                    sw.WriteLine(category);
                    sw.WriteLine(date);
                    sw.WriteLine(time);
                    sw.WriteLine(channel);

                    sw.Close();
                    fs.Close();
                }
            }
        }

        private void ResetMovieTitle()
        {
            for (int a = 0; a < 500; a++)
            {
                movieTitle[a] = "";
            }
        }

        public void ResetMissingChannels()
        {
            for (int a = 0; a < 100; a++)
            {
                missingChannels[a] = -1;
            }
        }

        public void GetMovieTitles(string locationString)
        {
            counter = 0;

            ResetMovieTitle();

            FileStream fs = new FileStream(locationString, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            while (sr.Peek() != -1)
            {
                movieTitle[counter] = sr.ReadLine();
                counter++;
            }

            sr.Close();
            fs.Close();

            counter = 0;
        }

        public void GetMissingChannels()
        {
            counter = 0;

            ResetMissingChannels();

            FileStream fs = new FileStream(MissingChannelsPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            while (sr.Peek() != -1)
            {
                missingChannels[counter] = Convert.ToInt32(sr.ReadLine().Trim());
                counter++;
            }

            sr.Close();
            fs.Close();

            counter = 0;
        }

        public void UpdateDate()
        {
            // Updates the date when the program was last used on the textbox
            // Reads day from txt file and writes it to the rich text box
            FileStream fs = new FileStream(PrevDayPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            tBPrevDay.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            Application.DoEvents();
        }

        public void AddRow(string title, string category, string date, string time, string channel)
        {
            // Convert channel into an integer
            int channelInt;

            if (SearchString(channel, " ")) channelInt = Convert.ToInt32(channel.Substring(0, InString(channel, " ")));
            else
            {
                channelInt = Convert.ToInt32(channel);
            }

            // Add to chart
            if (CheckMissingChannels(channelInt)) 
                dGVOutput.Rows.Add(new string[] { title, category, date, time, channel });
            else
            {
                dGVOutputMC.Rows.Add(new string[] { title, category, date, time, channel });
            }
        }

        public void LoadChart()
        {
            string results;
            string prevResults;
            int Ycounter = 0;
            counter = 0;
            
            // Read from txt file
            FileStream fs = new FileStream(SearchResultsPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            results = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            counter = 0;

            // trim

            // Record to array
            while (results != "")
            {
                prevResults = results;

                // take phrase line by line
                if (SearchString(results, "\r\n"))
                {
                    SearchResults[counter, Ycounter] = results.Substring(0, InString(results, "\r\n"));
                }
                else
                {
                    SearchResults[counter, Ycounter] = results;
                }

                // change results
                if (SearchString(results, "\r\n"))
                {
                    results = results.Substring(InString(results, "\r\n") + 2, 
                        results.Length - InString(results, "\r\n") - 2);
                }
                else
                {
                    results = "";
                }

                // change counters
                if (Ycounter != 4)
                {
                    Ycounter++;
                }
                else
                {
                    Ycounter = 0;
                    counter++;
                }
            }

            // Create the chart
            
            counter = 0;

            // Clear the output grid to prepare for new data.
            dGVOutput.Rows.Clear();
            dGVOutputMC.Rows.Clear();

            // Update Missing Channels
            GetMissingChannels();

            while (SearchResults[counter, 0] != "" && counter != 499)
            {
                AddRow(SearchResults[counter, 0], SearchResults[counter, 1], SearchResults[counter, 2], 
                    SearchResults[counter, 3], SearchResults[counter, 4]);
                counter++;
            }
        }

        public bool CheckFalseResults(string resultTitle)
        {
            bool passTest = true;

            int count = 0;
            while (falseResult[count] != "")
            {
                if (SearchString(resultTitle.ToUpper(), falseResult[count].ToUpper()))
                {
                    passTest = false;
                }
                count++;
            }

            return passTest;
        }

        public bool CheckMissingChannels(int channel)
        {
            bool passTest = true;

            int count = 0;
            while (missingChannels[count] != -1)
            {
                if (missingChannels[count] == channel) passTest = false;
                count++;
            }

            return passTest;
        }
        
        public string EditDate(string movieDate)
        {
            string newDate = movieDate.Substring(InString(movieDate, " ") + 1, movieDate.Length - InString(movieDate, " ") - 1);

            return newDate;
        }

        // Executed when the user presses the Search button.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Update progress bar
            Progress.Text = "Working...";
            Progress.BackColor = System.Drawing.Color.Red;

            // Reset Search Results
            ResetSearchResults();

            FileStream fs = File.Create(SearchResultsPath);
            fs.Close();

            fs = new FileStream(SearchResultsPath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            sw.Close();
            fs.Close();

            // Record Date
            fs = File.Create(PrevDayPath);
            fs.Close();

            fs = new FileStream(PrevDayPath, FileMode.Append);
            sw = new StreamWriter(fs);

            // records date in proper format
            string date = DateTime.Today.ToString("MMMM dd, yyyy");
            sw.WriteLine(date);

            sw.Close();
            fs.Close();

            UpdateDate();

            #region Updates Movie List
            // Exact Titles
            fs = File.Create(ExactSavePath);
            fs.Close();

            fs = new FileStream(ExactSavePath, FileMode.Append);
            sw = new StreamWriter(fs);

            sw.Write(tbSearchExact.Text);

            sw.Close();
            fs.Close();

            // Phrase Titles
            fs = File.Create(PhraseSavePath);
            fs.Close();

            fs = new FileStream(PhraseSavePath, FileMode.Append);
            sw = new StreamWriter(fs);

            sw.Write(tbSearchPhrase.Text);

            sw.Close();
            fs.Close();

            // Other Titles
            fs = File.Create(OtherSavePath);
            fs.Close();

            fs = new FileStream(OtherSavePath, FileMode.Append);
            sw = new StreamWriter(fs);

            sw.Write(tbSearchOther.Text);

            sw.Close();
            fs.Close();
            #endregion

            // Clear the output grid to prepare for new data.
            dGVOutput.Rows.Clear();
            dGVOutputMC.Rows.Clear();

            GetMissingChannels();

            // Refresh form
            Application.DoEvents();

            GetMovieTitles(ExactSavePath);
            while (movieTitle[counter] != "")
            {
                SearchExact();
                counter++;
            }

            GetMovieTitles(PhraseSavePath);
            while (movieTitle[counter] != "")
            {
                SearchPhrase();
                counter++;
            }

            GetMovieTitles(OtherSavePath);
            while (movieTitle[counter] != "")
            {
                SearchOther();
                counter++;
            }

            // Sort the data
            this.dGVOutput.Sort(this.Column3, ListSortDirection.Ascending);
            this.dGVOutputMC.Sort(this.Column8, ListSortDirection.Ascending);

            // Update progress bar
            Progress.Text = "Done";
            Progress.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnUpdateList_Click(object sender, EventArgs e)
        {
            // Exact Titles
            FileStream fs = File.Create(ExactSavePath);
            fs.Close();

            fs = new FileStream(ExactSavePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(tbSearchExact.Text);

            sw.Close();
            fs.Close();

            // Phrase Titles
            fs = File.Create(PhraseSavePath);
            fs.Close();

            fs = new FileStream(PhraseSavePath, FileMode.Append);
            sw = new StreamWriter(fs);

            sw.Write(tbSearchPhrase.Text);

            sw.Close();
            fs.Close();

            // Other Titles
            fs = File.Create(OtherSavePath);
            fs.Close();

            fs = new FileStream(OtherSavePath, FileMode.Append);
            sw = new StreamWriter(fs);

            sw.Write(tbSearchOther.Text);

            sw.Close();
            fs.Close();
        }

        private void btnViewFResultsList_Click(object sender, EventArgs e)
        {
            Process.Start(FalseResultsPath);
        }

        private void btnRecordedMovies_Click(object sender, EventArgs e)
        {
            Process.Start(RecordedMoviesPath);
        }

        private void btnMissingChannels_Click(object sender, EventArgs e)
        {
            Process.Start(MissingChannelsPath);
        }


    }
}