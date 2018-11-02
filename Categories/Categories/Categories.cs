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
            private int parenId { get; set; }
            private string Name { get; set; }
            private DateTime Created { get; set; }

            public Category() { }

            public Category(int Id, int parenId, string Name, DateTime Created)
            {
                this.Id = Id;
                this.parenId = parenId;
                this.Name = Name;
                this.Created = Created;
            }

            public int GetId() { return Id; }
            public int GetParentId() { return parenId; }
            public string GetName() { return Name; }
            public DateTime GetCreated() { return Created; }

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        private static int Count;
        static List<Category> catSet = new List<Category>();
        Category emptyItem = new Category(-1, -1, "", DateTime.Now);

        public Categories()
        {
            catSet.Add(new Category(Count++, -1, "cat", DateTime.Now));
            catSet.Add(new Category(Count++, -1, "cat", DateTime.Now));
            catSet.Add(new Category(Count++, 0, "sub-cat", DateTime.Now));
            catSet.Add(new Category(Count++, 1, "sub-cat", DateTime.Now));
            catSet.Add(new Category(Count++, 2, "sub-sub-cat", DateTime.Now));
            catSet.Add(new Category(Count++, 4, "sub-sub-sub-cat", DateTime.Now));

            InitializeComponent();
            
            comboCat.Add(emptyItem);
            FillComboBox("");
            comboBox1.DataSource = comboCat;
            comboBox1.SelectedItem = emptyItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            RecursiveCategoryTree("");
        }

        // Recursive
        void RecursiveCategoryTree(string subMark, int parentId = -1)
        {
            List<Category> query = catSet.Where(x => x.GetParentId() == parentId).ToList();
            if (query.Count > 0)
            {
                foreach (var row in query)
                {
                    richTextBox1.AppendText(subMark + "-" + row.GetName() + "\n");
                    RecursiveCategoryTree(subMark + "  ", row.GetId());
                }
            }
        }

        List<Category> comboCat = new List<Category>();

        private void FillComboBox(string subMark, int parentId = -1)
        {
            List<Category> query = catSet.Where(x => x.GetParentId() == parentId).ToList();
            if (query.Count > 0)
            {
                foreach (var row in query)
                {
                    Category cCat = new Category(row.GetId(), row.GetParentId(), subMark + "-" + row.GetName(), row.GetCreated());
                    comboCat.Add(cCat);
                    FillComboBox(subMark + "  ", row.GetId());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text;
            Category selectedCat = (Category)comboBox1.SelectedItem;
            int parentId = selectedCat.GetId();
            catSet.Add(new Category(Count++, parentId, title, DateTime.Now));

            richTextBox1.Clear();
            RecursiveCategoryTree("");
            comboBox1.DataSource = comboCat;
            textBox1.Text = null;
            comboBox1.SelectedItem = emptyItem;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            comboBox1.SelectedItem = emptyItem;
        }
    }
}
