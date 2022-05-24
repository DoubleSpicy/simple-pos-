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
using System.Text.RegularExpressions;
namespace ShopSales
{
    public partial class goods : Form
    {
        public goods()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // output data from SQL db
            this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("search", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("expenditure_DESC", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("P&L", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ADD BUTTON
            Regex rx = new Regex(@"^[a-zA-Z0-9\s]+,\s*[0-9]+,\s*[0-9]+,\s*[0-9]+,\s*[0-9]+\s*$");
            if (rx.IsMatch(textBox2.Text))
            {
                //this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("ADD/UPDATE", textBox2.Text, ShopSales.commons.SQLtools.getConnectionString());
                string[] textBox2Splitted = textBox2.Text.Split(',').ToArray();
                if (ShopSales.commons.SQLtools.IsExistInDB(textBox2Splitted[0], ShopSales.commons.SQLtools.getConnectionString()))
                {
                    ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("UPDATE", textBox2.Text, ShopSales.commons.SQLtools.getConnectionString());
                    this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("search", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
                }
                else
                {
                    ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("ADD", textBox2.Text, ShopSales.commons.SQLtools.getConnectionString());
                    this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("search", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
                }
            }
            else
            {
                MessageBox.Show("Invalid Input Sequence!", "ADD Error");
                this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("search", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
            }
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // DELETE
            Regex rx = new Regex(@"^[a-zA-Z0-9\s]+$");
            if (rx.IsMatch(textBox2.Text))
            {
                if (ShopSales.commons.SQLtools.IsExistInDB(textBox2.Text, ShopSales.commons.SQLtools.getConnectionString()))
                {
                    ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("DELETE", textBox2.Text, ShopSales.commons.SQLtools.getConnectionString());
                    this.dataGridView1.DataSource = ShopSales.commons.SQLtools.executeQuery_DispalyOnTable("search", textBox1.Text, ShopSales.commons.SQLtools.getConnectionString());
                }
                else
                {
                    MessageBox.Show("Name specified not found in database, check your spelling!", "DELETE ERROR");
                }
            }
            else
            {
                MessageBox.Show("Invalid Name Specified:\nName must be in alphabetical only!", "DELETE ERROR");
            }
        }
    }
}
