using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMRP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var clientes = ctx.Customers
                                  .OrderBy(c => c.CompanyName)
                                  .ToList();
                dataGridView1.DataSource = clientes;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Name = textBox1.Text;
            using (var ctx = new NorthwindEntities())
            {
                var resultado = ctx.Customers
                                   .Where(c => c.ContactName == Name)
                                   .ToList();
                dataGridView1.DataSource = resultado;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var nuevo = new Customers
                {
                    CustomerID = txtID.Text.ToUpper(),
                    CompanyName = txtEmpresa.Text,
                    ContactName = txtContacto.Text,
                    Country = txtPais.Text
                };
                ctx.Customers.Add(nuevo);
                ctx.SaveChanges();
                MessageBox.Show("Cliente agregado correctamente.");
            }
        }
    }
}
