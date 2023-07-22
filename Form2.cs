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
using System.Text.RegularExpressions;
//^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$
//@(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
namespace Employee_Management_System
{
    public partial class Form2 : Form
    {
        string emailPattern = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
        //string passwordPattern= @"@(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textID_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textID.Text) == true)
            {
                textID.Focus();
                errorProvider1.SetError(this.textID, "Please Fill ID");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textID_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsDigit(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textName.Text) == true)
            {
                textName.Focus();
                errorProvider2.SetError(this.textName, "Please Fill Name");
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void textName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else if (ch == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textEmail_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textEmail.Text, emailPattern) == false)
            {
                textEmail.Focus();
                errorProvider3.SetError(this.textEmail, "Email Format Not Correct");
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void textPass_Leave(object sender, EventArgs e)
        {
            if (textPass.Text=="")
            {
                textPass.Focus();
                errorProvider4.SetError(this.textPass, "Please Fill Password");
            }
            else
            {
                errorProvider4.Clear();
            }
        }

        private void textRPass_Leave(object sender, EventArgs e)
        {
            if (textRPass.Text != textPass.Text)
            {
                textRPass.Focus();
                errorProvider5.SetError(this.textRPass, "Password Mismatch");
            }
            else
            {
                errorProvider5.Clear();
            }
        }

        private void buttonSignup_Click(object sender, EventArgs e)
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
            else if (Regex.IsMatch(textEmail.Text, emailPattern) == false)
            {
                textEmail.Focus();
                errorProvider3.SetError(this.textEmail, "Email Format Not Correct");
            }
            else if (textPass.Text=="")
            {
                textPass.Focus();
                errorProvider4.SetError(this.textPass, "Please Fill Password");
            }
            else if (textRPass.Text != textPass.Text)
            {
                textRPass.Focus();
                errorProvider5.SetError(this.textRPass, "Password Mismatch");
            }
            else
            {
                string cons = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
                OleDbConnection conn = new OleDbConnection(cons);
                conn.Open();
                string query3 = "select * from SignupTable where id=@id";
                OleDbCommand cmd3 = new OleDbCommand(query3, conn);
                cmd3.Parameters.AddWithValue("@id", textID.Text);
                OleDbDataReader dr = cmd3.ExecuteReader();
                if (dr.HasRows == true)
                {
                    MessageBox.Show(textID.Text + " ID already exists!");
                }
                else
                {
                    string query1 = "insert into LoginTable values(@user,@password)";
                    OleDbCommand cmd = new OleDbCommand(query1, conn);
                    cmd.Parameters.AddWithValue("@user", textEmail.Text);
                    cmd.Parameters.AddWithValue("@password", textPass.Text);
                    cmd.ExecuteNonQuery();

                    string query2 = "insert into SignupTable values(@id,@uname,@email,@password)";
                    OleDbCommand cmd2 = new OleDbCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@id", textID.Text);
                    cmd2.Parameters.AddWithValue("@uname", textName.Text);
                    cmd2.Parameters.AddWithValue("@email", textEmail.Text);
                    cmd2.Parameters.AddWithValue("@password", textPass.Text);
                    int a = cmd2.ExecuteNonQuery();
                    if (a > 0)
                    {
                        MessageBox.Show("User Registered Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 loginForm = new Form1();
                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("User is not Registered", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textID.Clear();
            textName.Clear();
            textEmail.Clear();
            textPass.Clear();
            textRPass.Clear();

        }
    }
}
