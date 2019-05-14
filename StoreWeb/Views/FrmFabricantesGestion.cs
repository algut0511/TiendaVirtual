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
        private int id_fabricante;
        public FrmFabricantesGestion(int idFabricante)
        {
            InitializeComponent();
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
    }
}
