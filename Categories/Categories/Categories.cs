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
        }

        private static int Count;
        static List<Category> cat = new List<Category>();

        public Categories()
        {
            InitializeComponent();

            cat.Add(new Category(Count++, -1, "Parent Category", DateTime.Now));
            cat.Add(new Category(Count++, -1, "Parent Category", DateTime.Now));
            cat.Add(new Category(Count++, 0, "Sub Category", DateTime.Now));
            cat.Add(new Category(Count++, 1, "Sub Category", DateTime.Now));
            cat.Add(new Category(Count++, 2, "Sub Sub Category", DateTime.Now));
            cat.Add(new Category(Count++, 4, "Sub Sub Category", DateTime.Now));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            RecursiveCategoryTree("");
        }

        // Recursive
        void RecursiveCategoryTree(string subMark, int parentId = -1)
        {
            List<Category> query = cat.Where(x => x.GetParentId() == parentId).ToList();
            if (query.Count > 0)
            {
                foreach (var row in query)
                {
                    richTextBox1.AppendText(subMark + "- " + row.GetName() + "\n");
                    RecursiveCategoryTree(subMark + "   ", row.GetId());
                }
            }
        }
    }
}
