using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kafe
{
    public partial class AddToBill : Form
    {
        private int selectedMenu;

        public AddToBill(int id)
        {
            InitializeComponent();

            selectedMenu = id;
        }

        private void AddToBill_Load(object sender, EventArgs e)
        {
            loadRecipe();
        }

        private void loadRecipe()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<Recipe> recipes = database.Recipes.ToList<Recipe>();
                dataGridView1.DataSource = recipes;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
