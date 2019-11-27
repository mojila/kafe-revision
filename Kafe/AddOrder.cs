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
    public partial class AddOrder : Form
    {
        private int total;

        public AddOrder(int total)
        {
            InitializeComponent();
            this.total = total;
        }

        private void AddOrder_Load(object sender, EventArgs e)
        {
            loadCustomer();
            loadTotal();
        }

        private void loadTotal()
        {
            textBox1.Text = total.ToString();
        }

        private void loadCustomer()
        {
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                List<Member> members = database.Members.Where(d => d.role == 2).ToList<Member>();
                comboBox1.DataSource = members;
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "Id";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int cash = int.Parse(textBox2.Text);

            if (cash >= total)
            {
                button2.Enabled = true;
            } else
            {
                button2.Enabled = false;
            }
        }

        public int idCustomer;

        private void button2_Click(object sender, EventArgs e)
        {
            idCustomer = int.Parse(comboBox1.SelectedValue.ToString());

            DialogResult = DialogResult.OK;

            MessageBox.Show("Cashback: Rp. " + (int.Parse(textBox2.Text) - total));

            this.Close();
        }
    }
}
