using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopSales
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string text = "shop management system v1.0";
            MessageBox.Show(text, "About");
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            goods newForm = new goods();
            newForm.Show();
            //this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void goodsDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            goods newForm = new goods();
            newForm.Show();
            //this.Hide();
        }
    }
}
