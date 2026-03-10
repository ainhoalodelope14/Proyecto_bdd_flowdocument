using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ProyectoDesin_ParteA
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Todos los metodos de cada boton con cada ruta absoluta de los diferentes flow document
        private void btnAccion_Click(object sender, RoutedEventArgs e)
        {
            string ruta = @"C:\Users\AutoELEC\Desktop\Proyecto2ev_A_AinhoaLL\xml\flowdocumentAccion.xml"; 
            cargarXML(ruta);
        }

        private void btnAventura_Click(object sender, RoutedEventArgs e)
        {
            string ruta = @"C:\Users\AutoELEC\Desktop\Proyecto2ev_A_AinhoaLL\xml\flowdocumentAventura.xml";
            cargarXML(ruta);
        }

        private void btnRPG_Click(object sender, RoutedEventArgs e)
        {
            string ruta = @"C:\Users\AutoELEC\Desktop\Proyecto2ev_A_AinhoaLL\xml\flowdocumentRPG.xml";
            cargarXML(ruta);
        }

        //Métodos que leen los xml con el flow document y los carga en el flow document reader
        private void cargarXML(string ruta)
        {
            if (File.Exists(ruta))
            {
                try
                {
                    string contenidoXml = File.ReadAllText(ruta);
                    StringReader stringReader = new StringReader(contenidoXml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);

                    FlowDocument flowDocument = (FlowDocument)XamlReader.Parse(contenidoXml);
                    FlowDocumentReader.Document = flowDocument;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar el archivo XML: " + ex.Message);
                }
            }
        }

        //Botón que abre xml y lo lee en el flow document reader
        private void btnAbrir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos XML (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    cargarXML(openFileDialog.FileName);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show($"Error al cargar el archivo XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Botón que guarda el flow document en un archivo de texto
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Archivos XAML (*.xaml)|*.xaml";

            if (saveFileDialog.ShowDialog() == true)
            {
                string xamlContent = XamlWriter.Save(FlowDocumentReader.Document);

                File.WriteAllText(saveFileDialog.FileName, xamlContent);

                MessageBox.Show("Documento guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //Botón que limpia flow document
        private void bntLimpiar_Click(object sender, RoutedEventArgs e)
        {
            FlowDocumentReader.Document = null;
        }

        //Botón que puedas imprimir el contendio del flow document
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                FlowDocument flowDocument = FlowDocumentReader.Document as FlowDocument;

                if (flowDocument != null)
                {
                    printDialog.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "Impresión de FlowDocumentReader");
                }
                else
                {
                    MessageBox.Show("No hay ningún documento para imprimir", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Cargar URL
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
