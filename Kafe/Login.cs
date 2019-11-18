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
    public partial class Login : Form
    {
        public int id_user { get; set; }
        public UserView userView { get; set; }

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Database2019Entities database = new Database2019Entities())
            {
                user user = database.user.Where(d => d.username == textBox1.Text && d.password == maskedTextBox1.Text)
                    .FirstOrDefault<user>();
                id_user = user.Id;
                userView = database.UserView.Where(d => d.Id == id_user)
                    .FirstOrDefault<UserView>();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
