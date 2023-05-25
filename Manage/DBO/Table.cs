using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.DBO
{
    public class Table
    {
        public Table(int id, string name, string status)
        {
            this.ID = id;
            this.Ten = name;
            this.Status = status;
        }

        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Ten = row["Tenban"].ToString();
            this.Status = row["status"].ToString();
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string ten;

        public string Ten
        {
            get { return ten; }
            set { ten = value; }
        }

        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }

}
