using Manage.DAO;
using Manage.DBO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Manage.DBO.Cate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Manage
{
    public partial class Admin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();


        public Admin()
        {
            InitializeComponent();
            Load();
        }


        void Load()
        {
            dataGridView2.DataSource = foodList;
            dataGridView3.DataSource = accountList;

            LoadListFood();
            LoadAcc();
            LoadAccount();
            AddFoodBinding();
            AddAccountBinding();
            LoadCategoryIntoCombobox(CxCate);
        }
        void AddAccountBinding()
        {
            textBox5.DataBindings.Add(new Binding("Text", dataGridView3.DataSource, "Tài khoản", true, DataSourceUpdateMode.Never));
            textBox4.DataBindings.Add(new Binding("Text", dataGridView3.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dataGridView3.DataSource, "Loại", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void LoadAcc()
        {
            string query = " select ac.Taikhoan as N'Tài khoản', ac.Tenhienthi as N'Tên hiển thị', ac.Loai as N'Loại' from account ac";

            DataProvider provider = new DataProvider();

            dataGridView3.DataSource = provider.ExecuteQuery(query);
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        private void FoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedCells.Count > 0)
                {
                    int id = (int)dataGridView2.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    CxCate.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in CxCate.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    CxCate.SelectedIndex = index;
                }
            }
            catch { }
        }
 
        //private void FoodID(object sender, EventArgs e)
        //{

        //}

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dataGridView5.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        #region Food

        void AddFoodBinding()
        {
            FoodName.DataBindings.Add(new Binding("Text", dataGridView2.DataSource, "Name", true, DataSourceUpdateMode.Never));
            FoodID.DataBindings.Add(new Binding("Text", dataGridView2.DataSource, "ID", true, DataSourceUpdateMode.Never));
            FoodPrice.DataBindings.Add(new Binding("Value", dataGridView2.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        private void ViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void AddFood_Click(object sender, EventArgs e)
        {
            string name = FoodName.Text;
            int categoryID = (CxCate.SelectedItem as Category).ID;
            float price = (float)FoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

        private void DDeleteFood_Click(object sender, EventArgs e)
        {
            {
                int id = Convert.ToInt32(FoodID.Text);

                if (FoodDAO.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công");
                    LoadListFood();
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa thức ăn");

                }
            }
        }

        private void EditFood_Click(object sender, EventArgs e)
        {
            {
                string name = FoodName.Text;
                int categoryID = (CxCate.SelectedItem as Category).ID;
                float price = (float)FoodPrice.Value;
                int id = Convert.ToInt32(FoodID.Text);

                if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
                {
                    MessageBox.Show("Sửa món thành công");
                    LoadListFood();
                    if (updateFood != null)
                        updateFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa thức ăn");
                }
            }
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void FindFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(SearchName.Text);
        }

        private void Food_Click(object sender, EventArgs e)
        {

        }
        #endregion



        #region Acc
        void DeleteAccount(string userName)
        {
          
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAcc();
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAcc();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAcc();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        private void AddAcc_Click(object sender, EventArgs e)
        {
            string userName = textBox5.Text;
            string displayName = textBox5.Text;
            int type = (int)numericUpDown1.Value;

            AddAccount(userName, displayName, type);
        }

        private void DeleteAcc_Click(object sender, EventArgs e)
        {
            string userName = textBox5.Text;

            DeleteAccount(userName);
        }

        private void EditAcc_Click(object sender, EventArgs e)
        {
            string userName = textBox5.Text;
            string displayName = textBox4.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }

        private void ViewAcc_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void ResetAcc_Click(object sender, EventArgs e)
        {
            string userName = textBox5.Text;

            ResetPass(userName);
        }
        #endregion

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

