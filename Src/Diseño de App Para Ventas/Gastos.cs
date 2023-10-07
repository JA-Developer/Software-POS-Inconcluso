// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Gastos
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
  public class Gastos : Form
  {
    private double DoubleDePrueba;
    public OleDbConnection Conn;
    private string ConsultaActual = "SELECT * FROM Gastos";
    private int ColumnaDeSortingActual;
    private SortOrder OrdenDeSortingActual;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel panel2;
    private Panel panel5;
    private Button BtnBuscarPor;
    private Label CodLabelParaTexto;
    private TextBox TxBuscarPor;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private Panel PanelSuperior;
    private DataGridView ListaGastos;
    private Button BtnGuardar;
    private Button BtnVolverAGastos;
    private ComboBox ListaBuscarPor;
    private Label LabelBuscarPor;
    private Panel Panel2BusquedaNormal;
    private Panel Panel1BusquedaNormal;
    private Panel Panel3BusquedaNormal;
    private NumericUpDown NumTxBuscarPorDesde;
    private Label CodLabelParaNúmerosDesde;
    private Panel Panel4BusquedaNormal;
    private NumericUpDown NumTxBuscarPorHasta;
    private Label CodLabelParaNúmerosHasta;
    private Panel Panel6BusquedaNormal;
    private DateTimePicker FechaTxBuscarPorHasta;
    private Label label3;
    private Panel Panel5BusquedaNormal;
    private DateTimePicker FechaTxBuscarPorDesde;
    private Label label4;
    private Panel Panel7BusquedaNormal;
    private Button BtnImportarDesdeExcel;
    private Button BTN_Copiar;
    private DataGridViewTextBoxColumn ColNoFactura;
    private DataGridViewTextBoxColumn ColDescripcion;
    private DataGridViewTextBoxColumn ColMonto;
    private DataGridViewTextBoxColumn ColImpuesto;
    private DataGridViewTextBoxColumn ColFecha;
    private Panel panel1;
    private Panel StatusStrip;

    public Gastos()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.InitializeComponent();
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaGastos, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.ListaGastos.CellValueChanged += new DataGridViewCellEventHandler(this.ListaGastos_CellValueChanged);
      this.ListaGastos.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaGastos_UserDeletingRow);
      this.ListaGastos.CellDoubleClick += new DataGridViewCellEventHandler(this.ListaGastos_CellDoubleClick);
      this.FormClosing += new FormClosingEventHandler(this.Gastos_FormClosing);
      this.ListaGastos.UserAddedRow += new DataGridViewRowEventHandler(this.ListaGastos_UserAddedRow);
      this.ListaGastos.CellValidating += new DataGridViewCellValidatingEventHandler(this.ListaGastos_CellValidating);
      this.ListaGastos.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.ListaGastos_ColumnHeaderMouseClick);
    }

    private string DateToString_ddMMyyyy(DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Day >= 10 ? str1 + (object) Date.Day : str1 + "0" + (object) Date.Day) + "/";
      string str3 = (Date.Month >= 10 ? str2 + (object) Date.Month : str2 + "0" + (object) Date.Month) + "/";
      return Date.Year >= 10 ? (Date.Year >= 100 ? (Date.Year >= 1000 ? str3 + (object) Date.Year : str3 + "0" + (object) Date.Year) : str3 + "00" + (object) Date.Year) : str3 + "000" + (object) Date.Year;
    }

    private bool VerificarSiExiste(string Id)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Gastos WHERE Id = '" + Id + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    private bool VerificarElementosSinGuardar()
    {
      bool flag = false;
      for (int index = 0; index < this.ListaGastos.Rows.Count - 1; ++index)
      {
        if (this.ListaGastos.Rows[index].Tag != (object) "NotAdded")
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

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      this.ListaGastos.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText += Consulta;
      if (Orden != SortOrder.None)
      {
        oleDbCommand.CommandText += " ORDER BY Gastos.";
        switch (IndexColumnaDeOrden)
        {
          case 0:
            oleDbCommand.CommandText += "NoFactura";
            break;
          case 1:
            oleDbCommand.CommandText += "Descripcion";
            break;
          case 2:
            oleDbCommand.CommandText += "Monto";
            break;
          case 3:
            oleDbCommand.CommandText += "Impuesto";
            break;
          case 4:
            oleDbCommand.CommandText += "Fecha";
            break;
          default:
            oleDbCommand.CommandText += "NoFactura";
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
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        dataGridViewRow.CreateCells(this.ListaGastos);
        dataGridViewRow.Cells[4].Tag = (object) this.DateToString_ddMMyyyy((DateTime) oleDbDataReader.GetValue(5));
        dataGridViewRow.SetValues(oleDbDataReader.GetValue(1), oleDbDataReader.GetValue(2), oleDbDataReader.GetValue(3), oleDbDataReader.GetValue(4), (object) this.DateToString_ddMMyyyy((DateTime) oleDbDataReader.GetValue(5)));
        this.ListaGastos.Rows.Add(dataGridViewRow);
        dataGridViewRow.Cells[0].Tag = (object) oleDbDataReader.GetValue(0).ToString();
      }
      for (int index = 0; index < this.ListaGastos.Rows.Count - 1; ++index)
        this.ListaGastos.Rows[index].Tag = (object) "NotAdded";
      this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[4].Tag = (object) this.DateToString_ddMMyyyy(DateTime.Today);
      this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[4].Value = (object) this.DateToString_ddMMyyyy(DateTime.Today);
      oleDbDataReader.Close();
      this.ConsultaActual = Consulta;
      for (int index = 0; index < this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells.Count; ++index)
        this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    public void ActualizarGastos() => this.EjecutarConsulta("SELECT * FROM Gastos", this.OrdenDeSortingActual, this.ColumnaDeSortingActual);

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
label_71:
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
                goto label_71;
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
                goto label_71;
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
                  goto label_71;
                }
                else if (obj.ToString().Contains("\\"))
                {
                  int num7 = (int) MessageBox.Show("No se permite el uso de la barra diagonal inversa (\\) en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  Tabla.Rows[index1].Cells[index3].Selected = true;
                  flag1 = false;
                  goto label_71;
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
                  oleDbCommand1.CommandText += "cDate('1/1/1753')";
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
                goto label_71;
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
                goto label_71;
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
          }
        }
      }
      return flag1;
    }

    private bool Guardar() => this.Guardar(this.ListaGastos, new List<int>()
    {
      0,
      1,
      2,
      3,
      4
    }, new List<string>()
    {
      "NoFactura",
      "Descripcion",
      "Monto",
      "Impuesto",
      "Fecha"
    }, new List<string>()
    {
      "String",
      "String",
      "Double",
      "Double",
      "Date"
    }, "NotAdded", false, 0, nameof (Gastos), true, new List<int>()
    {
      4
    }, "Id");

    private void BuscarPor()
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaGastos.Rows.Clear();
      string Consulta = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          Consulta = "SELECT * FROM Gastos WHERE NoFactura LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 1:
          Consulta = "SELECT * FROM Gastos WHERE Descripcion LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 2:
          Consulta = "SELECT * FROM Gastos WHERE Monto >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND Monto <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 3:
          Consulta = "SELECT * FROM Gastos WHERE Impuesto >= " + this.NumTxBuscarPorDesde.Value.ToString().Replace(",", ".") + " AND Impuesto <= " + this.NumTxBuscarPorHasta.Value.ToString().Replace(",", ".");
          break;
        case 4:
          Consulta = "SELECT * FROM Gastos WHERE Fecha >= cDate('" + this.DateToString_ddMMyyyy(this.FechaTxBuscarPorDesde.Value) + "') AND Fecha <= cDate('" + this.DateToString_ddMMyyyy(this.FechaTxBuscarPorHasta.Value) + "')";
          break;
      }
      this.EjecutarConsulta(Consulta, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void ListaGastos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ListaGastos.Rows[e.RowIndex].Tag == (object) "NotAdded")
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = this.Conn;
        oleDbCommand1.CommandText += "UPDATE Gastos SET ";
        bool flag = true;
        switch (e.ColumnIndex)
        {
          case 0:
            if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              OleDbCommand oleDbCommand2 = oleDbCommand1;
              oleDbCommand2.CommandText = oleDbCommand2.CommandText + "NoFactura = '" + this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
              break;
            }
            OleDbCommand oleDbCommand3 = oleDbCommand1;
            oleDbCommand3.CommandText = oleDbCommand3.CommandText + "NoFactura = '' WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
            break;
          case 1:
            if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              OleDbCommand oleDbCommand4 = oleDbCommand1;
              oleDbCommand4.CommandText = oleDbCommand4.CommandText + "Descripcion = '" + this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
              break;
            }
            OleDbCommand oleDbCommand5 = oleDbCommand1;
            oleDbCommand5.CommandText = oleDbCommand5.CommandText + "Descripcion = '' WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
            break;
          case 2:
            if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              this.DoubleDePrueba = 0.0;
              if (double.TryParse(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
              {
                OleDbCommand oleDbCommand6 = oleDbCommand1;
                oleDbCommand6.CommandText = oleDbCommand6.CommandText + "Monto = " + this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + " WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
                break;
              }
              int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) 0;
              this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
              flag = false;
              break;
            }
            OleDbCommand oleDbCommand7 = oleDbCommand1;
            oleDbCommand7.CommandText = oleDbCommand7.CommandText + "Monto = 0 WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
            break;
          case 3:
            if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              this.DoubleDePrueba = 0.0;
              if (double.TryParse(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
              {
                OleDbCommand oleDbCommand8 = oleDbCommand1;
                oleDbCommand8.CommandText = oleDbCommand8.CommandText + "Impuesto = " + this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + " WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
                break;
              }
              int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) 0;
              this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
              flag = false;
              break;
            }
            OleDbCommand oleDbCommand9 = oleDbCommand1;
            oleDbCommand9.CommandText = oleDbCommand9.CommandText + "Impuesto = 0 WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
            break;
          case 4:
            if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
              if (this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
              {
                try
                {
                  DateTime.ParseExact(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
                  OleDbCommand oleDbCommand10 = oleDbCommand1;
                  oleDbCommand10.CommandText = oleDbCommand10.CommandText + "Fecha = cDate('" + this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag + "') WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
                  break;
                }
                catch (Exception ex)
                {
                  int num1 = (int) MessageBox.Show(ex.Message);
                  int num2 = (int) MessageBox.Show("Esta celda solo admite fechas.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                  flag = false;
                  break;
                }
              }
            }
            OleDbCommand oleDbCommand11 = oleDbCommand1;
            oleDbCommand11.CommandText = oleDbCommand11.CommandText + "Fecha = cDate('0/0/0') WHERE Id = " + this.ListaGastos.Rows[e.RowIndex].Cells[0].Tag + ";";
            break;
        }
        if (!flag)
          return;
        oleDbCommand1.ExecuteNonQuery();
      }
      else
      {
        switch (e.ColumnIndex)
        {
          case 2:
            this.DoubleDePrueba = 0.0;
            if (double.TryParse(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
              break;
            int num3 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            break;
          case 3:
            this.DoubleDePrueba = 0.0;
            if (double.TryParse(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out this.DoubleDePrueba))
              break;
            int num4 = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            break;
          case 4:
            try
            {
              DateTime.ParseExact(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
              break;
            }
            catch (Exception ex)
            {
              int num5 = (int) MessageBox.Show("Esta celda solo admite fechas.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
              break;
            }
        }
      }
    }

    private void ListaGastos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      switch (e.ColumnIndex)
      {
        case 2:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            e.Cancel = true;
            break;
          }
          break;
        case 3:
          this.DoubleDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.DoubleDePrueba))
          {
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            e.Cancel = true;
            break;
          }
          break;
        case 4:
          try
          {
            DateTime.ParseExact(e.FormattedValue.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Esta celda solo admite fechas.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            e.Cancel = true;
            break;
          }
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

    private void ListaGastos_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      for (int index = 0; index < this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells.Count; ++index)
        this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
      e.Row.Cells[4].Tag = (object) this.DateToString_ddMMyyyy(DateTime.Today);
      e.Row.Cells[4].Value = (object) this.DateToString_ddMMyyyy(DateTime.Today);
    }

    private void ListaGastos_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (MessageBox.Show("La celda sera eliminada permanentemente, ¿Esta seguro de eliminar la celda?", "¿Esta seguro?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        if (e.Row.Tag != (object) "NotAdded")
          return;
        new OleDbCommand("DELETE FROM Gastos WHERE Id = " + e.Row.Cells[0].Tag + ";", this.Conn).ExecuteNonQuery();
      }
      else
        e.Cancel = true;
    }

    private void Gastos_Load(object sender, EventArgs e)
    {
      this.ActualizarGastos();
      this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[4].Tag = (object) this.DateToString_ddMMyyyy(DateTime.Today);
      this.ListaGastos.Rows[this.ListaGastos.Rows.Count - 1].Cells[4].Value = (object) this.DateToString_ddMMyyyy(DateTime.Today);
      this.ListaBuscarPor.SelectedIndex = 0;
    }

    private void BtnGuardar_Click(object sender, EventArgs e) => this.Guardar();

    private void BtnBuscarPor_Click(object sender, EventArgs e) => this.BuscarPor();

    private void BtnVolverAGastos_Click(object sender, EventArgs e)
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaGastos.Rows.Clear();
      this.ActualizarGastos();
    }

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void Gastos_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.VerificarElementosSinGuardar())
        return;
      e.Cancel = true;
    }

    private void ListaGastos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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

    private void ListaGastos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != 4 || e.RowIndex == -1)
        return;
      SeleccionDeFecha seleccionDeFecha = new SeleccionDeFecha();
      try
      {
        seleccionDeFecha.ControlFecha.SelectionStart = DateTime.ParseExact(this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
        if (seleccionDeFecha.ShowDialog() != DialogResult.OK)
          return;
        this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = (object) this.DateToString_ddMMyyyy(seleccionDeFecha.ControlFecha.SelectionStart);
        this.ListaGastos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) this.DateToString_ddMMyyyy(seleccionDeFecha.ControlFecha.SelectionStart);
      }
      catch (Exception ex)
      {
      }
    }

    private void ListaBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 1:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 2:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 3:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = true;
          this.Panel6BusquedaNormal.Visible = false;
          this.Panel5BusquedaNormal.Visible = false;
          break;
        case 4:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel4BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = false;
          this.Panel6BusquedaNormal.Visible = true;
          this.Panel5BusquedaNormal.Visible = true;
          break;
      }
    }

    private void NumTxBuscarPorDesde_ValueChanged(object sender, EventArgs e) => this.NumTxBuscarPorHasta.Minimum = this.NumTxBuscarPorDesde.Value;

    private void NumTxBuscarPorHasta_ValueChanged(object sender, EventArgs e) => this.NumTxBuscarPorDesde.Maximum = this.NumTxBuscarPorHasta.Value;

    private void FechaTxBuscarPorDesde_ValueChanged(object sender, EventArgs e) => this.FechaTxBuscarPorHasta.MinDate = this.FechaTxBuscarPorDesde.Value;

    private void FechaTxBuscarPorHasta_ValueChanged(object sender, EventArgs e) => this.FechaTxBuscarPorDesde.MaxDate = this.FechaTxBuscarPorHasta.Value;

    private void BtnImportarDesdeExcel_Click(object sender, EventArgs e)
    {
      ImportacionDeDatosDesdeExcel deDatosDesdeExcel = new ImportacionDeDatosDesdeExcel();
      deDatosDesdeExcel.AddColumnString("Número De Factura");
      deDatosDesdeExcel.AddColumnString("Descripción");
      deDatosDesdeExcel.AddColumnDouble("Monto");
      deDatosDesdeExcel.AddColumnDouble("Impuesto");
      deDatosDesdeExcel.AddColumnDate("Fecha");
      while (deDatosDesdeExcel.ShowDialog() == DialogResult.OK)
      {
        if (!this.Guardar(deDatosDesdeExcel.TablaDeImporte, new List<int>()
        {
          0,
          1,
          2,
          3,
          4
        }, new List<string>()
        {
          "NoFactura",
          "Descripcion",
          "Monto",
          "Impuesto",
          "Fecha"
        }, new List<string>()
        {
          "String",
          "String",
          "Double",
          "Double",
          "Date"
        }, "NotAdded", false, 0, nameof (Gastos), false, (List<int>) null, (string) null))
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
      this.EjecutarConsulta(this.ConsultaActual, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void BTN_Copiar_Click(object sender, EventArgs e)
    {
      if (this.ListaGastos.SelectedCells.Count <= 0)
        return;
      int num1 = this.ListaGastos.Rows.Count;
      int num2 = this.ListaGastos.Columns.Count;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < this.ListaGastos.SelectedCells.Count; ++index)
      {
        if (this.ListaGastos.SelectedCells[index].ColumnIndex < num2)
          num2 = this.ListaGastos.SelectedCells[index].ColumnIndex;
        if (this.ListaGastos.SelectedCells[index].RowIndex < num1)
          num1 = this.ListaGastos.SelectedCells[index].RowIndex;
        if (this.ListaGastos.SelectedCells[index].ColumnIndex > num4)
          num4 = this.ListaGastos.SelectedCells[index].ColumnIndex;
        if (this.ListaGastos.SelectedCells[index].RowIndex > num3)
          num3 = this.ListaGastos.SelectedCells[index].RowIndex;
      }
      string text = "";
      for (int rowIndex = num1; rowIndex <= num3; ++rowIndex)
      {
        for (int columnIndex = num2; columnIndex <= num4; ++columnIndex)
        {
          if (this.ListaGastos[columnIndex, rowIndex].Selected)
          {
            if (this.ListaGastos[columnIndex, rowIndex].Value != null)
              text += this.ListaGastos[columnIndex, rowIndex].Value.ToString();
            if (columnIndex != num4)
              text += "\t";
          }
        }
        text += Environment.NewLine;
      }
      if (text == null || !(text != ""))
        return;
      Clipboard.SetText(text);
    }

    private void StatusStrip_Paint(object sender, PaintEventArgs e) => this.StatusStrip.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.StatusStrip.Width, 0));

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Gastos));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.panel5 = new Panel();
      this.Panel7BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel6BusquedaNormal = new Panel();
      this.FechaTxBuscarPorHasta = new DateTimePicker();
      this.label3 = new Label();
      this.Panel5BusquedaNormal = new Panel();
      this.FechaTxBuscarPorDesde = new DateTimePicker();
      this.label4 = new Label();
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
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.PanelSuperior = new Panel();
      this.BTN_Copiar = new Button();
      this.BtnImportarDesdeExcel = new Button();
      this.BtnVolverAGastos = new Button();
      this.BtnGuardar = new Button();
      this.ListaGastos = new DataGridView();
      this.ColNoFactura = new DataGridViewTextBoxColumn();
      this.ColDescripcion = new DataGridViewTextBoxColumn();
      this.ColMonto = new DataGridViewTextBoxColumn();
      this.ColImpuesto = new DataGridViewTextBoxColumn();
      this.ColFecha = new DataGridViewTextBoxColumn();
      this.StatusStrip = new Panel();
      this.panel2.SuspendLayout();
      this.panel5.SuspendLayout();
      this.Panel7BusquedaNormal.SuspendLayout();
      this.Panel6BusquedaNormal.SuspendLayout();
      this.Panel5BusquedaNormal.SuspendLayout();
      this.Panel4BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorHasta.BeginInit();
      this.Panel3BusquedaNormal.SuspendLayout();
      this.NumTxBuscarPorDesde.BeginInit();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      this.panel6.SuspendLayout();
      this.PanelSuperior.SuspendLayout();
      ((ISupportInitialize) this.ListaGastos).BeginInit();
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
      this.panel2.Size = new Size(300, 661);
      this.panel2.TabIndex = 6;
      this.panel1.BackColor = SystemColors.ScrollBar;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 266);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 395);
      this.panel1.TabIndex = 14;
      this.panel5.AutoSize = true;
      this.panel5.BackColor = Color.DimGray;
      this.panel5.Controls.Add((Control) this.Panel7BusquedaNormal);
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
      this.panel5.Size = new Size(299, 241);
      this.panel5.TabIndex = 8;
      this.Panel7BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel7BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel7BusquedaNormal.Dock = DockStyle.Top;
      this.Panel7BusquedaNormal.Location = new Point(0, 197);
      this.Panel7BusquedaNormal.Name = "Panel7BusquedaNormal";
      this.Panel7BusquedaNormal.Size = new Size(299, 43);
      this.Panel7BusquedaNormal.TabIndex = 14;
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
      this.Panel6BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel6BusquedaNormal.Controls.Add((Control) this.FechaTxBuscarPorHasta);
      this.Panel6BusquedaNormal.Controls.Add((Control) this.label3);
      this.Panel6BusquedaNormal.Dock = DockStyle.Top;
      this.Panel6BusquedaNormal.Location = new Point(0, 165);
      this.Panel6BusquedaNormal.Name = "Panel6BusquedaNormal";
      this.Panel6BusquedaNormal.Size = new Size(299, 32);
      this.Panel6BusquedaNormal.TabIndex = 13;
      this.Panel6BusquedaNormal.Visible = false;
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
      this.Panel5BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel5BusquedaNormal.Controls.Add((Control) this.FechaTxBuscarPorDesde);
      this.Panel5BusquedaNormal.Controls.Add((Control) this.label4);
      this.Panel5BusquedaNormal.Dock = DockStyle.Top;
      this.Panel5BusquedaNormal.Location = new Point(0, 133);
      this.Panel5BusquedaNormal.Name = "Panel5BusquedaNormal";
      this.Panel5BusquedaNormal.Size = new Size(299, 32);
      this.Panel5BusquedaNormal.TabIndex = 12;
      this.Panel5BusquedaNormal.Visible = false;
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
      this.Panel4BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel4BusquedaNormal.Controls.Add((Control) this.NumTxBuscarPorHasta);
      this.Panel4BusquedaNormal.Controls.Add((Control) this.CodLabelParaNúmerosHasta);
      this.Panel4BusquedaNormal.Dock = DockStyle.Top;
      this.Panel4BusquedaNormal.Location = new Point(0, 101);
      this.Panel4BusquedaNormal.Name = "Panel4BusquedaNormal";
      this.Panel4BusquedaNormal.Size = new Size(299, 32);
      this.Panel4BusquedaNormal.TabIndex = 11;
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
      this.NumTxBuscarPorHasta.ValueChanged += new EventHandler(this.NumTxBuscarPorHasta_ValueChanged);
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
      this.Panel3BusquedaNormal.TabIndex = 10;
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
      this.NumTxBuscarPorDesde.ValueChanged += new EventHandler(this.NumTxBuscarPorDesde_ValueChanged);
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
      this.Panel2BusquedaNormal.TabIndex = 9;
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
      this.ListaBuscarPor.Items.AddRange(new object[5]
      {
        (object) "Número De Factura",
        (object) "Descripción",
        (object) "Monto",
        (object) "Impuesto",
        (object) "Fecha"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.panel6.BackColor = Color.LightSteelBlue;
      this.panel6.BackgroundImage = (Image) componentResourceManager.GetObject("panel6.BackgroundImage");
      this.panel6.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel6.Controls.Add((Control) this.linkLabel2);
      this.panel6.Cursor = Cursors.Hand;
      this.panel6.Dock = DockStyle.Top;
      this.panel6.Font = new Font("Microsoft Sans Serif", 10f);
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
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BTN_Copiar);
      this.PanelSuperior.Controls.Add((Control) this.BtnImportarDesdeExcel);
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAGastos);
      this.PanelSuperior.Controls.Add((Control) this.BtnGuardar);
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 5;
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
      this.BtnVolverAGastos.BackColor = Color.Brown;
      this.BtnVolverAGastos.BackgroundImage = (Image) Resources.actualizar_pagina_opcion;
      this.BtnVolverAGastos.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnVolverAGastos.FlatAppearance.BorderColor = Color.Brown;
      this.BtnVolverAGastos.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnVolverAGastos.FlatStyle = FlatStyle.Flat;
      this.BtnVolverAGastos.Location = new Point(147, 3);
      this.BtnVolverAGastos.Name = "BtnVolverAGastos";
      this.BtnVolverAGastos.Size = new Size(42, 42);
      this.BtnVolverAGastos.TabIndex = 1;
      this.BtnVolverAGastos.UseVisualStyleBackColor = false;
      this.BtnVolverAGastos.Click += new EventHandler(this.BtnVolverAGastos_Click);
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
      this.ListaGastos.BackgroundColor = SystemColors.ScrollBar;
      this.ListaGastos.BorderStyle = BorderStyle.None;
      this.ListaGastos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaGastos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaGastos.Columns.AddRange((DataGridViewColumn) this.ColNoFactura, (DataGridViewColumn) this.ColDescripcion, (DataGridViewColumn) this.ColMonto, (DataGridViewColumn) this.ColImpuesto, (DataGridViewColumn) this.ColFecha);
      this.ListaGastos.Dock = DockStyle.Fill;
      this.ListaGastos.GridColor = Color.Gray;
      this.ListaGastos.Location = new Point(300, 50);
      this.ListaGastos.Margin = new Padding(4);
      this.ListaGastos.Name = "ListaGastos";
      this.ListaGastos.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaGastos.Size = new Size(745, 661);
      this.ListaGastos.TabIndex = 9;
      gridViewCellStyle1.NullValue = (object) null;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.ColNoFactura.DefaultCellStyle = gridViewCellStyle1;
      this.ColNoFactura.HeaderText = "Número De Factura";
      this.ColNoFactura.Name = "ColNoFactura";
      this.ColNoFactura.Resizable = DataGridViewTriState.False;
      this.ColNoFactura.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.ColNoFactura.Width = 200;
      gridViewCellStyle2.NullValue = (object) null;
      this.ColDescripcion.DefaultCellStyle = gridViewCellStyle2;
      this.ColDescripcion.HeaderText = "Descripción";
      this.ColDescripcion.Name = "ColDescripcion";
      this.ColDescripcion.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.ColDescripcion.Width = 200;
      gridViewCellStyle3.NullValue = (object) "0";
      this.ColMonto.DefaultCellStyle = gridViewCellStyle3;
      this.ColMonto.HeaderText = "Monto";
      this.ColMonto.Name = "ColMonto";
      this.ColMonto.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle4.NullValue = (object) "0";
      this.ColImpuesto.DefaultCellStyle = gridViewCellStyle4;
      this.ColImpuesto.HeaderText = "Impuesto";
      this.ColImpuesto.Name = "ColImpuesto";
      this.ColImpuesto.SortMode = DataGridViewColumnSortMode.Programmatic;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.True;
      this.ColFecha.DefaultCellStyle = gridViewCellStyle5;
      this.ColFecha.HeaderText = "Fecha";
      this.ColFecha.Name = "ColFecha";
      this.ColFecha.ReadOnly = true;
      this.ColFecha.Resizable = DataGridViewTriState.True;
      this.ColFecha.SortMode = DataGridViewColumnSortMode.Programmatic;
      this.ColFecha.Width = 120;
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 711);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 15;
      this.StatusStrip.Paint += new PaintEventHandler(this.StatusStrip_Paint);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1045, 733);
      this.Controls.Add((Control) this.ListaGastos);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Gastos);
      this.Text = nameof (Gastos);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Gastos_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.Panel7BusquedaNormal.ResumeLayout(false);
      this.Panel6BusquedaNormal.ResumeLayout(false);
      this.Panel6BusquedaNormal.PerformLayout();
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
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.PanelSuperior.ResumeLayout(false);
      ((ISupportInitialize) this.ListaGastos).EndInit();
      this.ResumeLayout(false);
    }
  }
}
