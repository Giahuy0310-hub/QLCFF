using Manage.DAO;
using Manage.DBO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using static Manage.DBO.Cate;

namespace Manage
{
    public partial class Manager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public Manager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwwitch);
        }
        #region Method
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "ten";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            comboBox1.DataSource = listCategory;
            comboBox1.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            comboBox2.DataSource = listFood;
            comboBox2.DisplayMember = "Name";

        }
        void LoadTable()
        {

            flowLayoutPanel1.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Ten + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "trống":
                        btn.BackColor = Color.SkyBlue;
                        break;
                    default:
                        btn.BackColor = Color.Salmon;
                        break;
                }

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            Bill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            foreach (Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                Bill.Items.Add(lsvItem);
            }

         //   CultureInfo culture = new CultureInfo("vi-VN");

            textBox1.Text = totalPrice.ToString();

            LoadTable(); 
        }
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            Bill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        #endregion

        #region Events
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Profile f = new Profile(loginAccount);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }
        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((comboBox1.SelectedItem as Category).ID);
            if (Bill.Tag != null)
                ShowBill((Bill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((comboBox1.SelectedItem as Category).ID);
            if (Bill.Tag != null)
                ShowBill((Bill.Tag as Table).ID);
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((comboBox1.SelectedItem as Category).ID);
            if (Bill.Tag != null)
                ShowBill((Bill.Tag as Table).ID);
        }
        private void Soluong_ValueChanged(object sender, EventArgs e)
        {

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Manager_Load(object sender, EventArgs e)
        {

        }
        private void Bill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        #endregion


        private void combobox1(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);

        }

        private void Add_Click(object sender, EventArgs e)
        {
            {
                Table table = Bill.Tag as Table;

                if (table == null)
                {
                    MessageBox.Show("Hãy chọn bàn");
                    return;
                }

                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int foodID = (comboBox2.SelectedItem as Food).ID;
                int count = (int)Soluong.Value;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                }

                ShowBill(table.ID);

                LoadTable();
            }

       }

        private void Total_Click(object sender, EventArgs e)
        {
            Table table = Bill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)numericUpDown1.Value;

            double totalPrice = Convert.ToDouble(textBox1.Text);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Ten, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }

        private void Switch_Click_1(object sender, EventArgs e)
        {
            int id1 = (Bill.Tag as Table).ID;

            int id2 = (cbSwwitch.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (Bill.Tag as Table).Ten, (cbSwwitch.SelectedItem as Table).Ten), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }
    }
}