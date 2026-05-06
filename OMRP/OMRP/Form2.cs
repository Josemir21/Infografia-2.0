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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var resultado = ctx.Customers
                    .Join(ctx.Orders,
                        c => c.CustomerID,
                        o => o.CustomerID,
                        (c, o) => new { c.CompanyName, o.OrderID })
                    .Join(ctx.Order_Details,
                        co => co.OrderID,
                        od => od.OrderID,
                        (co, od) => new
                        {
                            co.CompanyName,
                            Total = od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)
                        })
                    .GroupBy(x => x.CompanyName)
                    .Select(g => new
                    {
                        Cliente = g.Key,
                        TotalVentas = g.Sum(x => x.Total)
                    })
                    .OrderByDescending(x => x.TotalVentas)
                    .ToList();

                dataGridView1.DataSource = resultado;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var resultado = ctx.Customers
                    .GroupJoin(ctx.Orders,
                        c => c.CustomerID,
                        o => o.CustomerID,
                        (c, pedidos) => new
                        {
                            Cliente = c.CompanyName,
                            Pais = c.Country,
                            TotalPedidos = pedidos.Count()
                        })
                    .OrderByDescending(x => x.TotalPedidos)
                    .ToList();

                dataGridView1.DataSource = resultado;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var ctx = new NorthwindEntities())
            {
                var resultado = ctx.Customers
                    .GroupJoin(ctx.Orders,
                        c => c.CustomerID,
                        o => o.CustomerID,
                        (c, pedidos) => new
                        {
                            Cliente = c.CompanyName,
                            Ciudad = c.City,
                            UltimoPedido = pedidos
                                            .Max(o => (DateTime?)o.OrderDate)
                        })
                    .OrderBy(x => x.Cliente)
                    .ToList();

                dataGridView1.DataSource = resultado;
            }
        }
    }
}
