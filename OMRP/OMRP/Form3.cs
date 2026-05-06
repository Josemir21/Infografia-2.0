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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var resultado =
                    ctx.Customers
                       .Where(c =>
                           !c.Orders.Any(o =>          // NOT EXISTS (subconsulta)
                               o.Order_Details.Any(od =>
                                   od.Products.Discontinued)))
                       .Select(c => new
                       {
                           c.CustomerID,
                           c.CompanyName,
                           c.Country,
                           TotalPedidos = c.Orders.Count()
                       })
                       .OrderBy(c => c.CompanyName)
                       .ToList();

                dataGridView1.DataSource = resultado;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var resumen =
                    ctx.Customers
                       .Where(c => c.CustomerID == txtCustomerID.Text.Trim().ToUpper())

                       .Select(c => new
                       {
                           c.CompanyName,
                           c.ContactName,
                           c.Country,
                           TotalPedidos = c.Orders.Count(),
                           PrimerPedido = c.Orders
                                             .Min(o => (DateTime?)o.OrderDate),
                           UltimoPedido = c.Orders
                                             .Max(o => (DateTime?)o.OrderDate),
                           TotalFacturado = c.Orders
                                             .SelectMany(o => o.Order_Details)
                                             .Sum(od =>
                                                 od.UnitPrice * od.Quantity
                                                 * (decimal)(1 - od.Discount)),
                           ProductosDistintos = c.Orders
                                                 .SelectMany(o => o.Order_Details)
                                                 .Select(od => od.ProductID)
                                                 .Distinct()
                                                 .Count(),
                           PaisesEnvio = c.Orders
                                            .Select(o => o.ShipCountry)
                                            .Distinct()
                                            .ToList()
                       })
                       .FirstOrDefault();

                if (resumen != null)
                {
                    lblEmpresa.Text = resumen.CompanyName;
                    lblTotal.Text = resumen.TotalFacturado
                                              .ToString("C2");
                    lblPedidos.Text = resumen.TotalPedidos.ToString();
                    lblProductos.Text = resumen.ProductosDistintos.ToString();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
