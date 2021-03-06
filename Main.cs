using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AgentObjects;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
namespace Asset_Mng
{
    public partial class Main : Form
    {
        

        public AxAgentObjects.AxAgent agent = new AxAgentObjects.AxAgent();
        //
        public AgentObjects.IAgentCtlCharacter speaker;
        public Main()
        {
            InitializeComponent();
            //AxAgentObjects.AxAgent axAgent1 = new AxAgentObjects.AxAgent();
            
            try
            {
                axAgent1.Characters.Load("Merlin", "C:\\Windows\\MSAgent\\chars\\merlin.acs");
                speaker = axAgent1.Characters["Merlin"];

                speaker.MoveTo(800, 600, null);
                speaker.Show(0);
                speaker.Play("announce");

                speaker.Speak(" سلام", null);
                speaker.MoveTo(100, 100, null);


                speaker.Speak(" هستم روبی من ", null);
                agent = axAgent1;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("invalid");
            }

        }
//        public static SqlConnection connection = new SqlConnection("Data Source=KIM-PC\\SQLEXPRESS;Initial Catalog=Asset_Mng;Integrated Security=True");

        /// <summary>
        /// //
        /// </summary>

        public static SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=D:\\Bsc project\\Kimia's Documents\\After RFID\\Asset_Mng.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");

        private void Main_Load(object sender, EventArgs e)
        {

            connection.Open();
            string com = "select نام FROM نوع";
        //    connection.Open();
            SqlCommand command = new SqlCommand(com, connection);
            SqlDataReader reader = command.ExecuteReader();
            TreeNode node1 = new TreeNode();

            node1.Text = "کل دارایی ها";

            while (reader.Read())
            {
                node1.Nodes.Add(reader["نام"].ToString());

            }
            reader.Close();
            treeView1.Nodes.Add(node1);

            // making grouptree
            com = "select نام FROM واحد";
            command.CommandText = com;
            SqlDataReader reader2 = command.ExecuteReader();


            TreeNode node2 = new TreeNode();
            node2.Text = "کل ساختمان";

            while (reader2.Read())
            {

                node2.Nodes.Add(reader2["نام"].ToString());

            }
            treeView2.Nodes.Add(node2);


            connection.Close();
        }

        private void دستهبندیاصلیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type frm = new Type();
            frm.ShowDialog();
        }

        private void ساختمانToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loc frm = new Loc();
            frm.ShowDialog();
        }

        private void استفادهکنندگانToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User frm = new User();
            frm.ShowDialog();
        }

        private void فروشندگانToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vendor frm = new Vendor();
            frm.ShowDialog();
        }

        private void بهصورتدستیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New_Asset frm = new New_Asset();
            frm.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string temp = treeView1.SelectedNode.Text;
            //  MessageBox.Show(temp);
            dataGridView1.Refresh();
            connection.Open();

            if (treeView1.SelectedNode.IsSelected)
                treeView1.SelectedNode.SelectedImageIndex = 1;
            else
                treeView1.SelectedNode.SelectedImageIndex = 0;

            string com = " ";
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            if (temp == "کل دارایی ها")
                com = "SELECT [کالا].[شناسه],[کالا].[تاریخ_خرید],[کالا].[اتمام_گارانتی],[کالا].[مالکیت],[کالا].[مدل],[کالا].[زیرنوع],[زیر_نوع].[نوع],[فروشنده].[نام_شرکت],[فروشنده].[رابط],[کالا].[وضعیت] FROM کالا inner join زیر_نوع on [کالا].[زیرنوع]=[زیر_نوع].[نام] inner join فروشنده on [کالا].[فروشنده]=[فروشنده].[نام_شرکت] ";
            else if (temp != "کل درایی ها")
            {

                com = "SELECT [کالا].[شناسه],[کالا].[تاریخ_خرید],[کالا].[اتمام_گارانتی],[کالا].[مالکیت],[کالا].[مدل],[کالا].[زیرنوع],[زیر_نوع].[نوع],[فروشنده].[نام_شرکت],[فروشنده].[رابط],[کالا].[وضعیت] FROM کالا inner join زیر_نوع on [کالا].[زیرنوع]=[زیر_نوع].[نام] inner join فروشنده on [کالا].[فروشنده]=[فروشنده].[نام_شرکت] WHERE [زیر_نوع].[نوع]='" + temp + "'";
            }

            command.CommandText = com;
            SqlDataAdapter adapter = new SqlDataAdapter(command.CommandText.ToString(), connection);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "کالا");

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            BindingSource bs = new BindingSource(dt, null);
            dataGridView1.DataSource = bs;
            bindingNavigator1.BindingSource = bs;
            //  dataGridView1.DataSource = dataset.Tables["Asset_search"].DefaultView;



            connection.Close();
        }
        public static string searchid;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                SqlConnection connection = Main.connection;
                string ID = Convert.ToString(row.Cells[0].Value);
                //  MessageBox.Show(ID);

                searchid = ID;
                Info frm = new Info();
                frm.ShowDialog();
            }
        }

        private void ریدرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reader frm = new reader();
            frm.ShowDialog();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public static string report_kind;
        private void کالاToolStripMenuItem_Click(object sender, EventArgs e)
        {

            report_kind = "All-Reports";
            Report frm = new Report();
            frm.ShowDialog();
        }

        private void بررسیمغایرتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compare frm = new compare();
            frm.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result;
                result = MessageBox.Show("آیا میخواهید این دارایی را از پایگاه داده حذف کنید؟ ", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];
                    SqlConnection con = Main.connection;
                    SqlCommand command;
                    string ID = Convert.ToString(row.Cells[0].Value);
                    if (ID.Length != 0)
                    {
                        MessageBox.Show(ID);
                        //  string delete0 = "DELETE FROM [تاریخچه] WHERE [شناسه]= '" + ID + "';";
                        string delete1 = "DELETE FROM [کالا] WHERE [شناسه]= '" + ID + "';";
                        con.Open();
                        command = new SqlCommand(delete1, con);
                        command.ExecuteNonQuery();
                        
                        con.Close();
                    }
                    else
                        MessageBox.Show("DataBase is empty");
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            New_Asset frm = new New_Asset();
            frm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string temp = treeView2.SelectedNode.Text;

            dataGridView1.Refresh();
            SqlConnection connection = Main.connection;
            connection.Open();
            if (treeView2.SelectedNode.IsSelected)
                treeView2.SelectedNode.SelectedImageIndex = 1;
            else
                treeView2.SelectedNode.SelectedImageIndex = 0;

            string com = "";
            if (temp == "کل ساختمان")
                com = "SELECT * from V1;";
            else if (temp != "کل واحدها")
            {
                com = "SELECT * from V1 where واحد='" + temp + "';";
            }
            SqlCommand command = new SqlCommand(com, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command.CommandText.ToString(), connection);
            DataTable dt = new DataTable();
           
            adapter.Fill(dt);
            BindingSource bs = new BindingSource(dt, null);
            dataGridView1.DataSource = bs;
            bindingNavigator1.BindingSource = bs;
            connection.Close();
        }
        public static string id;
        private void button1_Click(object sender, EventArgs e)
        {
             
     
            id = textBox1.Text;
         
            fullsearch frm = new fullsearch();
            frm.ShowDialog();
        }

        private void باکمکپایانهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subsearch frm = new subsearch();
            frm.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}