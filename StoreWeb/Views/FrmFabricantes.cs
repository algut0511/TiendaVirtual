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
                                         nombre_fabricante = f.nombre_fabricante,
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

        private fabricantes getSelectedItem()
        {
            fabricantes f = new fabricantes();
            try
            {
                f.id_fabricante =int.Parse( grdDatos.Rows[grdDatos.CurrentRow.Index].Cells[0].Value.ToString());
                return f;
            }
            catch
            {
                return null;
            }
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
            FrmFabricantesGestion frmFabricantesGestion = new FrmFabricantesGestion(null);
            frmFabricantesGestion.ShowDialog();
            refrescarTabla();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            // retornar valor seleccionado en la tabla
            fabricantes f = getSelectedItem();

            if (f != null)
            {
                // llamar formulario en modo
                FrmFabricantesGestion frmFabricantesGestion = new FrmFabricantesGestion(f.id_fabricante);
                frmFabricantesGestion.ShowDialog();
                refrescarTabla();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {   // Obtener la fila que se va a eliminar
            fabricantes f = this.getSelectedItem();

            // Validar si hubo selección

            if(f != null)
            {
                if (MessageBox.Show("¿Seguro que quiere eliminar el registro ?",
                    "confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button1)
                    == DialogResult.Yes)
                {
                    using (tiendaEntities db = new tiendaEntities())
                    {
                        // buscar en la bd el fabricante a eliminar
                        fabricantes fabEliminar = db.fabricantes.Find(f.id_fabricante);
                        db.fabricantes.Remove(fabEliminar);
                        db.SaveChanges();
                    }
                    refrescarTabla();
                }

            }
        }
    }
}
