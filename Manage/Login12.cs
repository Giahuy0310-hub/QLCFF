using Manage.DAO;
using Manage.DBO;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Manage
{
    public partial class Login12 : Form
    {
        public Login12()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //tắt chương trình
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
      
            string userName = textBox1.Text;
            string passWord = textBox2.Text;
            if (Login(userName, passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                Manager f = new Manager(loginAccount);
                MessageBox.Show(" Đăng nhập thành công ");
                this.Hide();
                f.ShowDialog(); //top
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }
        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}