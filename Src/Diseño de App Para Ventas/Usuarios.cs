// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Usuarios
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
  public class Usuarios : Form
  {
    public OleDbConnection Conn;
    private string ConsultaActual = "SELECT * FROM Usuarios";
    private int ColumnaDeSortingActual;
    public bool ReadOnly;
    private SortOrder OrdenDeSortingActual;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel PanelFondoIzquierdo;
    private Panel panel5;
    private Label CodLabelParaTexto;
    private TextBox TxBuscarPor;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private DataGridView ListaUsuarios;
    private ComboBox ListaBuscarPor;
    private Label LabelBuscarPor;
    private Panel Panel2BusquedaNormal;
    private Panel Panel1BusquedaNormal;
    private Panel Panel4BusquedaNormal;
    private Button BtnBuscarPor;
    private Panel Panel3BusquedaNormal;
    private ComboBox CmBxTxBuscarPorTipoDeCuenta;
    private Label CodLabelParaProveedores;
    private DataGridViewTextBoxColumn ColNoFactura;
    private DataGridViewTextBoxColumn ColMonto;
    private DataGridViewTextBoxColumn ColDescripcion;
    private DataGridViewComboBoxColumn ColTipoDeCuenta;
    private Panel PanelSuperior;
    private Button BTN_Copiar;
    private Button BTN_ImportarDesdeExcel;
    private Button BtnVolverAProveedores;
    private Button BtnGuardar;
    private Panel panel1;
    private Panel StatusStrip;

    public Usuarios()
    {
      this.InitializeComponent();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaUsuarios, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.ListaUsuarios.CellValueChanged += new DataGridViewCellEventHandler(this.ListaUsuarios_CellValueChanged);
      this.ListaUsuarios.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaUsuarios_UserDeletingRow);
      this.ListaUsuarios.CellDoubleClick += new DataGridViewCellEventHandler(this.ListaUsuarios_CellDoubleClick);
      this.FormClosing += new FormClosingEventHandler(this.Usuarios_FormClosing);
      this.ListaUsuarios.UserAddedRow += new DataGridViewRowEventHandler(this.ListaUsuarios_UserAddedRow);
      this.ListaUsuarios.CellValidating += new DataGridViewCellValidatingEventHandler(this.ListaUsuarios_CellValidating);
    }

    private string DateToString_ddMMyyyy(DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Day >= 10 ? str1 + (object) Date.Day : str1 + "0" + (object) Date.Day) + "/";
      string str3 = (Date.Month >= 10 ? str2 + (object) Date.Month : str2 + "0" + (object) Date.Month) + "/";
      return Date.Year >= 10 ? (Date.Year >= 100 ? (Date.Year >= 1000 ? str3 + (object) Date.Year : str3 + "0" + (object) Date.Year) : str3 + "00" + (object) Date.Year) : str3 + "000" + (object) Date.Year;
    }

    private string GenerarMascaraDeClave(int Digitos)
    {
      string str = "";
      for (int index = 0; index < Digitos; ++index)
        str += "*";
      return str;
    }

    private bool VerificarSiExiste(string Usuario)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Usuarios WHERE Usuario = '" + Usuario + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    private bool VerificarElementosSinGuardar()
    {
      bool flag1 = false;
      for (int index = 0; index < this.ListaUsuarios.Rows.Count - 1; ++index)
      {
        if (this.ListaUsuarios.Rows[index].Tag != (object) "NotAdded")
        {
          flag1 = true;
          break;
        }
      }
      DialogResult dialogResult = DialogResult.Yes;
      if (flag1)
        dialogResult = MessageBox.Show("Aun hay elementos sin guardar en la tabla, ¿Desea guardarlos antes de continuar?", "Guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      bool flag2;
      if (dialogResult == DialogResult.Yes)
        flag2 = this.Guardar(this.ListaUsuarios, new List<int>()
        {
          0,
          1,
          2,
          3
        }, new List<string>()
        {
          "Usuario",
          "Nombre",
          "Clave",
          "TipoDeCuenta"
        }, new List<string>()
        {
          "String",
          "String",
          "String",
          "String"
        }, "NotAdded", true, 0, nameof (Usuarios), true, new List<int>()
        {
          2,
          3
        });
      else
        flag2 = true;
      return flag2;
    }

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      this.ListaUsuarios.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText += Consulta;
      if (Orden != SortOrder.None && IndexColumnaDeOrden != 2)
      {
        oleDbCommand.CommandText += " ORDER BY Usuarios.";
        switch (IndexColumnaDeOrden)
        {
          case 0:
            oleDbCommand.CommandText += "Usuario";
            break;
          case 1:
            oleDbCommand.CommandText += "Nombre";
            break;
          case 3:
            oleDbCommand.CommandText += "TipoDeCuenta";
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
        dataGridViewRow.CreateCells(this.ListaUsuarios);
        switch (oleDbDataReader.GetValue(3).ToString())
        {
          case "0":
            dataGridViewRow.SetValues(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), (object) this.GenerarMascaraDeClave(oleDbDataReader.GetValue(2).ToString().Count<char>()), (object) "Empleado");
            break;
          case "1":
            dataGridViewRow.SetValues(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), (object) this.GenerarMascaraDeClave(oleDbDataReader.GetValue(2).ToString().Count<char>()), (object) "Administrador");
            break;
          default:
            dataGridViewRow.SetValues(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), (object) this.GenerarMascaraDeClave(oleDbDataReader.GetValue(2).ToString().Count<char>()), (object) "Empleado");
            break;
        }
        this.ListaUsuarios.Rows.Add(dataGridViewRow);
        dataGridViewRow.Cells[3].Tag = (object) oleDbDataReader.GetValue(3).ToString();
        dataGridViewRow.Cells[2].Tag = (object) oleDbDataReader.GetValue(2).ToString();
        dataGridViewRow.Cells[0].Tag = (object) oleDbDataReader.GetValue(0).ToString();
      }
      for (int index = 0; index < this.ListaUsuarios.Rows.Count - 1; ++index)
        this.ListaUsuarios.Rows[index].Tag = (object) "NotAdded";
      oleDbDataReader.Close();
      this.ConsultaActual = Consulta;
      if (this.ReadOnly)
        return;
      for (int index = 0; index < this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells.Count; ++index)
        this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
      this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[2].Tag = (object) "";
    }

    public void ActualizarUsuarios() => this.EjecutarConsulta("SELECT * FROM Usuarios", this.OrdenDeSortingActual, this.ColumnaDeSortingActual);

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
      List<int> SaveJustTagFrom)
    {
      bool flag1 = true;
      int num1 = Tabla.Rows.Count;
      if (OmitirUltimaLinea)
        num1 = Tabla.Rows.Count - 1;
label_69:
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
                goto label_69;
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
                goto label_69;
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
                  goto label_69;
                }
                else if (obj.ToString().Contains("\\"))
                {
                  int num7 = (int) MessageBox.Show("No se permite el uso de la barra diagonal inversa (\\) en las celdas.", "Caracteres no soportados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  Tabla.Rows[index1].Cells[index3].Selected = true;
                  flag1 = false;
                  goto label_69;
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
                goto label_69;
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
                goto label_69;
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

    private void BuscarPor()
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaUsuarios.Rows.Clear();
      string Consulta = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          Consulta = "SELECT * FROM Usuarios WHERE Usuario LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 1:
          Consulta = "SELECT * FROM Usuarios WHERE Nombre LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 2:
          Consulta = "SELECT * FROM Usuarios WHERE TipoDeCuenta = " + (object) this.CmBxTxBuscarPorTipoDeCuenta.SelectedIndex;
          break;
      }
      this.EjecutarConsulta(Consulta, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void ListaUsuarios_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ListaUsuarios.Rows[e.RowIndex].Tag == (object) "NotAdded")
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = this.Conn;
        oleDbCommand1.CommandText += "UPDATE Usuarios SET ";
        bool flag = true;
        switch (e.ColumnIndex)
        {
          case 0:
            if (this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              if (!this.VerificarSiExiste(this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString()))
              {
                OleDbCommand oleDbCommand2 = oleDbCommand1;
                oleDbCommand2.CommandText = oleDbCommand2.CommandText + "Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
                int num1 = 0;
                if (this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Value != null)
                {
                  switch (this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Value.ToString())
                  {
                    case "Empleado":
                      num1 = 0;
                      break;
                    case "Administrador":
                      num1 = 1;
                      break;
                    default:
                      num1 = 0;
                      break;
                  }
                }
                string str1 = "";
                string str2 = "";
                if (this.ListaUsuarios.Rows[e.RowIndex].Cells[1].Value != null)
                  str1 = this.ListaUsuarios.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Tag != null)
                  str2 = this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Tag.ToString();
                string str3 = "INSERT INTO Usuarios Values ('" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString() + "', '" + str1 + "', '" + str2 + "', " + (object) num1 + ");";
                string str4 = "UPDATE Ventas SET UsuarioVendedor = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString() + "' WHERE UsuarioVendedor = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
                string str5 = "DELETE FROM Usuarios WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
                oleDbCommand1.Transaction = this.Conn.BeginTransaction();
                try
                {
                  oleDbCommand1.CommandText = str3;
                  oleDbCommand1.ExecuteNonQuery();
                  oleDbCommand1.CommandText = str4;
                  oleDbCommand1.ExecuteNonQuery();
                  oleDbCommand1.CommandText = str5;
                  oleDbCommand1.ExecuteNonQuery();
                  this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag = (object) this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString();
                  oleDbCommand1.Transaction.Commit();
                }
                catch (Exception ex)
                {
                  oleDbCommand1.Transaction.Rollback();
                  int num2 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                flag = false;
                break;
              }
              int num = (int) MessageBox.Show("Ya existe un usuario con este nombre de usuario.", "Nombre de usuario existente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.ListaUsuarios.Rows[e.RowIndex].Selected = true;
              flag = false;
              break;
            }
            this.ListaUsuarios.Rows[e.RowIndex].Selected = true;
            int num3 = (int) MessageBox.Show("No es posible asignar al nombre de usuario un valor vacío.", "Nombre de usuario vacío", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            flag = false;
            break;
          case 1:
            if (this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              OleDbCommand oleDbCommand3 = oleDbCommand1;
              oleDbCommand3.CommandText = oleDbCommand3.CommandText + "Nombre = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
              break;
            }
            OleDbCommand oleDbCommand4 = oleDbCommand1;
            oleDbCommand4.CommandText = oleDbCommand4.CommandText + "Nombre = '' WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
            break;
          case 2:
            flag = false;
            break;
          case 3:
            if (this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              switch (this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
              {
                case "Empleado":
                  OleDbCommand oleDbCommand5 = oleDbCommand1;
                  oleDbCommand5.CommandText = oleDbCommand5.CommandText + "TipoDeCuenta = 0 WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
                  this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 0;
                  break;
                case "Administrador":
                  OleDbCommand oleDbCommand6 = oleDbCommand1;
                  oleDbCommand6.CommandText = oleDbCommand6.CommandText + "TipoDeCuenta = 1 WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
                  this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 1;
                  break;
                default:
                  OleDbCommand oleDbCommand7 = oleDbCommand1;
                  oleDbCommand7.CommandText = oleDbCommand7.CommandText + "TipoDeCuenta = 0 WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
                  this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 0;
                  break;
              }
            }
            else
            {
              OleDbCommand oleDbCommand8 = oleDbCommand1;
              oleDbCommand8.CommandText = oleDbCommand8.CommandText + "TipoDeCuenta = 0 WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
              this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 0;
              break;
            }
            break;
        }
        if (!flag)
          return;
        oleDbCommand1.ExecuteNonQuery();
        if (e.ColumnIndex != 0)
          return;
        this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag = (object) this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString();
      }
      else
      {
        try
        {
          if (e.ColumnIndex != 3)
            return;
          switch (this.ListaUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
          {
            case "Empleado":
              this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 0;
              break;
            case "Administrador":
              this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 1;
              break;
            default:
              this.ListaUsuarios.Rows[e.RowIndex].Cells[3].Tag = (object) 0;
              break;
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void ListaUsuarios_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.ColumnIndex == 0 && e.RowIndex != -1 && e.RowIndex != this.ListaUsuarios.Rows.Count - 1 && (e.FormattedValue == null || e.FormattedValue == (object) ""))
      {
        int num = (int) MessageBox.Show("No es posible asignar al nombre de usuario un valor vacío.", "Nombre de usuario vacío", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.Cancel = true;
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

    private void ListaUsuarios_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[2].Tag = (object) "";
      this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[3].Tag = (object) 0;
      for (int index = 0; index < this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells.Count; ++index)
        this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    private void ListaUsuarios_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
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
            oleDbCommand.CommandText = "DELETE FROM Ventas WHERE UsuarioVendedor = '" + e.Row.Cells[0].Tag.ToString() + "';";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "DELETE FROM Usuarios WHERE Usuario = '" + e.Row.Cells[0].Tag.ToString() + "';";
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

    private void Usuarios_Load(object sender, EventArgs e)
    {
      this.ActualizarUsuarios();
      if (!this.ReadOnly)
      {
        this.ListaUsuarios.Rows[this.ListaUsuarios.Rows.Count - 1].Cells[3].Tag = (object) 0;
      }
      else
      {
        this.ListaUsuarios.ReadOnly = true;
        this.ListaUsuarios.AllowDrop = false;
        this.ListaUsuarios.AllowUserToDeleteRows = false;
        this.ListaUsuarios.AllowUserToAddRows = false;
        this.BTN_ImportarDesdeExcel.Enabled = false;
      }
      this.ListaBuscarPor.SelectedIndex = 0;
      this.CmBxTxBuscarPorTipoDeCuenta.SelectedIndex = 0;
    }

    private void BtnGuardar_Click(object sender, EventArgs e) => this.Guardar(this.ListaUsuarios, new List<int>()
    {
      0,
      1,
      2,
      3
    }, new List<string>()
    {
      "Usuario",
      "Nombre",
      "Clave",
      "TipoDeCuenta"
    }, new List<string>()
    {
      "String",
      "String",
      "String",
      "String"
    }, "NotAdded", true, 0, nameof (Usuarios), true, new List<int>()
    {
      2,
      3
    });

    private void BtnBuscarPor_Click(object sender, EventArgs e) => this.BuscarPor();

    private void BtnVolverAUsuarios_Click(object sender, EventArgs e)
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaUsuarios.Rows.Clear();
      this.ActualizarUsuarios();
    }

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void Usuarios_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.VerificarElementosSinGuardar())
        return;
      e.Cancel = true;
    }

    private void ListaUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ReadOnly || e.ColumnIndex != 2 || e.RowIndex == -1 || e.RowIndex == this.ListaUsuarios.Rows.Count - 1)
        return;
      IngresoDeClave ingresoDeClave = new IngresoDeClave();
      if (this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Tag == null)
        ingresoDeClave.TxBoxClave.Text = "";
      else
        ingresoDeClave.TxBoxClave.Text = this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Tag.ToString();
      try
      {
        if (ingresoDeClave.ShowDialog() != DialogResult.OK)
          return;
        this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Tag = (object) ingresoDeClave.TxBoxClave.Text;
        this.ListaUsuarios.Rows[e.RowIndex].Cells[2].Value = (object) this.GenerarMascaraDeClave(ingresoDeClave.TxBoxClave.Text.Count<char>());
        if (!(this.ListaUsuarios.Rows[e.RowIndex].Tag.ToString() == "NotAdded"))
          return;
        OleDbCommand oleDbCommand = new OleDbCommand();
        oleDbCommand.Connection = this.Conn;
        oleDbCommand.CommandText = "UPDATE Usuarios SET Clave = '" + ingresoDeClave.TxBoxClave.Text + "' WHERE Usuario = '" + this.ListaUsuarios.Rows[e.RowIndex].Cells[0].Tag + "';";
        oleDbCommand.ExecuteNonQuery();
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
          this.Panel3BusquedaNormal.Visible = false;
          break;
        case 1:
          this.Panel2BusquedaNormal.Visible = true;
          this.Panel3BusquedaNormal.Visible = false;
          break;
        case 2:
          this.Panel2BusquedaNormal.Visible = false;
          this.Panel3BusquedaNormal.Visible = true;
          break;
      }
    }

    private void BTN_ImportarDesdeExcel_Click(object sender, EventArgs e)
    {
      ImportacionDeDatosDesdeExcel deDatosDesdeExcel = new ImportacionDeDatosDesdeExcel();
      deDatosDesdeExcel.AddColumnString("Nombre De Usuario");
      deDatosDesdeExcel.AddColumnString("Nombre");
      deDatosDesdeExcel.AddColumnString("Clave");
      deDatosDesdeExcel.AddColumnList("Tipo De Cuenta", new List<string>()
      {
        "Empleado",
        "Administrador"
      }, "TipoDeCuenta");
      while (deDatosDesdeExcel.ShowDialog() == DialogResult.OK)
      {
        if (!this.Guardar(deDatosDesdeExcel.TablaDeImporte, new List<int>()
        {
          0,
          1,
          2,
          3
        }, new List<string>()
        {
          "Usuario",
          "Nombre",
          "Clave",
          "TipoDeCuenta"
        }, new List<string>()
        {
          "String",
          "String",
          "String",
          "#List:TipoDeCuenta"
        }, "NotAdded", true, 0, nameof (Usuarios), false, new List<int>()
        {
          3
        }))
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
      if (this.ListaUsuarios.SelectedCells.Count <= 0)
        return;
      int num1 = this.ListaUsuarios.Rows.Count;
      int num2 = this.ListaUsuarios.Columns.Count;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < this.ListaUsuarios.SelectedCells.Count; ++index)
      {
        if (this.ListaUsuarios.SelectedCells[index].ColumnIndex < num2)
          num2 = this.ListaUsuarios.SelectedCells[index].ColumnIndex;
        if (this.ListaUsuarios.SelectedCells[index].RowIndex < num1)
          num1 = this.ListaUsuarios.SelectedCells[index].RowIndex;
        if (this.ListaUsuarios.SelectedCells[index].ColumnIndex > num4)
          num4 = this.ListaUsuarios.SelectedCells[index].ColumnIndex;
        if (this.ListaUsuarios.SelectedCells[index].RowIndex > num3)
          num3 = this.ListaUsuarios.SelectedCells[index].RowIndex;
      }
      string text = "";
      for (int rowIndex = num1; rowIndex <= num3; ++rowIndex)
      {
        for (int columnIndex = num2; columnIndex <= num4; ++columnIndex)
        {
          if (this.ListaUsuarios[columnIndex, rowIndex].Selected)
          {
            if (this.ListaUsuarios[columnIndex, rowIndex].Value != null)
              text += this.ListaUsuarios[columnIndex, rowIndex].Value.ToString();
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
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Usuarios));
      this.PanelFondoIzquierdo = new Panel();
      this.panel1 = new Panel();
      this.panel5 = new Panel();
      this.Panel4BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel3BusquedaNormal = new Panel();
      this.CmBxTxBuscarPorTipoDeCuenta = new ComboBox();
      this.CodLabelParaProveedores = new Label();
      this.Panel2BusquedaNormal = new Panel();
      this.CodLabelParaTexto = new Label();
      this.TxBuscarPor = new TextBox();
      this.Panel1BusquedaNormal = new Panel();
      this.LabelBuscarPor = new Label();
      this.ListaBuscarPor = new ComboBox();
      this.ListaUsuarios = new DataGridView();
      this.ColNoFactura = new DataGridViewTextBoxColumn();
      this.ColMonto = new DataGridViewTextBoxColumn();
      this.ColDescripcion = new DataGridViewTextBoxColumn();
      this.ColTipoDeCuenta = new DataGridViewComboBoxColumn();
      this.PanelSuperior = new Panel();
      this.StatusStrip = new Panel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.BTN_Copiar = new Button();
      this.BTN_ImportarDesdeExcel = new Button();
      this.BtnVolverAProveedores = new Button();
      this.BtnGuardar = new Button();
      this.PanelFondoIzquierdo.SuspendLayout();
      this.panel5.SuspendLayout();
      this.Panel4BusquedaNormal.SuspendLayout();
      this.Panel3BusquedaNormal.SuspendLayout();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      ((ISupportInitialize) this.ListaUsuarios).BeginInit();
      this.PanelSuperior.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.PanelFondoIzquierdo.BackColor = Color.DimGray;
      this.PanelFondoIzquierdo.Controls.Add((Control) this.panel1);
      this.PanelFondoIzquierdo.Controls.Add((Control) this.panel5);
      this.PanelFondoIzquierdo.Controls.Add((Control) this.panel6);
      this.PanelFondoIzquierdo.Dock = DockStyle.Left;
      this.PanelFondoIzquierdo.Location = new Point(0, 50);
      this.PanelFondoIzquierdo.Margin = new Padding(4);
      this.PanelFondoIzquierdo.Name = "PanelFondoIzquierdo";
      this.PanelFondoIzquierdo.Padding = new Padding(0, 0, 1, 0);
      this.PanelFondoIzquierdo.Size = new Size(300, 582);
      this.PanelFondoIzquierdo.TabIndex = 6;
      this.panel1.BackColor = SystemColors.ScrollBar;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 170);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 412);
      this.panel1.TabIndex = 13;
      this.panel5.AutoSize = true;
      this.panel5.BackColor = Color.DimGray;
      this.panel5.Controls.Add((Control) this.Panel4BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel3BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel2BusquedaNormal);
      this.panel5.Controls.Add((Control) this.Panel1BusquedaNormal);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 25);
      this.panel5.Margin = new Padding(4);
      this.panel5.Name = "panel5";
      this.panel5.Padding = new Padding(0, 1, 0, 1);
      this.panel5.Size = new Size(299, 145);
      this.panel5.TabIndex = 8;
      this.Panel4BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel4BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel4BusquedaNormal.Dock = DockStyle.Top;
      this.Panel4BusquedaNormal.Location = new Point(0, 101);
      this.Panel4BusquedaNormal.Name = "Panel4BusquedaNormal";
      this.Panel4BusquedaNormal.Size = new Size(299, 43);
      this.Panel4BusquedaNormal.TabIndex = 19;
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
      this.Panel3BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel3BusquedaNormal.Controls.Add((Control) this.CmBxTxBuscarPorTipoDeCuenta);
      this.Panel3BusquedaNormal.Controls.Add((Control) this.CodLabelParaProveedores);
      this.Panel3BusquedaNormal.Dock = DockStyle.Top;
      this.Panel3BusquedaNormal.Location = new Point(0, 69);
      this.Panel3BusquedaNormal.Name = "Panel3BusquedaNormal";
      this.Panel3BusquedaNormal.Size = new Size(299, 32);
      this.Panel3BusquedaNormal.TabIndex = 18;
      this.CmBxTxBuscarPorTipoDeCuenta.DropDownStyle = ComboBoxStyle.DropDownList;
      this.CmBxTxBuscarPorTipoDeCuenta.FormattingEnabled = true;
      this.CmBxTxBuscarPorTipoDeCuenta.Items.AddRange(new object[2]
      {
        (object) "Empleado",
        (object) "Administrador"
      });
      this.CmBxTxBuscarPorTipoDeCuenta.Location = new Point(100, 4);
      this.CmBxTxBuscarPorTipoDeCuenta.Name = "CmBxTxBuscarPorTipoDeCuenta";
      this.CmBxTxBuscarPorTipoDeCuenta.Size = new Size(186, 24);
      this.CmBxTxBuscarPorTipoDeCuenta.TabIndex = 2;
      this.CodLabelParaProveedores.AutoSize = true;
      this.CodLabelParaProveedores.Location = new Point(12, 7);
      this.CodLabelParaProveedores.Margin = new Padding(4, 0, 4, 0);
      this.CodLabelParaProveedores.Name = "CodLabelParaProveedores";
      this.CodLabelParaProveedores.Size = new Size(43, 17);
      this.CodLabelParaProveedores.TabIndex = 5;
      this.CodLabelParaProveedores.Text = "Filtro:";
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
      this.ListaBuscarPor.Items.AddRange(new object[3]
      {
        (object) "Nombre De Usuario",
        (object) "Nombre",
        (object) "Tipo De Cuenta"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.ListaUsuarios.BackgroundColor = SystemColors.ScrollBar;
      this.ListaUsuarios.BorderStyle = BorderStyle.None;
      this.ListaUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = Color.Gray;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 10f);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.ListaUsuarios.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.ListaUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaUsuarios.Columns.AddRange((DataGridViewColumn) this.ColNoFactura, (DataGridViewColumn) this.ColMonto, (DataGridViewColumn) this.ColDescripcion, (DataGridViewColumn) this.ColTipoDeCuenta);
      this.ListaUsuarios.Dock = DockStyle.Fill;
      this.ListaUsuarios.GridColor = Color.Gray;
      this.ListaUsuarios.Location = new Point(300, 50);
      this.ListaUsuarios.Margin = new Padding(4);
      this.ListaUsuarios.Name = "ListaUsuarios";
      this.ListaUsuarios.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaUsuarios.Size = new Size(745, 582);
      this.ListaUsuarios.TabIndex = 9;
      gridViewCellStyle2.NullValue = (object) null;
      this.ColNoFactura.DefaultCellStyle = gridViewCellStyle2;
      this.ColNoFactura.HeaderText = "Nombre De Usuario";
      this.ColNoFactura.Name = "ColNoFactura";
      this.ColNoFactura.Width = 200;
      this.ColMonto.HeaderText = "Nombre";
      this.ColMonto.Name = "ColMonto";
      this.ColMonto.Width = 300;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.ColDescripcion.DefaultCellStyle = gridViewCellStyle3;
      this.ColDescripcion.HeaderText = "Clave";
      this.ColDescripcion.Name = "ColDescripcion";
      this.ColDescripcion.ReadOnly = true;
      this.ColDescripcion.Resizable = DataGridViewTriState.True;
      this.ColDescripcion.Width = 150;
      gridViewCellStyle4.NullValue = (object) "Empleado";
      this.ColTipoDeCuenta.DefaultCellStyle = gridViewCellStyle4;
      this.ColTipoDeCuenta.FlatStyle = FlatStyle.Flat;
      this.ColTipoDeCuenta.HeaderText = "Tipo De Cuenta";
      this.ColTipoDeCuenta.Items.AddRange((object) "Empleado", (object) "Administrador");
      this.ColTipoDeCuenta.Name = "ColTipoDeCuenta";
      this.ColTipoDeCuenta.SortMode = DataGridViewColumnSortMode.Automatic;
      this.ColTipoDeCuenta.Width = 150;
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BTN_Copiar);
      this.PanelSuperior.Controls.Add((Control) this.BTN_ImportarDesdeExcel);
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAProveedores);
      this.PanelSuperior.Controls.Add((Control) this.BtnGuardar);
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 13;
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 632);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 15;
      this.StatusStrip.Paint += new PaintEventHandler(this.StatusStrip_Paint);
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
      this.BTN_Copiar.BackColor = Color.Transparent;
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
      this.BTN_ImportarDesdeExcel.BackColor = Color.Transparent;
      this.BTN_ImportarDesdeExcel.BackgroundImage = (Image) Resources.sobresalir;
      this.BTN_ImportarDesdeExcel.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_ImportarDesdeExcel.FlatAppearance.BorderColor = Color.Brown;
      this.BTN_ImportarDesdeExcel.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BTN_ImportarDesdeExcel.FlatStyle = FlatStyle.Flat;
      this.BTN_ImportarDesdeExcel.Location = new Point(99, 3);
      this.BTN_ImportarDesdeExcel.Name = "BTN_ImportarDesdeExcel";
      this.BTN_ImportarDesdeExcel.Size = new Size(42, 42);
      this.BTN_ImportarDesdeExcel.TabIndex = 3;
      this.BTN_ImportarDesdeExcel.UseVisualStyleBackColor = false;
      this.BTN_ImportarDesdeExcel.Click += new EventHandler(this.BTN_ImportarDesdeExcel_Click);
      this.BtnVolverAProveedores.BackColor = Color.Transparent;
      this.BtnVolverAProveedores.BackgroundImage = (Image) Resources.actualizar_pagina_opcion;
      this.BtnVolverAProveedores.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnVolverAProveedores.FlatAppearance.BorderColor = Color.Brown;
      this.BtnVolverAProveedores.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnVolverAProveedores.FlatStyle = FlatStyle.Flat;
      this.BtnVolverAProveedores.Location = new Point(147, 3);
      this.BtnVolverAProveedores.Name = "BtnVolverAProveedores";
      this.BtnVolverAProveedores.Size = new Size(42, 42);
      this.BtnVolverAProveedores.TabIndex = 1;
      this.BtnVolverAProveedores.UseVisualStyleBackColor = false;
      this.BtnGuardar.BackColor = Color.Transparent;
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
      this.ClientSize = new Size(1045, 654);
      this.Controls.Add((Control) this.ListaUsuarios);
      this.Controls.Add((Control) this.PanelFondoIzquierdo);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Usuarios);
      this.Text = nameof (Usuarios);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Usuarios_Load);
      this.PanelFondoIzquierdo.ResumeLayout(false);
      this.PanelFondoIzquierdo.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.Panel4BusquedaNormal.ResumeLayout(false);
      this.Panel3BusquedaNormal.ResumeLayout(false);
      this.Panel3BusquedaNormal.PerformLayout();
      this.Panel2BusquedaNormal.ResumeLayout(false);
      this.Panel2BusquedaNormal.PerformLayout();
      this.Panel1BusquedaNormal.ResumeLayout(false);
      this.Panel1BusquedaNormal.PerformLayout();
      ((ISupportInitialize) this.ListaUsuarios).EndInit();
      this.PanelSuperior.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
