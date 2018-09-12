using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            AcceptButton = button2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySql.Reader("select * from t_user where username= 'admin'");
            Function.Pesan.Info(MySql.ReaderData[3]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Function.CekTextboxComboBox(this, errorProvider1);
            this.Text = Function.KOSONGTEXT.ToString() + " " + Function.KOSONGKOMBO.ToString(); 
            if ((Function.KOSONGTEXT == false) & (Function.KOSONGKOMBO == false))
            {
                MySql.QUERY = "SELECT * FROM t_user WHERE username='" + MySql.EscapeString(input_user.Text) + "'";
                MySql.Reader(MySql.QUERY);
                if (MySql.ReaderHasRow == true)
                {
                    if (input_md5_pass.Text == MySql.ReaderData[2])
                    {
                        Function.USERDATA[0] = MySql.ReaderData[1];
                        Function.USERDATA[1] = MySql.ReaderData[3];
                        Function.USERDATA[2] = MySql.ReaderData[4];
                        Function.ClearTextBoxComboBox(this);
                        Function.Pesan.Pupup("Berhasil Login !\n - Username : " + Function.USERDATA[0] + "\n - Nama : " + Function.USERDATA[1] + "\n - Level : " + Function.USERDATA[2]);
                        MySql.ShowDataQuery("SELECT * FROM t_user", dataGridView1);
                    }
                    else
                    {
                        errorProvider1.SetError(input_md5_pass, "Password Salah !");
                        input_md5_pass.Select();
                        input_md5_pass.SelectAll();
                    }
                }
                else
                {
                    errorProvider1.SetError(input_user, "Username Tidak ditemukan !");
                    input_user.Select();
                    input_user.SelectAll();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Function.Pesan.Info(Function.GetAllUpdate(this));
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            input_user.Text = Function.AmbilData(dataGridView1, 1);
            input_md5_pass.Text = Function.AmbilData(dataGridView1, 2);
            this.Text = dataGridView1.CurrentRow.Index.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Function.Pesan.Info(Function.GetAllInput(this));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Function.CekTextboxComboBox(this.groupBox1, errorProvider1);
        }
    }
}
