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
        private List<string> list = new List<string>();

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

        private void refreshBill(BillItem newItem = null)
        {
            try
            {
                dataGridView2.DataSource = null;
                int totalBill = 0;

                if (newItem != null)
                {
                    int billIndex = billItems.FindIndex(d => d.id == newItem.id);
                    if (billIndex != -1)
                    {
                        billItems[billIndex].quantity += newItem.quantity;
                    }
                    else
                    {
                        billItems.Add(newItem);
                    }
                }

                billItems.ForEach(d => {
                    totalBill += d.price * d.quantity;
                });

                dataGridView2.DataSource = billItems;
                textBox2.Text = totalBill.ToString();
                
                if (billItems.Count > 0)
                {
                    button1.Enabled = true;
                } else
                {
                    button1.Enabled = false;
                }
            } catch
            {
                MessageBox.Show("Error");
            }
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
                            newBill.id = menu.Id;
                            newBill.name = menu.name;
                            newBill.quantity = form.quantity;
                            newBill.price = Convert.ToInt32(menu.price);

                            refreshBill(newBill);
                        }
                    }
                }
            } catch
            {
                MessageBox.Show("No Selected Item.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
