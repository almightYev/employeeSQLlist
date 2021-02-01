using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmployeeSystem
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-G2ISOTR;Initial Catalog=employeeSystem;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowEmployees()
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "select * from employeesDB";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = reader["employeeName"].ToString();
                    item.SubItems.Add(reader["employeeSurname"].ToString());
                    item.SubItems.Add(reader["employeeGender"].ToString());
                    item.SubItems.Add(reader["employeeRole"].ToString());

                    listView1.Items.Add(item);
                }
                connection.Close();
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "insert into employeesDB (employeeName, employeeSurname, employeePhone, employeeAddress, employeeGender, employeeRole) values ('" + txtboxName.Text + "', '" + txtboxSurname.Text + "', '" + txtboxPhone.Text + "', '" + txtboxAddress.Text + "', '" + cbGender.Text + "', '" + cbRole.Text + "')";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                connection.Close();

                MessageBox.Show("New Employee has successfully registered.");
                listView1.Items.Clear();
                ShowEmployees();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowEmployees();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "delete from employeesDB where employeeName = @employeeName";
                    cmd.Parameters.AddWithValue("@employeeName", listView1.SelectedItems[0].Text);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    connection.Close();
                    listView1.Items.Clear();
                    ShowEmployees();
                    MessageBox.Show("Employees was successfully deleted.");
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtboxName.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtboxSurname.Text = listView1.SelectedItems[0].SubItems[1].Text;
            cbGender.Text = listView1.SelectedItems[0].SubItems[2].Text;
            cbRole.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "update employeesDB set employeeName='"+txtboxName.Text+"', employeeSurname='"+txtboxSurname.Text+"', employeeGender='"+cbGender.Text+"', employeeRole='"+cbRole.Text+"' where employeeName=@id";
                cmd.Parameters.AddWithValue("@id", listView1.SelectedItems[0].Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                connection.Close();
                listView1.Items.Clear();
                ShowEmployees();
            }
        }
    }
}
