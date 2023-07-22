using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;

namespace Employee_Management_System
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textID.Text) == true)
            {
                textID.Focus();
                errorProvider1.SetError(this.textID, "Please Fill ID");
            }
            else if (string.IsNullOrEmpty(textName.Text) == true)
            {
                textName.Focus();
                errorProvider2.SetError(this.textName, "Please Fill Name");
            }
            else if (string.IsNullOrEmpty(textAge.Text)==true)
            {
                textAge.Focus();
                errorProvider3.SetError(this.textAge, "Please Fill Size");
            }
            else if (textDes.Text == "")
            {
                textDes.Focus();
                errorProvider4.SetError(this.textDes, "Please Fill Quantity");
            }
            else if (textSalary.Text =="")
            {
                textSalary.Focus();
                errorProvider5.SetError(this.textSalary, "Please Fill Price");
            }
            else
            {
                errorProvider1.Clear();
                errorProvider2.Clear();
                errorProvider3.Clear();
                errorProvider4.Clear();
                errorProvider5.Clear();
                string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
                OleDbConnection conn = new OleDbConnection(cons);
                conn.Open();
                string query1 = "select * from EmpRecordsTable where id=@id";
                OleDbCommand cmd1 = new OleDbCommand(query1, conn);
                cmd1.Parameters.AddWithValue("@id", textID.Text);
                OleDbDataReader dr = cmd1.ExecuteReader();
                if (dr.HasRows == true)
                {
                    MessageBox.Show("Item with "+textID.Text + " ID already added!");
                }
                else
                {
                    string query2 = "insert into EmpRecordsTable values(@id,@uname,@age,@designation,@salary)";
                    OleDbCommand cmd2 = new OleDbCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@id", textID.Text);
                    cmd2.Parameters.AddWithValue("@uname", textName.Text);
                    cmd2.Parameters.AddWithValue("@age", textAge.Text);
                    cmd2.Parameters.AddWithValue("@designation", textDes.Text);
                    cmd2.Parameters.AddWithValue("@salary", textSalary.Text);
                    int a = cmd2.ExecuteNonQuery();
                    if (a > 0)
                    {
                        MessageBox.Show("Item Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        display();
                        reset();
                    }
                    else
                    {
                        MessageBox.Show("Item is not added", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        void reset()
        {
            textID.Clear();
            textName.Clear();
            textAge.Clear();
            textDes.Clear();
            textSalary.Clear();
        }

        void display()
        {
            string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(cons);
            conn.Open();
            string query = "select * from EmpRecordsTable";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            display();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                return;
            textID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textAge.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textDes.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textSalary.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(cons);
            conn.Open();
            string query = "update EmpRecordsTable set id=@id, uname=@uname, age=@age, designation=@des, salary=@salary where id=@id";
            OleDbCommand cmd2 = new OleDbCommand(query, conn);
            cmd2.Parameters.AddWithValue("@id", textID.Text);
            cmd2.Parameters.AddWithValue("@uname", textName.Text);
            cmd2.Parameters.AddWithValue("@age", textAge.Text);
            cmd2.Parameters.AddWithValue("@des", textDes.Text);
            cmd2.Parameters.AddWithValue("@salary", textSalary.Text);
            int a = cmd2.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                display();
                reset();
            }
            else
            {
                MessageBox.Show("Item is not updated", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(cons);
            conn.Open();
            string query = "delete from EmpRecordsTable where id=@id";
            OleDbCommand cmd2 = new OleDbCommand(query, conn);
            cmd2.Parameters.AddWithValue("@id", textID.Text);
            int a = cmd2.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                display();
                reset();
            }
            else
            {
                MessageBox.Show("Item is not deleted", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
             string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
             OleDbConnection conn = new OleDbConnection(cons);
             conn.Open();
             string query = "select * from EmpRecordsTable where uname like @uname + '%'";
             OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
             da.SelectCommand.Parameters.AddWithValue("@uname", textSearch.Text);
             DataTable dt = new DataTable();
             da.Fill(dt);
             if (dt.Rows.Count > 0)
             {
                 string message = "ID\tName\tSize\tQuantity\tPrice\tTotal Price\n";
                 foreach (DataRow row in dt.Rows)
                 {
                     string id = row["ID"].ToString();
                     string name = row["uname"].ToString();
                     string size = row["Age"].ToString();
                     string qty = row["Designation"].ToString();
                     string prc = row["Salary"].ToString();
                     int tp1 = Int32.Parse(qty);
                     int tp2 = Int32.Parse(prc);
                     string totalPrice = (tp1 * tp2).ToString();
             
                     message += $"{id}\t{name}\t{size}\t{qty}\t{prc}\t{totalPrice}\n";
                 }
                 MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             else
             {
                 MessageBox.Show("No Items Found With Your Search", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 dataGridView1.DataSource = null;
             }

        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(cons);
            conn.Open();
            string query = "select * from EmpRecordsTable where uname like @uname + '%'";
            OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@uname", textSearch.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No Item Found With Your Search", "failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataGridView1.DataSource = null;
            }
        }

        private void exitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
