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
using System.Data.Common;
using System.IO;

namespace LoadCSV
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        ConDB db = new ConDB();
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(db.GetConnection());
           
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            BindDataCSV(textBox1.Text);
            
           
        }
        private void BindDataCSV(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
           
            if (lines.Length > 0)
            {
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                for (int r = 1; r < lines.Length; r++)
                {
                    string[] dataWords = lines[r].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
               


                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    
                    con.Open();

                    cmd = new SqlCommand("Insert into student(ExternalStudentID,FirstName,LastName,DOB,SSN,Address,City,State,Email,MaritalSatus) values(@ExternalStudentID,@FirstName,@LastName,@DOB,@SSN,@Address,@City,@State,@Email,@MaritalSatus)", con);
                    cmd.Parameters.AddWithValue("@ExternalStudentID", dataGridView1.Rows[i].Cells[0].Value);
                    cmd.Parameters.AddWithValue("@FirstName", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@LastName", dataGridView1.Rows[i].Cells[2].Value.ToString());
                    cmd.Parameters.AddWithValue("@DOB", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    cmd.Parameters.AddWithValue("@SSN", dataGridView1.Rows[i].Cells[4].Value);
                    cmd.Parameters.AddWithValue("@Address", dataGridView1.Rows[i].Cells[5].Value.ToString());
                    cmd.Parameters.AddWithValue("@City", dataGridView1.Rows[i].Cells[6].Value.ToString());
                    cmd.Parameters.AddWithValue("@State", dataGridView1.Rows[i].Cells[7].Value.ToString());
                    cmd.Parameters.AddWithValue("@Email", dataGridView1.Rows[i].Cells[8].Value.ToString());
                    cmd.Parameters.AddWithValue("@MaritalSatus", dataGridView1.Rows[i].Cells[9].Value.ToString());

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Records Saved Successfully.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                


            }
             
            catch (SqlException ex)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    
                    MessageBox.Show("The Same Value Already Exists-Try giving Different Values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if(con.State==ConnectionState.Open)
                    con.Close() ;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            //SqlCommand cmd = new SqlCommand("select * from student where ExternalStudentID=@ExternalStudentID", con);

            SqlCommand cmd = new SqlCommand("select * from student where FirstName+LastName+DOB+cast(SSN as varchar(50)) in" +
                "(select FirstName+LastName+DOB+cast(SSN as varchar(50)) as keyy from student where ExternalStudentID=@ExternalStudentID )", con);
            cmd.Parameters.AddWithValue("ExternalStudentID",textBox2.Text);
            SqlDataAdapter da= new SqlDataAdapter();
            da.SelectCommand= cmd;
            DataTable dt= new DataTable();
            //dt.Clear();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
        }
    }
}

