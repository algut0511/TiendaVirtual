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
    public partial class FrmFabricantesGestion : Form
    {
        fabricantes oFabricantes = null;
        private int? idfabricante;
        public FrmFabricantesGestion(int? idfabricante)
        {
            // dibujar el formulario
            InitializeComponent();
            // recibir el dato de la PK ( si es nulo es modo inserción)
            this.idfabricante = idfabricante;

            // si hay datos
            if(idfabricante != null)
            {
                cargarDatos();
            }
        }
        private void cargarDatos()
        {
            using (tiendaEntities db = new tiendaEntities())
            {
                // consultar datos a editar de la base de datos
                oFabricantes = db.fabricantes.Find(idfabricante);
                txtNombre.Text = oFabricantes.nombre_fabricante;
                cboPais.SelectedValue = oFabricantes.pais_fabricantes;
            }
        }
        private void CboPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
                var listPaises = (from p1 in firstItem select p1).Union(from p in db.dominios
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

        private void FrmFabricantesGestion_Load(object sender, EventArgs e)
        {
            listarPaises();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (this.txtNombre.Text)|| this.cboPais.SelectedValue.ToString()== "T")
                {
                MessageBox.Show("Los campos marcados con ( * ) son obligatorios");
            }
            else
            {
                using (tiendaEntities db = new tiendaEntities())
                {
                    // si es modo inserción, inicializamos el objeto de fabricantes
                    if (this.idfabricante == null)
                    {
                        oFabricantes = new fabricantes();
                    }
                    // armar el objeto con los datos registrados en el formulario
                    oFabricantes.nombre_fabricante = this.txtNombre.Text;
                    oFabricantes.pais_fabricantes = this.cboPais.SelectedValue.ToString();

                    if (this.idfabricante == null)
                    {
                        // insertar nuevo fabricante
                        db.fabricantes.Add(oFabricantes);
                    }
                    else
                    {
                        // modificar fabricante existente
                        db.Entry(oFabricantes).State = System.Data.Entity.EntityState.Modified;
                    }

                    // confirmar cambios en la BD

                    db.SaveChanges();
                    // cerrar formulario de gestion de fabricantes
                    this.Close();
                }
            }              
        }
    }
}
