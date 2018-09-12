using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        //public Function haha = new Function();
        //public MySql Mysql = new MySql();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Function.BukaForm(new Form2(), FormStartPosition.CenterScreen);
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Function.Pesan.Konfirmasi("Apakah Yakin ingin Keluar ?", "Keluar") == DialogResult.Yes)
            {
                e.Cancel = false;
            } else
            {
                e.Cancel = true;
            }
        }
    }
}
