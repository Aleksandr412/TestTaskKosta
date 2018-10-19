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

namespace TestTaskKosta2
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source = .\SQLEXPRESS; Database=TestDB; Trusted_Connection=True;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT DISTINCT Name FROM Department";
            command.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cmbDep.Items.Add(dr["Name"].ToString());
            }
            connection.Close();
        }

        //Структура организации
        private void btnShowOrgStruct_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT DISTINCT Name, Position FROM Department, Employee";
            command.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        //Сотрудник выбранного отдела
        private void ShowDeptEmpl_Click(object sender, EventArgs e)
        {
            if (cmbDep.SelectedItem != null)
            {
                string some_str = cmbDep.SelectedItem.ToString();

                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT FirstName, Surname, Patronymic, Position, DateOfBirth FROM Employee WHERE" +
                    " DepartmentID IN (SELECT ID FROM Department WHERE Name = '" + cmbDep.SelectedItem.ToString() + "')";
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                connection.Close();
            }
            else
            {
                MessageBox.Show("Сначала выберите отдел");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    var today = DateTime.Now;

                    tbFirstName.Text = row.Cells["FirstName"].Value.ToString();
                    tbSurName.Text = row.Cells["SurName"].Value.ToString();
                    tbPatronymic.Text = row.Cells["Patronymic"].Value.ToString();
                    tbPosition.Text = row.Cells["Position"].Value.ToString();
                    var dob = Convert.ToDateTime(row.Cells["DateOfBirth"].Value.ToString());
                    int years = today.Year - dob.Year;

                    if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day))
                    {

                        years = years - 1;
                        int intCustomerAge = years;
                        tbAge.Text = Convert.ToString(intCustomerAge);
                    }
                    else
                    {
                        tbAge.Text = Convert.ToString(years);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
