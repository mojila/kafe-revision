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
        public int quantity = 1;

        public AddToBill(int id)
        {
            InitializeComponent();

            selectedMenu = id;
        }

        private void AddToBill_Load(object sender, EventArgs e)
        {
            loadRecipe();
            loadMenuDetail();
        }

        private void loadMenuDetail()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                Menu menu = database.Menus.Where(d => d.Id == selectedMenu).FirstOrDefault<Menu>();

                label3.Text = menu.name;
                label7.Text = "Rp. " + menu.price;
                textBox1.Text = (menu.price * quantity).ToString();
            }
        }

        private void loadRecipe()
        {
            listBox1.Items.Clear();

            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<Recipe> recipes = database.Recipes.Where(d => d.menu == selectedMenu).ToList<Recipe>();
                List<RecipeObject> recipeObjects = new List<RecipeObject>();

                recipes.ForEach(d => {
                    listBox1.Items.Add(d.Material1.name + " (Need " + (d.material_consume * quantity) + " from " + d.Material1.stock + ") " + (d.Material1.stock >= (d.material_consume * quantity) ? "Enought Material" : "Not Enought Material Stock"));
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            quantity = Convert.ToInt32(numericUpDown1.Value);
            loadRecipe();
            loadMenuDetail();
        }
    }
}
