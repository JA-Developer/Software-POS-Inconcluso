// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Punto_De_Ventas
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using Diseño_de_App_Para_Ventas.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class Punto_De_Ventas : Form
  {
    private double ISV = 0.15;
    public List<Carrito> Carritos = new List<Carrito>();
    private Bitmap ImagenDeFondoDeUnSoloColorParaDiseno;
    public OleDbConnection Conn;
    private string ConsultaActual = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario";
    private int ColumnaDeSortingActual;
    private SortOrder OrdenDeSortingActual;
    private int NumeroDeVentas = 1;
    private int NumeroDePestana;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel PanelLateralDerecho;
    private Panel PanelQueContienePestanasYControles;
    private Panel panel2;
    private Panel panel5;
    private Panel Panel6BusquedaNormal;
    private Button BtnBuscarPor;
    private Panel Panel4BusquedaNormal;
    private NumericUpDown NumTxBuscarPorHasta;
    private Label CodLabelParaNúmerosHasta;
    private Panel Panel3BusquedaNormal;
    private NumericUpDown NumTxBuscarPorDesde;
    private Label CodLabelParaNúmerosDesde;
    private Panel Panel2BusquedaNormal;
    private Label CodLabelParaTexto;
    private TextBox TxBuscarPor;
    private Panel Panel1BusquedaNormal;
    private Label LabelBuscarPor;
    private ComboBox ListaBuscarPor;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private Panel panel4;
    private Panel panel11;
    private Button BTN_AgregarAlCarrito;
    private Panel panel22;
    private Label label8;
    private TextBox TXT_AgregarAlCarrito;
    private Panel panel24;
    private LinkLabel LabelAgregarProductoAlCarrito;
    private DataGridView ListaParaVerProductosAVender;
    private Button BotonPestanasALaDerecha;
    private Button BotonPestanasALaIzquierda;
    private Panel PanelQueContienePestanas;
    private Button BotonAgregarPestana;
    public Panel PanelParaPestana;
    private Panel PanelDeOtrosDetalles;
    private TextBox TxBoxFechaYHora;
    private Label LabelFechaYHora;
    private Label LabelVendedor;
    private Button ButtonSeleccionarCliente;
    private Label LabelCliente;
    private Panel PanelTituloOtrosDetalles;
    private LinkLabel LinkLabelTituloOtrosDetalles;
    private Panel PanelDeDetallesDeVenta;
    private Panel PanelLineaDeContorno_NumBox_Descuento;
    private NumericUpDown NumBox_Descuento;
    private Panel Panel_LineaDeContorno_ModosDeDescuento;
    private ComboBox ListaMontosDeDescuento;
    private Label LabelDescuento;
    private Label LabelISV;
    private TextBox TxBox_Total;
    private Label LabelTotal;
    private TextBox TxBox_Monto;
    private Label LabelMonto;
    private TextBox TxBox_ISV;
    private Panel PanelTituloDetallesDeVenta;
    private LinkLabel LinkLabelTituloDetallesDeVenta;
    private Panel PanelCarrito;
    private DataGridView ListaCarrito;
    private DataGridViewTextBoxColumn Codigo;
    private DataGridViewTextBoxColumn Producto;
    private DataGridViewTextBoxColumn Cantidad;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private Button ButtonCarrito;
    private Label LabelProductos;
    private DataGridViewTextBoxColumn ColCodigo;
    private DataGridViewTextBoxColumn ColProducto;
    private DataGridViewTextBoxColumn ColCantidad;
    private DataGridViewTextBoxColumn PrecioDeVenta;
    private DataGridViewTextBoxColumn PrecioConImpuesto;
    public TextBox TxBox_Vendedor;
    private System.Windows.Forms.Timer Temporizador;
    private Panel panel10;
    private Panel StatusStrip;
    private Panel PanelSuperior;
    private Panel PanelTituloCarrito;
    private LinkLabel LinkLabelTituloCarrito;
    private Panel PanelRealizarVenta;
    private Button BTN_Vender;
    private Panel PanelTituloRealizarVenta;
    private LinkLabel LinkLabelTituloRealizarVenta;
    private Panel PanelFacturar;
    private Panel PanelTituloFacturar;
    private LinkLabel LinkLabelTituloFacturar;
    private CheckBox CheckBoxTituloFacturar;
    private TextBox TxBox_NumeroDeFactura;
    private Label LabelNumeroDeFactura;

    public Punto_De_Ventas()
    {
      this.InitializeComponent();
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaParaVerProductosAVender, new object[1]
      {
        (object) true
      });
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaCarrito, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.PanelQueContienePestanas.ControlRemoved += new ControlEventHandler(this.PanelQueContienePestanas_ControlRemoved);
      this.ListaParaVerProductosAVender.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.ListaParaVerProductosAVender_ColumnHeaderMouseClick);
      this.ListaCarrito.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaCarrito_UserDeletingRow);
    }

    private void ListaCarrito_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          string str = this.PanelQueContienePestanas.Controls[index1].Name.Substring(2);
          new OleDbCommand("DELETE FROM CarritosDeVentas WHERE CodigoDeProducto = '" + e.Row.Cells[0].Value.ToString() + "' AND IdDeCarrito = " + str + ";", this.Conn).ExecuteNonQuery();
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == str)
            {
              for (int index3 = this.Carritos[index2].Codigos.Count - 1; index3 >= 0; --index3)
              {
                if (this.Carritos[index2].Codigos[index3] == e.Row.Cells[0].Value.ToString())
                {
                  this.Carritos[index2].Codigos.RemoveAt(index3);
                  this.Carritos[index2].Cantidades.RemoveAt(index3);
                }
              }
            }
          }
        }
      }
    }

    private void ListaParaVerProductosAVender_ColumnHeaderMouseClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex != -1)
        return;
      switch (this.OrdenDeSortingActual)
      {
        case SortOrder.None:
          this.EjecutarConsulta(this.ConsultaActual, SortOrder.Ascending, e.ColumnIndex);
          this.OrdenDeSortingActual = SortOrder.Ascending;
          break;
        case SortOrder.Ascending:
          this.OrdenDeSortingActual = SortOrder.Descending;
          this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, e.ColumnIndex);
          break;
        case SortOrder.Descending:
          this.OrdenDeSortingActual = SortOrder.Ascending;
          this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, e.ColumnIndex);
          break;
      }
      this.ColumnaDeSortingActual = e.ColumnIndex;
    }

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.ListaParaVerProductosAVender.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText += Consulta;
      if (Orden != SortOrder.None)
      {
        oleDbCommand.CommandText += " ORDER BY Inventario.";
        switch (IndexColumnaDeOrden)
        {
          case 0:
            oleDbCommand.CommandText += "Codigo";
            break;
          case 1:
            oleDbCommand.CommandText += "Producto";
            break;
          case 2:
            oleDbCommand.CommandText += "Cantidad";
            break;
          case 3:
            oleDbCommand.CommandText += "PrecioUnitarioDeVenta";
            break;
          case 4:
            oleDbCommand.CommandText += "PrecioUnitarioDeVenta*1.15";
            break;
          default:
            oleDbCommand.CommandText += "Codigo";
            break;
        }
        switch (Orden)
        {
          case SortOrder.Ascending:
            oleDbCommand.CommandText += " ASC;";
            break;
          case SortOrder.Descending:
            oleDbCommand.CommandText += " DESC;";
            break;
          default:
            oleDbCommand.CommandText += " DESC;";
            break;
        }
      }
      else
        oleDbCommand.CommandText += ";";
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      while (oleDbDataReader.Read())
        this.ListaParaVerProductosAVender.Rows.Add(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), oleDbDataReader.GetValue(2), oleDbDataReader.GetValue(3), (object) (double.Parse(oleDbDataReader.GetValue(3).ToString()) * 1.15));
      oleDbDataReader.Close();
      this.ConsultaActual = Consulta;
      Cursor.Current = Cursors.Arrow;
    }

    private void Punto_De_Ventas_Load(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.ListaBuscarPor.SelectedIndex = 0;
      this.ImagenDeFondoDeUnSoloColorParaDiseno = new Bitmap(10, 10);
      Graphics.FromImage((Image) this.ImagenDeFondoDeUnSoloColorParaDiseno).Clear(Color.LightGray);
      this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
      if (this.PanelQueContienePestanas.Controls.Count == 0)
      {
        this.AgregarPestanaDeCarrito();
      }
      else
      {
        for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
        {
          if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
          {
            for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
            {
              if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
                this.ButtonSeleccionarCliente.Tag = (object) this.Carritos[index2].IDCliente;
            }
          }
        }
        OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT NombreDeCliente FROM Clientes WHERE IDDeCliente = '" + this.ButtonSeleccionarCliente.Tag.ToString() + "';", this.Conn).ExecuteReader();
        bool flag = false;
        string str = "";
        while (oleDbDataReader.Read())
        {
          str = oleDbDataReader.GetValue(0).ToString();
          flag = true;
        }
        if (flag)
        {
          this.ButtonSeleccionarCliente.Text = str;
        }
        else
        {
          this.ButtonSeleccionarCliente.Text = "Ninguno";
          this.ButtonSeleccionarCliente.Tag = (object) null;
        }
      }
      this.WindowState = FormWindowState.Maximized;
      this.Temporizador.Start();
      Cursor.Current = Cursors.Arrow;
    }

    private void ListaBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          break;
        case 1:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          break;
        case 2:
          this.NumTxBuscarPorDesde.DecimalPlaces = 0;
          this.NumTxBuscarPorHasta.DecimalPlaces = 0;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          break;
        case 3:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          break;
        case 4:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          break;
      }
    }

    private void BuscarPor()
    {
      string Consulta = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          Consulta = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario WHERE Codigo LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 1:
          Consulta = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario WHERE Producto LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 2:
          Decimal num1 = this.NumTxBuscarPorDesde.Value;
          string str1 = num1.ToString().Replace(",", ".");
          num1 = this.NumTxBuscarPorHasta.Value;
          string str2 = num1.ToString().Replace(",", ".");
          Consulta = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario WHERE Cantidad >= " + str1 + " AND Cantidad <= " + str2;
          break;
        case 3:
          Decimal num2 = this.NumTxBuscarPorDesde.Value;
          string str3 = num2.ToString().Replace(",", ".");
          num2 = this.NumTxBuscarPorHasta.Value;
          string str4 = num2.ToString().Replace(",", ".");
          Consulta = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario WHERE PrecioUnitarioDeVenta >= " + str3 + " AND PrecioUnitarioDeVenta <= " + str4;
          break;
        case 4:
          Decimal num3 = this.NumTxBuscarPorDesde.Value;
          string str5 = num3.ToString().Replace(",", ".");
          num3 = this.NumTxBuscarPorHasta.Value;
          string str6 = num3.ToString().Replace(",", ".");
          Consulta = "SELECT Codigo, Producto, Cantidad, PrecioUnitarioDeVenta FROM Inventario WHERE (PrecioUnitarioDeVenta*1.15) >= " + str5 + " AND (PrecioUnitarioDeVenta*1.15) <= " + str6;
          break;
      }
      this.EjecutarConsulta(Consulta, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void BtnBuscarPor_Click(object sender, EventArgs e) => this.BuscarPor();

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void SeleccionarPestanaParaCarrito(Button BotonDePestana)
    {
      if (BotonDePestana.Tag != (object) "Selected")
      {
        for (int index = 0; index < this.PanelQueContienePestanas.Controls.Count; ++index)
        {
          this.PanelQueContienePestanas.Controls[index].BackgroundImage = (Image) Resources.ImgIndexItem;
          this.PanelQueContienePestanas.Controls[index].Tag = (object) "";
        }
        BotonDePestana.Tag = (object) "Selected";
        BotonDePestana.BackgroundImage = (Image) this.ImagenDeFondoDeUnSoloColorParaDiseno;
        for (int index = 0; index < this.Carritos.Count; ++index)
        {
          if (this.Carritos[index].IdDeCarrito.ToString() == BotonDePestana.Name.ToString().Substring(2))
          {
            this.NumBox_Descuento.Value = (Decimal) this.Carritos[index].Descuento;
            this.TxBox_NumeroDeFactura.Text = this.Carritos[index].NumeroDeFactura;
            this.CheckBoxTituloFacturar.Checked = this.Carritos[index].AdjuntarNumeroDeFactura;
            if (this.Carritos[index].IsPercent)
              this.ListaMontosDeDescuento.SelectedIndex = 1;
            else
              this.ListaMontosDeDescuento.SelectedIndex = 0;
            this.ButtonSeleccionarCliente.Tag = (object) this.Carritos[index].IDCliente;
            if (this.Carritos[index].IDCliente == "")
            {
              this.ButtonSeleccionarCliente.Text = "Ninguno";
            }
            else
            {
              OleDbCommand oleDbCommand = new OleDbCommand("SELECT NombreDeCliente FROM Clientes WHERE IdDeCliente = '" + this.Carritos[index].IDCliente + "';", this.Conn);
              if (oleDbCommand.ExecuteScalar() != null)
                this.ButtonSeleccionarCliente.Text = oleDbCommand.ExecuteScalar().ToString();
              else
                this.ButtonSeleccionarCliente.Text = "Ninguno";
            }
          }
        }
      }
      this.ActualizarCarrito();
    }

    private void AgregarPestanaDeCarrito()
    {
      OleDbCommand oleDbCommand = new OleDbCommand("INSERT INTO IdsDeCarrito(FechaYHora) VALUES(Now());", this.Conn);
      oleDbCommand.ExecuteNonQuery();
      oleDbCommand.CommandText = "SELECT Max(Id) FROM IdsDeCarrito;";
      int Id = (int) oleDbCommand.ExecuteScalar();
      Button BotonDePestana = new Button();
      BotonDePestana.Font = new Font("Microsoft Sans Serif", 12f);
      BotonDePestana.TabStop = true;
      BotonDePestana.Text = "Venta " + (object) this.NumeroDeVentas;
      BotonDePestana.TextAlign = ContentAlignment.MiddleCenter;
      BotonDePestana.BackColor = Color.LightGray;
      BotonDePestana.BackgroundImage = (Image) Resources.ImgIndexItem;
      BotonDePestana.BackgroundImageLayout = ImageLayout.Stretch;
      BotonDePestana.FlatStyle = FlatStyle.Flat;
      BotonDePestana.Dock = DockStyle.Left;
      BotonDePestana.Width = 90;
      BotonDePestana.Click += new EventHandler(this.BotonPestanaNueva_Click);
      this.PanelQueContienePestanas.Controls.Add((Control) BotonDePestana);
      BotonDePestana.BringToFront();
      BotonDePestana.Name = "P_" + (object) Id;
      BotonDePestana.Tag = (object) "";
      this.Carritos.Add(new Carrito(Id));
      this.SeleccionarPestanaParaCarrito(BotonDePestana);
      this.ActualizarCarrito();
      ++this.NumeroDeVentas;
    }

    private void BotonAgregarPestana_Click(object sender, EventArgs e) => this.AgregarPestanaDeCarrito();

    private void BotonPestanaNueva_Click(object sender, EventArgs e) => this.SeleccionarPestanaParaCarrito((Button) sender);

    private void BotonPestanasALaDerecha_Click(object sender, EventArgs e)
    {
      if (this.PanelQueContienePestanas.Controls.Count > 0)
      {
        if (this.PanelQueContienePestanas.Controls.Count - this.NumeroDePestana > (int) ((double) this.PanelQueContienePestanas.Width / 90.0))
          ++this.NumeroDePestana;
        for (int index = 0; index < this.PanelQueContienePestanas.Controls.Count; ++index)
        {
          if (index < this.NumeroDePestana)
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = false;
          else
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = true;
        }
      }
      else
      {
        this.NumeroDePestana = 0;
        this.AgregarPestanaDeCarrito();
      }
    }

    private void BotonPestanasALaIzquierda_Click(object sender, EventArgs e)
    {
      if (this.PanelQueContienePestanas.Controls.Count > 0)
      {
        if (this.NumeroDePestana > 0)
          --this.NumeroDePestana;
        for (int index = 0; index < this.PanelQueContienePestanas.Controls.Count; ++index)
        {
          if (index < this.NumeroDePestana)
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = false;
          else
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = true;
        }
      }
      else
      {
        this.NumeroDePestana = 0;
        this.AgregarPestanaDeCarrito();
      }
    }

    private void PanelQueContienePestanas_ControlRemoved(object sender, ControlEventArgs e)
    {
      if (this.PanelQueContienePestanas.Controls.Count > 0)
      {
        int num1 = (int) ((double) this.PanelQueContienePestanas.Width / 90.0);
        if (this.PanelQueContienePestanas.Controls.Count - this.NumeroDePestana < num1)
        {
          int num2 = this.PanelQueContienePestanas.Controls.Count - num1;
          this.NumeroDePestana = num2 >= 0 ? num2 : 0;
        }
        for (int index = 0; index < this.PanelQueContienePestanas.Controls.Count; ++index)
        {
          if (index < this.NumeroDePestana)
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = false;
          else
            this.PanelQueContienePestanas.Controls[this.PanelQueContienePestanas.Controls.Count - 1 - index].Visible = true;
        }
      }
      else
      {
        this.NumeroDePestana = 0;
        this.AgregarPestanaDeCarrito();
      }
    }

    private void Temporizador_Tick(object sender, EventArgs e) => this.TxBoxFechaYHora.Text = DateTime.Now.ToString();

    private void TxBox_Monto_TextChanged(object sender, EventArgs e)
    {
    }

    private void NumBox_Descuento_ValueChanged(object sender, EventArgs e)
    {
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
              this.Carritos[index2].Descuento = (double) this.NumBox_Descuento.Value;
          }
        }
      }
      this.ActualizarCarrito();
    }

    private void ListaMontosDeDescuento_SelectedIndexChanged(object sender, EventArgs e)
    {
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
              this.Carritos[index2].IsPercent = this.ListaMontosDeDescuento.SelectedIndex == 1;
          }
        }
      }
      this.ActualizarCarrito();
    }

    private void TxBox_ISV_TextChanged(object sender, EventArgs e)
    {
    }

    private void TxBox_Total_TextChanged(object sender, EventArgs e)
    {
    }

    private void ActualizarCarrito()
    {
      this.ListaCarrito.Rows.Clear();
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          string str = this.PanelQueContienePestanas.Controls[index1].Name.Substring(2);
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == str)
            {
              double num1 = 0.0;
              for (int index3 = 0; index3 < this.Carritos[index2].Cantidades.Count; ++index3)
              {
                OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT Codigo, Producto, PrecioUnitarioDeVenta FROM Inventario WHERE Codigo = '" + this.Carritos[index2].Codigos[index3] + "';", this.Conn).ExecuteReader();
                while (oleDbDataReader.Read())
                {
                  this.ListaCarrito.Rows.Add(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), (object) this.Carritos[index2].Cantidades[index3], oleDbDataReader.GetValue(2));
                  num1 += (double) oleDbDataReader.GetValue(2) * (double) this.Carritos[index2].Cantidades[index3];
                }
              }
              this.TxBox_NumeroDeFactura.Text = this.Carritos[index2].NumeroDeFactura;
              this.CheckBoxTituloFacturar.Checked = this.Carritos[index2].AdjuntarNumeroDeFactura;
              this.TxBox_Monto.Text = string.Concat((object) Math.Round(num1, 2));
              double num2 = !this.Carritos[index2].IsPercent ? num1 - this.Carritos[index2].Descuento : num1 * (1.0 - this.Carritos[index2].Descuento / 100.0);
              double num3 = num2 * this.ISV;
              this.TxBox_ISV.Text = string.Concat((object) Math.Round(num3, 2));
              this.TxBox_Total.Text = string.Concat((object) Math.Round(num2 + num3, 2));
              break;
            }
          }
          break;
        }
      }
    }

    private void ButtonCarrito_Click(object sender, EventArgs e)
    {
      if (this.ListaParaVerProductosAVender.SelectedRows.Count > 0 && this.PanelQueContienePestanas.Controls.Count > 0)
      {
        int num1 = -1;
        int index1 = -1;
        for (int index2 = 0; index2 < this.PanelQueContienePestanas.Controls.Count; ++index2)
        {
          if (this.PanelQueContienePestanas.Controls[index2].Tag.ToString() == "Selected")
            num1 = int.Parse(this.PanelQueContienePestanas.Controls[index2].Name.Substring(2));
        }
        for (int index3 = 0; index3 < this.Carritos.Count; ++index3)
        {
          if (this.Carritos[index3].IdDeCarrito == num1)
            index1 = index3;
        }
        if (num1 != -1 && index1 != -1)
        {
          for (int index4 = 0; index4 < this.ListaParaVerProductosAVender.SelectedRows.Count; ++index4)
          {
            bool flag = false;
            int index5 = -1;
            for (int index6 = 0; index6 < this.Carritos[index1].Codigos.Count; ++index6)
            {
              if (this.Carritos[index1].Codigos[index6] == this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString())
              {
                flag = true;
                index5 = index6;
                break;
              }
            }
            OleDbCommand oleDbCommand1 = new OleDbCommand("SELECT Cantidad FROM Inventario WHERE Codigo = '" + this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString() + "';", this.Conn);
            object obj1 = oleDbCommand1.ExecuteScalar();
            int result1 = 0;
            if (obj1 != null && int.TryParse(obj1.ToString(), out result1))
              ;
            oleDbCommand1.CommandText = "SELECT Sum(Cantidad) FROM CarritosDeVentas WHERE CodigoDeProducto = '" + this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString() + "' AND IdDeCarrito <> " + (object) num1 + ";";
            object obj2 = oleDbCommand1.ExecuteScalar();
            int result2 = 0;
            if (obj2 != null)
              int.TryParse(obj2.ToString(), out result2);
            int num2 = !flag ? 1 : this.Carritos[index1].Cantidades[index5] + 1;
            if (result2 + num2 <= result1)
            {
              OleDbCommand oleDbCommand2 = new OleDbCommand();
              oleDbCommand2.Connection = this.Conn;
              if (flag)
              {
                oleDbCommand2.CommandText = "UPDATE CarritosDeVentas SET Cantidad = " + (object) num2 + " WHERE IdDeCarrito = " + (object) num1 + " AND CodigoDeProducto = '" + this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString() + "';";
                this.Carritos[index1].Cantidades[index5] = num2;
              }
              else
              {
                oleDbCommand2.CommandText = "INSERT INTO CarritosDeVentas(CodigoDeProducto,Cantidad,IdDeCarrito) VALUES('" + this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString() + "', " + (object) num2 + ", " + (object) num1 + ");";
                this.Carritos[index1].Cantidades.Add(num2);
                this.Carritos[index1].Codigos.Add(this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[0].Value.ToString());
              }
              oleDbCommand2.ExecuteNonQuery();
            }
            else
            {
              int num3 = (int) MessageBox.Show("No hay suficientes existencias de " + this.ListaParaVerProductosAVender.SelectedRows[index4].Cells[1].Value.ToString() + " en el inventario");
            }
          }
        }
      }
      this.ActualizarCarrito();
    }

    private bool VerificarSiExiste(string Codigo)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Inventario WHERE Codigo = '" + Codigo + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    private void AgregarElemento(string Codigo)
    {
      if (this.VerificarSiExiste(Codigo))
      {
        if (this.PanelQueContienePestanas.Controls.Count > 0)
        {
          int num1 = -1;
          int index1 = -1;
          for (int index2 = 0; index2 < this.PanelQueContienePestanas.Controls.Count; ++index2)
          {
            if (this.PanelQueContienePestanas.Controls[index2].Tag.ToString() == "Selected")
              num1 = int.Parse(this.PanelQueContienePestanas.Controls[index2].Name.Substring(2));
          }
          for (int index3 = 0; index3 < this.Carritos.Count; ++index3)
          {
            if (this.Carritos[index3].IdDeCarrito == num1)
              index1 = index3;
          }
          if (num1 != -1 && index1 != -1)
          {
            bool flag = false;
            int index4 = -1;
            for (int index5 = 0; index5 < this.Carritos[index1].Codigos.Count; ++index5)
            {
              if (this.Carritos[index1].Codigos[index5] == Codigo)
              {
                flag = true;
                index4 = index5;
                break;
              }
            }
            OleDbCommand oleDbCommand1 = new OleDbCommand("SELECT Cantidad FROM Inventario WHERE Codigo = '" + Codigo + "';", this.Conn);
            object obj1 = oleDbCommand1.ExecuteScalar();
            int result1 = 0;
            if (obj1 != null && int.TryParse(obj1.ToString(), out result1))
              ;
            oleDbCommand1.CommandText = "SELECT Sum(Cantidad) FROM CarritosDeVentas WHERE CodigoDeProducto = '" + Codigo + "' AND IdDeCarrito <> " + (object) num1 + ";";
            object obj2 = oleDbCommand1.ExecuteScalar();
            int result2 = 0;
            if (obj2 != null)
              int.TryParse(obj2.ToString(), out result2);
            int num2 = !flag ? 1 : this.Carritos[index1].Cantidades[index4] + 1;
            if (result2 + num2 <= result1)
            {
              OleDbCommand oleDbCommand2 = new OleDbCommand();
              oleDbCommand2.Connection = this.Conn;
              if (flag)
              {
                oleDbCommand2.CommandText = "UPDATE CarritosDeVentas SET Cantidad = " + (object) num2 + " WHERE IdDeCarrito = " + (object) num1 + " AND CodigoDeProducto = '" + Codigo + "';";
                this.Carritos[index1].Cantidades[index4] = num2;
              }
              else
              {
                oleDbCommand2.CommandText = "INSERT INTO CarritosDeVentas(CodigoDeProducto,Cantidad,IdDeCarrito) VALUES('" + Codigo + "', " + (object) num2 + ", " + (object) num1 + ");";
                this.Carritos[index1].Cantidades.Add(num2);
                this.Carritos[index1].Codigos.Add(Codigo);
              }
              oleDbCommand2.ExecuteNonQuery();
            }
            else
            {
              int num3 = (int) MessageBox.Show("No hay suficientes existencias de " + Codigo + " en el inventario", "Existencias insuficientes.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
        }
        this.ActualizarCarrito();
      }
      else
      {
        int num = (int) MessageBox.Show("No hay ningún elemento con código " + Codigo + " registrado.", "No existen elementos con este código.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void BTN_AgregarAlCarrito_Click(object sender, EventArgs e) => this.AgregarElemento(this.TXT_AgregarAlCarrito.Text);

    private void TXT_AgregarAlCarrito_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TXT_AgregarAlCarrito.Lines).Count<string>() <= 1)
        return;
      SystemSounds.Asterisk.Play();
      this.AgregarElemento(this.TXT_AgregarAlCarrito.Text.Replace(Environment.NewLine ?? "", ""));
      this.TXT_AgregarAlCarrito.Text = "";
    }

    private void ButtonSeleccionarCliente_Click(object sender, EventArgs e)
    {
      IngresoDeCliente ingresoDeCliente = new IngresoDeCliente();
      ingresoDeCliente.Conn = this.Conn;
      if (ingresoDeCliente.ShowDialog() != DialogResult.OK || ingresoDeCliente.ListaVerClientes.SelectedRows.Count < 1)
        return;
      if (ingresoDeCliente.ListaVerClientes.SelectedRows[0].Index == ingresoDeCliente.ListaVerClientes.Rows.Count - 1)
      {
        this.ButtonSeleccionarCliente.Text = "Ninguno";
        this.ButtonSeleccionarCliente.Tag = (object) "";
      }
      else
      {
        this.ButtonSeleccionarCliente.Text = ingresoDeCliente.ListaVerClientes.SelectedRows[0].Cells[0].Value.ToString();
        this.ButtonSeleccionarCliente.Tag = (object) ingresoDeCliente.ListaVerClientes.SelectedRows[0].Cells[1].Value.ToString();
        for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
        {
          if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
          {
            for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
            {
              if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
                this.Carritos[index2].IDCliente = ingresoDeCliente.ListaVerClientes.SelectedRows[0].Cells[1].Value.ToString();
            }
          }
        }
      }
    }

    public bool Vender()
    {
      for (int index1 = this.PanelQueContienePestanas.Controls.Count - 1; index1 >= 0; --index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          for (int index2 = this.Carritos.Count - 1; index2 >= 0; --index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
            {
              OleDbCommand oleDbCommand = new OleDbCommand();
              oleDbCommand.Connection = this.Conn;
              oleDbCommand.Transaction = this.Conn.BeginTransaction();
              try
              {
                oleDbCommand.CommandText = "SELECT Max(Id) FROM Ventas; ";
                object obj = oleDbCommand.ExecuteScalar();
                int num1 = obj == null || obj.ToString() == "" ? 0 : int.Parse(obj.ToString()) + 1;
                for (int index3 = this.Carritos[index2].Codigos.Count - 1; index3 >= 0; --index3)
                {
                  oleDbCommand.CommandText = "UPDATE Inventario SET Cantidad = Cantidad - " + (object) this.Carritos[index2].Cantidades[index3] + " WHERE Codigo = '" + this.Carritos[index2].Codigos[index3] + "';";
                  oleDbCommand.ExecuteNonQuery();
                  oleDbCommand.CommandText = "DELETE FROM CarritosDeVentas WHERE IdDeCarrito = " + (object) this.Carritos[index2].IdDeCarrito + " AND CodigoDeProducto = '" + this.Carritos[index2].Codigos[index3] + "';";
                  oleDbCommand.ExecuteNonQuery();
                  oleDbCommand.CommandText = "SELECT PrecioUnitarioDeVenta FROM Inventario WHERE Codigo = '" + this.Carritos[index2].Codigos[index3] + "';";
                  double num2 = (double) oleDbCommand.ExecuteScalar();
                  double num3 = !this.Carritos[index2].IsPercent ? this.Carritos[index2].Descuento : num2 * (this.Carritos[index2].Descuento / 100.0);
                  string str1 = "";
                  if (this.Carritos[index2].AdjuntarNumeroDeFactura)
                    str1 = this.Carritos[index2].NumeroDeFactura;
                  string str2 = this.Carritos[index2].IDCliente == null || this.Carritos[index2].IDCliente == "" ? "Null" : "'" + this.Carritos[index2].IDCliente + "'";
                  oleDbCommand.CommandText = "INSERT INTO Ventas(Id,CodigoDeProducto,CantidadVendida,Descuento, Fecha,Hora,NoFactura,IDDeCliente,UsuarioVendedor) VALUES(" + (object) num1 + ",'" + this.Carritos[index2].Codigos[index3] + "'," + (object) this.Carritos[index2].Cantidades[index3] + "," + (object) num3 + ",NOW(),TIME(), '" + str1 + "', " + str2 + ", '" + this.TxBox_Vendedor.Text + "');";
                  oleDbCommand.ExecuteNonQuery();
                  if (num1 == -1)
                  {
                    oleDbCommand.Transaction.Rollback();
                    int num4 = (int) MessageBox.Show("La venta no se ha podido realizar debido a un error interno: El Id de la venta no puede ser nulo.");
                    break;
                  }
                  this.Carritos[index2].Codigos.RemoveAt(index3);
                  this.Carritos[index2].Cantidades.RemoveAt(index3);
                }
                oleDbCommand.CommandText = "DELETE FROM IdsDeCarrito WHERE Id = " + (object) this.Carritos[index2].IdDeCarrito + ";";
                oleDbCommand.Transaction.Commit();
                this.PanelQueContienePestanas.Controls.RemoveAt(index1);
                this.Carritos.RemoveAt(index2);
                if (this.PanelQueContienePestanas.Controls.Count > 0)
                  this.SeleccionarPestanaParaCarrito((Button) this.PanelQueContienePestanas.Controls[0]);
                this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
                return true;
              }
              catch (Exception ex)
              {
                oleDbCommand.Transaction.Rollback();
                int num = (int) MessageBox.Show("La venta no se ha podido realizar debido a un error interno: " + ex.Message + ".", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
            }
          }
        }
      }
      return false;
    }

    private void BTN_Vender_Click(object sender, EventArgs e) => this.Vender();

    private void CheckBoxTituloFacturar_CheckedChanged(object sender, EventArgs e)
    {
      this.PanelFacturar.Visible = this.CheckBoxTituloFacturar.Checked;
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
              this.Carritos[index2].AdjuntarNumeroDeFactura = this.CheckBoxTituloFacturar.Checked;
          }
        }
      }
    }

    private void StatusStrip_Paint(object sender, PaintEventArgs e) => this.StatusStrip.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.StatusStrip.Width, 0));

    private void PanelQueContienePestanas_Paint(object sender, PaintEventArgs e)
    {
      this.PanelQueContienePestanas.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelQueContienePestanas.Width, 0));
      this.PanelQueContienePestanas.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelQueContienePestanas.Height - 1), new Point(this.PanelQueContienePestanas.Width, this.PanelQueContienePestanas.Height - 1));
    }

    private void PanelCarrito_Paint(object sender, PaintEventArgs e)
    {
      this.PanelCarrito.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelCarrito.Width, 0));
      this.PanelCarrito.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelCarrito.Height - 1), new Point(this.PanelCarrito.Width, this.PanelCarrito.Height - 1));
    }

    private void PanelDeDetallesDeVenta_Paint(object sender, PaintEventArgs e)
    {
      this.PanelDeDetallesDeVenta.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelDeDetallesDeVenta.Width, 0));
      this.PanelDeDetallesDeVenta.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelDeDetallesDeVenta.Height - 1), new Point(this.PanelDeDetallesDeVenta.Width, this.PanelDeDetallesDeVenta.Height - 1));
    }

    private void PanelDeOtrosDetalles_Paint(object sender, PaintEventArgs e)
    {
      this.PanelDeOtrosDetalles.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelDeOtrosDetalles.Width, 0));
      this.PanelDeOtrosDetalles.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelDeOtrosDetalles.Height - 1), new Point(this.PanelDeOtrosDetalles.Width, this.PanelDeOtrosDetalles.Height - 1));
    }

    private void PanelRealizarVenta_Paint(object sender, PaintEventArgs e)
    {
      this.PanelRealizarVenta.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelRealizarVenta.Width, 0));
      this.PanelRealizarVenta.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelRealizarVenta.Height - 1), new Point(this.PanelRealizarVenta.Width, this.PanelRealizarVenta.Height - 1));
    }

    private void TxBox_NumeroDeFactura_TextChanged(object sender, EventArgs e)
    {
      for (int index1 = 0; index1 < this.PanelQueContienePestanas.Controls.Count; ++index1)
      {
        if (this.PanelQueContienePestanas.Controls[index1].Tag.ToString() == "Selected")
        {
          for (int index2 = 0; index2 < this.Carritos.Count; ++index2)
          {
            if (this.Carritos[index2].IdDeCarrito.ToString() == this.PanelQueContienePestanas.Controls[index1].Name.ToString().Substring(2))
              this.Carritos[index2].NumeroDeFactura = this.TxBox_NumeroDeFactura.Text;
          }
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Punto_De_Ventas));
      this.PanelLateralDerecho = new Panel();
      this.PanelRealizarVenta = new Panel();
      this.BTN_Vender = new Button();
      this.PanelFacturar = new Panel();
      this.TxBox_NumeroDeFactura = new TextBox();
      this.LabelNumeroDeFactura = new Label();
      this.PanelParaPestana = new Panel();
      this.PanelDeOtrosDetalles = new Panel();
      this.TxBoxFechaYHora = new TextBox();
      this.LabelFechaYHora = new Label();
      this.TxBox_Vendedor = new TextBox();
      this.LabelVendedor = new Label();
      this.ButtonSeleccionarCliente = new Button();
      this.LabelCliente = new Label();
      this.PanelDeDetallesDeVenta = new Panel();
      this.PanelLineaDeContorno_NumBox_Descuento = new Panel();
      this.NumBox_Descuento = new NumericUpDown();
      this.Panel_LineaDeContorno_ModosDeDescuento = new Panel();
      this.ListaMontosDeDescuento = new ComboBox();
      this.LabelDescuento = new Label();
      this.LabelISV = new Label();
      this.TxBox_Total = new TextBox();
      this.LabelTotal = new Label();
      this.TxBox_Monto = new TextBox();
      this.LabelMonto = new Label();
      this.TxBox_ISV = new TextBox();
      this.PanelCarrito = new Panel();
      this.ListaCarrito = new DataGridView();
      this.Codigo = new DataGridViewTextBoxColumn();
      this.Producto = new DataGridViewTextBoxColumn();
      this.Cantidad = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.ButtonCarrito = new Button();
      this.LabelProductos = new Label();
      this.PanelQueContienePestanasYControles = new Panel();
      this.PanelQueContienePestanas = new Panel();
      this.BotonPestanasALaDerecha = new Button();
      this.BotonPestanasALaIzquierda = new Button();
      this.BotonAgregarPestana = new Button();
      this.panel2 = new Panel();
      this.panel10 = new Panel();
      this.panel5 = new Panel();
      this.Panel6BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel4BusquedaNormal = new Panel();
      this.NumTxBuscarPorHasta = new NumericUpDown();
      this.CodLabelParaNúmerosHasta = new Label();
      this.Panel3BusquedaNormal = new Panel();
      this.NumTxBuscarPorDesde = new NumericUpDown();
      this.CodLabelParaNúmerosDesde = new Label();
      this.Panel2BusquedaNormal = new Panel();
      this.CodLabelParaTexto = new Label();
      this.TxBuscarPor = new TextBox();
      this.Panel1BusquedaNormal = new Panel();
      this.LabelBuscarPor = new Label();
      this.ListaBuscarPor = new ComboBox();
      this.panel4 = new Panel();
      this.panel11 = new Panel();
      this.BTN_AgregarAlCarrito = new Button();
      this.panel22 = new Panel();
      this.label8 = new Label();
      this.TXT_AgregarAlCarrito = new TextBox();
      this.ListaParaVerProductosAVender = new DataGridView();
      this.ColCodigo = new DataGridViewTextBoxColumn();
      this.ColProducto = new DataGridViewTextBoxColumn();
      this.ColCantidad = new DataGridViewTextBoxColumn();
      this.PrecioDeVenta = new DataGridViewTextBoxColumn();
      this.PrecioConImpuesto = new DataGridViewTextBoxColumn();
      this.Temporizador = new System.Windows.Forms.Timer(this.components);
      this.StatusStrip = new Panel();
      this.PanelSuperior = new Panel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.panel24 = new Panel();
      this.LabelAgregarProductoAlCarrito = new LinkLabel();
      this.PanelTituloRealizarVenta = new Panel();
      this.LinkLabelTituloRealizarVenta = new LinkLabel();
      this.PanelTituloFacturar = new Panel();
      this.LinkLabelTituloFacturar = new LinkLabel();
      this.CheckBoxTituloFacturar = new CheckBox();
      this.PanelTituloOtrosDetalles = new Panel();
      this.LinkLabelTituloOtrosDetalles = new LinkLabel();
      this.PanelTituloDetallesDeVenta = new Panel();
      this.LinkLabelTituloDetallesDeVenta = new LinkLabel();
      this.PanelTituloCarrito = new Panel();
      this.LinkLabelTituloCarrito = new LinkLabel();
      this.PanelLateralDerecho.SuspendLayout();
      this.PanelRealizarVenta.SuspendLayout();
      this.PanelFacturar.SuspendLayout();
      this.PanelParaPestana.SuspendLayout();
      this.PanelDeOtrosDetalles.SuspendLayout();
      this.PanelDeDetallesDeVenta.SuspendLayout();
      this.PanelLineaDeContorno_NumBox_Descuento.SuspendLayout();
      this.NumBox_Descuento.BeginInit();
      this.Panel_LineaDeContorno_ModosDeDescuento.SuspendLayout();
      this.PanelCarrito.SuspendLayout();
      ((ISupportInitialize) this.ListaCarrito).BeginInit();
      this.PanelQueContienePestanasYControles.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel5.SuspendLayout();
      this.Panel6BusquedaNormal.SuspendLayout();
      this.Panel4BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorHasta.BeginInit();
      this.Panel3BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorDesde.BeginInit();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel11.SuspendLayout();
      this.panel22.SuspendLayout();
      ((ISupportInitialize) this.ListaParaVerProductosAVender).BeginInit();
      this.panel6.SuspendLayout();
      this.panel24.SuspendLayout();
      this.PanelTituloRealizarVenta.SuspendLayout();
      this.PanelTituloFacturar.SuspendLayout();
      this.PanelTituloOtrosDetalles.SuspendLayout();
      this.PanelTituloDetallesDeVenta.SuspendLayout();
      this.PanelTituloCarrito.SuspendLayout();
      this.SuspendLayout();
      this.PanelLateralDerecho.AutoScroll = true;
      this.PanelLateralDerecho.BackColor = Color.DimGray;
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelRealizarVenta);
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelTituloRealizarVenta);
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelFacturar);
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelTituloFacturar);
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelParaPestana);
      this.PanelLateralDerecho.Controls.Add((Control) this.PanelQueContienePestanasYControles);
      this.PanelLateralDerecho.Dock = DockStyle.Right;
      this.PanelLateralDerecho.Location = new Point(615, 50);
      this.PanelLateralDerecho.Margin = new Padding(4);
      this.PanelLateralDerecho.Name = "PanelLateralDerecho";
      this.PanelLateralDerecho.Padding = new Padding(1, 0, 0, 0);
      this.PanelLateralDerecho.Size = new Size(430, 677);
      this.PanelLateralDerecho.TabIndex = 3;
      this.PanelRealizarVenta.BackColor = SystemColors.Control;
      this.PanelRealizarVenta.Controls.Add((Control) this.BTN_Vender);
      this.PanelRealizarVenta.Dock = DockStyle.Top;
      this.PanelRealizarVenta.Location = new Point(1, 809);
      this.PanelRealizarVenta.Margin = new Padding(4);
      this.PanelRealizarVenta.Name = "PanelRealizarVenta";
      this.PanelRealizarVenta.Size = new Size(412, 58);
      this.PanelRealizarVenta.TabIndex = 17;
      this.PanelRealizarVenta.Paint += new PaintEventHandler(this.PanelRealizarVenta_Paint);
      this.BTN_Vender.BackColor = Color.DimGray;
      this.BTN_Vender.FlatStyle = FlatStyle.Flat;
      this.BTN_Vender.Location = new Point(14, 13);
      this.BTN_Vender.Margin = new Padding(4);
      this.BTN_Vender.Name = "BTN_Vender";
      this.BTN_Vender.Size = new Size(379, 28);
      this.BTN_Vender.TabIndex = 17;
      this.BTN_Vender.Text = "Vender";
      this.BTN_Vender.UseVisualStyleBackColor = false;
      this.BTN_Vender.Click += new EventHandler(this.BTN_Vender_Click);
      this.PanelFacturar.BackColor = SystemColors.Control;
      this.PanelFacturar.Controls.Add((Control) this.TxBox_NumeroDeFactura);
      this.PanelFacturar.Controls.Add((Control) this.LabelNumeroDeFactura);
      this.PanelFacturar.Dock = DockStyle.Top;
      this.PanelFacturar.Location = new Point(1, 735);
      this.PanelFacturar.Margin = new Padding(4);
      this.PanelFacturar.Name = "PanelFacturar";
      this.PanelFacturar.Size = new Size(412, 49);
      this.PanelFacturar.TabIndex = 19;
      this.PanelFacturar.Visible = false;
      this.TxBox_NumeroDeFactura.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_NumeroDeFactura.Location = new Point(155, 13);
      this.TxBox_NumeroDeFactura.Margin = new Padding(4);
      this.TxBox_NumeroDeFactura.Name = "TxBox_NumeroDeFactura";
      this.TxBox_NumeroDeFactura.Size = new Size(239, 23);
      this.TxBox_NumeroDeFactura.TabIndex = 8;
      this.TxBox_NumeroDeFactura.TextChanged += new EventHandler(this.TxBox_NumeroDeFactura_TextChanged);
      this.LabelNumeroDeFactura.AutoSize = true;
      this.LabelNumeroDeFactura.Location = new Point(11, 15);
      this.LabelNumeroDeFactura.Margin = new Padding(4, 0, 4, 0);
      this.LabelNumeroDeFactura.Name = "LabelNumeroDeFactura";
      this.LabelNumeroDeFactura.Size = new Size(136, 17);
      this.LabelNumeroDeFactura.TabIndex = 7;
      this.LabelNumeroDeFactura.Text = "Número De Factura:";
      this.PanelParaPestana.BackColor = Color.DimGray;
      this.PanelParaPestana.Controls.Add((Control) this.PanelDeOtrosDetalles);
      this.PanelParaPestana.Controls.Add((Control) this.PanelTituloOtrosDetalles);
      this.PanelParaPestana.Controls.Add((Control) this.PanelDeDetallesDeVenta);
      this.PanelParaPestana.Controls.Add((Control) this.PanelTituloDetallesDeVenta);
      this.PanelParaPestana.Controls.Add((Control) this.PanelCarrito);
      this.PanelParaPestana.Controls.Add((Control) this.PanelTituloCarrito);
      this.PanelParaPestana.Dock = DockStyle.Top;
      this.PanelParaPestana.Location = new Point(1, 30);
      this.PanelParaPestana.Margin = new Padding(4);
      this.PanelParaPestana.Name = "PanelParaPestana";
      this.PanelParaPestana.Size = new Size(412, 680);
      this.PanelParaPestana.TabIndex = 11;
      this.PanelDeOtrosDetalles.BackColor = SystemColors.Control;
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.TxBoxFechaYHora);
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.LabelFechaYHora);
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.TxBox_Vendedor);
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.LabelVendedor);
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.ButtonSeleccionarCliente);
      this.PanelDeOtrosDetalles.Controls.Add((Control) this.LabelCliente);
      this.PanelDeOtrosDetalles.Dock = DockStyle.Top;
      this.PanelDeOtrosDetalles.Location = new Point(0, 566);
      this.PanelDeOtrosDetalles.Margin = new Padding(4);
      this.PanelDeOtrosDetalles.Name = "PanelDeOtrosDetalles";
      this.PanelDeOtrosDetalles.Size = new Size(412, 118);
      this.PanelDeOtrosDetalles.TabIndex = 15;
      this.PanelDeOtrosDetalles.Paint += new PaintEventHandler(this.PanelDeOtrosDetalles_Paint);
      this.TxBoxFechaYHora.BorderStyle = BorderStyle.FixedSingle;
      this.TxBoxFechaYHora.Location = new Point(118, 79);
      this.TxBoxFechaYHora.Margin = new Padding(4);
      this.TxBoxFechaYHora.Name = "TxBoxFechaYHora";
      this.TxBoxFechaYHora.ReadOnly = true;
      this.TxBoxFechaYHora.Size = new Size(276, 23);
      this.TxBoxFechaYHora.TabIndex = 20;
      this.LabelFechaYHora.AutoSize = true;
      this.LabelFechaYHora.Location = new Point(11, 81);
      this.LabelFechaYHora.Margin = new Padding(4, 0, 4, 0);
      this.LabelFechaYHora.Name = "LabelFechaYHora";
      this.LabelFechaYHora.Size = new Size(99, 17);
      this.LabelFechaYHora.TabIndex = 19;
      this.LabelFechaYHora.Text = "Fecha Y Hora:";
      this.TxBox_Vendedor.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_Vendedor.Location = new Point(118, 48);
      this.TxBox_Vendedor.Margin = new Padding(4);
      this.TxBox_Vendedor.Name = "TxBox_Vendedor";
      this.TxBox_Vendedor.ReadOnly = true;
      this.TxBox_Vendedor.Size = new Size(276, 23);
      this.TxBox_Vendedor.TabIndex = 18;
      this.LabelVendedor.AutoSize = true;
      this.LabelVendedor.Location = new Point(11, 50);
      this.LabelVendedor.Margin = new Padding(4, 0, 4, 0);
      this.LabelVendedor.Name = "LabelVendedor";
      this.LabelVendedor.Size = new Size(74, 17);
      this.LabelVendedor.TabIndex = 17;
      this.LabelVendedor.Text = "Vendedor:";
      this.ButtonSeleccionarCliente.FlatStyle = FlatStyle.Flat;
      this.ButtonSeleccionarCliente.Location = new Point(118, 13);
      this.ButtonSeleccionarCliente.Name = "ButtonSeleccionarCliente";
      this.ButtonSeleccionarCliente.Size = new Size(276, 28);
      this.ButtonSeleccionarCliente.TabIndex = 16;
      this.ButtonSeleccionarCliente.Text = "Ninguno";
      this.ButtonSeleccionarCliente.UseVisualStyleBackColor = true;
      this.ButtonSeleccionarCliente.Click += new EventHandler(this.ButtonSeleccionarCliente_Click);
      this.LabelCliente.AutoSize = true;
      this.LabelCliente.Location = new Point(11, 19);
      this.LabelCliente.Margin = new Padding(4, 0, 4, 0);
      this.LabelCliente.Name = "LabelCliente";
      this.LabelCliente.Size = new Size(55, 17);
      this.LabelCliente.TabIndex = 15;
      this.LabelCliente.Text = "Cliente:";
      this.PanelDeDetallesDeVenta.BackColor = SystemColors.Control;
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.PanelLineaDeContorno_NumBox_Descuento);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.Panel_LineaDeContorno_ModosDeDescuento);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.LabelDescuento);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.LabelISV);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.TxBox_Total);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.LabelTotal);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.TxBox_Monto);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.LabelMonto);
      this.PanelDeDetallesDeVenta.Controls.Add((Control) this.TxBox_ISV);
      this.PanelDeDetallesDeVenta.Dock = DockStyle.Top;
      this.PanelDeDetallesDeVenta.Location = new Point(0, 393);
      this.PanelDeDetallesDeVenta.Margin = new Padding(4);
      this.PanelDeDetallesDeVenta.Name = "PanelDeDetallesDeVenta";
      this.PanelDeDetallesDeVenta.Size = new Size(412, 148);
      this.PanelDeDetallesDeVenta.TabIndex = 13;
      this.PanelDeDetallesDeVenta.Paint += new PaintEventHandler(this.PanelDeDetallesDeVenta_Paint);
      this.PanelLineaDeContorno_NumBox_Descuento.BackColor = Color.Black;
      this.PanelLineaDeContorno_NumBox_Descuento.Controls.Add((Control) this.NumBox_Descuento);
      this.PanelLineaDeContorno_NumBox_Descuento.Location = new Point(100, 44);
      this.PanelLineaDeContorno_NumBox_Descuento.Name = "PanelLineaDeContorno_NumBox_Descuento";
      this.PanelLineaDeContorno_NumBox_Descuento.Padding = new Padding(1);
      this.PanelLineaDeContorno_NumBox_Descuento.Size = new Size(234, 24);
      this.PanelLineaDeContorno_NumBox_Descuento.TabIndex = 14;
      this.NumBox_Descuento.BorderStyle = BorderStyle.None;
      this.NumBox_Descuento.DecimalPlaces = 2;
      this.NumBox_Descuento.Dock = DockStyle.Fill;
      this.NumBox_Descuento.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.NumBox_Descuento.Location = new Point(1, 1);
      this.NumBox_Descuento.Maximum = new Decimal(new int[4]
      {
        999999999,
        0,
        0,
        0
      });
      this.NumBox_Descuento.Minimum = new Decimal(new int[4]
      {
        999999999,
        0,
        0,
        int.MinValue
      });
      this.NumBox_Descuento.Name = "NumBox_Descuento";
      this.NumBox_Descuento.Size = new Size(232, 22);
      this.NumBox_Descuento.TabIndex = 11;
      this.NumBox_Descuento.ValueChanged += new EventHandler(this.NumBox_Descuento_ValueChanged);
      this.Panel_LineaDeContorno_ModosDeDescuento.BackColor = Color.Black;
      this.Panel_LineaDeContorno_ModosDeDescuento.Controls.Add((Control) this.ListaMontosDeDescuento);
      this.Panel_LineaDeContorno_ModosDeDescuento.Location = new Point(340, 44);
      this.Panel_LineaDeContorno_ModosDeDescuento.Name = "Panel_LineaDeContorno_ModosDeDescuento";
      this.Panel_LineaDeContorno_ModosDeDescuento.Padding = new Padding(1);
      this.Panel_LineaDeContorno_ModosDeDescuento.Size = new Size(54, 26);
      this.Panel_LineaDeContorno_ModosDeDescuento.TabIndex = 12;
      this.ListaMontosDeDescuento.Dock = DockStyle.Fill;
      this.ListaMontosDeDescuento.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ListaMontosDeDescuento.FlatStyle = FlatStyle.Flat;
      this.ListaMontosDeDescuento.Font = new Font("Microsoft Sans Serif", 10f);
      this.ListaMontosDeDescuento.FormattingEnabled = true;
      this.ListaMontosDeDescuento.ItemHeight = 16;
      this.ListaMontosDeDescuento.Items.AddRange(new object[2]
      {
        (object) "L.",
        (object) "%"
      });
      this.ListaMontosDeDescuento.Location = new Point(1, 1);
      this.ListaMontosDeDescuento.Name = "ListaMontosDeDescuento";
      this.ListaMontosDeDescuento.Size = new Size(52, 24);
      this.ListaMontosDeDescuento.TabIndex = 5;
      this.ListaMontosDeDescuento.SelectedIndexChanged += new EventHandler(this.ListaMontosDeDescuento_SelectedIndexChanged);
      this.LabelDescuento.AutoSize = true;
      this.LabelDescuento.Location = new Point(11, 47);
      this.LabelDescuento.Margin = new Padding(4, 0, 4, 0);
      this.LabelDescuento.Name = "LabelDescuento";
      this.LabelDescuento.Size = new Size(80, 17);
      this.LabelDescuento.TabIndex = 9;
      this.LabelDescuento.Text = "Descuento:";
      this.LabelISV.AutoSize = true;
      this.LabelISV.Location = new Point(11, 77);
      this.LabelISV.Margin = new Padding(4, 0, 4, 0);
      this.LabelISV.Name = "LabelISV";
      this.LabelISV.Size = new Size(41, 17);
      this.LabelISV.TabIndex = 7;
      this.LabelISV.Text = "I.S.V:";
      this.TxBox_Total.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_Total.Location = new Point(100, 106);
      this.TxBox_Total.Margin = new Padding(4);
      this.TxBox_Total.Name = "TxBox_Total";
      this.TxBox_Total.ReadOnly = true;
      this.TxBox_Total.Size = new Size(294, 23);
      this.TxBox_Total.TabIndex = 6;
      this.TxBox_Total.Text = "0";
      this.LabelTotal.AutoSize = true;
      this.LabelTotal.Location = new Point(11, 108);
      this.LabelTotal.Margin = new Padding(4, 0, 4, 0);
      this.LabelTotal.Name = "LabelTotal";
      this.LabelTotal.Size = new Size(44, 17);
      this.LabelTotal.TabIndex = 5;
      this.LabelTotal.Text = "Total:";
      this.TxBox_Monto.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_Monto.Location = new Point(100, 13);
      this.TxBox_Monto.Margin = new Padding(4);
      this.TxBox_Monto.Name = "TxBox_Monto";
      this.TxBox_Monto.ReadOnly = true;
      this.TxBox_Monto.Size = new Size(294, 23);
      this.TxBox_Monto.TabIndex = 6;
      this.TxBox_Monto.Text = "0";
      this.LabelMonto.AutoSize = true;
      this.LabelMonto.Location = new Point(11, 15);
      this.LabelMonto.Margin = new Padding(4, 0, 4, 0);
      this.LabelMonto.Name = "LabelMonto";
      this.LabelMonto.Size = new Size(51, 17);
      this.LabelMonto.TabIndex = 5;
      this.LabelMonto.Text = "Monto:";
      this.TxBox_ISV.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_ISV.Location = new Point(100, 75);
      this.TxBox_ISV.Margin = new Padding(4);
      this.TxBox_ISV.Name = "TxBox_ISV";
      this.TxBox_ISV.ReadOnly = true;
      this.TxBox_ISV.Size = new Size(294, 23);
      this.TxBox_ISV.TabIndex = 5;
      this.TxBox_ISV.Text = "0";
      this.PanelCarrito.BackColor = SystemColors.Control;
      this.PanelCarrito.Controls.Add((Control) this.ListaCarrito);
      this.PanelCarrito.Controls.Add((Control) this.ButtonCarrito);
      this.PanelCarrito.Controls.Add((Control) this.LabelProductos);
      this.PanelCarrito.Dock = DockStyle.Top;
      this.PanelCarrito.Location = new Point(0, 25);
      this.PanelCarrito.Margin = new Padding(4);
      this.PanelCarrito.Name = "PanelCarrito";
      this.PanelCarrito.Size = new Size(412, 343);
      this.PanelCarrito.TabIndex = 11;
      this.PanelCarrito.Paint += new PaintEventHandler(this.PanelCarrito_Paint);
      this.ListaCarrito.AllowUserToAddRows = false;
      this.ListaCarrito.AllowUserToResizeRows = false;
      this.ListaCarrito.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaCarrito.Columns.AddRange((DataGridViewColumn) this.Codigo, (DataGridViewColumn) this.Producto, (DataGridViewColumn) this.Cantidad, (DataGridViewColumn) this.dataGridViewTextBoxColumn1);
      this.ListaCarrito.Location = new Point(14, 28);
      this.ListaCarrito.Name = "ListaCarrito";
      this.ListaCarrito.ReadOnly = true;
      this.ListaCarrito.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      this.ListaCarrito.Size = new Size(380, 264);
      this.ListaCarrito.TabIndex = 6;
      this.Codigo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Codigo.HeaderText = "Código";
      this.Codigo.Name = "Codigo";
      this.Codigo.ReadOnly = true;
      this.Producto.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Producto.HeaderText = "Producto";
      this.Producto.Name = "Producto";
      this.Producto.ReadOnly = true;
      this.Cantidad.HeaderText = "Cantidad";
      this.Cantidad.Name = "Cantidad";
      this.Cantidad.ReadOnly = true;
      this.Cantidad.Width = 70;
      this.dataGridViewTextBoxColumn1.HeaderText = "Precio";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 90;
      this.ButtonCarrito.BackColor = Color.DimGray;
      this.ButtonCarrito.FlatStyle = FlatStyle.Flat;
      this.ButtonCarrito.Location = new Point(14, 302);
      this.ButtonCarrito.Margin = new Padding(4);
      this.ButtonCarrito.Name = "ButtonCarrito";
      this.ButtonCarrito.Size = new Size(380, 28);
      this.ButtonCarrito.TabIndex = 5;
      this.ButtonCarrito.Text = "Añadir";
      this.ButtonCarrito.UseVisualStyleBackColor = false;
      this.ButtonCarrito.Click += new EventHandler(this.ButtonCarrito_Click);
      this.LabelProductos.AutoSize = true;
      this.LabelProductos.Location = new Point(11, 8);
      this.LabelProductos.Margin = new Padding(4, 0, 4, 0);
      this.LabelProductos.Name = "LabelProductos";
      this.LabelProductos.Size = new Size(76, 17);
      this.LabelProductos.TabIndex = 5;
      this.LabelProductos.Text = "Productos:";
      this.PanelQueContienePestanasYControles.BackColor = Color.LightGray;
      this.PanelQueContienePestanasYControles.Controls.Add((Control) this.PanelQueContienePestanas);
      this.PanelQueContienePestanasYControles.Controls.Add((Control) this.BotonPestanasALaDerecha);
      this.PanelQueContienePestanasYControles.Controls.Add((Control) this.BotonPestanasALaIzquierda);
      this.PanelQueContienePestanasYControles.Controls.Add((Control) this.BotonAgregarPestana);
      this.PanelQueContienePestanasYControles.Dock = DockStyle.Top;
      this.PanelQueContienePestanasYControles.Location = new Point(1, 0);
      this.PanelQueContienePestanasYControles.Name = "PanelQueContienePestanasYControles";
      this.PanelQueContienePestanasYControles.Size = new Size(412, 30);
      this.PanelQueContienePestanasYControles.TabIndex = 10;
      this.PanelQueContienePestanas.BackColor = SystemColors.Control;
      this.PanelQueContienePestanas.Dock = DockStyle.Fill;
      this.PanelQueContienePestanas.Location = new Point(23, 0);
      this.PanelQueContienePestanas.Name = "PanelQueContienePestanas";
      this.PanelQueContienePestanas.Size = new Size(343, 30);
      this.PanelQueContienePestanas.TabIndex = 14;
      this.PanelQueContienePestanas.Paint += new PaintEventHandler(this.PanelQueContienePestanas_Paint);
      this.BotonPestanasALaDerecha.BackColor = Color.DimGray;
      this.BotonPestanasALaDerecha.Dock = DockStyle.Right;
      this.BotonPestanasALaDerecha.FlatStyle = FlatStyle.Flat;
      this.BotonPestanasALaDerecha.Location = new Point(366, 0);
      this.BotonPestanasALaDerecha.Name = "BotonPestanasALaDerecha";
      this.BotonPestanasALaDerecha.Size = new Size(23, 30);
      this.BotonPestanasALaDerecha.TabIndex = 13;
      this.BotonPestanasALaDerecha.Text = ">";
      this.BotonPestanasALaDerecha.UseVisualStyleBackColor = false;
      this.BotonPestanasALaDerecha.Click += new EventHandler(this.BotonPestanasALaDerecha_Click);
      this.BotonPestanasALaIzquierda.BackColor = Color.DimGray;
      this.BotonPestanasALaIzquierda.Dock = DockStyle.Left;
      this.BotonPestanasALaIzquierda.FlatStyle = FlatStyle.Flat;
      this.BotonPestanasALaIzquierda.Location = new Point(0, 0);
      this.BotonPestanasALaIzquierda.Name = "BotonPestanasALaIzquierda";
      this.BotonPestanasALaIzquierda.Size = new Size(23, 30);
      this.BotonPestanasALaIzquierda.TabIndex = 12;
      this.BotonPestanasALaIzquierda.Text = "<";
      this.BotonPestanasALaIzquierda.UseVisualStyleBackColor = false;
      this.BotonPestanasALaIzquierda.Click += new EventHandler(this.BotonPestanasALaIzquierda_Click);
      this.BotonAgregarPestana.BackColor = Color.DimGray;
      this.BotonAgregarPestana.Dock = DockStyle.Right;
      this.BotonAgregarPestana.FlatStyle = FlatStyle.Flat;
      this.BotonAgregarPestana.Location = new Point(389, 0);
      this.BotonAgregarPestana.Name = "BotonAgregarPestana";
      this.BotonAgregarPestana.Size = new Size(23, 30);
      this.BotonAgregarPestana.TabIndex = 16;
      this.BotonAgregarPestana.Text = "+";
      this.BotonAgregarPestana.UseVisualStyleBackColor = false;
      this.BotonAgregarPestana.Click += new EventHandler(this.BotonAgregarPestana_Click);
      this.panel2.BackColor = Color.DimGray;
      this.panel2.Controls.Add((Control) this.panel10);
      this.panel2.Controls.Add((Control) this.panel5);
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Controls.Add((Control) this.panel4);
      this.panel2.Controls.Add((Control) this.panel24);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 50);
      this.panel2.Margin = new Padding(4);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(0, 0, 1, 0);
      this.panel2.Size = new Size(300, 677);
      this.panel2.TabIndex = 7;
      this.panel10.BackColor = SystemColors.ScrollBar;
      this.panel10.Dock = DockStyle.Fill;
      this.panel10.Location = new Point(0, 309);
      this.panel10.Name = "panel10";
      this.panel10.Size = new Size(299, 368);
      this.panel10.TabIndex = 15;
      this.panel5.AutoSize = true;
      this.panel5.BackColor = Color.DimGray;
      this.panel5.Controls.Add((Control) this.Panel6BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel4BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel3BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel2BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel1BusquedaNormal);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 132);
      this.panel5.Margin = new Padding(4);
      this.panel5.Name = "panel5";
      this.panel5.Padding = new Padding(0, 1, 0, 1);
      this.panel5.Size = new Size(299, 177);
      this.panel5.TabIndex = 8;
      this.Panel6BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel6BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel6BusquedaNormal.Dock = DockStyle.Top;
      this.Panel6BusquedaNormal.Location = new Point(0, 133);
      this.Panel6BusquedaNormal.Name = "Panel6BusquedaNormal";
      this.Panel6BusquedaNormal.Size = new Size(299, 43);
      this.Panel6BusquedaNormal.TabIndex = 17;
      this.BtnBuscarPor.BackColor = Color.DimGray;
      this.BtnBuscarPor.FlatStyle = FlatStyle.Flat;
      this.BtnBuscarPor.Location = new Point(10, 7);
      this.BtnBuscarPor.Margin = new Padding(4);
      this.BtnBuscarPor.Name = "BtnBuscarPor";
      this.BtnBuscarPor.Size = new Size(275, 28);
      this.BtnBuscarPor.TabIndex = 5;
      this.BtnBuscarPor.Text = "Buscar";
      this.BtnBuscarPor.UseVisualStyleBackColor = false;
      this.BtnBuscarPor.Click += new EventHandler(this.BtnBuscarPor_Click);
      this.Panel4BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel4BusquedaNormal.Controls.Add((Control) this.NumTxBuscarPorHasta);
      this.Panel4BusquedaNormal.Controls.Add((Control) this.CodLabelParaNúmerosHasta);
      this.Panel4BusquedaNormal.Dock = DockStyle.Top;
      this.Panel4BusquedaNormal.Location = new Point(0, 101);
      this.Panel4BusquedaNormal.Name = "Panel4BusquedaNormal";
      this.Panel4BusquedaNormal.Size = new Size(299, 32);
      this.Panel4BusquedaNormal.TabIndex = 15;
      this.Panel4BusquedaNormal.Visible = false;
      this.NumTxBuscarPorHasta.BorderStyle = BorderStyle.FixedSingle;
      this.NumTxBuscarPorHasta.DecimalPlaces = 2;
      this.NumTxBuscarPorHasta.Location = new Point(100, 5);
      this.NumTxBuscarPorHasta.Maximum = new Decimal(new int[4]
      {
        1215752191,
        23,
        0,
        0
      });
      this.NumTxBuscarPorHasta.Name = "NumTxBuscarPorHasta";
      this.NumTxBuscarPorHasta.Size = new Size(186, 23);
      this.NumTxBuscarPorHasta.TabIndex = 11;
      this.CodLabelParaNúmerosHasta.AutoSize = true;
      this.CodLabelParaNúmerosHasta.Location = new Point(12, 7);
      this.CodLabelParaNúmerosHasta.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaNúmerosHasta.Name = "CodLabelParaNúmerosHasta";
      this.CodLabelParaNúmerosHasta.Size = new Size(59, 17);
      this.CodLabelParaNúmerosHasta.TabIndex = 5;
      this.CodLabelParaNúmerosHasta.Text = "Maximo:";
      this.Panel3BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel3BusquedaNormal.Controls.Add((Control) this.NumTxBuscarPorDesde);
      this.Panel3BusquedaNormal.Controls.Add((Control) this.CodLabelParaNúmerosDesde);
      this.Panel3BusquedaNormal.Dock = DockStyle.Top;
      this.Panel3BusquedaNormal.Location = new Point(0, 69);
      this.Panel3BusquedaNormal.Name = "Panel3BusquedaNormal";
      this.Panel3BusquedaNormal.Size = new Size(299, 32);
      this.Panel3BusquedaNormal.TabIndex = 14;
      this.Panel3BusquedaNormal.Visible = false;
      this.NumTxBuscarPorDesde.BorderStyle = BorderStyle.FixedSingle;
      this.NumTxBuscarPorDesde.DecimalPlaces = 2;
      this.NumTxBuscarPorDesde.Location = new Point(100, 5);
      this.NumTxBuscarPorDesde.Maximum = new Decimal(new int[4]
      {
        1215752191,
        23,
        0,
        0
      });
      this.NumTxBuscarPorDesde.Name = "NumTxBuscarPorDesde";
      this.NumTxBuscarPorDesde.Size = new Size(186, 23);
      this.NumTxBuscarPorDesde.TabIndex = 11;
      this.CodLabelParaNúmerosDesde.AutoSize = true;
      this.CodLabelParaNúmerosDesde.Location = new Point(12, 7);
      this.CodLabelParaNúmerosDesde.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaNúmerosDesde.Name = "CodLabelParaNúmerosDesde";
      this.CodLabelParaNúmerosDesde.Size = new Size(56, 17);
      this.CodLabelParaNúmerosDesde.TabIndex = 5;
      this.CodLabelParaNúmerosDesde.Text = "Mínimo:";
      this.Panel2BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel2BusquedaNormal.Controls.Add((Control) this.CodLabelParaTexto);
      this.Panel2BusquedaNormal.Controls.Add((Control) this.TxBuscarPor);
      this.Panel2BusquedaNormal.Dock = DockStyle.Top;
      this.Panel2BusquedaNormal.Location = new Point(0, 37);
      this.Panel2BusquedaNormal.Name = "Panel2BusquedaNormal";
      this.Panel2BusquedaNormal.Size = new Size(299, 32);
      this.Panel2BusquedaNormal.TabIndex = 13;
      this.CodLabelParaTexto.AutoSize = true;
      this.CodLabelParaTexto.Location = new Point(12, 7);
      this.CodLabelParaTexto.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaTexto.Name = "CodLabelParaTexto";
      this.CodLabelParaTexto.Size = new Size(43, 17);
      this.CodLabelParaTexto.TabIndex = 5;
      this.CodLabelParaTexto.Text = "Filtro:";
      this.TxBuscarPor.BorderStyle = BorderStyle.FixedSingle;
      this.TxBuscarPor.Location = new Point(100, 5);
      this.TxBuscarPor.Margin = new Padding(4);
      this.TxBuscarPor.Multiline = true;
      this.TxBuscarPor.Name = "TxBuscarPor";
      this.TxBuscarPor.Size = new Size(186, 23);
      this.TxBuscarPor.TabIndex = 5;
      this.TxBuscarPor.TextChanged += new EventHandler(this.TxBuscarPor_TextChanged);
      this.Panel1BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel1BusquedaNormal.Controls.Add((Control) this.LabelBuscarPor);
      this.Panel1BusquedaNormal.Controls.Add((Control) this.ListaBuscarPor);
      this.Panel1BusquedaNormal.Dock = DockStyle.Top;
      this.Panel1BusquedaNormal.Location = new Point(0, 1);
      this.Panel1BusquedaNormal.Name = "Panel1BusquedaNormal";
      this.Panel1BusquedaNormal.Size = new Size(299, 36);
      this.Panel1BusquedaNormal.TabIndex = 12;
      this.LabelBuscarPor.AutoSize = true;
      this.LabelBuscarPor.Location = new Point(12, 13);
      this.LabelBuscarPor.Margin = new Padding(4, 0, 4, 0);
      this.LabelBuscarPor.Name = "LabelBuscarPor";
      this.LabelBuscarPor.Size = new Size(81, 17);
      this.LabelBuscarPor.TabIndex = 6;
      this.LabelBuscarPor.Text = "Buscar por:";
      this.ListaBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ListaBuscarPor.FormattingEnabled = true;
      this.ListaBuscarPor.Items.AddRange(new object[5]
      {
        (object) "Código",
        (object) "Producto",
        (object) "Cantidad",
        (object) "Precio De Venta Por Unidad",
        (object) "Precio Con I.S.V"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.panel4.AutoSize = true;
      this.panel4.BackColor = Color.DimGray;
      this.panel4.Controls.Add((Control) this.panel11);
      this.panel4.Controls.Add((Control) this.panel22);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 25);
      this.panel4.Margin = new Padding(4);
      this.panel4.Name = "panel4";
      this.panel4.Padding = new Padding(0, 1, 0, 1);
      this.panel4.Size = new Size(299, 82);
      this.panel4.TabIndex = 13;
      this.panel11.BackColor = SystemColors.Control;
      this.panel11.Controls.Add((Control) this.BTN_AgregarAlCarrito);
      this.panel11.Dock = DockStyle.Top;
      this.panel11.Location = new Point(0, 38);
      this.panel11.Name = "panel11";
      this.panel11.Size = new Size(299, 43);
      this.panel11.TabIndex = 17;
      this.BTN_AgregarAlCarrito.BackColor = Color.DimGray;
      this.BTN_AgregarAlCarrito.FlatStyle = FlatStyle.Flat;
      this.BTN_AgregarAlCarrito.Location = new Point(10, 7);
      this.BTN_AgregarAlCarrito.Margin = new Padding(4);
      this.BTN_AgregarAlCarrito.Name = "BTN_AgregarAlCarrito";
      this.BTN_AgregarAlCarrito.Size = new Size(275, 28);
      this.BTN_AgregarAlCarrito.TabIndex = 5;
      this.BTN_AgregarAlCarrito.Text = "Agregar";
      this.BTN_AgregarAlCarrito.UseVisualStyleBackColor = false;
      this.BTN_AgregarAlCarrito.Click += new EventHandler(this.BTN_AgregarAlCarrito_Click);
      this.panel22.BackColor = SystemColors.Control;
      this.panel22.Controls.Add((Control) this.label8);
      this.panel22.Controls.Add((Control) this.TXT_AgregarAlCarrito);
      this.panel22.Dock = DockStyle.Top;
      this.panel22.Location = new Point(0, 1);
      this.panel22.Name = "panel22";
      this.panel22.Size = new Size(299, 37);
      this.panel22.TabIndex = 13;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 12);
      this.label8.Margin = new Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(56, 17);
      this.label8.TabIndex = 5;
      this.label8.Text = "Código:";
      this.TXT_AgregarAlCarrito.BorderStyle = BorderStyle.FixedSingle;
      this.TXT_AgregarAlCarrito.Location = new Point(100, 10);
      this.TXT_AgregarAlCarrito.Margin = new Padding(4);
      this.TXT_AgregarAlCarrito.Multiline = true;
      this.TXT_AgregarAlCarrito.Name = "TXT_AgregarAlCarrito";
      this.TXT_AgregarAlCarrito.Size = new Size(186, 23);
      this.TXT_AgregarAlCarrito.TabIndex = 5;
      this.TXT_AgregarAlCarrito.TextChanged += new EventHandler(this.TXT_AgregarAlCarrito_TextChanged);
      this.ListaParaVerProductosAVender.AllowUserToAddRows = false;
      this.ListaParaVerProductosAVender.AllowUserToDeleteRows = false;
      this.ListaParaVerProductosAVender.AllowUserToOrderColumns = true;
      this.ListaParaVerProductosAVender.BackgroundColor = SystemColors.ScrollBar;
      this.ListaParaVerProductosAVender.BorderStyle = BorderStyle.None;
      this.ListaParaVerProductosAVender.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaParaVerProductosAVender.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaParaVerProductosAVender.Columns.AddRange((DataGridViewColumn) this.ColCodigo, (DataGridViewColumn) this.ColProducto, (DataGridViewColumn) this.ColCantidad, (DataGridViewColumn) this.PrecioDeVenta, (DataGridViewColumn) this.PrecioConImpuesto);
      this.ListaParaVerProductosAVender.Dock = DockStyle.Fill;
      this.ListaParaVerProductosAVender.GridColor = Color.Gray;
      this.ListaParaVerProductosAVender.Location = new Point(300, 50);
      this.ListaParaVerProductosAVender.Margin = new Padding(4);
      this.ListaParaVerProductosAVender.Name = "ListaParaVerProductosAVender";
      this.ListaParaVerProductosAVender.ReadOnly = true;
      this.ListaParaVerProductosAVender.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaParaVerProductosAVender.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.ListaParaVerProductosAVender.Size = new Size(315, 677);
      this.ListaParaVerProductosAVender.TabIndex = 10;
      this.ColCodigo.HeaderText = "Código";
      this.ColCodigo.Name = "ColCodigo";
      this.ColCodigo.ReadOnly = true;
      this.ColCodigo.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.ColProducto.HeaderText = "Producto";
      this.ColProducto.Name = "ColProducto";
      this.ColProducto.ReadOnly = true;
      this.ColProducto.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle1.NullValue = (object) "0";
      this.ColCantidad.DefaultCellStyle = gridViewCellStyle1;
      this.ColCantidad.HeaderText = "Cantidad";
      this.ColCantidad.Name = "ColCantidad";
      this.ColCantidad.ReadOnly = true;
      this.ColCantidad.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle2.NullValue = (object) "0";
      this.PrecioDeVenta.DefaultCellStyle = gridViewCellStyle2;
      this.PrecioDeVenta.HeaderText = "Precio De Venta Por Unidad";
      this.PrecioDeVenta.Name = "PrecioDeVenta";
      this.PrecioDeVenta.ReadOnly = true;
      this.PrecioDeVenta.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.PrecioDeVenta.Width = 160;
      gridViewCellStyle3.NullValue = (object) "0";
      this.PrecioConImpuesto.DefaultCellStyle = gridViewCellStyle3;
      this.PrecioConImpuesto.HeaderText = "Precio Con I.S.V";
      this.PrecioConImpuesto.Name = "PrecioConImpuesto";
      this.PrecioConImpuesto.ReadOnly = true;
      this.PrecioConImpuesto.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.Temporizador.Interval = 1000;
      this.Temporizador.Tick += new EventHandler(this.Temporizador_Tick);
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 727);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 16;
      this.StatusStrip.Paint += new PaintEventHandler(this.StatusStrip_Paint);
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 17;
      this.panel6.BackColor = Color.LightSteelBlue;
      this.panel6.BackgroundImage = (Image) componentResourceManager.GetObject("panel6.BackgroundImage");
      this.panel6.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel6.Controls.Add((Control) this.linkLabel2);
      this.panel6.Cursor = Cursors.Hand;
      this.panel6.Dock = DockStyle.Top;
      this.panel6.Location = new Point(0, 107);
      this.panel6.Margin = new Padding(0);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(299, 25);
      this.panel6.TabIndex = 7;
      this.linkLabel2.ActiveLinkColor = Color.LightGray;
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.BackColor = Color.Transparent;
      this.linkLabel2.Dock = DockStyle.Left;
      this.linkLabel2.Font = new Font("Microsoft Sans Serif", 12f);
      this.linkLabel2.LinkBehavior = LinkBehavior.NeverUnderline;
      this.linkLabel2.LinkColor = Color.Black;
      this.linkLabel2.Location = new Point(0, 0);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new Size(164, 20);
      this.linkLabel2.TabIndex = 6;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "Búsqueda por código:";
      this.linkLabel2.VisitedLinkColor = Color.Black;
      this.panel24.BackColor = SystemColors.Control;
      this.panel24.BackgroundImage = (Image) componentResourceManager.GetObject("panel24.BackgroundImage");
      this.panel24.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel24.Controls.Add((Control) this.LabelAgregarProductoAlCarrito);
      this.panel24.Cursor = Cursors.Hand;
      this.panel24.Dock = DockStyle.Top;
      this.panel24.Location = new Point(0, 0);
      this.panel24.Margin = new Padding(0);
      this.panel24.Name = "panel24";
      this.panel24.Size = new Size(299, 25);
      this.panel24.TabIndex = 14;
      this.LabelAgregarProductoAlCarrito.ActiveLinkColor = Color.LightGray;
      this.LabelAgregarProductoAlCarrito.AutoSize = true;
      this.LabelAgregarProductoAlCarrito.BackColor = Color.Transparent;
      this.LabelAgregarProductoAlCarrito.Dock = DockStyle.Left;
      this.LabelAgregarProductoAlCarrito.Font = new Font("Microsoft Sans Serif", 12f);
      this.LabelAgregarProductoAlCarrito.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LabelAgregarProductoAlCarrito.LinkColor = Color.Black;
      this.LabelAgregarProductoAlCarrito.Location = new Point(0, 0);
      this.LabelAgregarProductoAlCarrito.Name = "LabelAgregarProductoAlCarrito";
      this.LabelAgregarProductoAlCarrito.Size = new Size(201, 20);
      this.LabelAgregarProductoAlCarrito.TabIndex = 6;
      this.LabelAgregarProductoAlCarrito.TabStop = true;
      this.LabelAgregarProductoAlCarrito.Text = "Agregar producto al carrito:";
      this.LabelAgregarProductoAlCarrito.VisitedLinkColor = Color.Black;
      this.PanelTituloRealizarVenta.BackColor = Color.LightSteelBlue;
      this.PanelTituloRealizarVenta.BackgroundImage = (Image) componentResourceManager.GetObject("PanelTituloRealizarVenta.BackgroundImage");
      this.PanelTituloRealizarVenta.BackgroundImageLayout = ImageLayout.Stretch;
      this.PanelTituloRealizarVenta.Controls.Add((Control) this.LinkLabelTituloRealizarVenta);
      this.PanelTituloRealizarVenta.Cursor = Cursors.Hand;
      this.PanelTituloRealizarVenta.Dock = DockStyle.Top;
      this.PanelTituloRealizarVenta.Location = new Point(1, 784);
      this.PanelTituloRealizarVenta.Margin = new Padding(0);
      this.PanelTituloRealizarVenta.Name = "PanelTituloRealizarVenta";
      this.PanelTituloRealizarVenta.Size = new Size(412, 25);
      this.PanelTituloRealizarVenta.TabIndex = 16;
      this.LinkLabelTituloRealizarVenta.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTituloRealizarVenta.AutoSize = true;
      this.LinkLabelTituloRealizarVenta.BackColor = Color.Transparent;
      this.LinkLabelTituloRealizarVenta.Dock = DockStyle.Left;
      this.LinkLabelTituloRealizarVenta.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTituloRealizarVenta.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTituloRealizarVenta.LinkColor = Color.Black;
      this.LinkLabelTituloRealizarVenta.Location = new Point(0, 0);
      this.LinkLabelTituloRealizarVenta.Name = "LinkLabelTituloRealizarVenta";
      this.LinkLabelTituloRealizarVenta.Size = new Size(114, 20);
      this.LinkLabelTituloRealizarVenta.TabIndex = 6;
      this.LinkLabelTituloRealizarVenta.TabStop = true;
      this.LinkLabelTituloRealizarVenta.Text = "Realizar venta:";
      this.LinkLabelTituloRealizarVenta.VisitedLinkColor = Color.Black;
      this.PanelTituloFacturar.BackColor = Color.LightSteelBlue;
      this.PanelTituloFacturar.BackgroundImage = (Image) componentResourceManager.GetObject("PanelTituloFacturar.BackgroundImage");
      this.PanelTituloFacturar.BackgroundImageLayout = ImageLayout.Stretch;
      this.PanelTituloFacturar.Controls.Add((Control) this.LinkLabelTituloFacturar);
      this.PanelTituloFacturar.Controls.Add((Control) this.CheckBoxTituloFacturar);
      this.PanelTituloFacturar.Cursor = Cursors.Hand;
      this.PanelTituloFacturar.Dock = DockStyle.Top;
      this.PanelTituloFacturar.Location = new Point(1, 710);
      this.PanelTituloFacturar.Margin = new Padding(0);
      this.PanelTituloFacturar.Name = "PanelTituloFacturar";
      this.PanelTituloFacturar.Padding = new Padding(5, 0, 0, 0);
      this.PanelTituloFacturar.Size = new Size(412, 25);
      this.PanelTituloFacturar.TabIndex = 18;
      this.LinkLabelTituloFacturar.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTituloFacturar.AutoSize = true;
      this.LinkLabelTituloFacturar.BackColor = Color.Transparent;
      this.LinkLabelTituloFacturar.Dock = DockStyle.Left;
      this.LinkLabelTituloFacturar.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTituloFacturar.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTituloFacturar.LinkColor = Color.Black;
      this.LinkLabelTituloFacturar.Location = new Point(20, 0);
      this.LinkLabelTituloFacturar.Name = "LinkLabelTituloFacturar";
      this.LinkLabelTituloFacturar.Size = new Size(73, 20);
      this.LinkLabelTituloFacturar.TabIndex = 6;
      this.LinkLabelTituloFacturar.TabStop = true;
      this.LinkLabelTituloFacturar.Text = "Facturar:";
      this.LinkLabelTituloFacturar.VisitedLinkColor = Color.Black;
      this.CheckBoxTituloFacturar.AutoSize = true;
      this.CheckBoxTituloFacturar.BackColor = Color.Transparent;
      this.CheckBoxTituloFacturar.Dock = DockStyle.Left;
      this.CheckBoxTituloFacturar.FlatAppearance.MouseDownBackColor = Color.LightGray;
      this.CheckBoxTituloFacturar.Location = new Point(5, 0);
      this.CheckBoxTituloFacturar.Name = "CheckBoxTituloFacturar";
      this.CheckBoxTituloFacturar.Size = new Size(15, 25);
      this.CheckBoxTituloFacturar.TabIndex = 7;
      this.CheckBoxTituloFacturar.UseVisualStyleBackColor = false;
      this.CheckBoxTituloFacturar.CheckedChanged += new EventHandler(this.CheckBoxTituloFacturar_CheckedChanged);
      this.PanelTituloOtrosDetalles.BackColor = Color.LightSteelBlue;
      this.PanelTituloOtrosDetalles.BackgroundImage = (Image) componentResourceManager.GetObject("PanelTituloOtrosDetalles.BackgroundImage");
      this.PanelTituloOtrosDetalles.BackgroundImageLayout = ImageLayout.Stretch;
      this.PanelTituloOtrosDetalles.Controls.Add((Control) this.LinkLabelTituloOtrosDetalles);
      this.PanelTituloOtrosDetalles.Cursor = Cursors.Hand;
      this.PanelTituloOtrosDetalles.Dock = DockStyle.Top;
      this.PanelTituloOtrosDetalles.Location = new Point(0, 541);
      this.PanelTituloOtrosDetalles.Margin = new Padding(0);
      this.PanelTituloOtrosDetalles.Name = "PanelTituloOtrosDetalles";
      this.PanelTituloOtrosDetalles.Size = new Size(412, 25);
      this.PanelTituloOtrosDetalles.TabIndex = 14;
      this.LinkLabelTituloOtrosDetalles.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTituloOtrosDetalles.AutoSize = true;
      this.LinkLabelTituloOtrosDetalles.BackColor = Color.Transparent;
      this.LinkLabelTituloOtrosDetalles.Dock = DockStyle.Left;
      this.LinkLabelTituloOtrosDetalles.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTituloOtrosDetalles.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTituloOtrosDetalles.LinkColor = Color.Black;
      this.LinkLabelTituloOtrosDetalles.Location = new Point(0, 0);
      this.LinkLabelTituloOtrosDetalles.Name = "LinkLabelTituloOtrosDetalles";
      this.LinkLabelTituloOtrosDetalles.Size = new Size(111, 20);
      this.LinkLabelTituloOtrosDetalles.TabIndex = 6;
      this.LinkLabelTituloOtrosDetalles.TabStop = true;
      this.LinkLabelTituloOtrosDetalles.Text = "Otros detalles:";
      this.LinkLabelTituloOtrosDetalles.VisitedLinkColor = Color.Black;
      this.PanelTituloDetallesDeVenta.BackColor = Color.LightSteelBlue;
      this.PanelTituloDetallesDeVenta.BackgroundImage = (Image) componentResourceManager.GetObject("PanelTituloDetallesDeVenta.BackgroundImage");
      this.PanelTituloDetallesDeVenta.BackgroundImageLayout = ImageLayout.Stretch;
      this.PanelTituloDetallesDeVenta.Controls.Add((Control) this.LinkLabelTituloDetallesDeVenta);
      this.PanelTituloDetallesDeVenta.Cursor = Cursors.Hand;
      this.PanelTituloDetallesDeVenta.Dock = DockStyle.Top;
      this.PanelTituloDetallesDeVenta.Location = new Point(0, 368);
      this.PanelTituloDetallesDeVenta.Margin = new Padding(0);
      this.PanelTituloDetallesDeVenta.Name = "PanelTituloDetallesDeVenta";
      this.PanelTituloDetallesDeVenta.Size = new Size(412, 25);
      this.PanelTituloDetallesDeVenta.TabIndex = 12;
      this.LinkLabelTituloDetallesDeVenta.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTituloDetallesDeVenta.AutoSize = true;
      this.LinkLabelTituloDetallesDeVenta.BackColor = Color.Transparent;
      this.LinkLabelTituloDetallesDeVenta.Dock = DockStyle.Left;
      this.LinkLabelTituloDetallesDeVenta.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTituloDetallesDeVenta.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTituloDetallesDeVenta.LinkColor = Color.Black;
      this.LinkLabelTituloDetallesDeVenta.Location = new Point(0, 0);
      this.LinkLabelTituloDetallesDeVenta.Name = "LinkLabelTituloDetallesDeVenta";
      this.LinkLabelTituloDetallesDeVenta.Size = new Size(136, 20);
      this.LinkLabelTituloDetallesDeVenta.TabIndex = 6;
      this.LinkLabelTituloDetallesDeVenta.TabStop = true;
      this.LinkLabelTituloDetallesDeVenta.Text = "Detalles de venta:";
      this.LinkLabelTituloDetallesDeVenta.VisitedLinkColor = Color.Black;
      this.PanelTituloCarrito.BackColor = Color.LightSteelBlue;
      this.PanelTituloCarrito.BackgroundImage = (Image) componentResourceManager.GetObject("PanelTituloCarrito.BackgroundImage");
      this.PanelTituloCarrito.BackgroundImageLayout = ImageLayout.Stretch;
      this.PanelTituloCarrito.Controls.Add((Control) this.LinkLabelTituloCarrito);
      this.PanelTituloCarrito.Cursor = Cursors.Hand;
      this.PanelTituloCarrito.Dock = DockStyle.Top;
      this.PanelTituloCarrito.Location = new Point(0, 0);
      this.PanelTituloCarrito.Margin = new Padding(0);
      this.PanelTituloCarrito.Name = "PanelTituloCarrito";
      this.PanelTituloCarrito.Size = new Size(412, 25);
      this.PanelTituloCarrito.TabIndex = 19;
      this.LinkLabelTituloCarrito.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTituloCarrito.AutoSize = true;
      this.LinkLabelTituloCarrito.BackColor = Color.Transparent;
      this.LinkLabelTituloCarrito.Dock = DockStyle.Left;
      this.LinkLabelTituloCarrito.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTituloCarrito.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTituloCarrito.LinkColor = Color.Black;
      this.LinkLabelTituloCarrito.Location = new Point(0, 0);
      this.LinkLabelTituloCarrito.Name = "LinkLabelTituloCarrito";
      this.LinkLabelTituloCarrito.Size = new Size(190, 20);
      this.LinkLabelTituloCarrito.TabIndex = 5;
      this.LinkLabelTituloCarrito.TabStop = true;
      this.LinkLabelTituloCarrito.Text = "Añadir producto al carrito:";
      this.LinkLabelTituloCarrito.VisitedLinkColor = Color.Black;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1045, 749);
      this.Controls.Add((Control) this.ListaParaVerProductosAVender);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.PanelLateralDerecho);
      this.Controls.Add((Control) this.StatusStrip);
      this.Controls.Add((Control) this.PanelSuperior);
      this.DoubleBuffered = true;
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Punto_De_Ventas);
      this.Text = "Punto de ventas";
      this.TransparencyKey = Color.Magenta;
      this.WindowState = FormWindowState.Minimized;
      this.Load += new EventHandler(this.Punto_De_Ventas_Load);
      this.PanelLateralDerecho.ResumeLayout(false);
      this.PanelRealizarVenta.ResumeLayout(false);
      this.PanelFacturar.ResumeLayout(false);
      this.PanelFacturar.PerformLayout();
      this.PanelParaPestana.ResumeLayout(false);
      this.PanelDeOtrosDetalles.ResumeLayout(false);
      this.PanelDeOtrosDetalles.PerformLayout();
      this.PanelDeDetallesDeVenta.ResumeLayout(false);
      this.PanelDeDetallesDeVenta.PerformLayout();
      this.PanelLineaDeContorno_NumBox_Descuento.ResumeLayout(false);
      this.NumBox_Descuento.EndInit();
      this.Panel_LineaDeContorno_ModosDeDescuento.ResumeLayout(false);
      this.PanelCarrito.ResumeLayout(false);
      this.PanelCarrito.PerformLayout();
      ((ISupportInitialize) this.ListaCarrito).EndInit();
      this.PanelQueContienePestanasYControles.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.Panel6BusquedaNormal.ResumeLayout(false);
      this.Panel4BusquedaNormal.ResumeLayout(false);
      this.Panel4BusquedaNormal.PerformLayout();
      this.NumTxBuscarPorHasta.EndInit();
      this.Panel3BusquedaNormal.ResumeLayout(false);
      this.Panel3BusquedaNormal.PerformLayout();
      this.NumTxBuscarPorDesde.EndInit();
      this.Panel2BusquedaNormal.ResumeLayout(false);
      this.Panel2BusquedaNormal.PerformLayout();
      this.Panel1BusquedaNormal.ResumeLayout(false);
      this.Panel1BusquedaNormal.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel11.ResumeLayout(false);
      this.panel22.ResumeLayout(false);
      this.panel22.PerformLayout();
      ((ISupportInitialize) this.ListaParaVerProductosAVender).EndInit();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.panel24.ResumeLayout(false);
      this.panel24.PerformLayout();
      this.PanelTituloRealizarVenta.ResumeLayout(false);
      this.PanelTituloRealizarVenta.PerformLayout();
      this.PanelTituloFacturar.ResumeLayout(false);
      this.PanelTituloFacturar.PerformLayout();
      this.PanelTituloOtrosDetalles.ResumeLayout(false);
      this.PanelTituloOtrosDetalles.PerformLayout();
      this.PanelTituloDetallesDeVenta.ResumeLayout(false);
      this.PanelTituloDetallesDeVenta.PerformLayout();
      this.PanelTituloCarrito.ResumeLayout(false);
      this.PanelTituloCarrito.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
