using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoDesin_ParteB
{
    /// <summary>
    /// Lógica de interacción para NuevoProducto.xaml
    /// </summary>
    public partial class NuevoProducto : Window
    {
        //Guardar nuevo producto
        public Producto producto { get; set; }

        //Cargar la ventana para insertar un nuevo producto
        public NuevoProducto()
        {
            InitializeComponent();
            producto = new Producto();
            DataContext = producto;
        }

        //Cargar la ventana para producto a modificar
        public NuevoProducto(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
            this.DataContext = this.producto;
            this.Title = "Modificar prdoucto";
            btnAceptar.Content = "Modificar";
            txtId.IsReadOnly = true;
        }

        //Botón cancelar
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        //Botón aceptar
        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
       
    }

    //La clase producto con sus respectivos atributos
    public class Producto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; }
        public string RutaImagen { get; set; }
    }
}
