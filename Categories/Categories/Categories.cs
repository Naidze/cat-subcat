using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Categories
{
    public partial class Categories : Form
    {
        class Category
        {
            private int Id { get; set; }
            private List<Category> SubCategories { get; set; }
            private string Name { get; set; }

            public Category() { }

            public Category(int Id, string Name)
            {
                this.Id = Id;
                this.Name = Name;
                SubCategories = new List<Category>();
            }

            public int GetId() { return Id; }
            //public int GetParentId() { return parenId; }
            public string GetName() { return Name; }
            public List<Category> GetSubCategories() { return SubCategories; }

            public void SetName(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        private static int Count;
        static List<Category> catSet = new List<Category>();
        Category emptyItem = new Category(-1, "");

        public Categories()
        {
            catSet.Add(new Category(Count++, "cat1"));
            catSet.Add(new Category(Count++, "cat2"));
            catSet.Add(new Category(Count++, "cat3"));
            catSet[0].GetSubCategories().Add(new Category(Count++, "sub-cat1"));
            catSet[0].GetSubCategories().Add(new Category(Count++, "sub-cat1.2"));
            catSet[1].GetSubCategories().Add(new Category(Count++, "sub-cat2"));
            catSet[0].GetSubCategories()[0].GetSubCategories().Add(new Category(Count++, "sub-sub-cat1"));

            InitializeComponent();
            UpdateComboBox();
        }

        private void recursive_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Recursive\n\n");
            RecursiveCategoryTree("", catSet);
        }


        private void iterative_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Iterative (DFS)\n\n");
            IterativeCategoryTree();
        }

        // Iterative Printing (DFS)
        void IterativeCategoryTree()
        {
            foreach (Category parent in catSet)
            {
                var visited = new HashSet<Category>();
                var stack = new Stack<Category>();
                stack.Push(parent);
                string subMark = "";
                while (stack.Count != 0)
                {
                    var current = stack.Pop();
                    visited.Add(current);
                    var neighbours = current.GetSubCategories().Where(node => !visited.Contains(node));

                    richTextBox1.AppendText(subMark + current.GetName() + "\n");
                    subMark += "  ";
                    foreach (var neighbour in neighbours.Reverse())
                    {
                        stack.Push(neighbour);
                    }
                }
            }
        }

        // Recursive
        void RecursiveCategoryTree(string subMark, List<Category> cat)
        {
            if (cat.Count > 0)
            {
                foreach (var row in cat)
                {
                    richTextBox1.AppendText(subMark + row.GetName() + "\n");
                    RecursiveCategoryTree(subMark + "  ", row.GetSubCategories());
                }
            }
        }

        public void UpdateComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(emptyItem);
            FillComboBox("", catSet);
            comboBox1.SelectedItem = emptyItem;
        }

        private void FillComboBox(string subMark, List<Category> cat)
        {
            if (cat.Count > 0)
            {
                foreach (var row in cat)
                {
                    comboBox1.Items.Add(row);
                    FillComboBox(subMark + "  ", row.GetSubCategories());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Category> categorySet = catSet;
            string title = textBox1.Text;
            Category selectedCat = (Category)comboBox1.SelectedItem;
            if (selectedCat != null)
                if (!selectedCat.Equals(emptyItem))
                    categorySet = selectedCat.GetSubCategories();

            Category catToAdd = new Category(Count++, title);

            if (!textBox1.Text.Equals(""))
            {
                label4.Visible = false;
                categorySet.Add(catToAdd);
            }
            else
            {
                label4.Visible = true;
                return;
            }

            richTextBox1.Clear();
            richTextBox1.AppendText(catToAdd + " added successfully!");
            UpdateComboBox();
            textBox1.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //    textBox1.Text = null;
            //    comboBox1.SelectedItem = emptyItem;
        }
    }
}
