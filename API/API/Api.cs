using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public struct Property
    {
        public string Name;
        public string ExamplePageValue;
    }

    public class Api
    {
        public string ExamplePage { get; set; }
        public Property[] Properties = null;
        public int Property_Len = 0;
        
        public Api(string example_page)
        {
            ExamplePage = example_page;
        }

        public void AddProperty(string name, string value_on_example_page)
        {
            if (Properties == null)
            {
                Properties = new Property[4];
            }

            if (Property_Len >= Properties.Length)
            {
                Property[] temp = new Property[Properties.Length * 2];

                for (int i = 0; i < Property_Len; i++)
                {
                    temp[i] = Properties[i];
                }

                Properties = temp;
            }

            Properties[Property_Len].Name = name;
            Properties[Property_Len].ExamplePageValue = value_on_example_page;
            Property_Len++;
        }

        public void AddTable(string name, string[] headings_on_example_page, 
            string[] row_on_example_page)
        {

        }

    }
}
