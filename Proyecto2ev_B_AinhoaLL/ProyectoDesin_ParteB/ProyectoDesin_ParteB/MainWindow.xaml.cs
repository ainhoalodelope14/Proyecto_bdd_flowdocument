using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace ProyectoDesin_ParteB
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection conexion;
        private DataSet miDataSet;
        private OleDbDataAdapter adaptador;

        public static readonly RoutedCommand InsertarCommand = new RoutedCommand();
        public static readonly RoutedCommand ModificarCommand = new RoutedCommand();
        public static readonly RoutedCommand BorrarCommand = new RoutedCommand();

        private int indiceFilaSeleccionada;
        private string RutaImagen { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            conectarBaseDatos();
            cargarDG();
        }

        //Método para conectar a la base de datos
        public void conectarBaseDatos()
        {
            string archivo = "DBProductos.accdb";
            string cadenaConexion = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Persist Security Info=True", archivo);
            conexion= new OleDbConnection(cadenaConexion);
        }
        
        //Método para cargar el contenido en el DataGrid
        public void cargarDG()
        {
            adaptador = new OleDbDataAdapter("SELECT * FROM Productos;", conexion);
            miDataSet = new DataSet();
            adaptador.Fill(miDataSet, "Productos");
            dgvProductos.DataContext = miDataSet;
        }

        //Método para comando insertar
        private void InsertarCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NuevoProducto nuevoProductoDialogo = new NuevoProducto();
            if (nuevoProductoDialogo.ShowDialog() == true)
            {
                Producto nuevoProducto = nuevoProductoDialogo.producto;
                // Insertar el nuevo producto en la base de datos
                InsertarProductoEnBaseDeDatos(nuevoProducto);
            }
        }

        //Método para insertar producto en base de datos
        private void InsertarProductoEnBaseDeDatos(Producto producto)
        {
            try
            {
                conectarBaseDatos();
                {
                    // Abrir la conexión a la base de datos
                    conexion.Open();

                    OleDbCommand comando = conexion.CreateCommand();
                    comando.CommandText = "INSERT INTO Productos(Id, Nombre , Descripción, Precio, Stock, Categoría, Imagen) VALUES('" +
                   producto.ID + "','" + producto.Nombre + "','" + producto.Descripcion + "','"
                   + producto.Precio + "','" + producto.Stock+ "','" + producto.Categoria + "','"  + producto.RutaImagen+ "')";

                    // Ejecutar el comando
                    comando.ExecuteNonQuery();

                    // Cerrar la conexión
                    conexion.Close();
                    cargarDG();

                    // Obtener el número total de filas antes de insertar la nueva fila
                    int numeroFilasAntes = dgvProductos.Items.Count;

                    // Insertar la nueva fila
                    // (Aquí iría tu lógica para insertar la fila en el DataGrid)

                    // Obtener el número total de filas después de insertar la nueva fila
                    int numeroFilasDespues = dgvProductos.Items.Count;

                    // Seleccionar el índice de la fila recién insertada
                    int indiceNuevaFila = numeroFilasDespues - 1;

                    // Seleccionar la fila recién insertada
                    dgvProductos.SelectedIndex = indiceNuevaFila;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void InsertarCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Lógica para determinar si el comando puede ejecutarse
            e.CanExecute = true; // Por ejemplo, siempre se puede ejecutar
        }

        //Método para cargar contenido para modificarlo
        private void ModificarCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Obtener el producto seleccionado en el DataGrid
            indiceFilaSeleccionada = dgvProductos.SelectedIndex;
            if (dgvProductos.SelectedItem != null)
            {
                Producto producto = new Producto();
                producto.ID = (int)((DataRowView)dgvProductos.SelectedItem).Row["Id"];
                producto.Nombre = ((DataRowView)dgvProductos.SelectedItem).Row["Nombre"].ToString();
                producto.Descripcion = ((DataRowView)dgvProductos.SelectedItem).Row["Descripción"].ToString();
                producto.Precio = (decimal)((DataRowView)dgvProductos.SelectedItem).Row["Precio"];
                producto.Stock = (int)((DataRowView)dgvProductos.SelectedItem).Row["Stock"];
                producto.Categoria = ((DataRowView)dgvProductos.SelectedItem).Row["Categoría"].ToString();
                producto.RutaImagen = ((DataRowView)dgvProductos.SelectedItem).Row["Imagen"].ToString();
                NuevoProducto dlgModificar = new NuevoProducto(producto);
                if (dlgModificar.ShowDialog() == true)
                {
                    producto = dlgModificar.producto;
                    ModificarProductoEnBaseDeDatos(producto);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para modificar.");
            }
        }

        //Método que modifica en base de datos
        public void ModificarProductoEnBaseDeDatos(Producto producto)
        {
            conectarBaseDatos();
            {
                try
                {
                    conexion.Open();
                    OleDbCommand comando = conexion.CreateCommand();
                    comando.CommandText = "UPDATE Productos SET Nombre='" + producto.Nombre + "', Descripción='" +
                   producto.Descripcion + "', Precio='" + producto.Precio + "', Stock='" + producto.Stock + "', Categoría='"
                   + producto.Categoria + "', Imagen='" + producto.RutaImagen + "' WHERE Id=" + producto.ID;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    cargarDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar los productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                dgvProductos.SelectedIndex = indiceFilaSeleccionada;
            }
        }

        private void ModificarCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Lógica para determinar si el comando puede ejecutarse
            e.CanExecute = true; // Por ejemplo, siempre se puede ejecutar
        }

        //Método para borrar en base de datos
        private void BorrarCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Verificar si se ha seleccionado un producto en el DataGrid
            if (dgvProductos.SelectedItem != null)
            {
                // Obtener el ID del producto seleccionado
                int id = (int)((DataRowView)dgvProductos.SelectedItem).Row["ID"];

                // Confirmar con el usuario si realmente desea borrar el producto
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres borrar este producto?", "Confirmar borrado", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Llamar al método para borrar el producto de la base de datos
                    if (BorraEnBD(id))
                    {
                        MessageBox.Show("Producto borrado correctamente.", "Borrado exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Actualizar el DataGrid después de borrar el producto
                        cargarDG();
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para borrar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Método para borrar en base de datos
        private bool BorraEnBD(int id)
        {
            try
            {
                // Establecer la conexión con la base de datos
                conectarBaseDatos();
                {
                    // Abrir la conexión
                    conexion.Open();

                    // Definir la consulta SQL para eliminar el producto
                    string consulta = "DELETE FROM Productos WHERE ID = @ID";

                    // Crear un comando con parámetros
                    using (OleDbCommand comando = new OleDbCommand(consulta, conexion))
                    {
                        // Agregar el parámetro ID
                        comando.Parameters.AddWithValue("@ID", id);

                        // Ejecutar la consulta y obtener el número de filas afectadas
                        int filasAfectadas = comando.ExecuteNonQuery();

                        // Verificar si se borró algún registro
                        if (filasAfectadas > 0)
                        {
                            // Se borró el producto correctamente
                            return true;
                        }
                        else
                        {
                            // No se borró ningún registro
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error en caso de excepción
                MessageBox.Show("Error al borrar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void BorrarCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Lógica para determinar si el comando puede ejecutarse
            e.CanExecute = true; // Por ejemplo, siempre se puede ejecutar
        }

        //Método para ir hacia atrás en el DataGrid
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (dgvProductos.SelectedIndex > 0)
            {
                dgvProductos.SelectedIndex--;
            }
        }

        //Método para ir hacia adelante en el DataGrid
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (dgvProductos.Items.Count - 1 > dgvProductos.SelectedIndex)
            {
                dgvProductos.SelectedIndex++;
            }
        }
    }
}
