using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu
{
    public class Table<T> : IHaveScope where T : TableRecord
    {
        public List<ElementScope> Header;
        public List<string> HeaderCaptions;
        private List<List<ElementScope>> RowsOfElements;

        public IEnumerable<T> Data
        {
            get
            {
                if (scope == null)
                    throw new Exception("Table scope was not initialized. Use SetScope() before calling Data.");

                InitMultiple();

                foreach (var row in RowsOfElements)
                {
                    var tr = (T)Activator.CreateInstance(typeof(T));   // create new TableRecord that will store data got from table row

                    foreach (var field in tr.GetType().GetFields())
                    {
                        // element in row
                        if (field.GetValue(tr) is TableAttribute td)
                        {
                            if (td.locator == null)
                                td.locator = CapsNoSpaces(field.Name);   // if locator not set, use field name

                            int tdIndex = -1;
                            for (int i = 0; i < HeaderCaptions.Count; ++i)
                            {
                                if (CapsNoSpaces(HeaderCaptions[i]).Contains(td.locator) || HeaderCaptions[i] == td.locator)
                                {
                                    tdIndex = i;
                                    break;
                                }
                            }

                            if (tdIndex == -1)
                                continue;

                            var tdNew = new TableAttribute(td.locator, row[tdIndex]);
                            field.SetValue(tr, tdNew);  // switch old td to new td
                        }
                    }

                    yield return tr;
                }
            }
        }

        protected Table()
        {

        }

        public void SetScope(DriverScope s)
        {
            scope = s;
        }

        public Table(DriverScope b, params string[] locators)
        {
            SetScope(b);
            this.locators = locators;
        }

        public Table(params string[] locators)
        {
            this.locators = locators;
        }

        string[] locators;
        DriverScope scope;

        public void InitMultiple()
        {
            Init();

            for (int i = 1; i < locators.Length; i++)
            {
                var right = new Table<T>(scope, locators[i]);
                right.Init();
                Merge(right);
            }
        }

        public void Init()
        {
            ElementScope table = scope.FindXPath(locators[0]);

            // body
            var tbody = table.FindXPath(".//tbody");
            RowsOfElements = new List<List<ElementScope>>();
            foreach (var tr in tbody.FindAllXPath(".//tr"))
            {
                var row = new List<ElementScope>();
                RowsOfElements.Add(row);
                foreach (var td in tr.FindAllXPath(".//td"))
                    row.Add(td);
            }

            // head
            ElementScope theader;
            IEnumerable<ElementScope> headerLines;
            try
            {
                theader = table.FindAllXPath(".//thead").First();
                headerLines = (IEnumerable<ElementScope>)theader.FindAllXPath(".//tr");
            }
            catch (NoSuchElementException)
            {
                theader = table;
                headerLines = new ElementScope[] { theader.FindAllXPath(".//tr").First() };
                RowsOfElements.RemoveAt(0);
            }

            Header = new List<ElementScope>();
            HeaderCaptions = new List<string>();
            foreach (var tr in headerLines)
            {
                var cells = tr.FindAllXPath(".//th");
                if (cells.Count() > 0)
                {
                    if (Header.Count > 0)
                        throw new Exception($"Too many headers in the table {locators[0]}. Contents: {theader.Text}");

                    foreach (var td in cells)
                    {
                        Header.Add(td);
                        HeaderCaptions.Add(td.Text);
                    }
                }
            }
        }

        public void Merge(Table<T> right)
        {
            if (this.RowsOfElements.Count != right.RowsOfElements.Count)
                throw new Exception("Row count is different in the tables being merged.");

            var result = new Table<T>
            {
                RowsOfElements = new List<List<ElementScope>>(),
                Header = new List<ElementScope>(),
                HeaderCaptions = new List<string>()
            };

            for (int r = 0; r < this.RowsOfElements.Count; r++)
            {
                result.RowsOfElements.Add(new List<ElementScope>());
                foreach (var c in this.RowsOfElements[r])
                    result.RowsOfElements[r].Add(c);
                foreach (var c in right.RowsOfElements[r])
                    result.RowsOfElements[r].Add(c);
            }

            foreach (var c in this.Header)
            {
                result.Header.Add(c);
                result.HeaderCaptions.Add(c.Text);
            }
            foreach (var c in right.Header)
            {
                result.Header.Add(c);
                result.HeaderCaptions.Add(c.Text);
            }

            this.RowsOfElements = result.RowsOfElements;
            this.Header = result.Header;
            this.HeaderCaptions = result.HeaderCaptions;
        }

        static string Beautify(string s)
        {
            string result = s;

            // all caps? return as is
            if (s.All(c => char.IsUpper(c)))
                return s;

            // else, capitalize first letters
            try
            {
                result = s.First().ToString().ToUpper() + s.Substring(1);
                if (result.IndexOf(' ') == -1)
                    result = String.Join(" ", Regex.Split(result, @"(?<!^)(?=[A-Z])"));
            }
            catch { }
            return result;
        }

        static string CapsNoSpaces(string s)
        {
            return Regex.Replace(s.ToLower(), @"[\s-]", x => "");
        }
    }

    public class TableRecord
    {
        public static TableAttribute Find(string locator = null)
        {
            return new TableAttribute(locator);
        }
    }

    public struct TableAttribute
    {
        public ElementScope element;
        public string locator;

        public string Text => element.Text;

        public TableAttribute(string locator, ElementScope e = null)
        {
            this.locator = locator;
            this.element = e;
        }
    }
}