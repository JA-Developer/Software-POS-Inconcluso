// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Ventas
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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class Ventas : Form
  {
    private string ConsultaActual = "SELECT * FROM Cliente";
    private int ColumnaDeSortingActual;
    private SortOrder OrdenDeSortingActual;
    public bool ReadOnly;
    public FormularioDeInicio ParentForm;
    public OleDbConnection Conn;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel PanelLateralIzquierdo;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private Panel PanelSuperior;
    private Button BtnVolverAVentas;
    private Panel StatusStrip;
    private SplitContainer splitContainer1;
    private DataGridView ListaVentas;
    private DataGridView ListaProductosDeVenta;
    private Panel panel3;
    private LinkLabel linkLabel1;
    private Panel panel4;
    private LinkLabel linkLabel4;
    private DataGridViewTextBoxColumn ColNombreDeProducto;
    private DataGridViewTextBoxColumn ColCodigoDeProducto;
    private DataGridViewTextBoxColumn ColCantidadVendidaDeProducto;
    private DataGridViewTextBoxColumn ColPrecioUnitarioDeCompra;
    private DataGridViewTextBoxColumn ColPrecioUnitarioDeVenta;
    private DataGridViewTextBoxColumn ColTotalRecibidoDeProducto;
    private DataGridViewTextBoxColumn ColGanancia;
    private Panel PanelBuscadorNormal;
    private Panel Panel12BusquedaNormal;
    private Button BtnBuscarPor;
    private Panel Panel3BusquedaNormal;
    private DateTimePicker FechaTxBuscarPorHasta;
    private Label label3;
    private Panel Panel2BusquedaNormal;
    private DateTimePicker FechaTxBuscarPorDesde;
    private Label label4;
    private Panel Panel10BusquedaNormal;
    private NumericUpDown NumTxBuscarPorHasta;
    private Label CodLabelParaNúmerosHasta;
    private Panel Panel9BusquedaNormal;
    private NumericUpDown NumTxBuscarPorDesde;
    private Label CodLabelParaNúmerosDesde;
    private Panel Panel11BusquedaNormal;
    private Label CodLabelParaTexto;
    private TextBox TxBuscarPor;
    private Panel Panel1BusquedaNormal;
    private Label LabelBuscarPor;
    private ComboBox ListaBuscarPor;
    private Panel Panel6BusquedaNormal;
    private ComboBox CmBxTxBuscarPorNombreDeCliente;
    private Label CodLabelParaNombreDeCliente;
    private Panel Panel5BusquedaNormal;
    private DateTimePicker HoraTxBuscarPorHasta;
    private Label label1;
    private Panel Panel4BusquedaNormal;
    private DateTimePicker HoraTxBuscarPorDesde;
    private Label label2;
    private Panel Panel8BusquedaNormal;
    private ComboBox CmBxTxBuscarPorUsuarioVendedor;
    private Label CodLabelParaUsuarioVendedor;
    private Panel Panel7BusquedaNormal;
    private ComboBox CmBxTxBuscarPorIDDeCliente;
    private Label CodLabelParaIDDeCliente;
    private Panel panel1;
    private DataGridViewTextBoxColumn ColIDDeCliente;
    private DataGridViewTextBoxColumn COlHora;
    private DataGridViewTextBoxColumn ColRTNDeCliente;
    private DataGridViewTextBoxColumn ColNombreDeCliente;
    private DataGridViewTextBoxColumn ColTotalRecibido;
    private DataGridViewTextBoxColumn ColDescuento;
    private DataGridViewTextBoxColumn ColUsuarioVendedor;
    private DataGridViewTextBoxColumn ColNoFactura;

    public Ventas()
    {
      this.InitializeComponent();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaVentas, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.ListaVentas.CellEnter += new DataGridViewCellEventHandler(this.ListaVentas_CellEnter);
    }

    private void ActualizarProductosDeVenta()
    {
      if (this.ListaVentas.SelectedRows.Count != 1)
        return;
      this.ListaProductosDeVenta.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText = "SELECT Inventario.Producto, Ventas.CodigoDeProducto, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeCompra, Inventario.PrecioUnitarioDeVenta FROM Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto WHERE Ventas.Id = " + this.ListaVentas.SelectedRows[0].Tag.ToString() + ";";
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      while (oleDbDataReader.Read())
      {
        string str = "";
        if (oleDbDataReader.GetValue(0) != null)
          str = oleDbDataReader.GetValue(0).ToString();
        this.ListaProductosDeVenta.Rows.Add((object) str, (object) oleDbDataReader.GetValue(1).ToString(), (object) oleDbDataReader.GetInt32(2), (object) oleDbDataReader.GetDouble(3), (object) oleDbDataReader.GetDouble(4));
      }
      oleDbDataReader.Close();
    }

    private void ListaVentas_CellEnter(object sender, DataGridViewCellEventArgs e) => this.ActualizarProductosDeVenta();

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      this.ListaVentas.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText += Consulta;
      if (Orden != SortOrder.None && IndexColumnaDeOrden != 2)
      {
        oleDbCommand.CommandText += " ORDER BY Ventas.";
        switch (IndexColumnaDeOrden)
        {
          case 0:
            oleDbCommand.CommandText += "IDDeCliente";
            break;
          case 1:
            oleDbCommand.CommandText += "RTNDeCliente";
            break;
          case 3:
            oleDbCommand.CommandText += "NombreDeCliente";
            break;
        }
        switch (Orden)
        {
          case SortOrder.Ascending:
            oleDbCommand.CommandText += " ASC";
            break;
          case SortOrder.Descending:
            oleDbCommand.CommandText += " DESC";
            break;
          default:
            oleDbCommand.CommandText += " DESC";
            break;
        }
      }
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      while (oleDbDataReader.Read())
      {
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        dataGridViewRow.CreateCells(this.ListaVentas);
        dataGridViewRow.SetValues(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), oleDbDataReader.GetValue(2));
        this.ListaVentas.Rows.Add(dataGridViewRow);
        dataGridViewRow.Cells[0].Tag = dataGridViewRow.Cells[0].Value;
      }
      for (int index = 0; index < this.ListaVentas.Rows.Count - 1; ++index)
        this.ListaVentas.Rows[index].Tag = (object) "NotAdded";
      oleDbDataReader.Close();
      this.ConsultaActual = Consulta;
    }

    private void Actualizar()
    {
      this.ListaVentas.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText = "SELECT Ventas.Id, Ventas.Fecha, Ventas.Hora, Clientes.NombreDeCliente, Ventas.IDDeCliente, Ventas.Descuento, Ventas.UsuarioVendedor, Ventas.NoFactura, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeVenta FROM Clientes INNER JOIN (Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto) ON Clientes.IDDeCliente = Ventas.IDDeCliente ORDER BY Ventas.Id;";
      OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
      int num1 = -1;
      double num2 = 0.0;
      int num3 = 0;
      double num4 = 0.0;
      while (oleDbDataReader1.Read())
      {
        int num5 = int.Parse(oleDbDataReader1.GetValue(0).ToString());
        if (num5 > num1)
        {
          object obj1 = oleDbDataReader1.GetValue(3);
          object obj2 = oleDbDataReader1.GetValue(4);
          object obj3 = oleDbDataReader1.GetValue(7);
          string str = "Ninguno";
          if (obj3 != null)
            str = obj3.ToString();
          if (obj1 != null && obj2 != null)
          {
            this.ListaVentas.Rows.Add((object) oleDbDataReader1.GetDateTime(1), (object) oleDbDataReader1.GetDateTime(2), (object) obj1.ToString(), (object) obj2.ToString(), (object) 0, (object) oleDbDataReader1.GetDouble(5), (object) oleDbDataReader1.GetValue(6).ToString(), (object) str);
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Tag = (object) oleDbDataReader1.GetValue(0).ToString();
          }
          if (num3 != 0)
          {
            double result = 0.0;
            double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[5].Value.ToString(), out result);
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num2 - result) * 1.15);
            num2 = 0.0;
          }
          num1 = num5;
          ++num3;
        }
        num2 += (double) oleDbDataReader1.GetInt32(8) * oleDbDataReader1.GetDouble(9);
      }
      double num6;
      if (this.ListaVentas.Rows.Count > 0)
      {
        double result = 0.0;
        double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[5].Value.ToString(), out result);
        this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num2 - result) * 1.15);
        num6 = 0.0;
      }
      oleDbDataReader1.Close();
      oleDbCommand.CommandText = "SELECT Ventas.Id, Ventas.Fecha, Ventas.Hora, 'Ninguno', 'Ninguno', Ventas.Descuento, Ventas.UsuarioVendedor, Ventas.NoFactura, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeVenta FROM Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto WHERE IsNull(Ventas.IDDeCliente) OR Ventas.IDDeCliente = '' ORDER BY Ventas.Id;";
      OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
      double num7 = 0.0;
      int num8 = 0;
      int num9 = -1;
      while (oleDbDataReader2.Read())
      {
        int num10 = int.Parse(oleDbDataReader2.GetValue(0).ToString());
        if (num10 > num9)
        {
          object obj4 = oleDbDataReader2.GetValue(3);
          object obj5 = oleDbDataReader2.GetValue(4);
          object obj6 = oleDbDataReader2.GetValue(7);
          string str = "Ninguno";
          if (obj6 != null)
            str = obj6.ToString();
          if (obj4 != null && obj5 != null)
          {
            this.ListaVentas.Rows.Add((object) oleDbDataReader2.GetDateTime(1), (object) oleDbDataReader2.GetDateTime(2), (object) obj4.ToString(), (object) obj5.ToString(), (object) 0, (object) oleDbDataReader2.GetDouble(5), (object) oleDbDataReader2.GetValue(6).ToString(), (object) str);
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Tag = (object) oleDbDataReader2.GetValue(0).ToString();
          }
          if (num8 != 0)
          {
            double result = 0.0;
            double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[5].Value.ToString(), out result);
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num7 - result) * 1.15);
            num7 = 0.0;
          }
          num9 = num10;
          ++num8;
        }
        num7 += (double) oleDbDataReader2.GetInt32(8) * oleDbDataReader2.GetDouble(9);
      }
      if (this.ListaVentas.Rows.Count > 0)
      {
        double result = 0.0;
        double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[5].Value.ToString(), out result);
        this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num7 - result) * 1.15);
        num6 = 0.0;
      }
      num4 = 0.0;
      oleDbDataReader2.Close();
      this.ConsultaActual = oleDbCommand.CommandText;
    }

    private bool VerificarSiExiste(string IDDeCliente)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Ventas WHERE IDDeCliente = '" + IDDeCliente + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    public void ActualizarVentas() => this.Actualizar();

    private void Ventas_Load(object sender, EventArgs e)
    {
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText = "SELECT NombreDeCliente FROM Clientes GROUP BY NombreDeCliente;";
      OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
      while (oleDbDataReader1.Read())
      {
        object obj = oleDbDataReader1.GetValue(0);
        if (obj != null)
          this.CmBxTxBuscarPorNombreDeCliente.Items.Add((object) obj.ToString());
      }
      oleDbDataReader1.Close();
      oleDbCommand.CommandText = "SELECT IDDeCliente FROM Clientes;";
      OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
      while (oleDbDataReader2.Read())
      {
        object obj = oleDbDataReader2.GetValue(0);
        if (obj != null)
          this.CmBxTxBuscarPorIDDeCliente.Items.Add((object) obj.ToString());
      }
      oleDbDataReader2.Close();
      oleDbCommand.CommandText = "SELECT Usuario FROM Usuarios;";
      OleDbDataReader oleDbDataReader3 = oleDbCommand.ExecuteReader();
      while (oleDbDataReader3.Read())
      {
        object obj = oleDbDataReader3.GetValue(0);
        if (obj != null)
          this.CmBxTxBuscarPorUsuarioVendedor.Items.Add((object) obj.ToString());
      }
      oleDbDataReader3.Close();
      this.CmBxTxBuscarPorNombreDeCliente.Items.Add((object) "Ninguno");
      this.CmBxTxBuscarPorIDDeCliente.Items.Add((object) "Ninguno");
      this.CmBxTxBuscarPorUsuarioVendedor.Items.Add((object) "Ninguno");
      this.CmBxTxBuscarPorNombreDeCliente.SelectedIndex = 0;
      this.CmBxTxBuscarPorIDDeCliente.SelectedIndex = 0;
      this.CmBxTxBuscarPorUsuarioVendedor.SelectedIndex = 0;
      this.ListaBuscarPor.SelectedIndex = 0;
      this.Actualizar();
      if (this.ListaVentas.Rows.Count <= 0)
        return;
      this.ListaVentas.Rows[0].Selected = true;
      this.ActualizarProductosDeVenta();
    }

    private string DateToString_ddMMyyyy(DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Day >= 10 ? str1 + (object) Date.Day : str1 + "0" + (object) Date.Day) + "/";
      string str3 = (Date.Month >= 10 ? str2 + (object) Date.Month : str2 + "0" + (object) Date.Month) + "/";
      return Date.Year >= 10 ? (Date.Year >= 100 ? (Date.Year >= 1000 ? str3 + (object) Date.Year : str3 + "0" + (object) Date.Year) : str3 + "00" + (object) Date.Year) : str3 + "000" + (object) Date.Year;
    }

    private void BuscarPor()
    {
      string str1 = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          str1 = "WHERE DateSerial(Year(Ventas.Fecha),Month(Ventas.Fecha),Day(Ventas.Fecha)) >= cDate('" + this.DateToString_ddMMyyyy(this.FechaTxBuscarPorDesde.Value) + "') AND DateSerial(Year(Ventas.Fecha),Month(Ventas.Fecha),Day(Ventas.Fecha)) <= cDate('" + this.DateToString_ddMMyyyy(this.FechaTxBuscarPorHasta.Value) + "')";
          break;
        case 1:
          object[] objArray = new object[13];
          objArray[0] = (object) "WHERE Ventas.Hora >= TimeSerial(";
          objArray[1] = (object) this.HoraTxBuscarPorDesde.Value.Hour;
          objArray[2] = (object) ", ";
          DateTime dateTime = this.HoraTxBuscarPorDesde.Value;
          objArray[3] = (object) dateTime.Minute;
          objArray[4] = (object) ", ";
          dateTime = this.HoraTxBuscarPorDesde.Value;
          objArray[5] = (object) dateTime.Second;
          objArray[6] = (object) ") AND Ventas.Hora <= TimeSerial(";
          dateTime = this.HoraTxBuscarPorHasta.Value;
          objArray[7] = (object) dateTime.Hour;
          objArray[8] = (object) ", ";
          dateTime = this.HoraTxBuscarPorHasta.Value;
          objArray[9] = (object) dateTime.Minute;
          objArray[10] = (object) ", ";
          dateTime = this.HoraTxBuscarPorHasta.Value;
          objArray[11] = (object) dateTime.Second;
          objArray[12] = (object) ")";
          str1 = string.Concat(objArray);
          break;
        case 2:
          str1 = this.CmBxTxBuscarPorNombreDeCliente.SelectedIndex != this.CmBxTxBuscarPorNombreDeCliente.Items.Count - 1 ? "WHERE Clientes.NombreDeCliente = '" + this.CmBxTxBuscarPorNombreDeCliente.Items[this.CmBxTxBuscarPorNombreDeCliente.SelectedIndex] + "'" : "WHERE Clientes.NombreDeCliente = Null";
          break;
        case 3:
          str1 = this.CmBxTxBuscarPorIDDeCliente.SelectedIndex != this.CmBxTxBuscarPorIDDeCliente.Items.Count - 1 ? "WHERE Ventas.IDDeCliente = '" + this.CmBxTxBuscarPorIDDeCliente.Items[this.CmBxTxBuscarPorIDDeCliente.SelectedIndex] + "'" : "WHERE Ventas.IDDeCliente = Null";
          break;
        case 4:
          str1 = "";
          break;
        case 5:
          Decimal num1 = this.NumTxBuscarPorDesde.Value;
          string str2 = num1.ToString().Replace(",", ".");
          num1 = this.NumTxBuscarPorHasta.Value;
          string str3 = num1.ToString().Replace(",", ".");
          str1 = "WHERE Ventas.Descuento >= " + str2 + " AND Ventas.Descuento <= " + str3;
          break;
        case 6:
          if (this.CmBxTxBuscarPorUsuarioVendedor.Items.Count > 0)
          {
            str1 = "WHERE Ventas.UsuarioVendedor = '" + this.CmBxTxBuscarPorUsuarioVendedor.Items[this.CmBxTxBuscarPorUsuarioVendedor.SelectedIndex] + "'";
            break;
          }
          break;
        case 7:
          str1 = "WHERE Ventas.NoFactura LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 8:
          str1 = "WHERE Inventario.Producto LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 9:
          str1 = "WHERE Ventas.CodigoDeProducto LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 10:
          Decimal num2 = this.NumTxBuscarPorDesde.Value;
          string str4 = num2.ToString().Replace(",", ".");
          num2 = this.NumTxBuscarPorHasta.Value;
          string str5 = num2.ToString().Replace(",", ".");
          str1 = "WHERE Ventas.CantidadVendida >= " + str4 + " AND Ventas.CantidadVendida <= " + str5;
          break;
        case 11:
          Decimal num3 = this.NumTxBuscarPorDesde.Value;
          string str6 = num3.ToString().Replace(",", ".");
          num3 = this.NumTxBuscarPorHasta.Value;
          string str7 = num3.ToString().Replace(",", ".");
          str1 = "WHERE Inventario.PrecioUnitarioDeCompra >= " + str6 + " AND Inventario.PrecioUnitarioDeCompra <= " + str7;
          break;
        case 12:
          Decimal num4 = this.NumTxBuscarPorDesde.Value;
          string str8 = num4.ToString().Replace(",", ".");
          num4 = this.NumTxBuscarPorHasta.Value;
          string str9 = num4.ToString().Replace(",", ".");
          str1 = "WHERE Inventario.PrecioUnitarioDeVenta >= " + str8 + " AND Inventario.PrecioUnitarioDeVenta <= " + str9;
          break;
      }
      this.ListaVentas.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText = "SELECT Ventas.Id, Ventas.Fecha, Ventas.Hora, Clientes.NombreDeCliente, Ventas.IDDeCliente, Ventas.Descuento, Ventas.UsuarioVendedor, Ventas.NoFactura, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeVenta FROM Clientes INNER JOIN (Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto) ON Clientes.IDDeCliente = Ventas.IDDeCliente " + str1 + " ORDER BY Ventas.Id;";
      OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
      int num5 = -1;
      double num6 = 0.0;
      int num7 = 0;
      while (oleDbDataReader1.Read())
      {
        int num8 = int.Parse(oleDbDataReader1.GetValue(0).ToString());
        if (num8 > num5)
        {
          object obj1 = oleDbDataReader1.GetValue(3);
          object obj2 = oleDbDataReader1.GetValue(4);
          object obj3 = oleDbDataReader1.GetValue(7);
          string str10 = "Ninguno";
          if (obj3 != null)
            str10 = obj3.ToString();
          if (obj1 != null && obj2 != null)
          {
            this.ListaVentas.Rows.Add((object) oleDbDataReader1.GetDateTime(1), (object) oleDbDataReader1.GetDateTime(2), (object) obj1.ToString(), (object) obj2.ToString(), (object) 0, (object) oleDbDataReader1.GetDouble(5), (object) oleDbDataReader1.GetValue(6).ToString(), (object) str10);
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Tag = (object) oleDbDataReader1.GetValue(0).ToString();
          }
          if (num7 != 0)
          {
            double result = 0.0;
            double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[5].Value.ToString(), out result);
            double num9 = (num6 - result) * 1.15;
            if (this.ListaBuscarPor.SelectedIndex == 4)
            {
              if (num9 >= (double) this.NumTxBuscarPorDesde.Value && num9 <= (double) this.NumTxBuscarPorHasta.Value)
                this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num6 - result) * 1.15);
              else
                this.ListaVentas.Rows.RemoveAt(this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1);
            }
            else
              this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num6 - result) * 1.15);
            num6 = 0.0;
          }
          num5 = num8;
          ++num7;
        }
        num6 += (double) oleDbDataReader1.GetInt32(8) * oleDbDataReader1.GetDouble(9);
      }
      double num10;
      if (this.ListaVentas.Rows.Count > 0)
      {
        double result = 0.0;
        double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[5].Value.ToString(), out result);
        double num11 = (num6 - result) * 1.15;
        if (this.ListaBuscarPor.SelectedIndex == 4)
        {
          if (num11 >= (double) this.NumTxBuscarPorDesde.Value && num11 <= (double) this.NumTxBuscarPorHasta.Value)
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num6 - result) * 1.15);
          else
            this.ListaVentas.Rows.RemoveAt(this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None));
        }
        else
          this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num6 - result) * 1.15);
        num10 = 0.0;
      }
      oleDbDataReader1.Close();
      if (this.ListaBuscarPor.SelectedIndex != 2 && this.ListaBuscarPor.SelectedIndex != 3 || this.ListaBuscarPor.SelectedIndex == 3 && this.CmBxTxBuscarPorIDDeCliente.SelectedIndex == this.CmBxTxBuscarPorIDDeCliente.Items.Count - 1 || this.ListaBuscarPor.SelectedIndex == 2 && this.CmBxTxBuscarPorNombreDeCliente.SelectedIndex == this.CmBxTxBuscarPorNombreDeCliente.Items.Count - 1)
      {
        if (this.ListaBuscarPor.SelectedIndex == 3 && this.CmBxTxBuscarPorIDDeCliente.SelectedIndex == this.CmBxTxBuscarPorIDDeCliente.Items.Count - 1 || this.ListaBuscarPor.SelectedIndex == 2 && this.CmBxTxBuscarPorNombreDeCliente.SelectedIndex == this.CmBxTxBuscarPorNombreDeCliente.Items.Count - 1 || this.ListaBuscarPor.SelectedIndex == 4)
          oleDbCommand.CommandText = "SELECT Ventas.Id, Ventas.Fecha, Ventas.Hora, 'Ninguno', 'Ninguno', Ventas.Descuento, Ventas.UsuarioVendedor, Ventas.NoFactura, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeVenta FROM Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto WHERE (IsNull(Ventas.IDDeCliente) OR Ventas.IDDeCliente = '') ORDER BY Ventas.Id;";
        else
          oleDbCommand.CommandText = "SELECT Ventas.Id, Ventas.Fecha, Ventas.Hora, 'Ninguno', 'Ninguno', Ventas.Descuento, Ventas.UsuarioVendedor, Ventas.NoFactura, Ventas.CantidadVendida, Inventario.PrecioUnitarioDeVenta FROM Inventario INNER JOIN Ventas ON Inventario.Codigo = Ventas.CodigoDeProducto " + str1 + " AND (IsNull(Ventas.IDDeCliente) OR Ventas.IDDeCliente = '') ORDER BY Ventas.Id;";
        OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
        double num12 = 0.0;
        int num13 = 0;
        int num14 = -1;
        while (oleDbDataReader2.Read())
        {
          int num15 = int.Parse(oleDbDataReader2.GetValue(0).ToString());
          if (num15 > num14)
          {
            object obj4 = oleDbDataReader2.GetValue(3);
            object obj5 = oleDbDataReader2.GetValue(4);
            object obj6 = oleDbDataReader2.GetValue(7);
            string str11 = "Ninguno";
            if (obj6 != null)
              str11 = obj6.ToString();
            if (obj4 != null && obj5 != null)
            {
              this.ListaVentas.Rows.Add((object) oleDbDataReader2.GetDateTime(1), (object) oleDbDataReader2.GetDateTime(2), (object) obj4.ToString(), (object) obj5.ToString(), (object) 0, (object) oleDbDataReader2.GetDouble(5), (object) oleDbDataReader2.GetValue(6).ToString(), (object) str11);
              this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Tag = (object) oleDbDataReader2.GetValue(0).ToString();
            }
            if (num13 != 0)
            {
              double result = 0.0;
              double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[5].Value.ToString(), out result);
              double num16 = (num12 - result) * 1.15;
              if (this.ListaBuscarPor.SelectedIndex == 4)
              {
                if (num16 >= (double) this.NumTxBuscarPorDesde.Value && num16 <= (double) this.NumTxBuscarPorHasta.Value)
                  this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num12 - result) * 1.15);
                else
                  this.ListaVentas.Rows.RemoveAt(this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1);
              }
              else
                this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None) - 1].Cells[4].Value = (object) ((num12 - result) * 1.15);
              num12 = 0.0;
            }
            num14 = num15;
            ++num13;
          }
          num12 += (double) oleDbDataReader2.GetInt32(8) * oleDbDataReader2.GetDouble(9);
        }
        if (this.ListaVentas.Rows.Count > 0)
        {
          double result = 0.0;
          double.TryParse(this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[5].Value.ToString(), out result);
          double num17 = (num12 - result) * 1.15;
          if (this.ListaBuscarPor.SelectedIndex == 4)
          {
            if (num17 >= (double) this.NumTxBuscarPorDesde.Value && num17 <= (double) this.NumTxBuscarPorHasta.Value)
              this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num12 - result) * 1.15);
            else
              this.ListaVentas.Rows.RemoveAt(this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None));
          }
          else
            this.ListaVentas.Rows[this.ListaVentas.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[4].Value = (object) ((num12 - result) * 1.15);
          num10 = 0.0;
        }
        oleDbDataReader2.Close();
      }
      this.ConsultaActual = oleDbCommand.CommandText;
    }

    private void BtnBuscarPor_Click(object sender, EventArgs e)
    {
      this.BuscarPor();
      if (this.ListaVentas.SelectedRows.Count <= 0)
        return;
      this.ActualizarProductosDeVenta();
    }

    private void BtnVolverAVentas_Click(object sender, EventArgs e)
    {
      this.Actualizar();
      if (this.ListaVentas.SelectedRows.Count <= 0)
        return;
      this.ActualizarProductosDeVenta();
    }

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void ListaVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void ListaBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.ListaBuscarPor.SelectedIndex == -1)
        return;
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 1:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 2:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = true;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 3:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = true;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 4:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = true;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 5:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = true;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 6:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 7:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = true;
          break;
        case 8:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = true;
          break;
        case 9:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel9BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = false;
          this.Panel11BusquedaNormal.Visible = true;
          break;
        case 10:
          this.NumTxBuscarPorDesde.DecimalPlaces = 0;
          this.NumTxBuscarPorHasta.DecimalPlaces = 0;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = true;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 11:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = true;
          this.Panel11BusquedaNormal.Visible = false;
          break;
        case 12:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel7BusquedaNormal.Visible = false;
          this.Panel8BusquedaNormal.Visible = false;
          this.Panel10BusquedaNormal.Visible = true;
          this.Panel9BusquedaNormal.Visible = true;
          this.Panel11BusquedaNormal.Visible = false;
          break;
      }
    }

    private void StatusStrip_Paint(object sender, PaintEventArgs e) => this.StatusStrip.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.StatusStrip.Width, 0));

    private void NumTxBuscarPorDesde_ValueChanged(object sender, EventArgs e) => this.NumTxBuscarPorHasta.Minimum = this.NumTxBuscarPorDesde.Value;

    private void NumTxBuscarPorHasta_ValueChanged(object sender, EventArgs e) => this.NumTxBuscarPorDesde.Maximum = this.NumTxBuscarPorHasta.Value;

    private void FechaTxBuscarPorDesde_ValueChanged(object sender, EventArgs e) => this.FechaTxBuscarPorHasta.MinDate = this.FechaTxBuscarPorDesde.Value;

    private void FechaTxBuscarPorHasta_ValueChanged(object sender, EventArgs e) => this.FechaTxBuscarPorDesde.MaxDate = this.FechaTxBuscarPorHasta.Value;

    private void HoraTxBuscarPorDesde_ValueChanged(object sender, EventArgs e) => this.HoraTxBuscarPorHasta.MinDate = this.HoraTxBuscarPorDesde.Value;

    private void HoraTxBuscarPorHasta_ValueChanged(object sender, EventArgs e) => this.HoraTxBuscarPorDesde.MaxDate = this.HoraTxBuscarPorHasta.Value;

    private void BTN_Copiar_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Ventas));
      this.PanelLateralIzquierdo = new Panel();
      this.panel1 = new Panel();
      this.PanelBuscadorNormal = new Panel();
      this.Panel12BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel11BusquedaNormal = new Panel();
      this.CodLabelParaTexto = new Label();
      this.TxBuscarPor = new TextBox();
      this.Panel10BusquedaNormal = new Panel();
      this.NumTxBuscarPorHasta = new NumericUpDown();
      this.CodLabelParaNúmerosHasta = new Label();
      this.Panel9BusquedaNormal = new Panel();
      this.NumTxBuscarPorDesde = new NumericUpDown();
      this.CodLabelParaNúmerosDesde = new Label();
      this.Panel8BusquedaNormal = new Panel();
      this.CmBxTxBuscarPorUsuarioVendedor = new ComboBox();
      this.CodLabelParaUsuarioVendedor = new Label();
      this.Panel7BusquedaNormal = new Panel();
      this.CmBxTxBuscarPorIDDeCliente = new ComboBox();
      this.CodLabelParaIDDeCliente = new Label();
      this.Panel6BusquedaNormal = new Panel();
      this.CmBxTxBuscarPorNombreDeCliente = new ComboBox();
      this.CodLabelParaNombreDeCliente = new Label();
      this.Panel5BusquedaNormal = new Panel();
      this.HoraTxBuscarPorHasta = new DateTimePicker();
      this.label1 = new Label();
      this.Panel4BusquedaNormal = new Panel();
      this.HoraTxBuscarPorDesde = new DateTimePicker();
      this.label2 = new Label();
      this.Panel3BusquedaNormal = new Panel();
      this.FechaTxBuscarPorHasta = new DateTimePicker();
      this.label3 = new Label();
      this.Panel2BusquedaNormal = new Panel();
      this.FechaTxBuscarPorDesde = new DateTimePicker();
      this.label4 = new Label();
      this.Panel1BusquedaNormal = new Panel();
      this.LabelBuscarPor = new Label();
      this.ListaBuscarPor = new ComboBox();
      this.PanelSuperior = new Panel();
      this.StatusStrip = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.ListaVentas = new DataGridView();
      this.ColIDDeCliente = new DataGridViewTextBoxColumn();
      this.COlHora = new DataGridViewTextBoxColumn();
      this.ColRTNDeCliente = new DataGridViewTextBoxColumn();
      this.ColNombreDeCliente = new DataGridViewTextBoxColumn();
      this.ColTotalRecibido = new DataGridViewTextBoxColumn();
      this.ColDescuento = new DataGridViewTextBoxColumn();
      this.ColUsuarioVendedor = new DataGridViewTextBoxColumn();
      this.ColNoFactura = new DataGridViewTextBoxColumn();
      this.ListaProductosDeVenta = new DataGridView();
      this.ColNombreDeProducto = new DataGridViewTextBoxColumn();
      this.ColCodigoDeProducto = new DataGridViewTextBoxColumn();
      this.ColCantidadVendidaDeProducto = new DataGridViewTextBoxColumn();
      this.ColPrecioUnitarioDeCompra = new DataGridViewTextBoxColumn();
      this.ColPrecioUnitarioDeVenta = new DataGridViewTextBoxColumn();
      this.ColTotalRecibidoDeProducto = new DataGridViewTextBoxColumn();
      this.ColGanancia = new DataGridViewTextBoxColumn();
      this.panel3 = new Panel();
      this.linkLabel1 = new LinkLabel();
      this.panel4 = new Panel();
      this.linkLabel4 = new LinkLabel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.BtnVolverAVentas = new Button();
      this.PanelLateralIzquierdo.SuspendLayout();
      this.PanelBuscadorNormal.SuspendLayout();
      this.Panel12BusquedaNormal.SuspendLayout();
      this.Panel11BusquedaNormal.SuspendLayout();
      this.Panel10BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorHasta.BeginInit();
      this.Panel9BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorDesde.BeginInit();
      this.Panel8BusquedaNormal.SuspendLayout();
      this.Panel7BusquedaNormal.SuspendLayout();
      this.Panel6BusquedaNormal.SuspendLayout();
      this.Panel5BusquedaNormal.SuspendLayout();
      this.Panel4BusquedaNormal.SuspendLayout();
      this.Panel3BusquedaNormal.SuspendLayout();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      this.PanelSuperior.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.ListaVentas).BeginInit();
      ((ISupportInitialize) this.ListaProductosDeVenta).BeginInit();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.PanelLateralIzquierdo.BackColor = Color.DimGray;
      this.PanelLateralIzquierdo.Controls.Add((Control) this.panel1);
      this.PanelLateralIzquierdo.Controls.Add((Control) this.PanelBuscadorNormal);
      this.PanelLateralIzquierdo.Controls.Add((Control) this.panel6);
      this.PanelLateralIzquierdo.Dock = DockStyle.Left;
      this.PanelLateralIzquierdo.Location = new Point(0, 50);
      this.PanelLateralIzquierdo.Margin = new Padding(4);
      this.PanelLateralIzquierdo.Name = "PanelLateralIzquierdo";
      this.PanelLateralIzquierdo.Padding = new Padding(0, 0, 1, 0);
      this.PanelLateralIzquierdo.Size = new Size(300, 582);
      this.PanelLateralIzquierdo.TabIndex = 6;
      this.panel1.BackColor = SystemColors.ScrollBar;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 426);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 156);
      this.panel1.TabIndex = 14;
      this.PanelBuscadorNormal.AutoSize = true;
      this.PanelBuscadorNormal.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.PanelBuscadorNormal.BackColor = Color.DimGray;
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel12BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel11BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel10BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel9BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel8BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel7BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel6BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel5BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel4BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel3BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel2BusquedaNormal);
      this.PanelBuscadorNormal.Controls.Add((Control) this.Panel1BusquedaNormal);
      this.PanelBuscadorNormal.Dock = DockStyle.Top;
      this.PanelBuscadorNormal.Location = new Point(0, 25);
      this.PanelBuscadorNormal.Margin = new Padding(4);
      this.PanelBuscadorNormal.Name = "PanelBuscadorNormal";
      this.PanelBuscadorNormal.Padding = new Padding(0, 1, 0, 1);
      this.PanelBuscadorNormal.Size = new Size(299, 401);
      this.PanelBuscadorNormal.TabIndex = 15;
      this.Panel12BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel12BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel12BusquedaNormal.Dock = DockStyle.Top;
      this.Panel12BusquedaNormal.Location = new Point(0, 357);
      this.Panel12BusquedaNormal.Name = "Panel12BusquedaNormal";
      this.Panel12BusquedaNormal.Size = new Size(299, 43);
      this.Panel12BusquedaNormal.TabIndex = 14;
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
      this.Panel11BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel11BusquedaNormal.Controls.Add((Control) this.CodLabelParaTexto);
      this.Panel11BusquedaNormal.Controls.Add((Control) this.TxBuscarPor);
      this.Panel11BusquedaNormal.Dock = DockStyle.Top;
      this.Panel11BusquedaNormal.Location = new Point(0, 325);
      this.Panel11BusquedaNormal.Name = "Panel11BusquedaNormal";
      this.Panel11BusquedaNormal.Size = new Size(299, 32);
      this.Panel11BusquedaNormal.TabIndex = 9;
      this.Panel11BusquedaNormal.Visible = false;
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
      this.Panel10BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel10BusquedaNormal.Controls.Add((Control) this.NumTxBuscarPorHasta);
      this.Panel10BusquedaNormal.Controls.Add((Control) this.CodLabelParaNúmerosHasta);
      this.Panel10BusquedaNormal.Dock = DockStyle.Top;
      this.Panel10BusquedaNormal.Location = new Point(0, 293);
      this.Panel10BusquedaNormal.Name = "Panel10BusquedaNormal";
      this.Panel10BusquedaNormal.Size = new Size(299, 32);
      this.Panel10BusquedaNormal.TabIndex = 11;
      this.Panel10BusquedaNormal.Visible = false;
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
      this.NumTxBuscarPorHasta.ValueChanged += new EventHandler(this.NumTxBuscarPorHasta_ValueChanged);
      this.CodLabelParaNúmerosHasta.AutoSize = true;
      this.CodLabelParaNúmerosHasta.Location = new Point(12, 7);
      this.CodLabelParaNúmerosHasta.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaNúmerosHasta.Name = "CodLabelParaNúmerosHasta";
      this.CodLabelParaNúmerosHasta.Size = new Size(59, 17);
      this.CodLabelParaNúmerosHasta.TabIndex = 5;
      this.CodLabelParaNúmerosHasta.Text = "Maximo:";
      this.Panel9BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel9BusquedaNormal.Controls.Add((Control) this.NumTxBuscarPorDesde);
      this.Panel9BusquedaNormal.Controls.Add((Control) this.CodLabelParaNúmerosDesde);
      this.Panel9BusquedaNormal.Dock = DockStyle.Top;
      this.Panel9BusquedaNormal.Location = new Point(0, 261);
      this.Panel9BusquedaNormal.Name = "Panel9BusquedaNormal";
      this.Panel9BusquedaNormal.Size = new Size(299, 32);
      this.Panel9BusquedaNormal.TabIndex = 10;
      this.Panel9BusquedaNormal.Visible = false;
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
      this.NumTxBuscarPorDesde.ValueChanged += new EventHandler(this.NumTxBuscarPorDesde_ValueChanged);
      this.CodLabelParaNúmerosDesde.AutoSize = true;
      this.CodLabelParaNúmerosDesde.Location = new Point(12, 7);
      this.CodLabelParaNúmerosDesde.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaNúmerosDesde.Name = "CodLabelParaNúmerosDesde";
      this.CodLabelParaNúmerosDesde.Size = new Size(56, 17);
      this.CodLabelParaNúmerosDesde.TabIndex = 5;
      this.CodLabelParaNúmerosDesde.Text = "Mínimo:";
      this.Panel8BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel8BusquedaNormal.Controls.Add((Control) this.CmBxTxBuscarPorUsuarioVendedor);
      this.Panel8BusquedaNormal.Controls.Add((Control) this.CodLabelParaUsuarioVendedor);
      this.Panel8BusquedaNormal.Dock = DockStyle.Top;
      this.Panel8BusquedaNormal.Location = new Point(0, 229);
      this.Panel8BusquedaNormal.Name = "Panel8BusquedaNormal";
      this.Panel8BusquedaNormal.Size = new Size(299, 32);
      this.Panel8BusquedaNormal.TabIndex = 23;
      this.Panel8BusquedaNormal.Visible = false;
      this.CmBxTxBuscarPorUsuarioVendedor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.CmBxTxBuscarPorUsuarioVendedor.FormattingEnabled = true;
      this.CmBxTxBuscarPorUsuarioVendedor.Location = new Point(100, 4);
      this.CmBxTxBuscarPorUsuarioVendedor.Name = "CmBxTxBuscarPorUsuarioVendedor";
      this.CmBxTxBuscarPorUsuarioVendedor.Size = new Size(186, 24);
      this.CmBxTxBuscarPorUsuarioVendedor.TabIndex = 2;
      this.CodLabelParaUsuarioVendedor.AutoSize = true;
      this.CodLabelParaUsuarioVendedor.Location = new Point(12, 7);
      this.CodLabelParaUsuarioVendedor.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaUsuarioVendedor.Name = "CodLabelParaUsuarioVendedor";
      this.CodLabelParaUsuarioVendedor.Size = new Size(43, 17);
      this.CodLabelParaUsuarioVendedor.TabIndex = 5;
      this.CodLabelParaUsuarioVendedor.Text = "Filtro:";
      this.Panel7BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel7BusquedaNormal.Controls.Add((Control) this.CmBxTxBuscarPorIDDeCliente);
      this.Panel7BusquedaNormal.Controls.Add((Control) this.CodLabelParaIDDeCliente);
      this.Panel7BusquedaNormal.Dock = DockStyle.Top;
      this.Panel7BusquedaNormal.Location = new Point(0, 197);
      this.Panel7BusquedaNormal.Name = "Panel7BusquedaNormal";
      this.Panel7BusquedaNormal.Size = new Size(299, 32);
      this.Panel7BusquedaNormal.TabIndex = 22;
      this.Panel7BusquedaNormal.Visible = false;
      this.CmBxTxBuscarPorIDDeCliente.DropDownStyle = ComboBoxStyle.DropDownList;
      this.CmBxTxBuscarPorIDDeCliente.FormattingEnabled = true;
      this.CmBxTxBuscarPorIDDeCliente.Location = new Point(100, 4);
      this.CmBxTxBuscarPorIDDeCliente.Name = "CmBxTxBuscarPorIDDeCliente";
      this.CmBxTxBuscarPorIDDeCliente.Size = new Size(186, 24);
      this.CmBxTxBuscarPorIDDeCliente.TabIndex = 2;
      this.CodLabelParaIDDeCliente.AutoSize = true;
      this.CodLabelParaIDDeCliente.Location = new Point(12, 7);
      this.CodLabelParaIDDeCliente.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaIDDeCliente.Name = "CodLabelParaIDDeCliente";
      this.CodLabelParaIDDeCliente.Size = new Size(43, 17);
      this.CodLabelParaIDDeCliente.TabIndex = 5;
      this.CodLabelParaIDDeCliente.Text = "Filtro:";
      this.Panel6BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel6BusquedaNormal.Controls.Add((Control) this.CmBxTxBuscarPorNombreDeCliente);
      this.Panel6BusquedaNormal.Controls.Add((Control) this.CodLabelParaNombreDeCliente);
      this.Panel6BusquedaNormal.Dock = DockStyle.Top;
      this.Panel6BusquedaNormal.Location = new Point(0, 165);
      this.Panel6BusquedaNormal.Name = "Panel6BusquedaNormal";
      this.Panel6BusquedaNormal.Size = new Size(299, 32);
      this.Panel6BusquedaNormal.TabIndex = 19;
      this.Panel6BusquedaNormal.Visible = false;
      this.CmBxTxBuscarPorNombreDeCliente.DropDownStyle = ComboBoxStyle.DropDownList;
      this.CmBxTxBuscarPorNombreDeCliente.FormattingEnabled = true;
      this.CmBxTxBuscarPorNombreDeCliente.Location = new Point(100, 4);
      this.CmBxTxBuscarPorNombreDeCliente.Name = "CmBxTxBuscarPorNombreDeCliente";
      this.CmBxTxBuscarPorNombreDeCliente.Size = new Size(186, 24);
      this.CmBxTxBuscarPorNombreDeCliente.TabIndex = 2;
      this.CodLabelParaNombreDeCliente.AutoSize = true;
      this.CodLabelParaNombreDeCliente.Location = new Point(12, 7);
      this.CodLabelParaNombreDeCliente.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaNombreDeCliente.Name = "CodLabelParaNombreDeCliente";
      this.CodLabelParaNombreDeCliente.Size = new Size(43, 17);
      this.CodLabelParaNombreDeCliente.TabIndex = 5;
      this.CodLabelParaNombreDeCliente.Text = "Filtro:";
      this.Panel5BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel5BusquedaNormal.Controls.Add((Control) this.HoraTxBuscarPorHasta);
      this.Panel5BusquedaNormal.Controls.Add((Control) this.label1);
      this.Panel5BusquedaNormal.Dock = DockStyle.Top;
      this.Panel5BusquedaNormal.Location = new Point(0, 133);
      this.Panel5BusquedaNormal.Name = "Panel5BusquedaNormal";
      this.Panel5BusquedaNormal.Size = new Size(299, 32);
      this.Panel5BusquedaNormal.TabIndex = 21;
      this.Panel5BusquedaNormal.Visible = false;
      this.HoraTxBuscarPorHasta.Format = DateTimePickerFormat.Time;
      this.HoraTxBuscarPorHasta.Location = new Point(100, 5);
      this.HoraTxBuscarPorHasta.Name = "HoraTxBuscarPorHasta";
      this.HoraTxBuscarPorHasta.ShowUpDown = true;
      this.HoraTxBuscarPorHasta.Size = new Size(186, 23);
      this.HoraTxBuscarPorHasta.TabIndex = 7;
      this.HoraTxBuscarPorHasta.ValueChanged += new EventHandler(this.HoraTxBuscarPorHasta_ValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 7);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 17);
      this.label1.TabIndex = 5;
      this.label1.Text = "Hasta:";
      this.Panel4BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel4BusquedaNormal.Controls.Add((Control) this.HoraTxBuscarPorDesde);
      this.Panel4BusquedaNormal.Controls.Add((Control) this.label2);
      this.Panel4BusquedaNormal.Dock = DockStyle.Top;
      this.Panel4BusquedaNormal.Location = new Point(0, 101);
      this.Panel4BusquedaNormal.Name = "Panel4BusquedaNormal";
      this.Panel4BusquedaNormal.Size = new Size(299, 32);
      this.Panel4BusquedaNormal.TabIndex = 20;
      this.Panel4BusquedaNormal.Visible = false;
      this.HoraTxBuscarPorDesde.Format = DateTimePickerFormat.Time;
      this.HoraTxBuscarPorDesde.Location = new Point(100, 5);
      this.HoraTxBuscarPorDesde.Name = "HoraTxBuscarPorDesde";
      this.HoraTxBuscarPorDesde.ShowUpDown = true;
      this.HoraTxBuscarPorDesde.Size = new Size(186, 23);
      this.HoraTxBuscarPorDesde.TabIndex = 6;
      this.HoraTxBuscarPorDesde.ValueChanged += new EventHandler(this.HoraTxBuscarPorDesde_ValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 7);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(53, 17);
      this.label2.TabIndex = 5;
      this.label2.Text = "Desde:";
      this.Panel3BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel3BusquedaNormal.Controls.Add((Control) this.FechaTxBuscarPorHasta);
      this.Panel3BusquedaNormal.Controls.Add((Control) this.label3);
      this.Panel3BusquedaNormal.Dock = DockStyle.Top;
      this.Panel3BusquedaNormal.Location = new Point(0, 69);
      this.Panel3BusquedaNormal.Name = "Panel3BusquedaNormal";
      this.Panel3BusquedaNormal.Size = new Size(299, 32);
      this.Panel3BusquedaNormal.TabIndex = 13;
      this.FechaTxBuscarPorHasta.Location = new Point(100, 5);
      this.FechaTxBuscarPorHasta.Name = "FechaTxBuscarPorHasta";
      this.FechaTxBuscarPorHasta.Size = new Size(186, 23);
      this.FechaTxBuscarPorHasta.TabIndex = 7;
      this.FechaTxBuscarPorHasta.ValueChanged += new EventHandler(this.FechaTxBuscarPorHasta_ValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 7);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 17);
      this.label3.TabIndex = 5;
      this.label3.Text = "Hasta:";
      this.Panel2BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel2BusquedaNormal.Controls.Add((Control) this.FechaTxBuscarPorDesde);
      this.Panel2BusquedaNormal.Controls.Add((Control) this.label4);
      this.Panel2BusquedaNormal.Dock = DockStyle.Top;
      this.Panel2BusquedaNormal.Location = new Point(0, 37);
      this.Panel2BusquedaNormal.Name = "Panel2BusquedaNormal";
      this.Panel2BusquedaNormal.Size = new Size(299, 32);
      this.Panel2BusquedaNormal.TabIndex = 12;
      this.FechaTxBuscarPorDesde.Location = new Point(100, 5);
      this.FechaTxBuscarPorDesde.Name = "FechaTxBuscarPorDesde";
      this.FechaTxBuscarPorDesde.Size = new Size(186, 23);
      this.FechaTxBuscarPorDesde.TabIndex = 6;
      this.FechaTxBuscarPorDesde.ValueChanged += new EventHandler(this.FechaTxBuscarPorDesde_ValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 7);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(53, 17);
      this.label4.TabIndex = 5;
      this.label4.Text = "Desde:";
      this.Panel1BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel1BusquedaNormal.Controls.Add((Control) this.LabelBuscarPor);
      this.Panel1BusquedaNormal.Controls.Add((Control) this.ListaBuscarPor);
      this.Panel1BusquedaNormal.Dock = DockStyle.Top;
      this.Panel1BusquedaNormal.Location = new Point(0, 1);
      this.Panel1BusquedaNormal.Name = "Panel1BusquedaNormal";
      this.Panel1BusquedaNormal.Size = new Size(299, 36);
      this.Panel1BusquedaNormal.TabIndex = 8;
      this.LabelBuscarPor.AutoSize = true;
      this.LabelBuscarPor.Location = new Point(12, 13);
      this.LabelBuscarPor.Margin = new Padding(4, 0, 4, 0);
      this.LabelBuscarPor.Name = "LabelBuscarPor";
      this.LabelBuscarPor.Size = new Size(81, 17);
      this.LabelBuscarPor.TabIndex = 6;
      this.LabelBuscarPor.Text = "Buscar por:";
      this.ListaBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ListaBuscarPor.FormattingEnabled = true;
      this.ListaBuscarPor.Items.AddRange(new object[13]
      {
        (object) "Fecha",
        (object) "Hora",
        (object) "Nombre De Cliente",
        (object) "ID De Cliente",
        (object) "Total Recibido",
        (object) "Descuento",
        (object) "Usuario Vendedor",
        (object) "Numero De Factura",
        (object) "Nombre De Producto",
        (object) "Codigo De Producto",
        (object) "Cantidad Vendida",
        (object) "Precio De Compra (C/U)",
        (object) "Precio De Venta (C/U)"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAVentas);
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 10;
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 632);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 15;
      this.StatusStrip.Paint += new PaintEventHandler(this.StatusStrip_Paint);
      this.splitContainer1.BackColor = Color.DimGray;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(300, 50);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.ListaVentas);
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel3);
      this.splitContainer1.Panel2.Controls.Add((Control) this.ListaProductosDeVenta);
      this.splitContainer1.Panel2.Controls.Add((Control) this.panel4);
      this.splitContainer1.Size = new Size(745, 582);
      this.splitContainer1.SplitterDistance = 439;
      this.splitContainer1.SplitterWidth = 1;
      this.splitContainer1.TabIndex = 16;
      this.ListaVentas.AllowUserToAddRows = false;
      this.ListaVentas.AllowUserToDeleteRows = false;
      this.ListaVentas.BackgroundColor = SystemColors.ScrollBar;
      this.ListaVentas.BorderStyle = BorderStyle.None;
      this.ListaVentas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaVentas.Columns.AddRange((DataGridViewColumn) this.ColIDDeCliente, (DataGridViewColumn) this.COlHora, (DataGridViewColumn) this.ColRTNDeCliente, (DataGridViewColumn) this.ColNombreDeCliente, (DataGridViewColumn) this.ColTotalRecibido, (DataGridViewColumn) this.ColDescuento, (DataGridViewColumn) this.ColUsuarioVendedor, (DataGridViewColumn) this.ColNoFactura);
      this.ListaVentas.Dock = DockStyle.Fill;
      this.ListaVentas.GridColor = Color.Gray;
      this.ListaVentas.Location = new Point(0, 25);
      this.ListaVentas.Margin = new Padding(4);
      this.ListaVentas.MultiSelect = false;
      this.ListaVentas.Name = "ListaVentas";
      this.ListaVentas.ReadOnly = true;
      this.ListaVentas.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaVentas.RowHeadersVisible = false;
      this.ListaVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.ListaVentas.Size = new Size(439, 557);
      this.ListaVentas.TabIndex = 10;
      gridViewCellStyle1.Format = "dd/MM/yyyy";
      gridViewCellStyle1.NullValue = (object) null;
      this.ColIDDeCliente.DefaultCellStyle = gridViewCellStyle1;
      this.ColIDDeCliente.HeaderText = "Fecha";
      this.ColIDDeCliente.MaxInputLength = 13;
      this.ColIDDeCliente.Name = "ColIDDeCliente";
      this.ColIDDeCliente.ReadOnly = true;
      this.ColIDDeCliente.Width = 150;
      gridViewCellStyle2.Format = "T";
      gridViewCellStyle2.NullValue = (object) null;
      this.COlHora.DefaultCellStyle = gridViewCellStyle2;
      this.COlHora.HeaderText = "Hora";
      this.COlHora.Name = "COlHora";
      this.COlHora.ReadOnly = true;
      this.ColRTNDeCliente.HeaderText = "Nombre De Cliente";
      this.ColRTNDeCliente.MaxInputLength = 14;
      this.ColRTNDeCliente.Name = "ColRTNDeCliente";
      this.ColRTNDeCliente.ReadOnly = true;
      this.ColRTNDeCliente.Width = 150;
      this.ColNombreDeCliente.HeaderText = "ID De Cliente";
      this.ColNombreDeCliente.MaxInputLength = 200;
      this.ColNombreDeCliente.Name = "ColNombreDeCliente";
      this.ColNombreDeCliente.ReadOnly = true;
      this.ColNombreDeCliente.Width = 300;
      this.ColTotalRecibido.HeaderText = "Total Recibido";
      this.ColTotalRecibido.Name = "ColTotalRecibido";
      this.ColTotalRecibido.ReadOnly = true;
      this.ColDescuento.HeaderText = "Descuento";
      this.ColDescuento.Name = "ColDescuento";
      this.ColDescuento.ReadOnly = true;
      this.ColUsuarioVendedor.HeaderText = "Usuario Vendedor";
      this.ColUsuarioVendedor.Name = "ColUsuarioVendedor";
      this.ColUsuarioVendedor.ReadOnly = true;
      this.ColNoFactura.HeaderText = "No Factura";
      this.ColNoFactura.Name = "ColNoFactura";
      this.ColNoFactura.ReadOnly = true;
      this.ListaProductosDeVenta.AllowUserToAddRows = false;
      this.ListaProductosDeVenta.AllowUserToDeleteRows = false;
      this.ListaProductosDeVenta.BackgroundColor = SystemColors.ScrollBar;
      this.ListaProductosDeVenta.BorderStyle = BorderStyle.None;
      this.ListaProductosDeVenta.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaProductosDeVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaProductosDeVenta.Columns.AddRange((DataGridViewColumn) this.ColNombreDeProducto, (DataGridViewColumn) this.ColCodigoDeProducto, (DataGridViewColumn) this.ColCantidadVendidaDeProducto, (DataGridViewColumn) this.ColPrecioUnitarioDeCompra, (DataGridViewColumn) this.ColPrecioUnitarioDeVenta, (DataGridViewColumn) this.ColTotalRecibidoDeProducto, (DataGridViewColumn) this.ColGanancia);
      this.ListaProductosDeVenta.Dock = DockStyle.Fill;
      this.ListaProductosDeVenta.GridColor = Color.Gray;
      this.ListaProductosDeVenta.Location = new Point(0, 25);
      this.ListaProductosDeVenta.Margin = new Padding(4);
      this.ListaProductosDeVenta.Name = "ListaProductosDeVenta";
      this.ListaProductosDeVenta.ReadOnly = true;
      this.ListaProductosDeVenta.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaProductosDeVenta.RowHeadersVisible = false;
      this.ListaProductosDeVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.ListaProductosDeVenta.Size = new Size(305, 557);
      this.ListaProductosDeVenta.TabIndex = 11;
      this.ColNombreDeProducto.HeaderText = "Nombre";
      this.ColNombreDeProducto.MaxInputLength = 13;
      this.ColNombreDeProducto.Name = "ColNombreDeProducto";
      this.ColNombreDeProducto.ReadOnly = true;
      this.ColNombreDeProducto.Width = 150;
      this.ColCodigoDeProducto.HeaderText = "Codigo";
      this.ColCodigoDeProducto.MaxInputLength = 14;
      this.ColCodigoDeProducto.Name = "ColCodigoDeProducto";
      this.ColCodigoDeProducto.ReadOnly = true;
      this.ColCodigoDeProducto.Width = 150;
      this.ColCantidadVendidaDeProducto.HeaderText = "Cantidad Vendida";
      this.ColCantidadVendidaDeProducto.MaxInputLength = 200;
      this.ColCantidadVendidaDeProducto.Name = "ColCantidadVendidaDeProducto";
      this.ColCantidadVendidaDeProducto.ReadOnly = true;
      this.ColCantidadVendidaDeProducto.Width = 300;
      this.ColPrecioUnitarioDeCompra.HeaderText = "Precio de compra (C/U)";
      this.ColPrecioUnitarioDeCompra.Name = "ColPrecioUnitarioDeCompra";
      this.ColPrecioUnitarioDeCompra.ReadOnly = true;
      this.ColPrecioUnitarioDeCompra.Width = 120;
      this.ColPrecioUnitarioDeVenta.HeaderText = "Precio de venta (C/U)";
      this.ColPrecioUnitarioDeVenta.Name = "ColPrecioUnitarioDeVenta";
      this.ColPrecioUnitarioDeVenta.ReadOnly = true;
      this.ColPrecioUnitarioDeVenta.Width = 120;
      this.ColTotalRecibidoDeProducto.HeaderText = "Total recibido";
      this.ColTotalRecibidoDeProducto.Name = "ColTotalRecibidoDeProducto";
      this.ColTotalRecibidoDeProducto.ReadOnly = true;
      this.ColGanancia.HeaderText = "Ganancia (Sin I.S.V)";
      this.ColGanancia.Name = "ColGanancia";
      this.ColGanancia.ReadOnly = true;
      this.panel3.BackColor = Color.Brown;
      this.panel3.BackgroundImage = (Image) componentResourceManager.GetObject("panel3.BackgroundImage");
      this.panel3.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel3.Controls.Add((Control) this.linkLabel1);
      this.panel3.Cursor = Cursors.Hand;
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Margin = new Padding(0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(439, 25);
      this.panel3.TabIndex = 11;
      this.linkLabel1.ActiveLinkColor = Color.LightGray;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.BackColor = Color.Transparent;
      this.linkLabel1.Dock = DockStyle.Left;
      this.linkLabel1.Font = new Font("Microsoft Sans Serif", 12f);
      this.linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
      this.linkLabel1.LinkColor = Color.Black;
      this.linkLabel1.Location = new Point(0, 0);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(64, 20);
      this.linkLabel1.TabIndex = 6;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "Ventas:";
      this.linkLabel1.VisitedLinkColor = Color.Black;
      this.panel4.BackColor = Color.Brown;
      this.panel4.BackgroundImage = (Image) componentResourceManager.GetObject("panel4.BackgroundImage");
      this.panel4.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel4.Controls.Add((Control) this.linkLabel4);
      this.panel4.Cursor = Cursors.Hand;
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Margin = new Padding(0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(305, 25);
      this.panel4.TabIndex = 12;
      this.linkLabel4.ActiveLinkColor = Color.LightGray;
      this.linkLabel4.AutoSize = true;
      this.linkLabel4.BackColor = Color.Transparent;
      this.linkLabel4.Dock = DockStyle.Left;
      this.linkLabel4.Font = new Font("Microsoft Sans Serif", 12f);
      this.linkLabel4.LinkBehavior = LinkBehavior.NeverUnderline;
      this.linkLabel4.LinkColor = Color.Black;
      this.linkLabel4.Location = new Point(0, 0);
      this.linkLabel4.Name = "linkLabel4";
      this.linkLabel4.Size = new Size(185, 20);
      this.linkLabel4.TabIndex = 6;
      this.linkLabel4.TabStop = true;
      this.linkLabel4.Text = "Productos en esta venta:";
      this.linkLabel4.VisitedLinkColor = Color.Black;
      this.panel6.BackColor = Color.LightSteelBlue;
      this.panel6.BackgroundImage = (Image) componentResourceManager.GetObject("panel6.BackgroundImage");
      this.panel6.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel6.Controls.Add((Control) this.linkLabel2);
      this.panel6.Cursor = Cursors.Hand;
      this.panel6.Dock = DockStyle.Top;
      this.panel6.Location = new Point(0, 0);
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
      this.linkLabel2.Size = new Size(138, 20);
      this.linkLabel2.TabIndex = 6;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "Búsqueda normal:";
      this.linkLabel2.VisitedLinkColor = Color.Black;
      this.BtnVolverAVentas.BackColor = Color.Brown;
      this.BtnVolverAVentas.BackgroundImage = (Image) Resources.actualizar_pagina_opcion;
      this.BtnVolverAVentas.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnVolverAVentas.FlatAppearance.BorderColor = Color.Brown;
      this.BtnVolverAVentas.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnVolverAVentas.FlatStyle = FlatStyle.Flat;
      this.BtnVolverAVentas.Location = new Point(3, 5);
      this.BtnVolverAVentas.Name = "BtnVolverAVentas";
      this.BtnVolverAVentas.Size = new Size(42, 42);
      this.BtnVolverAVentas.TabIndex = 1;
      this.BtnVolverAVentas.UseVisualStyleBackColor = false;
      this.BtnVolverAVentas.Click += new EventHandler(this.BtnVolverAVentas_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1045, 654);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.PanelLateralIzquierdo);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Ventas);
      this.Text = nameof (Ventas);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Ventas_Load);
      this.PanelLateralIzquierdo.ResumeLayout(false);
      this.PanelLateralIzquierdo.PerformLayout();
      this.PanelBuscadorNormal.ResumeLayout(false);
      this.Panel12BusquedaNormal.ResumeLayout(false);
      this.Panel11BusquedaNormal.ResumeLayout(false);
      this.Panel11BusquedaNormal.PerformLayout();
      this.Panel10BusquedaNormal.ResumeLayout(false);
      this.Panel10BusquedaNormal.PerformLayout();
      this.NumTxBuscarPorHasta.EndInit();
      this.Panel9BusquedaNormal.ResumeLayout(false);
      this.Panel9BusquedaNormal.PerformLayout();
      this.NumTxBuscarPorDesde.EndInit();
      this.Panel8BusquedaNormal.ResumeLayout(false);
      this.Panel8BusquedaNormal.PerformLayout();
      this.Panel7BusquedaNormal.ResumeLayout(false);
      this.Panel7BusquedaNormal.PerformLayout();
      this.Panel6BusquedaNormal.ResumeLayout(false);
      this.Panel6BusquedaNormal.PerformLayout();
      this.Panel5BusquedaNormal.ResumeLayout(false);
      this.Panel5BusquedaNormal.PerformLayout();
      this.Panel4BusquedaNormal.ResumeLayout(false);
      this.Panel4BusquedaNormal.PerformLayout();
      this.Panel3BusquedaNormal.ResumeLayout(false);
      this.Panel3BusquedaNormal.PerformLayout();
      this.Panel2BusquedaNormal.ResumeLayout(false);
      this.Panel2BusquedaNormal.PerformLayout();
      this.Panel1BusquedaNormal.ResumeLayout(false);
      this.Panel1BusquedaNormal.PerformLayout();
      this.PanelSuperior.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.ListaVentas).EndInit();
      ((ISupportInitialize) this.ListaProductosDeVenta).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
