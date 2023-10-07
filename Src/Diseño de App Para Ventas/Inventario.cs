// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Inventario
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using Diseño_de_App_Para_Ventas.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ZXing;
using ZXing.Rendering;

namespace Diseño_de_App_Para_Ventas
{
  public class Inventario : Form
  {
    private BarcodeWriter _Writer = new BarcodeWriter();
    private double DoubleDePrueba;
    public OleDbConnection Conn;
    private string ConsultaActual = "SELECT * FROM Gastos";
    private int ColumnaDeSortingActual;
    public bool ReadOnly;
    private SortOrder OrdenDeSortingActual;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel panel2;
    private Panel panel5;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private DataGridView ListaInventario;
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
    private Panel Panel5BusquedaNormal;
    private ComboBox CmBxTxBuscarPorProveedores;
    private Label CodLabelParaProveedores;
    private Panel Panel6BusquedaNormal;
    private Button BtnBuscarPor;
    private Panel PanelSuperior;
    private Button BTN_Copiar;
    private Button BtnImportarDesdeExcel;
    private Button BtnVolverAClientes;
    private Button BtnGuardar;
    private Panel panel1;
    private Panel StatusStrip;
    private DataGridViewTextBoxColumn ColCodigo;
    private DataGridViewTextBoxColumn ColProducto;
    private DataGridViewTextBoxColumn ColCantidad;
    private DataGridViewTextBoxColumn PrecioDeCompra;
    private DataGridViewTextBoxColumn PrecioDeVenta;
    private DataGridViewTextBoxColumn PrecioConImpuesto;
    private DataGridViewComboBoxColumn Proveedor;
    private Panel PanelDeInformacionDeProducto;
    private Panel panel4;
    private Panel PanelDetallesDeProducto;
    private Panel panel9;
    private LinkLabel InformacionDeProducto;
    private Button BtnCambiarImagen;
    private PictureBox ImagenDeProducto;
    private Button BtnCerrarInformaciónDelProducto;
    private Button BtnVerInformaciónDelProducto;
    private Panel Espaciador0;
    private Panel Espaciador1;
    private Panel Espaciador2;
    private Label LabelNombreDeProducto;
    private Label LabelCodigo;
    private Label LabelImage;
    private Label label1;
    private Label label2;
    private Panel panel8;
    private Button BtnCopiarImagenDeCodigo;
    private Panel panel7;
    private PictureBox ImgBarCode;
    private Panel panel10;
    private Panel panel3;
    private Panel panel11;
    private Label LbCodigo;
    private Panel panel12;
    private Label label3;
    private Panel panel13;
    private ComboBox ListaFormatos;

    public Inventario()
    {
      this.InitializeComponent();
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaInventario, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.ListaInventario.CellValueChanged += new DataGridViewCellEventHandler(this.ListaInventario_CellValueChanged);
      this.ListaInventario.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaInventario_UserDeletingRow);
      this.ListaInventario.CellValidating += new DataGridViewCellValidatingEventHandler(this.ListaInventario_CellValidating);
      this.ListaInventario.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.ListaInventario_ColumnHeaderMouseClick);
      this.ListaInventario.UserAddedRow += new DataGridViewRowEventHandler(this.ListaInventario_UserAddedRow);
      this.ListaInventario.CellEnter += new DataGridViewCellEventHandler(this.ListaInventario_CellEnter);
      this.ListaFormatos.SelectedIndex = 15;
      this.TxBuscarPor.TextChanged += new EventHandler(this.TxBuscarPor_TextChanged);
      this.FormClosing += new FormClosingEventHandler(this.Inventario_FormClosing);
      this._Writer.Renderer = (IBarcodeRenderer<Bitmap>) new BitmapRenderer();
      ((BitmapRenderer) this._Writer.Renderer).Foreground = Color.Black;
      this._Writer.Options.Width = 273;
      this._Writer.Options.Height = 273;
      this._Writer.Format = BarcodeFormat.QR_CODE;
    }

    private bool VerificarElementosSinGuardar()
    {
      bool flag = false;
      for (int index = 0; index < this.ListaInventario.Rows.Count - 1; ++index)
      {
        if (this.ListaInventario.Rows[index].Tag != (object) "NotAdded")
        {
          flag = true;
          break;
        }
      }
      DialogResult dialogResult = DialogResult.Yes;
      if (flag)
        dialogResult = MessageBox.Show("Aun hay elementos sin guardar en la tabla, ¿Desea guardarlos antes de continuar?", "Guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      return dialogResult != DialogResult.Yes || this.Guardar();
    }

    private bool VerificarSiExiste(string Codigo)
    {
      OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM Inventario WHERE Codigo = '" + Codigo + "';", this.Conn);
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      oleDbCommand.Dispose();
      return false;
    }

    private static bool VerificarSiExiste(
      string ValorDeCampo,
      string NombreDeCampo,
      string Tabla,
      OleDbConnection Conexion)
    {
      OleDbCommand oleDbCommand = new OleDbCommand("SELECT " + NombreDeCampo + " FROM " + Tabla + " WHERE " + NombreDeCampo + " = '" + ValorDeCampo + "';", Conexion);
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      oleDbCommand.Dispose();
      return false;
    }

    private void ListaInventario_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (MessageBox.Show("La celda sera eliminada permanentemente, ¿Esta seguro de eliminar la celda?", "¿Esta seguro?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        if (e.Row.Tag != (object) "NotAdded")
          return;
        if (MessageBox.Show("Si elimina este registro de inventario se eliminaran tambien los registros de ventas que esten relacionados con el, ¿Esta seguro de que desea eliminarlo?", "¿Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = this.Conn;
          oleDbCommand.Transaction = this.Conn.BeginTransaction();
          try
          {
            oleDbCommand.CommandText = "DELETE FROM Ventas WHERE CodigoDeProducto = '" + e.Row.Cells[0].Tag.ToString() + "';";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "DELETE FROM Inventario WHERE Codigo = '" + e.Row.Cells[0].Tag.ToString() + "';";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.Transaction.Commit();
          }
          catch (Exception ex)
          {
            oleDbCommand.Transaction.Rollback();
            int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          oleDbCommand.Dispose();
        }
        else
          e.Cancel = true;
      }
      else
        e.Cancel = true;
    }

    private void ListaInventario_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != 5)
      {
        if (this.ListaInventario.Rows[e.RowIndex].Tag == (object) "NotAdded")
        {
          OleDbCommand oleDbCommand1 = new OleDbCommand();
          oleDbCommand1.Connection = this.Conn;
          oleDbCommand1.CommandText += "UPDATE Inventario SET ";
          bool flag = true;
          switch (e.ColumnIndex)
          {
            case 0:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                if (!this.VerificarSiExiste(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                  string str1 = "Null";
                  if (this.ListaInventario.Rows[e.RowIndex].Cells[6].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[6].Value.ToString() != "" && this.ListaInventario.Rows[e.RowIndex].Cells[6].Value.ToString() != "Ninguno")
                    str1 = "'" + this.ListaInventario.Rows[e.RowIndex].Cells[6].Value.ToString() + "'";
                  string str2 = "";
                  if (this.ListaInventario.Rows[e.RowIndex].Cells[1].Value != null)
                    str2 = "'" + this.ListaInventario.Rows[e.RowIndex].Cells[1].Value.ToString() + "'";
                  double num1 = 0.0;
                  if (this.ListaInventario.Rows[e.RowIndex].Cells[2].Value != null)
                    num1 = (double) int.Parse(this.ListaInventario.Rows[e.RowIndex].Cells[2].Value.ToString());
                  double num2 = 0.0;
                  if (this.ListaInventario.Rows[e.RowIndex].Cells[3].Value != null)
                    num2 = double.Parse(this.ListaInventario.Rows[e.RowIndex].Cells[3].Value.ToString());
                  double num3 = 0.0;
                  if (this.ListaInventario.Rows[e.RowIndex].Cells[4].Value != null)
                    num3 = double.Parse(this.ListaInventario.Rows[e.RowIndex].Cells[4].Value.ToString());
                  string str3 = "INSERT INTO Inventario Values ('" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Value.ToString() + "', '" + str2 + "', " + (object) num1 + ", " + (object) num2 + ", " + (object) num3 + "," + str1 + ");";
                  string str4 = "UPDATE Ventas SET CodigoDeProducto = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Value.ToString() + "' WHERE CodigoDeProducto = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
                  string str5 = "DELETE FROM Inventario WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
                  oleDbCommand1.Transaction = this.Conn.BeginTransaction();
                  try
                  {
                    oleDbCommand1.CommandText = str3;
                    oleDbCommand1.ExecuteNonQuery();
                    oleDbCommand1.CommandText = str4;
                    oleDbCommand1.ExecuteNonQuery();
                    oleDbCommand1.CommandText = str5;
                    oleDbCommand1.ExecuteNonQuery();
                    this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag = (object) this.ListaInventario.Rows[e.RowIndex].Cells[0].Value.ToString();
                    oleDbCommand1.Transaction.Commit();
                  }
                  catch (Exception ex)
                  {
                    oleDbCommand1.Transaction.Rollback();
                    int num4 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  }
                  flag = false;
                  break;
                }
                int num = (int) MessageBox.Show("Ya existe un artículo con este código en el inventario.", "Código existente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = false;
                break;
              }
              int num5 = (int) MessageBox.Show("No es posible asignar al código un valor vacío.", "Código vacío", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              flag = false;
              break;
            case 1:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                OleDbCommand oleDbCommand2 = oleDbCommand1;
                oleDbCommand2.CommandText = oleDbCommand2.CommandText + "Producto = '" + this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                break;
              }
              OleDbCommand oleDbCommand3 = oleDbCommand1;
              oleDbCommand3.CommandText = oleDbCommand3.CommandText + "Producto = '' WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
              break;
            case 2:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                this.DoubleDePrueba = 0.0;
                if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
                {
                  OleDbCommand oleDbCommand4 = oleDbCommand1;
                  oleDbCommand4.CommandText = oleDbCommand4.CommandText + "Cantidad = " + this.DoubleDePrueba.ToString().Replace(",", ".") + " WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                  break;
                }
                int num6 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                flag = false;
                break;
              }
              OleDbCommand oleDbCommand5 = oleDbCommand1;
              oleDbCommand5.CommandText = oleDbCommand5.CommandText + "Cantidad = 0 WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
              break;
            case 3:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                this.DoubleDePrueba = 0.0;
                if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
                {
                  OleDbCommand oleDbCommand6 = oleDbCommand1;
                  oleDbCommand6.CommandText = oleDbCommand6.CommandText + "PrecioUnitarioDeCompra = " + this.DoubleDePrueba.ToString().Replace(",", ".") + " WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                  break;
                }
                int num7 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                flag = false;
                break;
              }
              OleDbCommand oleDbCommand7 = oleDbCommand1;
              oleDbCommand7.CommandText = oleDbCommand7.CommandText + "PrecioUnitarioDeCompra = 0 WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
              break;
            case 4:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                this.DoubleDePrueba = 0.0;
                if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
                {
                  OleDbCommand oleDbCommand8 = oleDbCommand1;
                  oleDbCommand8.CommandText = oleDbCommand8.CommandText + "PrecioUnitarioDeVenta = " + this.DoubleDePrueba.ToString().Replace(",", ".") + " WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                  this.ListaInventario.Rows[e.RowIndex].Cells[5].Value = (object) (this.DoubleDePrueba * 1.15);
                  break;
                }
                int num8 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                flag = false;
                break;
              }
              OleDbCommand oleDbCommand9 = oleDbCommand1;
              oleDbCommand9.CommandText = oleDbCommand9.CommandText + "PrecioUnitarioDeVenta = 0 WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
              this.ListaInventario.Rows[e.RowIndex].Cells[5].Value = (object) 0;
              break;
            case 5:
              flag = false;
              break;
            case 6:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                if ((string) this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "Ninguno")
                {
                  OleDbCommand oleDbCommand10 = oleDbCommand1;
                  oleDbCommand10.CommandText = oleDbCommand10.CommandText + "Proveedor = Null WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                  break;
                }
                OleDbCommand oleDbCommand11 = oleDbCommand1;
                oleDbCommand11.CommandText = oleDbCommand11.CommandText + "Proveedor = '" + this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
                break;
              }
              OleDbCommand oleDbCommand12 = oleDbCommand1;
              oleDbCommand12.CommandText = oleDbCommand12.CommandText + "Proveedor = Null WHERE Codigo = '" + this.ListaInventario.Rows[e.RowIndex].Cells[0].Tag + "';";
              break;
          }
          if (flag)
          {
            oleDbCommand1.ExecuteNonQuery();
            this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
          }
          oleDbCommand1.Dispose();
        }
        else
        {
          switch (e.ColumnIndex)
          {
            case 2:
              this.DoubleDePrueba = 0.0;
              if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
                break;
              int num9 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
              break;
            case 3:
              this.DoubleDePrueba = 0.0;
              if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
                break;
              int num10 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
              break;
            case 4:
              if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                double result = 0.0;
                if (double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[4].Value.ToString(), out result))
                {
                  this.ListaInventario.Rows[e.RowIndex].Cells[5].Value = (object) (result * 1.15);
                  break;
                }
                int num11 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                break;
              }
              this.ListaInventario.Rows[e.RowIndex].Cells[5].Value = (object) 0;
              break;
          }
        }
      }
      else
      {
        double result = 0.0;
        if (this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out result))
        {
          double num12 = result / 1.15;
          result = 0.0;
          if (this.ListaInventario.Rows[e.RowIndex].Cells[4].Value == null || double.TryParse(this.ListaInventario.Rows[e.RowIndex].Cells[4].Value.ToString(), out result))
          {
            if (Math.Abs(result - num12) <= 0.01)
              return;
            this.ListaInventario.Rows[e.RowIndex].Cells[4].Value = (object) num12.ToString().Replace(',', '.');
          }
          else
            this.ListaInventario.Rows[e.RowIndex].Cells[4].Value = (object) num12.ToString().Replace(',', '.');
        }
        else
        {
          int num13 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.ListaInventario.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
        }
      }
    }

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.ListaInventario.Rows.Clear();
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
            oleDbCommand.CommandText += "PrecioUnitarioDeCompra";
            break;
          case 4:
            oleDbCommand.CommandText += "PrecioUnitarioDeVenta";
            break;
          case 5:
            oleDbCommand.CommandText += "PrecioUnitarioDeVenta*1.15";
            break;
          case 6:
            oleDbCommand.CommandText += "Proveedor";
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
      {
        string str = oleDbDataReader.GetValue(5) == null || !(oleDbDataReader.GetValue(5).ToString() != "") || !((DataGridViewComboBoxColumn) this.ListaInventario.Columns[6]).Items.Contains((object) oleDbDataReader.GetValue(5).ToString()) ? "Ninguno" : oleDbDataReader.GetValue(5).ToString();
        this.ListaInventario.Rows.Add(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), oleDbDataReader.GetValue(2), oleDbDataReader.GetValue(3), oleDbDataReader.GetValue(4), (object) (double.Parse(oleDbDataReader.GetValue(4).ToString()) * 1.15), (object) str);
      }
      for (int index = 0; index < this.ListaInventario.Rows.Count - 1; ++index)
      {
        this.ListaInventario.Rows[index].Tag = (object) "NotAdded";
        this.ListaInventario.Rows[index].Cells[0].Tag = this.ListaInventario.Rows[index].Cells[0].Value;
      }
      oleDbDataReader.Close();
      oleDbCommand.Dispose();
      this.ConsultaActual = Consulta;
      if (!this.ReadOnly)
      {
        for (int index = 0; index < this.ListaInventario.Rows[this.ListaInventario.Rows.Count - 1].Cells.Count; ++index)
          this.ListaInventario.Rows[this.ListaInventario.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
      }
      Cursor.Current = Cursors.Arrow;
    }

    public void ActualizarInventario() => this.EjecutarConsulta("SELECT * FROM Inventario", this.OrdenDeSortingActual, this.ColumnaDeSortingActual);

    private bool Guardar()
    {
      for (int index1 = 0; index1 < this.ListaInventario.Rows.Count - 1; ++index1)
      {
        if (this.ListaInventario.Rows[index1].Tag != (object) "NotAdded")
        {
          if (this.ListaInventario.Rows[index1].Cells[0].Value != null)
          {
            if (this.ListaInventario.Rows[index1].Cells[0].Value != (object) "")
            {
              if (!this.VerificarSiExiste(this.ListaInventario.Rows[index1].Cells[0].Value.ToString()))
              {
                OleDbCommand oleDbCommand1 = new OleDbCommand();
                oleDbCommand1.Connection = this.Conn;
                OleDbCommand oleDbCommand2 = oleDbCommand1;
                oleDbCommand2.CommandText = oleDbCommand2.CommandText + "INSERT INTO Inventario Values ('" + this.ListaInventario.Rows[index1].Cells[0].Value + "', '";
                if (this.ListaInventario.Rows[index1].Cells[1].Value == null)
                {
                  oleDbCommand1.CommandText += "', ";
                }
                else
                {
                  OleDbCommand oleDbCommand3 = oleDbCommand1;
                  oleDbCommand3.CommandText = oleDbCommand3.CommandText + this.ListaInventario.Rows[index1].Cells[1].Value + "', ";
                }
                if (this.ListaInventario.Rows[index1].Cells[2].Value == null)
                {
                  oleDbCommand1.CommandText += "0, ";
                }
                else
                {
                  this.DoubleDePrueba = 0.0;
                  if (double.TryParse(this.ListaInventario.Rows[index1].Cells[2].Value.ToString(), out this.DoubleDePrueba))
                  {
                    OleDbCommand oleDbCommand4 = oleDbCommand1;
                    oleDbCommand4.CommandText = oleDbCommand4.CommandText + this.ListaInventario.Rows[index1].Cells[2].Value.ToString().Replace(",", ".") + ", ";
                  }
                  else
                  {
                    int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.ListaInventario.Rows[index1].Cells[2].Selected = true;
                    return false;
                  }
                }
                if (this.ListaInventario.Rows[index1].Cells[3].Value == null)
                {
                  oleDbCommand1.CommandText += "0, ";
                }
                else
                {
                  this.DoubleDePrueba = 0.0;
                  if (double.TryParse(this.ListaInventario.Rows[index1].Cells[3].Value.ToString(), out this.DoubleDePrueba))
                  {
                    OleDbCommand oleDbCommand5 = oleDbCommand1;
                    oleDbCommand5.CommandText = oleDbCommand5.CommandText + this.ListaInventario.Rows[index1].Cells[3].Value.ToString().Replace(",", ".") + ", ";
                  }
                  else
                  {
                    int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.ListaInventario.Rows[index1].Cells[3].Selected = true;
                    return false;
                  }
                }
                if (this.ListaInventario.Rows[index1].Cells[4].Value == null)
                {
                  oleDbCommand1.CommandText += "0, ";
                }
                else
                {
                  this.DoubleDePrueba = 0.0;
                  if (double.TryParse(this.ListaInventario.Rows[index1].Cells[4].Value.ToString(), out this.DoubleDePrueba))
                  {
                    OleDbCommand oleDbCommand6 = oleDbCommand1;
                    oleDbCommand6.CommandText = oleDbCommand6.CommandText + this.ListaInventario.Rows[index1].Cells[4].Value.ToString().Replace(",", ".") + ", ";
                  }
                  else
                  {
                    int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.ListaInventario.Rows[index1].Cells[4].Selected = true;
                    return false;
                  }
                }
                if (this.ListaInventario.Rows[index1].Cells[6].Value == null || this.ListaInventario.Rows[index1].Cells[6].Value.ToString() == "Ninguno")
                {
                  oleDbCommand1.CommandText += "Null);";
                }
                else
                {
                  OleDbCommand oleDbCommand7 = oleDbCommand1;
                  oleDbCommand7.CommandText = oleDbCommand7.CommandText + "'" + this.ListaInventario.Rows[index1].Cells[6].Value + "');";
                }
                try
                {
                  oleDbCommand1.ExecuteNonQuery();
                  this.ListaInventario.Rows[index1].Cells[0].Tag = this.ListaInventario.Rows[index1].Cells[0].Value;
                  this.ListaInventario.Rows[index1].Tag = (object) "NotAdded";
                  for (int index2 = 0; index2 < this.ListaInventario.Rows[index1].Cells.Count; ++index2)
                    this.ListaInventario.Rows[index1].Cells[index2].Style.BackColor = SystemColors.Window;
                }
                catch (Exception ex)
                {
                  int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return false;
                }
                oleDbCommand1.Dispose();
              }
              else
              {
                int num = (int) MessageBox.Show("Ya existe un artículo con este código en el inventario.", "Código existente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.ListaInventario.Rows[index1].Selected = true;
                return false;
              }
            }
            else
            {
              int num = (int) MessageBox.Show("Debe asignar un código al elemento antes de guardarlo.", "Fila sin datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaInventario.Rows[index1].Selected = true;
              return false;
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Debe asignar un código al elemento antes de guardarlo.", "Fila sin datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaInventario.Rows[index1].Selected = true;
            return false;
          }
        }
      }
      return true;
    }

    private void BuscarPor()
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      string Consulta = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          Consulta = "SELECT * FROM Inventario WHERE Codigo LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 1:
          Consulta = "SELECT * FROM Inventario WHERE Producto LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 2:
          Consulta = "SELECT * FROM Inventario WHERE Cantidad >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND Cantidad <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 3:
          Consulta = "SELECT * FROM Inventario WHERE PrecioUnitarioDeCompra >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND PrecioUnitarioDeCompra <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 4:
          Consulta = "SELECT * FROM Inventario WHERE PrecioUnitarioDeVenta >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND PrecioUnitarioDeVenta <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 5:
          Consulta = "SELECT * FROM Inventario WHERE (PrecioUnitarioDeVenta*1.15) >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND (PrecioUnitarioDeVenta*1.15) <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 6:
          Consulta = this.CmBxTxBuscarPorProveedores.SelectedIndex != this.CmBxTxBuscarPorProveedores.Items.Count - 1 ? "SELECT * FROM Inventario WHERE Proveedor = '" + this.CmBxTxBuscarPorProveedores.Items[this.CmBxTxBuscarPorProveedores.SelectedIndex].ToString() + "'" : "SELECT * FROM Inventario WHERE Proveedor Is Null OR Proveedor = ''";
          break;
      }
      this.EjecutarConsulta(Consulta, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void Inventario_Load(object sender, EventArgs e)
    {
      if (this.ReadOnly)
      {
        this.ListaInventario.ReadOnly = true;
        this.ListaInventario.AllowDrop = false;
        this.ListaInventario.AllowUserToDeleteRows = false;
        this.ListaInventario.AllowUserToAddRows = false;
        this.BtnImportarDesdeExcel.Enabled = false;
      }
      Cursor.Current = Cursors.WaitCursor;
      OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM Proveedores;", this.Conn);
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      while (oleDbDataReader.Read())
      {
        ((DataGridViewComboBoxColumn) this.ListaInventario.Columns[6]).Items.Add((object) oleDbDataReader.GetValue(0).ToString());
        this.CmBxTxBuscarPorProveedores.Items.Add((object) oleDbDataReader.GetValue(0).ToString());
      }
      ((DataGridViewComboBoxColumn) this.ListaInventario.Columns[6]).Items.Add((object) "Ninguno");
      this.CmBxTxBuscarPorProveedores.Items.Add((object) "Ninguno");
      oleDbDataReader.Close();
      oleDbCommand.Dispose();
      this.ListaBuscarPor.SelectedIndex = 0;
      this.CmBxTxBuscarPorProveedores.SelectedIndex = 0;
      this.ActualizarInventario();
      this.WindowState = FormWindowState.Maximized;
      Cursor.Current = Cursors.Arrow;
    }

    private void BtnGuardar_Click(object sender, EventArgs e) => this.Guardar();

    private void BtnVolverAlInventario_Click(object sender, EventArgs e)
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaInventario.Rows.Clear();
      this.ActualizarInventario();
    }

    private void Inventario_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.VerificarElementosSinGuardar())
        return;
      e.Cancel = true;
    }

    private void ListaBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 1:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 2:
          this.NumTxBuscarPorDesde.DecimalPlaces = 0;
          this.NumTxBuscarPorHasta.DecimalPlaces = 0;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 3:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 4:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 5:
          this.NumTxBuscarPorDesde.DecimalPlaces = 2;
          this.NumTxBuscarPorHasta.DecimalPlaces = 2;
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 6:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = true;
          break;
      }
    }

    private void BtnBuscarPor_Click(object sender, EventArgs e) => this.BuscarPor();

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void ListaInventario_CellValidating(
      object sender,
      DataGridViewCellValidatingEventArgs e)
    {
      switch (e.ColumnIndex)
      {
        case 0:
          if (e.FormattedValue.ToString() == "" && e.RowIndex != this.ListaInventario.Rows.Count - 1)
          {
            int num = (int) MessageBox.Show("No es posible asignar al código un valor vacío.", "Código vacío", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            break;
          }
          break;
        case 2:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            break;
          }
          break;
        case 3:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            break;
          }
          break;
        case 4:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            break;
          }
          break;
        case 5:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            break;
          }
          break;
      }
      if (e.FormattedValue.ToString().Contains<char>('\''))
      {
        int num = (int) MessageBox.Show("No se permite el uso de comillas simples (') en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.Cancel = true;
      }
      else
      {
        if (!e.FormattedValue.ToString().Contains<char>('\\'))
          return;
        int num = (int) MessageBox.Show("No se permite el uso de la barra diagonal inversa (\\) en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.Cancel = true;
      }
    }

    private void ListaInventario_ColumnHeaderMouseClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex != -1 || !this.VerificarElementosSinGuardar())
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

    private void ListaInventario_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      for (int index = 0; index < this.ListaInventario.Rows[this.ListaInventario.Rows.Count - 1].Cells.Count; ++index)
        this.ListaInventario.Rows[this.ListaInventario.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    private bool Guardar(
      DataGridView Tabla,
      List<int> IndicesDeColumnas,
      List<string> NombreColumnasBD,
      List<string> ColumnsTypes,
      string TagWhenSaved,
      bool HasIndexColumnKey,
      int IndexColumnKey,
      string BDTableName,
      bool OmitirUltimaLinea,
      List<int> SaveJustTagFrom,
      string NombreDeIndicePorAutoIncremento)
    {
      bool flag1 = true;
      int num1 = Tabla.Rows.Count;
      if (OmitirUltimaLinea)
        num1 = Tabla.Rows.Count - 1;
label_72:
      for (int index1 = 0; index1 < num1; ++index1)
      {
        if (Tabla.Rows[index1].Tag != (object) TagWhenSaved)
        {
          if (HasIndexColumnKey)
          {
            if (Tabla.Rows[index1].Cells[IndexColumnKey].Value != null && Tabla.Rows[index1].Cells[IndexColumnKey].Value != (object) "")
            {
              if (this.VerificarSiExiste(Tabla.Rows[index1].Cells[IndexColumnKey].Value.ToString()))
              {
                int num2 = (int) MessageBox.Show("Ya hay un elemento con el mismo valor en la columna '" + Tabla.Columns[IndexColumnKey].HeaderText + "' registrado.", "Elemento existente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tabla.Rows[index1].Selected = true;
                flag1 = false;
                continue;
              }
            }
            else
            {
              int num3 = (int) MessageBox.Show("Debe asignar un valor a la columna '" + Tabla.Columns[IndexColumnKey].HeaderText + "' antes de guardar elemento.", "Fila sin datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              Tabla.Rows[index1].Selected = true;
              flag1 = false;
              continue;
            }
          }
          OleDbCommand oleDbCommand1 = new OleDbCommand();
          oleDbCommand1.Connection = this.Conn;
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          oleDbCommand2.CommandText = oleDbCommand2.CommandText + "INSERT INTO " + BDTableName + "(";
          for (int index2 = 0; index2 < NombreColumnasBD.Count; ++index2)
          {
            oleDbCommand1.CommandText += NombreColumnasBD[index2];
            if (index2 >= NombreColumnasBD.Count - 1)
              oleDbCommand1.CommandText += ")";
            else
              oleDbCommand1.CommandText += ",";
          }
          oleDbCommand1.CommandText += " VALUES (";
          for (int index3 = 0; index3 < IndicesDeColumnas.Count; ++index3)
          {
            object obj;
            if (SaveJustTagFrom != null)
            {
              bool flag2 = false;
              for (int index4 = 0; index4 < SaveJustTagFrom.Count; ++index4)
              {
                if (index3.ToString() == SaveJustTagFrom[index4].ToString())
                  flag2 = true;
              }
              obj = !flag2 ? Tabla.Rows[index1].Cells[index3].Value : Tabla.Rows[index1].Cells[index3].Tag;
            }
            else
              obj = Tabla.Rows[index1].Cells[index3].Value;
            switch (ColumnsTypes[index3])
            {
              case "Intenger":
                if (obj == null || obj.ToString() == "")
                {
                  oleDbCommand1.CommandText += "0";
                  break;
                }
                double result1 = 0.0;
                if (double.TryParse(obj.ToString(), out result1))
                {
                  oleDbCommand1.CommandText += (string) obj;
                  break;
                }
                int num4 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tabla.Rows[index1].Cells[index3].Selected = true;
                flag1 = false;
                goto label_72;
              case "Double":
                if (obj == null || obj.ToString() == "")
                {
                  oleDbCommand1.CommandText += "0";
                  break;
                }
                double result2 = 0.0;
                if (double.TryParse(obj.ToString(), out result2))
                {
                  oleDbCommand1.CommandText += (string) obj;
                  break;
                }
                int num5 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tabla.Rows[index1].Cells[index3].Selected = true;
                flag1 = false;
                goto label_72;
              case "String":
                if (obj == null || obj.ToString() == "")
                {
                  oleDbCommand1.CommandText += "''";
                  break;
                }
                if (obj.ToString().Contains("'"))
                {
                  int num6 = (int) MessageBox.Show("No se permite el uso de comillas simples (') en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  Tabla.Rows[index1].Cells[index3].Selected = true;
                  flag1 = false;
                  goto label_72;
                }
                else if (obj.ToString().Contains("\\"))
                {
                  int num7 = (int) MessageBox.Show("No se permite el uso de la barra diagonal inversa (\\) en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  Tabla.Rows[index1].Cells[index3].Selected = true;
                  flag1 = false;
                  goto label_72;
                }
                else
                {
                  OleDbCommand oleDbCommand3 = oleDbCommand1;
                  oleDbCommand3.CommandText = oleDbCommand3.CommandText + "'" + obj + "'";
                  break;
                }
              case "Date":
                if (obj == null || obj.ToString() == "")
                {
                  oleDbCommand1.CommandText += "cDate('0/0/0')";
                  break;
                }
                if (DateTime.TryParseExact(obj.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                {
                  OleDbCommand oleDbCommand4 = oleDbCommand1;
                  oleDbCommand4.CommandText = oleDbCommand4.CommandText + "cDate('" + obj.ToString() + "')";
                  break;
                }
                int num8 = (int) MessageBox.Show("Esta celda solo admite fechas.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tabla.Rows[index1].Cells[index3].Selected = true;
                flag1 = false;
                goto label_72;
              case "Time":
                if (obj == null || obj.ToString() == "")
                {
                  oleDbCommand1.CommandText += "cDate('00:00:00.000')";
                  break;
                }
                if (DateTime.TryParseExact(obj.ToString(), "HH:mm:ss.fff", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                {
                  OleDbCommand oleDbCommand5 = oleDbCommand1;
                  oleDbCommand5.CommandText = oleDbCommand5.CommandText + "cDate('" + obj.ToString() + "')";
                  break;
                }
                int num9 = (int) MessageBox.Show("Esta celda solo admite horas.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Tabla.Rows[index1].Cells[index3].Selected = true;
                goto label_72;
              default:
                if (obj == null || obj.ToString() == "Ninguno")
                {
                  oleDbCommand1.CommandText += "Null";
                  break;
                }
                if (SaveJustTagFrom != null && SaveJustTagFrom.Contains(IndicesDeColumnas[index3]))
                {
                  oleDbCommand1.CommandText += obj.ToString();
                  break;
                }
                OleDbCommand oleDbCommand6 = oleDbCommand1;
                oleDbCommand6.CommandText = oleDbCommand6.CommandText + "'" + obj.ToString() + "'";
                break;
            }
            if (index3 >= IndicesDeColumnas.Count - 1)
              oleDbCommand1.CommandText += ");";
            else
              oleDbCommand1.CommandText += ",";
          }
          try
          {
            oleDbCommand1.ExecuteNonQuery();
            if (HasIndexColumnKey)
              Tabla.Rows[index1].Cells[IndexColumnKey].Tag = Tabla.Rows[index1].Cells[IndexColumnKey].Value;
            else if (NombreDeIndicePorAutoIncremento != null && NombreDeIndicePorAutoIncremento != "")
            {
              oleDbCommand1.CommandText = "SELECT MAX(" + NombreDeIndicePorAutoIncremento + ")  FROM " + BDTableName + ";";
              Tabla.Rows[index1].Cells[IndexColumnKey].Tag = (object) oleDbCommand1.ExecuteScalar().ToString();
            }
            Tabla.Rows[index1].Tag = (object) TagWhenSaved;
            for (int index5 = 0; index5 < Tabla.Rows[index1].Cells.Count; ++index5)
              Tabla.Rows[index1].Cells[index5].Style.BackColor = SystemColors.Window;
          }
          catch (Exception ex)
          {
            int num10 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            continue;
          }
          oleDbCommand1.Dispose();
        }
      }
      return flag1;
    }

    private void BtnImportarDesdeExcel_Click(object sender, EventArgs e)
    {
      ImportacionDeDatosDesdeExcel deDatosDesdeExcel = new ImportacionDeDatosDesdeExcel();
      deDatosDesdeExcel.AddColumnString("Código");
      deDatosDesdeExcel.AddColumnString("Producto");
      deDatosDesdeExcel.AddColumnIntenger("Cantidad");
      deDatosDesdeExcel.AddColumnDouble("Precio De Compra Por Unidad");
      deDatosDesdeExcel.AddColumnDouble("Precio De Venta Por Unidad");
      List<string> Items = new List<string>();
      Items.Add("Ninguno");
      OleDbCommand oleDbCommand = new OleDbCommand("SELECT Nombre FROM Proveedores;", this.Conn);
      OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
      while (oleDbDataReader.Read())
        Items.Add(oleDbDataReader.GetValue(0).ToString());
      oleDbDataReader.Close();
      oleDbCommand.Dispose();
      deDatosDesdeExcel.AddColumnList("Proveedor", Items, "Proveedor");
      while (deDatosDesdeExcel.ShowDialog() == DialogResult.OK)
      {
        if (!this.Guardar(deDatosDesdeExcel.TablaDeImporte, new List<int>()
        {
          0,
          1,
          2,
          3,
          4,
          6
        }, new List<string>()
        {
          "Codigo",
          "Producto",
          "Cantidad",
          "PrecioUnitarioDeCompra",
          "PrecioUnitarioDeVenta",
          "Proveedor"
        }, new List<string>()
        {
          "String",
          "String",
          "Intenger",
          "Double",
          "Double",
          "#List:Proveedor"
        }, "NotAdded", true, 0, nameof (Inventario), false, (List<int>) null, (string) null))
        {
          for (int index = deDatosDesdeExcel.TablaDeImporte.RowCount - 1; index >= 0; --index)
          {
            if (deDatosDesdeExcel.TablaDeImporte.Rows[index].Tag != null && deDatosDesdeExcel.TablaDeImporte.Rows[index].Tag.ToString() == "NotAdded")
              deDatosDesdeExcel.TablaDeImporte.Rows.RemoveAt(index);
          }
          int num = (int) MessageBox.Show("No se pudo guardar todas las filas debido a que no cumplieron con los requisitos necesarios. Porfavor, reviselas y vuelva a intentarlo.", "No se pudo guardar todas las filas", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          break;
      }
      deDatosDesdeExcel.Dispose();
      this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void BTN_Copiar_Click(object sender, EventArgs e)
    {
      if (this.ListaInventario.SelectedCells.Count <= 0)
        return;
      int num1 = this.ListaInventario.Rows.Count;
      int num2 = this.ListaInventario.Columns.Count;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < this.ListaInventario.SelectedCells.Count; ++index)
      {
        if (this.ListaInventario.SelectedCells[index].ColumnIndex < num2)
          num2 = this.ListaInventario.SelectedCells[index].ColumnIndex;
        if (this.ListaInventario.SelectedCells[index].RowIndex < num1)
          num1 = this.ListaInventario.SelectedCells[index].RowIndex;
        if (this.ListaInventario.SelectedCells[index].ColumnIndex > num4)
          num4 = this.ListaInventario.SelectedCells[index].ColumnIndex;
        if (this.ListaInventario.SelectedCells[index].RowIndex > num3)
          num3 = this.ListaInventario.SelectedCells[index].RowIndex;
      }
      string text = "";
      for (int rowIndex = num1; rowIndex <= num3; ++rowIndex)
      {
        for (int columnIndex = num2; columnIndex <= num4; ++columnIndex)
        {
          if (this.ListaInventario[columnIndex, rowIndex].Selected)
          {
            if (this.ListaInventario[columnIndex, rowIndex].Value != null)
              text += this.ListaInventario[columnIndex, rowIndex].Value.ToString();
            if (columnIndex != num4)
              text += "\t";
          }
        }
        text += Environment.NewLine;
      }
      if (text != null && text != "")
        Clipboard.SetText(text);
    }

    public static bool GuardarImagen(byte[] AbImagen, string Codigo, OleDbConnection Conexion)
    {
      try
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        oleDbCommand.Connection = Conexion;
        if (Inventario.VerificarSiExiste(Codigo, "CodigoDeProducto", "InformacionDeProducto", Conexion))
          oleDbCommand.CommandText = "UPDATE InformacionDeProducto SET Imagen =(?) WHERE CodigoDeProducto = '" + Codigo + "';";
        else
          oleDbCommand.CommandText = "INSERT INTO InformacionDeProducto(CodigoDeProducto, Imagen) VALUES('" + Codigo + "',?);";
        OleDbParameter oleDbParameter = new OleDbParameter("@imagen", OleDbType.VarBinary, AbImagen.Length);
        oleDbParameter.Value = (object) AbImagen;
        oleDbCommand.Parameters.Add(oleDbParameter);
        return Convert.ToBoolean(oleDbCommand.ExecuteNonQuery());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error interno al tratar de guardar la imagen: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public static byte[] ObtenerImagen(string Codigo, OleDbConnection Conexion)
    {
      try
      {
        OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT Imagen FROM InformacionDeProducto WHERE CodigoDeProducto='" + Codigo + "';", Conexion).ExecuteReader();
        byte[] numArray = (byte[]) null;
        if (oleDbDataReader.Read())
          numArray = (byte[]) oleDbDataReader.GetValue(0);
        oleDbDataReader.Close();
        return numArray;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error interno al tratar de obtener la imagen: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (byte[]) null;
      }
    }

    public static byte[] Convertir_Imagen_Bytes(string Path)
    {
      try
      {
        FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
        fileStream.Position = 0L;
        int int32 = Convert.ToInt32(fileStream.Length);
        byte[] buffer = new byte[int32];
        fileStream.Read(buffer, 0, int32);
        fileStream.Close();
        return buffer;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error interno al tratar de convertir la imagen para guardarla en la abse de datos: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (byte[]) null;
      }
    }

    public static byte[] Convertir_Imagen_Bytes(Image img)
    {
      FileStream fileStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite);
      img.Save((Stream) fileStream, ImageFormat.Png);
      fileStream.Position = 0L;
      int int32 = Convert.ToInt32(fileStream.Length);
      byte[] buffer = new byte[int32];
      fileStream.Read(buffer, 0, int32);
      fileStream.Close();
      return buffer;
    }

    public static Image Convertir_Bytes_Imagen(byte[] bytes)
    {
      try
      {
        if (bytes == null)
          return (Image) null;
        MemoryStream memoryStream = new MemoryStream(bytes);
        Bitmap bitmap = (Bitmap) null;
        try
        {
          bitmap = new Bitmap((Stream) memoryStream);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error interno al tratar de almacenar la imagen en memoria: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return (Image) bitmap;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error interno al tratar de convertir la imagen para mostrarla: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (Image) null;
      }
    }

    private void BtnCambiarImagen_Click(object sender, EventArgs e)
    {
      if (this.ListaInventario.SelectedCells.Count <= 0)
        return;
      object tag = this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Tag;
      if (tag == null)
        return;
      string Codigo = tag.ToString();
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Multiselect = false;
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          Image image1 = Image.FromFile(openFileDialog.FileName);
          Image image2 = (Image) new Bitmap(273, 273);
          Graphics.FromImage(image2).DrawImage(image1, 0, 0, 273, 273);
          Inventario.GuardarImagen(Inventario.Convertir_Imagen_Bytes(image2), Codigo, this.Conn);
          this.ImagenDeProducto.Image = image2;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("No se pudo abrir la imagen: " + ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      openFileDialog.Dispose();
    }

    private void ActualizarInformacionDelProducto()
    {
      if (!this.PanelDeInformacionDeProducto.Visible)
        return;
      if (this.ImagenDeProducto.Image != null)
      {
        this.ImagenDeProducto.Image.Dispose();
        this.ImagenDeProducto.Image = (Image) null;
      }
      this.ImagenDeProducto.CreateGraphics().Clear(SystemColors.ScrollBar);
      if (this.ListaInventario.SelectedCells.Count > 0)
      {
        if (this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Tag == null)
        {
          this.BtnCambiarImagen.Enabled = false;
          this.LabelNombreDeProducto.Text = (string) this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[1].Value ?? "";
          this.LabelCodigo.Text = (string) this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Value ?? "";
        }
        else
        {
          this.LabelNombreDeProducto.Text = (string) this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[1].Value ?? "";
          this.LabelCodigo.Text = (string) this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Value ?? "";
          this.BtnCambiarImagen.Enabled = true;
          Image image = Inventario.Convertir_Bytes_Imagen(Inventario.ObtenerImagen(this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Tag.ToString(), this.Conn));
          if (image != null)
            this.ImagenDeProducto.Image = image;
        }
        if (this.ImgBarCode.Image != null)
        {
          this.ImgBarCode.Image.Dispose();
          this.ImgBarCode.Image = (Image) null;
        }
        this.ImgBarCode.CreateGraphics().Clear(SystemColors.ScrollBar);
        try
        {
          this.ImgBarCode.Image = (Image) this._Writer.Write((string) this.ListaInventario.Rows[this.ListaInventario.SelectedCells[0].RowIndex].Cells[0].Value);
        }
        catch (Exception ex)
        {
          if (this.ImgBarCode.Image != null)
          {
            this.ImgBarCode.Image.Dispose();
            this.ImgBarCode.Image = (Image) null;
          }
          this.ImgBarCode.CreateGraphics().Clear(SystemColors.ScrollBar);
          this.ImgBarCode.CreateGraphics().DrawString("Este formato no esta disponible para este codigo.", this.Font, Brushes.Black, (PointF) this.Origen);
        }
      }
      else
      {
        this.LabelNombreDeProducto.Text = "Nombre: Ninguno";
        this.LabelCodigo.Text = "Código: Ninguno";
        this.BtnCambiarImagen.Enabled = false;
      }
    }

    private void ListaInventario_CellEnter(object sender, DataGridViewCellEventArgs e) => this.ActualizarInformacionDelProducto();

    private void BtnVerInformaciónDelProducto_Click(object sender, EventArgs e)
    {
      if (this.PanelDeInformacionDeProducto.Visible)
      {
        this.PanelDeInformacionDeProducto.Visible = false;
      }
      else
      {
        this.PanelDeInformacionDeProducto.Visible = true;
        this.ActualizarInformacionDelProducto();
      }
    }

    private void BtnCerrarInformaciónDelProducto_Click(object sender, EventArgs e) => this.PanelDeInformacionDeProducto.Visible = false;

    private void StatusStrip_Paint(object sender, PaintEventArgs e) => this.StatusStrip.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.StatusStrip.Width, 0));

    private void PanelDetallesDeProducto_Paint(object sender, PaintEventArgs e)
    {
      this.PanelDetallesDeProducto.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelDetallesDeProducto.Width, 0));
      this.PanelDetallesDeProducto.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelDetallesDeProducto.Height - 1), new Point(this.PanelDetallesDeProducto.Width, this.PanelDetallesDeProducto.Height - 1));
    }

    private void ListaFormatos_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.ListaFormatos.SelectedIndex)
      {
        case 0:
          this._Writer.Format = BarcodeFormat.All_1D;
          break;
        case 1:
          this._Writer.Format = BarcodeFormat.AZTEC;
          break;
        case 2:
          this._Writer.Format = BarcodeFormat.CODABAR;
          break;
        case 3:
          this._Writer.Format = BarcodeFormat.CODE_128;
          break;
        case 4:
          this._Writer.Format = BarcodeFormat.CODE_39;
          break;
        case 5:
          this._Writer.Format = BarcodeFormat.CODE_93;
          break;
        case 6:
          this._Writer.Format = BarcodeFormat.DATA_MATRIX;
          break;
        case 7:
          this._Writer.Format = BarcodeFormat.EAN_13;
          break;
        case 8:
          this._Writer.Format = BarcodeFormat.EAN_8;
          break;
        case 9:
          this._Writer.Format = BarcodeFormat.IMB;
          break;
        case 10:
          this._Writer.Format = BarcodeFormat.ITF;
          break;
        case 11:
          this._Writer.Format = BarcodeFormat.MAXICODE;
          break;
        case 12:
          this._Writer.Format = BarcodeFormat.MSI;
          break;
        case 13:
          this._Writer.Format = BarcodeFormat.PDF_417;
          break;
        case 14:
          this._Writer.Format = BarcodeFormat.PLESSEY;
          break;
        case 15:
          this._Writer.Format = BarcodeFormat.QR_CODE;
          break;
        case 16:
          this._Writer.Format = BarcodeFormat.RSS_14;
          break;
        case 17:
          this._Writer.Format = BarcodeFormat.RSS_EXPANDED;
          break;
        case 18:
          this._Writer.Format = BarcodeFormat.UPC_A;
          break;
        case 19:
          this._Writer.Format = BarcodeFormat.UPC_E;
          break;
        case 20:
          this._Writer.Format = BarcodeFormat.UPC_EAN_EXTENSION;
          break;
        default:
          this._Writer.Format = BarcodeFormat.QR_CODE;
          break;
      }
      this.ActualizarInformacionDelProducto();
    }

    private void BtnCopiarImagenDeCodigo_Click(object sender, EventArgs e)
    {
      if (this.ImgBarCode.Image == null)
        return;
      Clipboard.SetImage(this.ImgBarCode.Image);
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
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Inventario));
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.panel5 = new Panel();
      this.Panel6BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel5BusquedaNormal = new Panel();
      this.CmBxTxBuscarPorProveedores = new ComboBox();
      this.CodLabelParaProveedores = new Label();
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
      this.ListaInventario = new DataGridView();
      this.ColCodigo = new DataGridViewTextBoxColumn();
      this.ColProducto = new DataGridViewTextBoxColumn();
      this.ColCantidad = new DataGridViewTextBoxColumn();
      this.PrecioDeCompra = new DataGridViewTextBoxColumn();
      this.PrecioDeVenta = new DataGridViewTextBoxColumn();
      this.PrecioConImpuesto = new DataGridViewTextBoxColumn();
      this.Proveedor = new DataGridViewComboBoxColumn();
      this.PanelSuperior = new Panel();
      this.StatusStrip = new Panel();
      this.PanelDeInformacionDeProducto = new Panel();
      this.panel4 = new Panel();
      this.PanelDetallesDeProducto = new Panel();
      this.panel10 = new Panel();
      this.BtnCopiarImagenDeCodigo = new Button();
      this.panel13 = new Panel();
      this.panel12 = new Panel();
      this.ListaFormatos = new ComboBox();
      this.label3 = new Label();
      this.panel7 = new Panel();
      this.panel11 = new Panel();
      this.LbCodigo = new Label();
      this.panel3 = new Panel();
      this.BtnCambiarImagen = new Button();
      this.Espaciador2 = new Panel();
      this.panel8 = new Panel();
      this.LabelImage = new Label();
      this.Espaciador0 = new Panel();
      this.LabelNombreDeProducto = new Label();
      this.label2 = new Label();
      this.Espaciador1 = new Panel();
      this.LabelCodigo = new Label();
      this.label1 = new Label();
      this.ImgBarCode = new PictureBox();
      this.ImagenDeProducto = new PictureBox();
      this.panel9 = new Panel();
      this.BtnCerrarInformaciónDelProducto = new Button();
      this.InformacionDeProducto = new LinkLabel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.BtnVerInformaciónDelProducto = new Button();
      this.BTN_Copiar = new Button();
      this.BtnImportarDesdeExcel = new Button();
      this.BtnVolverAClientes = new Button();
      this.BtnGuardar = new Button();
      this.panel2.SuspendLayout();
      this.panel5.SuspendLayout();
      this.Panel6BusquedaNormal.SuspendLayout();
      this.Panel5BusquedaNormal.SuspendLayout();
      this.Panel4BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorHasta.BeginInit();
      this.Panel3BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorDesde.BeginInit();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      ((ISupportInitialize) this.ListaInventario).BeginInit();
      this.PanelSuperior.SuspendLayout();
      this.PanelDeInformacionDeProducto.SuspendLayout();
      this.PanelDetallesDeProducto.SuspendLayout();
      this.panel12.SuspendLayout();
      this.Espaciador0.SuspendLayout();
      this.Espaciador1.SuspendLayout();
      ((ISupportInitialize) this.ImgBarCode).BeginInit();
      ((ISupportInitialize) this.ImagenDeProducto).BeginInit();
      this.panel9.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.DimGray;
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.panel5);
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 50);
      this.panel2.Margin = new Padding(4);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(0, 0, 1, 0);
      this.panel2.Size = new Size(300, 635);
      this.panel2.TabIndex = 6;
      this.panel1.BackColor = SystemColors.ScrollBar;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 234);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 401);
      this.panel1.TabIndex = 15;
      this.panel5.AutoSize = true;
      this.panel5.BackColor = Color.DimGray;
      this.panel5.Controls.Add((Control) this.Panel6BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel5BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel4BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel3BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel2BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel1BusquedaNormal);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 25);
      this.panel5.Margin = new Padding(4);
      this.panel5.Name = "panel5";
      this.panel5.Padding = new Padding(0, 1, 0, 1);
      this.panel5.Size = new Size(299, 209);
      this.panel5.TabIndex = 8;
      this.Panel6BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel6BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel6BusquedaNormal.Dock = DockStyle.Top;
      this.Panel6BusquedaNormal.Location = new Point(0, 165);
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
      this.Panel5BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel5BusquedaNormal.Controls.Add((Control) this.CmBxTxBuscarPorProveedores);
      this.Panel5BusquedaNormal.Controls.Add((Control) this.CodLabelParaProveedores);
      this.Panel5BusquedaNormal.Dock = DockStyle.Top;
      this.Panel5BusquedaNormal.Location = new Point(0, 133);
      this.Panel5BusquedaNormal.Name = "Panel5BusquedaNormal";
      this.Panel5BusquedaNormal.Size = new Size(299, 32);
      this.Panel5BusquedaNormal.TabIndex = 16;
      this.CmBxTxBuscarPorProveedores.DropDownStyle = ComboBoxStyle.DropDownList;
      this.CmBxTxBuscarPorProveedores.FormattingEnabled = true;
      this.CmBxTxBuscarPorProveedores.Location = new Point(100, 4);
      this.CmBxTxBuscarPorProveedores.Name = "CmBxTxBuscarPorProveedores";
      this.CmBxTxBuscarPorProveedores.Size = new Size(186, 24);
      this.CmBxTxBuscarPorProveedores.TabIndex = 2;
      this.CodLabelParaProveedores.AutoSize = true;
      this.CodLabelParaProveedores.Location = new Point(12, 7);
      this.CodLabelParaProveedores.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaProveedores.Name = "CodLabelParaProveedores";
      this.CodLabelParaProveedores.Size = new Size(43, 17);
      this.CodLabelParaProveedores.TabIndex = 5;
      this.CodLabelParaProveedores.Text = "Filtro:";
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
      this.ListaBuscarPor.Items.AddRange(new object[7]
      {
        (object) "Código",
        (object) "Producto",
        (object) "Cantidad",
        (object) "Precio De Compra Por Unidad",
        (object) "Precio De Venta Por Unidad",
        (object) "Precio Con I.S.V",
        (object) "Proveedor"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.ListaInventario.BackgroundColor = SystemColors.ScrollBar;
      this.ListaInventario.BorderStyle = BorderStyle.None;
      this.ListaInventario.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaInventario.Columns.AddRange((DataGridViewColumn) this.ColCodigo, (DataGridViewColumn) this.ColProducto, (DataGridViewColumn) this.ColCantidad, (DataGridViewColumn) this.PrecioDeCompra, (DataGridViewColumn) this.PrecioDeVenta, (DataGridViewColumn) this.PrecioConImpuesto, (DataGridViewColumn) this.Proveedor);
      this.ListaInventario.Dock = DockStyle.Fill;
      this.ListaInventario.GridColor = Color.Gray;
      this.ListaInventario.Location = new Point(300, 50);
      this.ListaInventario.Margin = new Padding(4);
      this.ListaInventario.Name = "ListaInventario";
      this.ListaInventario.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaInventario.Size = new Size(428, 635);
      this.ListaInventario.TabIndex = 9;
      this.ColCodigo.HeaderText = "Código";
      this.ColCodigo.Name = "ColCodigo";
      this.ColCodigo.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.ColProducto.HeaderText = "Producto";
      this.ColProducto.Name = "ColProducto";
      this.ColProducto.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle1.NullValue = (object) "0";
      this.ColCantidad.DefaultCellStyle = gridViewCellStyle1;
      this.ColCantidad.HeaderText = "Cantidad";
      this.ColCantidad.Name = "ColCantidad";
      this.ColCantidad.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle2.NullValue = (object) "0";
      this.PrecioDeCompra.DefaultCellStyle = gridViewCellStyle2;
      this.PrecioDeCompra.HeaderText = "Precio De Compra Por Unidad";
      this.PrecioDeCompra.Name = "PrecioDeCompra";
      this.PrecioDeCompra.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.PrecioDeCompra.Width = 200;
      gridViewCellStyle3.NullValue = (object) "0";
      this.PrecioDeVenta.DefaultCellStyle = gridViewCellStyle3;
      this.PrecioDeVenta.HeaderText = "Precio De Venta Por Unidad";
      this.PrecioDeVenta.Name = "PrecioDeVenta";
      this.PrecioDeVenta.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.PrecioDeVenta.Width = 200;
      gridViewCellStyle4.NullValue = (object) "0";
      this.PrecioConImpuesto.DefaultCellStyle = gridViewCellStyle4;
      this.PrecioConImpuesto.HeaderText = "Precio Con I.S.V";
      this.PrecioConImpuesto.Name = "PrecioConImpuesto";
      this.PrecioConImpuesto.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle5.NullValue = (object) "Ninguno";
      this.Proveedor.DefaultCellStyle = gridViewCellStyle5;
      this.Proveedor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      this.Proveedor.FlatStyle = FlatStyle.Flat;
      this.Proveedor.HeaderText = "Proveedor";
      this.Proveedor.Name = "Proveedor";
      this.Proveedor.Resizable = DataGridViewTriState.True;
      this.Proveedor.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BtnVerInformaciónDelProducto);
      this.PanelSuperior.Controls.Add((Control) this.BTN_Copiar);
      this.PanelSuperior.Controls.Add((Control) this.BtnImportarDesdeExcel);
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAClientes);
      this.PanelSuperior.Controls.Add((Control) this.BtnGuardar);
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 11;
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 685);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 15;
      this.StatusStrip.Paint += new PaintEventHandler(this.StatusStrip_Paint);
      this.PanelDeInformacionDeProducto.AutoScroll = true;
      this.PanelDeInformacionDeProducto.BackColor = Color.DimGray;
      this.PanelDeInformacionDeProducto.Controls.Add((Control) this.panel4);
      this.PanelDeInformacionDeProducto.Controls.Add((Control) this.PanelDetallesDeProducto);
      this.PanelDeInformacionDeProducto.Controls.Add((Control) this.panel9);
      this.PanelDeInformacionDeProducto.Dock = DockStyle.Right;
      this.PanelDeInformacionDeProducto.Location = new Point(728, 50);
      this.PanelDeInformacionDeProducto.Margin = new Padding(4);
      this.PanelDeInformacionDeProducto.Name = "PanelDeInformacionDeProducto";
      this.PanelDeInformacionDeProducto.Padding = new Padding(1, 0, 0, 0);
      this.PanelDeInformacionDeProducto.Size = new Size(317, 635);
      this.PanelDeInformacionDeProducto.TabIndex = 16;
      this.PanelDeInformacionDeProducto.Visible = false;
      this.panel4.BackColor = SystemColors.ScrollBar;
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(1, 848);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(299, 0);
      this.panel4.TabIndex = 14;
      this.PanelDetallesDeProducto.AutoSize = true;
      this.PanelDetallesDeProducto.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.PanelDetallesDeProducto.BackColor = SystemColors.Control;
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel10);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.BtnCopiarImagenDeCodigo);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel13);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel12);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel7);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.ImgBarCode);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel11);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.LbCodigo);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel3);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.BtnCambiarImagen);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.Espaciador2);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.ImagenDeProducto);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.panel8);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.LabelImage);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.Espaciador0);
      this.PanelDetallesDeProducto.Controls.Add((Control) this.Espaciador1);
      this.PanelDetallesDeProducto.Dock = DockStyle.Top;
      this.PanelDetallesDeProducto.Location = new Point(1, 25);
      this.PanelDetallesDeProducto.Margin = new Padding(4);
      this.PanelDetallesDeProducto.Name = "PanelDetallesDeProducto";
      this.PanelDetallesDeProducto.Padding = new Padding(13, 13, 13, 0);
      this.PanelDetallesDeProducto.Size = new Size(299, 823);
      this.PanelDetallesDeProducto.TabIndex = 8;
      this.PanelDetallesDeProducto.Paint += new PaintEventHandler(this.PanelDetallesDeProducto_Paint);
      this.panel10.Dock = DockStyle.Top;
      this.panel10.Location = new Point(13, 813);
      this.panel10.Name = "panel10";
      this.panel10.Size = new Size(273, 10);
      this.panel10.TabIndex = 14;
      this.BtnCopiarImagenDeCodigo.BackColor = Color.DimGray;
      this.BtnCopiarImagenDeCodigo.Dock = DockStyle.Top;
      this.BtnCopiarImagenDeCodigo.FlatStyle = FlatStyle.Flat;
      this.BtnCopiarImagenDeCodigo.Location = new Point(13, 785);
      this.BtnCopiarImagenDeCodigo.Margin = new Padding(4);
      this.BtnCopiarImagenDeCodigo.Name = "BtnCopiarImagenDeCodigo";
      this.BtnCopiarImagenDeCodigo.Size = new Size(273, 28);
      this.BtnCopiarImagenDeCodigo.TabIndex = 16;
      this.BtnCopiarImagenDeCodigo.Text = "Copiar Imagen De Código";
      this.BtnCopiarImagenDeCodigo.UseVisualStyleBackColor = false;
      this.BtnCopiarImagenDeCodigo.Click += new EventHandler(this.BtnCopiarImagenDeCodigo_Click);
      this.panel13.Dock = DockStyle.Top;
      this.panel13.Location = new Point(13, 775);
      this.panel13.Name = "panel13";
      this.panel13.Size = new Size(273, 10);
      this.panel13.TabIndex = 22;
      this.panel12.Controls.Add((Control) this.ListaFormatos);
      this.panel12.Controls.Add((Control) this.label3);
      this.panel12.Dock = DockStyle.Top;
      this.panel12.Location = new Point(13, 751);
      this.panel12.Name = "panel12";
      this.panel12.Size = new Size(273, 24);
      this.panel12.TabIndex = 21;
      this.ListaFormatos.Dock = DockStyle.Fill;
      this.ListaFormatos.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ListaFormatos.FlatStyle = FlatStyle.Flat;
      this.ListaFormatos.FormattingEnabled = true;
      this.ListaFormatos.Items.AddRange(new object[21]
      {
        (object) "All_1D",
        (object) "AZTEC",
        (object) "CODABAR",
        (object) "CODE_128",
        (object) "CODE_39",
        (object) "CODE_93",
        (object) "DATA_MATRIX",
        (object) "EAN_13",
        (object) "EAN_8",
        (object) "IMB",
        (object) "ITF",
        (object) "MAXICODE",
        (object) "MSI",
        (object) "PDF_417",
        (object) "PLESSEY",
        (object) "QR_CODE",
        (object) "RSS_14",
        (object) "RSS_EXPANDED",
        (object) "UPC_A",
        (object) "UPC_E",
        (object) "UPC_EAN_EXTENSION"
      });
      this.ListaFormatos.Location = new Point(64, 0);
      this.ListaFormatos.Name = "ListaFormatos";
      this.ListaFormatos.Size = new Size(209, 24);
      this.ListaFormatos.TabIndex = 22;
      this.ListaFormatos.SelectedIndexChanged += new EventHandler(this.ListaFormatos_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Dock = DockStyle.Left;
      this.label3.Font = new Font("Microsoft Sans Serif", 10f);
      this.label3.Location = new Point(0, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(64, 17);
      this.label3.TabIndex = 20;
      this.label3.Text = "Formato:";
      this.panel7.Dock = DockStyle.Top;
      this.panel7.Location = new Point(13, 741);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(273, 10);
      this.panel7.TabIndex = 17;
      this.panel11.Dock = DockStyle.Top;
      this.panel11.Location = new Point(13, 458);
      this.panel11.Name = "panel11";
      this.panel11.Size = new Size(273, 10);
      this.panel11.TabIndex = 20;
      this.LbCodigo.AutoSize = true;
      this.LbCodigo.Dock = DockStyle.Top;
      this.LbCodigo.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold | FontStyle.Underline);
      this.LbCodigo.Location = new Point(13, 441);
      this.LbCodigo.Name = "LbCodigo";
      this.LbCodigo.Size = new Size(63, 17);
      this.LbCodigo.TabIndex = 19;
      this.LbCodigo.Text = "Código:";
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(13, 431);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(273, 10);
      this.panel3.TabIndex = 18;
      this.BtnCambiarImagen.BackColor = Color.DimGray;
      this.BtnCambiarImagen.Dock = DockStyle.Top;
      this.BtnCambiarImagen.FlatStyle = FlatStyle.Flat;
      this.BtnCambiarImagen.Location = new Point(13, 403);
      this.BtnCambiarImagen.Margin = new Padding(4);
      this.BtnCambiarImagen.Name = "BtnCambiarImagen";
      this.BtnCambiarImagen.Size = new Size(273, 28);
      this.BtnCambiarImagen.TabIndex = 6;
      this.BtnCambiarImagen.Text = "Cambiar Imagen";
      this.BtnCambiarImagen.UseVisualStyleBackColor = false;
      this.BtnCambiarImagen.Click += new EventHandler(this.BtnCambiarImagen_Click);
      this.Espaciador2.Dock = DockStyle.Top;
      this.Espaciador2.Location = new Point(13, 393);
      this.Espaciador2.Name = "Espaciador2";
      this.Espaciador2.Size = new Size(273, 10);
      this.Espaciador2.TabIndex = 11;
      this.panel8.Dock = DockStyle.Top;
      this.panel8.Location = new Point(13, 110);
      this.panel8.Name = "panel8";
      this.panel8.Size = new Size(273, 10);
      this.panel8.TabIndex = 13;
      this.LabelImage.AutoSize = true;
      this.LabelImage.Dock = DockStyle.Top;
      this.LabelImage.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold | FontStyle.Underline);
      this.LabelImage.Location = new Point(13, 93);
      this.LabelImage.Name = "LabelImage";
      this.LabelImage.Size = new Size(65, 17);
      this.LabelImage.TabIndex = 12;
      this.LabelImage.Text = "Imagen:";
      this.Espaciador0.AutoSize = true;
      this.Espaciador0.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Espaciador0.Controls.Add((Control) this.LabelNombreDeProducto);
      this.Espaciador0.Controls.Add((Control) this.label2);
      this.Espaciador0.Dock = DockStyle.Top;
      this.Espaciador0.Location = new Point(13, 53);
      this.Espaciador0.MinimumSize = new Size(0, 40);
      this.Espaciador0.Name = "Espaciador0";
      this.Espaciador0.Size = new Size(273, 40);
      this.Espaciador0.TabIndex = 9;
      this.LabelNombreDeProducto.AutoSize = true;
      this.LabelNombreDeProducto.Dock = DockStyle.Fill;
      this.LabelNombreDeProducto.Location = new Point(69, 0);
      this.LabelNombreDeProducto.MaximumSize = new Size(205, 0);
      this.LabelNombreDeProducto.Name = "LabelNombreDeProducto";
      this.LabelNombreDeProducto.Size = new Size(61, 17);
      this.LabelNombreDeProducto.TabIndex = 8;
      this.LabelNombreDeProducto.Text = "Ninguno";
      this.label2.AutoSize = true;
      this.label2.Dock = DockStyle.Left;
      this.label2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold | FontStyle.Underline);
      this.label2.Location = new Point(0, 0);
      this.label2.MaximumSize = new Size(273, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 17);
      this.label2.TabIndex = 9;
      this.label2.Text = "Nombre:";
      this.Espaciador1.AutoSize = true;
      this.Espaciador1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Espaciador1.Controls.Add((Control) this.LabelCodigo);
      this.Espaciador1.Controls.Add((Control) this.label1);
      this.Espaciador1.Dock = DockStyle.Top;
      this.Espaciador1.Location = new Point(13, 13);
      this.Espaciador1.MinimumSize = new Size(0, 40);
      this.Espaciador1.Name = "Espaciador1";
      this.Espaciador1.Size = new Size(273, 40);
      this.Espaciador1.TabIndex = 8;
      this.LabelCodigo.AutoSize = true;
      this.LabelCodigo.Dock = DockStyle.Fill;
      this.LabelCodigo.Location = new Point(63, 0);
      this.LabelCodigo.MaximumSize = new Size(205, 0);
      this.LabelCodigo.Name = "LabelCodigo";
      this.LabelCodigo.Size = new Size(61, 17);
      this.LabelCodigo.TabIndex = 11;
      this.LabelCodigo.Text = "Ninguno";
      this.label1.AutoSize = true;
      this.label1.Dock = DockStyle.Left;
      this.label1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold | FontStyle.Underline);
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 17);
      this.label1.TabIndex = 12;
      this.label1.Text = "Código:";
      this.ImgBarCode.BackColor = SystemColors.ScrollBar;
      this.ImgBarCode.BackgroundImageLayout = ImageLayout.Stretch;
      this.ImgBarCode.BorderStyle = BorderStyle.FixedSingle;
      this.ImgBarCode.Dock = DockStyle.Top;
      this.ImgBarCode.Location = new Point(13, 468);
      this.ImgBarCode.Margin = new Padding(3, 13, 3, 3);
      this.ImgBarCode.Name = "ImgBarCode";
      this.ImgBarCode.Size = new Size(273, 273);
      this.ImgBarCode.TabIndex = 15;
      this.ImgBarCode.TabStop = false;
      this.ImagenDeProducto.BackColor = SystemColors.ScrollBar;
      this.ImagenDeProducto.BackgroundImageLayout = ImageLayout.Stretch;
      this.ImagenDeProducto.BorderStyle = BorderStyle.FixedSingle;
      this.ImagenDeProducto.Dock = DockStyle.Top;
      this.ImagenDeProducto.Location = new Point(13, 120);
      this.ImagenDeProducto.Margin = new Padding(3, 13, 3, 3);
      this.ImagenDeProducto.Name = "ImagenDeProducto";
      this.ImagenDeProducto.Size = new Size(273, 273);
      this.ImagenDeProducto.TabIndex = 0;
      this.ImagenDeProducto.TabStop = false;
      this.panel9.BackColor = Color.LightSteelBlue;
      this.panel9.BackgroundImage = (Image) componentResourceManager.GetObject("panel9.BackgroundImage");
      this.panel9.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel9.Controls.Add((Control) this.BtnCerrarInformaciónDelProducto);
      this.panel9.Controls.Add((Control) this.InformacionDeProducto);
      this.panel9.Cursor = Cursors.Hand;
      this.panel9.Dock = DockStyle.Top;
      this.panel9.Location = new Point(1, 0);
      this.panel9.Margin = new Padding(0);
      this.panel9.Name = "panel9";
      this.panel9.Size = new Size(299, 25);
      this.panel9.TabIndex = 7;
      this.BtnCerrarInformaciónDelProducto.BackColor = Color.DimGray;
      this.BtnCerrarInformaciónDelProducto.BackgroundImage = (Image) Resources.cerrar;
      this.BtnCerrarInformaciónDelProducto.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnCerrarInformaciónDelProducto.Dock = DockStyle.Right;
      this.BtnCerrarInformaciónDelProducto.FlatAppearance.BorderColor = Color.DimGray;
      this.BtnCerrarInformaciónDelProducto.FlatStyle = FlatStyle.Flat;
      this.BtnCerrarInformaciónDelProducto.Location = new Point(274, 0);
      this.BtnCerrarInformaciónDelProducto.Margin = new Padding(4);
      this.BtnCerrarInformaciónDelProducto.Name = "BtnCerrarInformaciónDelProducto";
      this.BtnCerrarInformaciónDelProducto.Size = new Size(25, 25);
      this.BtnCerrarInformaciónDelProducto.TabIndex = 7;
      this.BtnCerrarInformaciónDelProducto.UseVisualStyleBackColor = false;
      this.BtnCerrarInformaciónDelProducto.Click += new EventHandler(this.BtnCerrarInformaciónDelProducto_Click);
      this.InformacionDeProducto.ActiveLinkColor = Color.LightGray;
      this.InformacionDeProducto.AutoSize = true;
      this.InformacionDeProducto.BackColor = Color.Transparent;
      this.InformacionDeProducto.Dock = DockStyle.Left;
      this.InformacionDeProducto.Font = new Font("Microsoft Sans Serif", 12f);
      this.InformacionDeProducto.LinkBehavior = LinkBehavior.NeverUnderline;
      this.InformacionDeProducto.LinkColor = Color.Black;
      this.InformacionDeProducto.Location = new Point(0, 0);
      this.InformacionDeProducto.Name = "InformacionDeProducto";
      this.InformacionDeProducto.Size = new Size(189, 20);
      this.InformacionDeProducto.TabIndex = 6;
      this.InformacionDeProducto.TabStop = true;
      this.InformacionDeProducto.Text = "Información del producto:";
      this.InformacionDeProducto.VisitedLinkColor = Color.Black;
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
      this.BtnVerInformaciónDelProducto.BackColor = Color.Brown;
      this.BtnVerInformaciónDelProducto.BackgroundImage = (Image) Resources.etiqueta;
      this.BtnVerInformaciónDelProducto.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnVerInformaciónDelProducto.FlatAppearance.BorderColor = Color.Brown;
      this.BtnVerInformaciónDelProducto.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnVerInformaciónDelProducto.FlatStyle = FlatStyle.Flat;
      this.BtnVerInformaciónDelProducto.Location = new Point(195, 3);
      this.BtnVerInformaciónDelProducto.Name = "BtnVerInformaciónDelProducto";
      this.BtnVerInformaciónDelProducto.Size = new Size(42, 42);
      this.BtnVerInformaciónDelProducto.TabIndex = 5;
      this.BtnVerInformaciónDelProducto.UseVisualStyleBackColor = false;
      this.BtnVerInformaciónDelProducto.Click += new EventHandler(this.BtnVerInformaciónDelProducto_Click);
      this.BTN_Copiar.BackColor = Color.Brown;
      this.BTN_Copiar.BackgroundImage = (Image) Resources.copiar;
      this.BTN_Copiar.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Copiar.FlatAppearance.BorderColor = Color.Brown;
      this.BTN_Copiar.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BTN_Copiar.FlatStyle = FlatStyle.Flat;
      this.BTN_Copiar.Location = new Point(51, 3);
      this.BTN_Copiar.Name = "BTN_Copiar";
      this.BTN_Copiar.Size = new Size(42, 42);
      this.BTN_Copiar.TabIndex = 4;
      this.BTN_Copiar.UseVisualStyleBackColor = false;
      this.BTN_Copiar.Click += new EventHandler(this.BTN_Copiar_Click);
      this.BtnImportarDesdeExcel.BackColor = Color.Brown;
      this.BtnImportarDesdeExcel.BackgroundImage = (Image) Resources.sobresalir;
      this.BtnImportarDesdeExcel.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnImportarDesdeExcel.FlatAppearance.BorderColor = Color.Brown;
      this.BtnImportarDesdeExcel.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnImportarDesdeExcel.FlatStyle = FlatStyle.Flat;
      this.BtnImportarDesdeExcel.Location = new Point(99, 3);
      this.BtnImportarDesdeExcel.Name = "BtnImportarDesdeExcel";
      this.BtnImportarDesdeExcel.Size = new Size(42, 42);
      this.BtnImportarDesdeExcel.TabIndex = 3;
      this.BtnImportarDesdeExcel.UseVisualStyleBackColor = false;
      this.BtnImportarDesdeExcel.Click += new EventHandler(this.BtnImportarDesdeExcel_Click);
      this.BtnVolverAClientes.BackColor = Color.Brown;
      this.BtnVolverAClientes.BackgroundImage = (Image) Resources.actualizar_pagina_opcion;
      this.BtnVolverAClientes.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnVolverAClientes.FlatAppearance.BorderColor = Color.Brown;
      this.BtnVolverAClientes.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnVolverAClientes.FlatStyle = FlatStyle.Flat;
      this.BtnVolverAClientes.Location = new Point(147, 3);
      this.BtnVolverAClientes.Name = "BtnVolverAClientes";
      this.BtnVolverAClientes.Size = new Size(42, 42);
      this.BtnVolverAClientes.TabIndex = 1;
      this.BtnVolverAClientes.UseVisualStyleBackColor = false;
      this.BtnVolverAClientes.Click += new EventHandler(this.BtnVolverAlInventario_Click);
      this.BtnGuardar.BackColor = Color.Brown;
      this.BtnGuardar.BackgroundImage = (Image) Resources.guardar_archivo_opcion;
      this.BtnGuardar.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnGuardar.FlatAppearance.BorderColor = Color.Brown;
      this.BtnGuardar.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnGuardar.FlatStyle = FlatStyle.Flat;
      this.BtnGuardar.Location = new Point(3, 3);
      this.BtnGuardar.Name = "BtnGuardar";
      this.BtnGuardar.Size = new Size(42, 42);
      this.BtnGuardar.TabIndex = 0;
      this.BtnGuardar.UseVisualStyleBackColor = false;
      this.BtnGuardar.Click += new EventHandler(this.BtnGuardar_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1045, 707);
      this.Controls.Add((Control) this.ListaInventario);
      this.Controls.Add((Control) this.PanelDeInformacionDeProducto);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Inventario);
      this.Text = nameof (Inventario);
      this.WindowState = FormWindowState.Minimized;
      this.Load += new EventHandler(this.Inventario_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.Panel6BusquedaNormal.ResumeLayout(false);
      this.Panel5BusquedaNormal.ResumeLayout(false);
      this.Panel5BusquedaNormal.PerformLayout();
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
      ((ISupportInitialize) this.ListaInventario).EndInit();
      this.PanelSuperior.ResumeLayout(false);
      this.PanelDeInformacionDeProducto.ResumeLayout(false);
      this.PanelDeInformacionDeProducto.PerformLayout();
      this.PanelDetallesDeProducto.ResumeLayout(false);
      this.PanelDetallesDeProducto.PerformLayout();
      this.panel12.ResumeLayout(false);
      this.panel12.PerformLayout();
      this.Espaciador0.ResumeLayout(false);
      this.Espaciador0.PerformLayout();
      this.Espaciador1.ResumeLayout(false);
      this.Espaciador1.PerformLayout();
      ((ISupportInitialize) this.ImgBarCode).EndInit();
      ((ISupportInitialize) this.ImagenDeProducto).EndInit();
      this.panel9.ResumeLayout(false);
      this.panel9.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
