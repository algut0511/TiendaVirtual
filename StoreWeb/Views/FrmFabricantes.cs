using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StoreWeb.Models;

namespace StoreWeb.Views
{
    public partial class FrmFabricantes : Form
    {
        public FrmFabricantes()
        {
            InitializeComponent();
        }
        public void refrescarTabla()
        {
            using (tiendaEntities db = new tiendaEntities())
            {
                var lstFabricantes = from f in db.fabricantes
                                     select f;
                grdDatos.DataSource = lstFabricantes.ToList();
            }
        }
        private void FrmFabricantes_Load(object sender, EventArgs e)
        {
            refrescarTabla();
        }
    }
}
