using System;
using System.Data;
using System.Linq;
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
            using (Database2019EntitiesRevision database = new Database2019EntitiesRevision())
            {
                Member member = database.Members.Where(d => d.username == textBox1.Text && d.password == maskedTextBox1.Text)
                    .FirstOrDefault<Member>();
                id_user = member.Id;
                userView = database.UserViews.Where(d => d.Id == id_user)
                    .FirstOrDefault<UserView>();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
