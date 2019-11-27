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
        private List<BillItem> billItems;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            billItems = new List<BillItem>();
            // belum perlu login
            //hideAll();
            //showLogin();
            loadMenu();
            loginnedUser = new UserView();
            loginnedUser.Id = 1;
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

        private void refreshIncome()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<Order> orders = database.Orders.ToList<Order>();
                int total = 0;

                if (orders.Count > 0)
                {
                    orders.ForEach(d => total += (int)d.total);
                }

                label10.Text = total.ToString();
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
                dataGridView4.DataSource = null;
                int totalBill = 0;

                if (newItem != null)
                {
                    //int billIndex = billItems.FindIndex(d => d.id == newItem.id);
                    //if (billIndex >= 0)
                    //{
                    //  billItems[billIndex].quantity += newItem.quantity;
                    //}
                    //else
                    //{

                    //}
                    billItems.Add(newItem);
                }

                billItems.ForEach(d => {
                    totalBill += d.price * d.quantity;
                });

                dataGridView4.DataSource = billItems;
                dataGridView4.Refresh();
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
            addBillItem();
        }

        private void addBillItem()
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
                            List<Recipe> recipes = database.Recipes.Where(d => d.Id == menu.Id).ToList<Recipe>();

                            recipes.ForEach(d =>
                            {
                                List<Material> materials = database.Materials.Where(da => da.Id == d.material)
                                    .ToList<Material>();
                                materials.ForEach(da => da.stock = da.stock - d.material_consume * form.quantity);

                                database.SaveChanges();
                            });

                            int index = billItems.FindIndex(d => d.id == menu.Id);

                            if (index == -1)
                            {
                                BillItem newBill = new BillItem();
                                newBill.id = menu.Id;
                                newBill.name = menu.name;
                                newBill.quantity = form.quantity;
                                newBill.price = Convert.ToInt32(menu.price);

                                billItems.Add(newBill);
                            }
                            else
                            {
                                billItems[index].quantity += form.quantity;
                                billItems[index].price = billItems[index].quantity * (int) menu.price;
                            }


                            refreshBillFix();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("No Selected Item.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int total = int.Parse(textBox2.Text);

            using (AddOrder addOrder = new AddOrder(total))
            {
                var result = addOrder.ShowDialog();
                int idCustomer;

                if (result == DialogResult.OK)
                {
                    idCustomer = addOrder.idCustomer;

                    Order order = new Order();
                    order.member = idCustomer;
                    order.cashier = loginnedUser.Id;
                    order.status = 0;
                    order.total = total;
                    order.comment = "";
                    order.date = dateTimePicker1.Value;

                    createOrder(order);
                }
            }
        }

        private void refreshOrder()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                dataGridView3.DataSource = null;

                List<OrderData> orderDatas = new List<OrderData>();
                List<Order> orders = database.Orders.Where(d => d.status == 0).ToList<Order>();

                orders.ForEach(d =>
                {
                    OrderData orderData = new OrderData();
                    orderData.id = d.Id;

                    Member member = database.Members.Where(e => e.Id == d.member).FirstOrDefault();
                    orderData.customerName = member.name;
                    orderData.date = (DateTime) d.date;
                    orderData.totalBill = (int) d.total;

                    orderDatas.Add(orderData);
                });

                dataGridView3.DataSource = orderDatas;
                refreshIncome();
            }
        }

        private void refreshBillFix()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                dataGridView4.DataSource = null;
                int total = 0;

                List<BillItem> newBills = new List<BillItem>();
                billItems.ForEach(d =>
                {
                    BillItem billItem = new BillItem();
                    billItem.id = d.id;
                    billItem.name = d.name;
                    billItem.quantity = d.quantity;
                    billItem.price = d.price;

                    newBills.Add(billItem);
                    total += d.price;
                });

                dataGridView4.DataSource = newBills;
                textBox2.Text = total.ToString();

                if (newBills.Count > 0)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                } else
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                }
            }
        }

        private void createOrder(Order order)
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                try
                {
                    database.Orders.Add(order);
                    Order order1 = new Order();
                    database.SaveChanges();

                    //billItems.ForEach(d =>
                    //{
                    //  OrderItem orderItem = new OrderItem();
                    //orderItem.menu = d.id;
                    //orderItem.order = order1.Id;
                    //orderItem.quantity = d.quantity;

                    //database.OrderItems.Add(orderItem);
                    //database.SaveChanges();
                    //});
                    billItems = new List<BillItem>();
                    refreshBillFix();
                    refreshOrder();
                }
                catch
                {
                    MessageBox.Show("Failed!");
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedBillItemId = int.Parse(dataGridView4.SelectedCells[0].Value.ToString());
                BillItem item = billItems.Where(d => d.id == selectedBillItemId).FirstOrDefault();

                using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
                {
                    List<Recipe> recipes = database.Recipes.Where(d => d.menu == selectedBillItemId).ToList<Recipe>();

                    recipes.ForEach(d =>
                    {
                        List<Material> materials = database.Materials.Where(da => da.Id == d.material).ToList<Material>();
                        materials.ForEach(res => res.stock += d.material_consume * item.quantity);

                        database.SaveChanges();
                    });
                }

                billItems.Remove(item);
                refreshBillFix();
                MessageBox.Show("Bill Item Canceled!");
            } catch
            {
                MessageBox.Show("Please select bill item first.");
            }
        }
    }
}
