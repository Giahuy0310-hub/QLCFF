using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.DBO
{
    public class Cate
    {
        public int id { get; internal set; }

        public class Category
        {
            public Category(int id, string name)
            {
                this.ID = id;
                this.Name = name;
            }

            public Category(DataRow row)
            {
                this.ID = (int)row["id"];
                this.Name = row["tenloai"].ToString();
            }

            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private int iD;

            public int ID
            {
                get { return iD; }
                set { iD = value; }
            }
        }
    }
}
