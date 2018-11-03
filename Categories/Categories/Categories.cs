using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Categories
{
    public partial class Categories : Form
    {
        class Category
        {
            private int SubLevel { get; set; }
            private List<Category> SubCategories { get; set; }
            private string Name { get; set; }

            public Category() { }

            public Category(int SubLevel, string Name)
            {
                this.SubLevel = SubLevel;
                this.Name = Name;
                SubCategories = new List<Category>();
            }

            public int GetSubLevel() { return SubLevel; }
            public string GetName() { return Name; }
            public List<Category> GetSubCategories() { return SubCategories; }

            public void SetName(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                string subMark = "";
                if (SubLevel >= 0)
                    subMark = new StringBuilder().Insert(0, "  ", SubLevel).ToString();
                return string.Format("{0}", subMark + Name);
            }
        }

        // Set of categories
        static List<Category> catSet = new List<Category>();
        // Empty item to use in dropdown
        Category emptyItem = new Category(-1, "");

        public Categories()
        {
            // Add mocked data
            catSet.Add(new Category(0, "cat1"));
            catSet.Add(new Category(0, "cat2"));
            catSet.Add(new Category(0, "cat3"));
            catSet[0].GetSubCategories().Add(new Category(catSet[0].GetSubLevel() + 1, "sub-cat1"));
            catSet[0].GetSubCategories().Add(new Category(catSet[0].GetSubLevel() + 1, "sub-cat1.2"));
            catSet[1].GetSubCategories().Add(new Category(catSet[1].GetSubLevel() + 1, "sub-cat2"));
            catSet[0].GetSubCategories()[0].GetSubCategories()
                .Add(new Category(catSet[0].GetSubCategories()[0].GetSubLevel() + 1, "sub-sub-cat1"));

            InitializeComponent();
            UpdateComboBox();
        }

        private void recursive_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Recursive\n\n");
            RecursiveCategoryTree(catSet);
        }


        private void iterative_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Iterative (DFS)\n\n");
            IterativeCategoryTree();
        }

        // Recursive
        void RecursiveCategoryTree(List<Category> cat)
        {
            if (cat.Count > 0)
            {
                foreach (Category current in cat)
                {
                    richTextBox1.AppendText(current + "\n");
                    RecursiveCategoryTree(current.GetSubCategories());
                }
            }
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

                    richTextBox1.AppendText(current + "\n");

                    foreach (var neighbour in neighbours.Reverse())
                        stack.Push(neighbour);
                }
            }
        }

        public void UpdateComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(emptyItem);
            FillComboBox(catSet);
            comboBox1.SelectedItem = emptyItem;
        }

        private void FillComboBox(List<Category> cat)
        {
            if (cat.Count > 0)
            {
                foreach (Category current in cat)
                {
                    comboBox1.Items.Add(current);
                    FillComboBox(current.GetSubCategories());
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            List<Category> categorySet = catSet;
            string title = textBox1.Text;
            Category selectedCat = (Category)comboBox1.SelectedItem;
            if (selectedCat != null)
                if (!selectedCat.Equals(emptyItem))
                    categorySet = selectedCat.GetSubCategories();

            Category catToAdd = new Category(selectedCat.GetSubLevel() + 1, title);

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
            richTextBox1.AppendText(catToAdd.GetName() + " added successfully!");
            UpdateComboBox();
            textBox1.Text = null;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            comboBox1.SelectedItem = emptyItem;
        }
    }
}
