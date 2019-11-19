using System;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace Kafe
{
    public partial class Form1 : Form
    {
        private UserView loginnedUser;
        private List<BillItem> billItems = new List<BillItem>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // belum perlu login
            //hideAll();
            //showLogin();
            loadMenu();
            initBill();
        }

        private List<MenuView> searchMenu(string keyword)
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<MenuView> list = database.MenuViews.Where(d => d.name.Contains(keyword)).ToList<MenuView>();

                return list;
            }
        }

        private void loadMenu()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<MenuView> list = database.MenuViews.ToList<MenuView>();
                dataGridView1.DataSource = list;
            }
        }

        private void hideAll()
        {
            panel1.Hide();
            panel2.Hide();
            panel3.Hide();
        }

        private void showAll()
        {
            panel1.Show();
            panel2.Show();
            panel3.Show();
        }

        private void showLogin()
        {
            using (Login login = new Login())
            {
                var result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    loginnedUser = login.userView;

                    label1.Text = loginnedUser.name;
                    showAll();
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = searchMenu(textBox1.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void initBill()
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.AllowUserToAddRows = false;
            refreshBill();

            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.Name = "name";
            column1.HeaderText = "Name";
            column1.DataPropertyName = "menu.name";
            dataGridView2.Columns.Add(column1);
        }

        private void refreshBill()
        {
            dataGridView2.DataSource = billItems;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int selected = Convert.ToInt32(dataGridView1.SelectedCells[0].Value);

                using (AddToBill form = new AddToBill(selected))
                {
                    var result = form.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
                        {
                            Menu menu = database.Menus.Where(d => d.Id == selected).FirstOrDefault<Menu>();
                            BillItem newBill = new BillItem();
                            newBill.menu = menu;
                            newBill.quantity = form.quantity;

                            billItems.Add(newBill);

                            refreshBill();
                            MessageBox.Show(newBill.menu.name);
                        }
                    }
                }
            } catch
            {
                MessageBox.Show("No Selected Item.");
            }
        }
    }
}
