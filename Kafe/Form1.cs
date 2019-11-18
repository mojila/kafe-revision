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
    public partial class Form1 : Form
    {
        private user_view loginnedUser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hideAll();
            showLogin();
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
                    loginnedUser = login.user_view;

                    label1.Text = loginnedUser.name;
                    showAll();
                }
            }
        }
    }
}
