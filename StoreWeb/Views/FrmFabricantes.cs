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
        {// llena el listado por primera vez
            using (tiendaEntities db = new tiendaEntities())
            {
                var lstFabricantes = from f in db.fabricantes
                                     select new
                                     {
                                         id_fabricante = f.id_fabricante,
                                         nombre_fabricante = f.id_fabricante,
                                         pais_fabricante = f.pais_fabricantes
                                     };
                grdDatos.DataSource = lstFabricantes.ToList();
            }
        }

        public void listarPaises()
            // llena la caja de paises para seleccionar
        {
            using (tiendaEntities db = new tiendaEntities())
            {
                var firstItem = new List<dynamic>()
                {
                    new
                    {
                        id_dominio ="T", valor_dominio ="Todos"
                    }
                };
                // escribir consulta a Bd con LINQ
                var listPaises =(from p1 in firstItem select p1).Union( from p in db.dominios
                                 where p.tipo_dominio.Equals("PAISES")
                                 orderby p.valor_dominio
                                 select new
                                 {
                                     id_dominio = p.id_dominio,
                                     valor_dominio = p.valor_dominio
                                 });
                this.cboPais.DataSource = listPaises.ToList();
                this.cboPais.DisplayMember = "valor_dominio";
                this.cboPais.ValueMember = "id_dominio";
            };
        }
        private void FrmFabricantes_Load(object sender, EventArgs e)
        {
            refrescarTabla();
            this.txtNombre.Focus();
            listarPaises();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (tiendaEntities db = new tiendaEntities())
            {
                //consultar todos los fabricantes a través de linq

                var lstFabricantes = from f in db.fabricantes
                                     select new
                                     {
                                         id_fabricante = f.id_fabricante,
                                         nombre_fabricante = f.nombre_fabricante,
                                         pais_fabricante = f.pais_fabricantes
                                     };
                // aplicar filtros dependiendo de lo que haya escrito o seleccionado el usuario
                if(!string.IsNullOrEmpty(this.txtNombre.Text))
                {
                    // filtrar por nombre de fabricante a través de Entity Framework
                    lstFabricantes = lstFabricantes.Where(f => f.nombre_fabricante.Contains(this.txtNombre.Text));
                }
                if(!this.cboPais.SelectedValue.ToString().Equals("T"))
                {
                    // filtrar por nombre de pais a través de Entity Framework
                    lstFabricantes = lstFabricantes.Where(f => f.pais_fabricante.Equals(this.cboPais.SelectedValue.ToString()));
                }
                grdDatos.DataSource = lstFabricantes.ToList();
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtNombre.Text = "";
            this.cboPais.SelectedValue = "T";
            refrescarTabla();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            FrmFabricantesGestion frmFabricantesGestion = new FrmFabricantesGestion();
            frmFabricantesGestion.ShowDialog();
            refrescarTabla();
        }
    }
}
