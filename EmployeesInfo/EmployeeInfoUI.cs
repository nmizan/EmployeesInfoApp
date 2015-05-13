using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EmployeesInfo
{
    public partial class EmployeeInfoUI : Form
    {
        public EmployeeInfoUI()
        {
            InitializeComponent();
            ShowAllEmployee();
        }

        private string connetionString = ConfigurationManager.ConnectionStrings["EmployeesDB"].ConnectionString;
        private bool isUpdateMode = false;
      
        private int employeeId;
       
        private void saveButton_Click(object sender, EventArgs e)
        {

          string name = nameTextBox.Text;
          string address = addressTextBox.Text;
          string email = emailTextBox.Text;
         float salary = float.Parse(salaryTextBox.Text);

            if (IsEmailExists(email))
            {
                MessageBox.Show("Email is already exist");
            }
            else
            {
                SqlConnection connection = new SqlConnection(connetionString);

                string query = "INSERT INTO tblemployees Values('" + name + "','" + email + "','" +
                               address + "','" + salary + "')";


                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Inserted Successfully");
                    ShowAllEmployee();
                }
                else
                {
                    MessageBox.Show("Insertion Failed");

                }

            }

                nameTextBox.Clear();
                addressTextBox.Clear();
                emailTextBox.Clear();
                salaryTextBox.Clear();


            
        }


        public bool IsEmailExists  (string email)
            {


                SqlConnection connection = new SqlConnection(connetionString);

               
                string query = "SELECT Email FROM tblemployees Where Email='" + email + "' ";
                bool isEmailExists = false;
          
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    isEmailExists = true;
                    break;
                }
                reader.Close();
                connection.Close();
                return isEmailExists;

            }

        public void LoadEmployeeListView(List<Employee> employees)
        {
             employeeListView.Items.Clear();
            foreach (var Employee in employees)
               
            {
                ListViewItem item = new ListViewItem(Employee.ID.ToString());
                item.SubItems.Add(Employee.name);
                item.SubItems.Add(Employee.email);
                item.SubItems.Add(Employee.salary.ToString());
                item.SubItems.Add(Employee.address);

                employeeListView.Items.Add(item);

            }
            
        }

        public void ShowAllEmployee()
        {
            SqlConnection connection = new SqlConnection(connetionString);


            string query = "SELECT * FROM tblemployees  ";
            
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee>employeeList=new List<Employee>();
            while (reader.Read())
            {
                Employee aEmployee=new Employee();
                aEmployee.ID = int.Parse(reader["ID"].ToString());
                aEmployee.name = reader["Name"].ToString();
                aEmployee.email= reader["Email"].ToString();
                aEmployee.salary= float.Parse(reader["Salary"].ToString());
                aEmployee.address = reader["Address"].ToString();

                employeeList.Add(aEmployee);

            }
            reader.Close();
            connection.Close();
            LoadEmployeeListView(employeeList);
           }

        private void updateButton_Click(object sender, EventArgs e)
        {

            string name = nameTextBox.Text;
            string address = addressTextBox.Text;
            string email = emailTextBox.Text;
            float salary = float.Parse(salaryTextBox.Text);

            if (isUpdateMode)
           
            {
                if (IsEmailExists(email))
                {
                    MessageBox.Show("Email is already exist");
                }
                else
                {
                   

                    SqlConnection connection = new SqlConnection(connetionString);

                    string query = "UPDATE tblemployees SET Name='" + name + "', Email='" + email + "', Address='" +
                                   address + "', Salary='" + salary + "' WHERE ID = '" + employeeId + "'";


                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Updated Successfully");
                        employeeId = 0;
                        isUpdateMode = false;
                        //emailTextBox.Enabled = false;
                        ShowAllEmployee();
                    }
                    else
                    {
                        MessageBox.Show("Not Updated");

                    }

                }

                nameTextBox.Clear();
                addressTextBox.Clear();
                emailTextBox.Clear();
                salaryTextBox.Clear();


                    
                }
                
            
        }

        private void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = employeeListView.SelectedItems[0];
            int id = int.Parse(item.Text.ToString());
            Employee employee = GetEmployeeByID(id);

            if (employee != null)
            {

                isUpdateMode = true;
                

                employeeId = employee.ID;
                //emailTextBox.Enabled = false;


                nameTextBox.Text = employee.name;
                emailTextBox.Text = employee.email;
                salaryTextBox.Text = employee.salary.ToString();
                addressTextBox.Text = employee.address;
            }

        }


        public Employee GetEmployeeByID(int id)
        {
            SqlConnection connection = new SqlConnection(connetionString);


            string query = "SELECT * FROM tblemployees WHERE ID='"+id+"' ";
            
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee>employeeList=new List<Employee>();
            while (reader.Read())
            {
                Employee aEmployee=new Employee();
                aEmployee.ID = int.Parse(reader["ID"].ToString());
                aEmployee.name = reader["Name"].ToString();
                aEmployee.email= reader["Email"].ToString();
                aEmployee.salary= float.Parse(reader["Salary"].ToString());
                aEmployee.address = reader["Address"].ToString();

                employeeList.Add(aEmployee);

            }
            reader.Close();
            connection.Close();
            return employeeList.FirstOrDefault();

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            var confirmationResult = MessageBox.Show("Are you want to delete this item ??", "Confirm Delete ??", MessageBoxButtons.YesNo);

            //string name = nameTextBox.Text;
            //string address = addressTextBox.Text;
            //string email = emailTextBox.Text;
            //float salary = float.Parse(salaryTextBox.Text);

            

           if (confirmationResult == DialogResult.Yes)
            {
                
                    SqlConnection connection = new SqlConnection(connetionString);

                    string query = "DELETE FROM tblemployees WHERE ID ='" + employeeId + "'";


                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Deleted Successfully");
                        
                        //emailTextBox.Enabled = false;
                        ShowAllEmployee();
                    }
                    else
                    {
                        MessageBox.Show("Deletion is failed!");

                    }
            }

            else
            {
               MessageBox.Show(" Item Is Not Deleted");
            }
            nameTextBox.Clear();
            addressTextBox.Clear();
            emailTextBox.Clear();
            salaryTextBox.Clear();


        }
    }
}
                

               
           
                   
           

                

               
                
  

    




 
            
            

