// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Clientes
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
  public class Clientes : Form
  {
    private string ConsultaActual = "SELECT * FROM Cliente";
    private int ColumnaDeSortingActual;
    private SortOrder OrdenDeSortingActual;
    public bool ReadOnly;
    public FormularioDeInicio ParentForm;
    public OleDbConnection Conn;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel panel2;
    private Panel PanelFondoBusquedaNormal;
    private Button BtnBuscarPor;
    private Label CodLabelParaTexto;
    private TextBox TxBuscarPor;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private DataGridView ListaClientes;
    private ComboBox ListaBuscarPor;
    private Label label1;
    private Panel Panel2BusquedaNormal;
    private Panel Panel1BusquedaNormal;
    private Panel Panel7BusquedaNormal;
    private DataGridViewTextBoxColumn ColIDDeCliente;
    private DataGridViewTextBoxColumn ColRTNDeCliente;
    private DataGridViewTextBoxColumn ColNombreDeCliente;
    private Panel PanelSuperior;
    private Button BTN_Copiar;
    private Button BtnImportarDesdeExcel;
    private Button BtnVolverAClientes;
    private Button BtnGuardar;
    private Panel panel1;
    private Panel StatusStrip;

    public Clientes()
    {
      this.InitializeComponent();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaClientes, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.ListaClientes.CellValueChanged += new DataGridViewCellEventHandler(this.ListaClientes_CellValueChanged);
      this.ListaClientes.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaClientes_UserDeletingRow);
      this.ListaClientes.UserAddedRow += new DataGridViewRowEventHandler(this.ListaClientes_UserAddedRow);
      this.ListaClientes.CellValidating += new DataGridViewCellValidatingEventHandler(this.ListaClientes_CellValidating);
      this.FormClosing += new FormClosingEventHandler(this.Clientes_FormClosing);
    }

    private void EjecutarConsulta(string Consulta, SortOrder Orden, int IndexColumnaDeOrden)
    {
      this.ListaClientes.Rows.Clear();
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      oleDbCommand.CommandText += Consulta;
      if (Orden != SortOrder.None && IndexColumnaDeOrden != 2)
      {
        oleDbCommand.CommandText += " ORDER BY Clientes.";
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
        dataGridViewRow.CreateCells(this.ListaClientes);
        dataGridViewRow.SetValues(oleDbDataReader.GetValue(0), oleDbDataReader.GetValue(1), oleDbDataReader.GetValue(2));
        this.ListaClientes.Rows.Add(dataGridViewRow);
        dataGridViewRow.Cells[0].Tag = dataGridViewRow.Cells[0].Value;
      }
      for (int index = 0; index < this.ListaClientes.Rows.Count - 1; ++index)
        this.ListaClientes.Rows[index].Tag = (object) "NotAdded";
      oleDbDataReader.Close();
      this.ConsultaActual = Consulta;
      if (this.ReadOnly)
        return;
      for (int index = 0; index < this.ListaClientes.Rows[this.ListaClientes.Rows.Count - 1].Cells.Count; ++index)
        this.ListaClientes.Rows[this.ListaClientes.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    private void ListaClientes_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      for (int index = 0; index < e.Row.Cells.Count; ++index)
        e.Row.Cells[index].Style.BackColor = Color.LightGray;
    }

    private void ListaClientes_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (MessageBox.Show("La celda sera eliminada permanentemente, ¿Esta seguro de eliminar la celda?", "¿Esta seguro?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        if (e.Row.Tag != (object) "NotAdded")
          return;
        if (MessageBox.Show("Si elimina este registro de la tabla clientes se eliminara tambien de la tabla de ventas, ¿Esta seguro de que desea eliminarlo?", "¿Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = this.Conn;
          oleDbCommand.Transaction = this.Conn.BeginTransaction();
          try
          {
            oleDbCommand.CommandText = "UPDATE Ventas SET IDDeCliente = Null WHERE IDDeCliente = '" + e.Row.Cells[0].Tag.ToString() + "';";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "DELETE FROM Clientes WHERE IDDeCliente = '" + e.Row.Cells[0].Tag.ToString() + "';";
            oleDbCommand.ExecuteNonQuery();
            for (int index = 0; index < this.ParentForm.POS.Carritos.Count; ++index)
            {
              if (this.ParentForm.POS.Carritos[index].IDCliente == e.Row.Cells[0].Tag.ToString())
              {
                this.ParentForm.POS.Carritos[index].IDCliente = "";
                break;
              }
            }
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

    private bool VerificarSiExiste(string IDDeCliente)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Clientes WHERE IDDeCliente = '" + IDDeCliente + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    private void ListaClientes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ListaClientes.Rows[e.RowIndex].Tag != (object) "NotAdded")
        return;
      OleDbCommand oleDbCommand1 = new OleDbCommand();
      oleDbCommand1.Connection = this.Conn;
      oleDbCommand1.CommandText += "UPDATE Clientes SET ";
      bool flag = true;
      switch (e.ColumnIndex)
      {
        case 0:
          if (this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
          {
            if (!this.VerificarSiExiste(this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
            {
              OleDbCommand oleDbCommand2 = oleDbCommand1;
              oleDbCommand2.CommandText = oleDbCommand2.CommandText + "IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag + "';";
              string str1 = "";
              if (this.ListaClientes.Rows[e.RowIndex].Cells[1].Value != null)
                str1 = this.ListaClientes.Rows[e.RowIndex].Cells[1].Value.ToString();
              string str2 = "";
              if (this.ListaClientes.Rows[e.RowIndex].Cells[2].Value != null)
                str2 = this.ListaClientes.Rows[e.RowIndex].Cells[2].Value.ToString();
              string str3 = "INSERT INTO Clientes Values ('" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Value.ToString() + "', '" + str1 + "', '" + str2 + "');";
              string str4 = "UPDATE Ventas SET IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Value.ToString() + "' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
              string str5 = "DELETE FROM Clientes WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag.ToString() + "';";
              oleDbCommand1.Transaction = this.Conn.BeginTransaction();
              try
              {
                oleDbCommand1.CommandText = str3;
                oleDbCommand1.ExecuteNonQuery();
                oleDbCommand1.CommandText = str4;
                oleDbCommand1.ExecuteNonQuery();
                oleDbCommand1.CommandText = str5;
                oleDbCommand1.ExecuteNonQuery();
                this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag = (object) this.ListaClientes.Rows[e.RowIndex].Cells[0].Value.ToString();
                oleDbCommand1.Transaction.Commit();
              }
              catch (Exception ex)
              {
                oleDbCommand1.Transaction.Rollback();
                int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              flag = false;
              break;
            }
            int num1 = (int) MessageBox.Show("Ya existe un cliente con este Id en el registro.", "Cliente existente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            flag = false;
            break;
          }
          int num2 = (int) MessageBox.Show("No es posible asignar al ID de cliente un valor vacío.", "ID vacío", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = false;
          break;
        case 1:
          if (this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
          {
            OleDbCommand oleDbCommand3 = oleDbCommand1;
            oleDbCommand3.CommandText = oleDbCommand3.CommandText + "RTNDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag + "';";
            break;
          }
          OleDbCommand oleDbCommand4 = oleDbCommand1;
          oleDbCommand4.CommandText = oleDbCommand4.CommandText + "RTNDeCliente = '' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag + "';";
          break;
        case 2:
          if (this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
          {
            OleDbCommand oleDbCommand5 = oleDbCommand1;
            oleDbCommand5.CommandText = oleDbCommand5.CommandText + "NombreDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag + "';";
            break;
          }
          OleDbCommand oleDbCommand6 = oleDbCommand1;
          oleDbCommand6.CommandText = oleDbCommand6.CommandText + "NombreDeCliente = '' WHERE IDDeCliente = '" + this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag + "';";
          break;
      }
      if (!flag)
        return;
      oleDbCommand1.ExecuteNonQuery();
      for (int index = 0; index < this.ParentForm.POS.Carritos.Count; ++index)
      {
        if (this.ParentForm.POS.Carritos[index].IDCliente == this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag.ToString())
        {
          this.ParentForm.POS.Carritos[index].IDCliente = this.ListaClientes.Rows[e.RowIndex].Cells[0].Value.ToString();
          break;
        }
      }
      this.ListaClientes.Rows[e.RowIndex].Cells[0].Tag = this.ListaClientes.Rows[e.RowIndex].Cells[0].Value;
    }

    public void ActualizarClientes() => this.EjecutarConsulta("SELECT * FROM Clientes", this.OrdenDeSortingActual, this.ColumnaDeSortingActual);

    private void Clientes_Load(object sender, EventArgs e)
    {
      if (this.ReadOnly)
      {
        this.ListaClientes.ReadOnly = true;
        this.ListaClientes.AllowDrop = false;
        this.ListaClientes.AllowUserToDeleteRows = false;
        this.ListaClientes.AllowUserToAddRows = false;
        this.BtnImportarDesdeExcel.Enabled = false;
      }
      this.ActualizarClientes();
      this.ListaBuscarPor.SelectedIndex = 0;
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
                  oleDbCommand1.CommandText += "cDate('01/01/1753')";
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

    private bool Guardar() => this.Guardar(this.ListaClientes, new List<int>()
    {
      0,
      1,
      2
    }, new List<string>()
    {
      "IDDeCliente",
      "RTNDeCliente",
      "NombreDeCliente"
    }, new List<string>() { "String", "String", "String" }, "NotAdded", true, 0, nameof (Clientes), true, (List<int>) null);

    private bool VerificarElementosSinGuardar()
    {
      bool flag = false;
      for (int index = 0; index < this.ListaClientes.Rows.Count - 1; ++index)
      {
        if (this.ListaClientes.Rows[index].Tag != (object) "NotAdded")
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

    private void BtnGuardar_Click(object sender, EventArgs e) => this.Guardar();

    private void BuscarPor()
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      string Consulta = "";
      switch (this.ListaBuscarPor.SelectedIndex)
      {
        case 0:
          Consulta = "SELECT * FROM Clientes WHERE IDDeCliente LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 1:
          Consulta = "SELECT * FROM Clientes WHERE RTNDeCliente LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
        case 2:
          Consulta = "SELECT * FROM Clientes WHERE NombreDeCliente LIKE '%" + this.TxBuscarPor.Text + "%'";
          break;
      }
      this.EjecutarConsulta(Consulta, this.OrdenDeSortingActual, this.ColumnaDeSortingActual);
    }

    private void BtnBuscarPor_Click(object sender, EventArgs e) => this.BuscarPor();

    private void BtnVolverAClientes_Click(object sender, EventArgs e)
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaClientes.Rows.Clear();
      this.ActualizarClientes();
    }

    private void TxBuscarPor_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPor.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPor.Text = this.TxBuscarPor.Text.Replace(Environment.NewLine, "");
      this.BuscarPor();
    }

    private void Clientes_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.VerificarElementosSinGuardar())
        return;
      e.Cancel = true;
    }

    private void ListaClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void ListaBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void ListaClientes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
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

    private void BtnImportarDesdeExcel_Click(object sender, EventArgs e)
    {
      ImportacionDeDatosDesdeExcel deDatosDesdeExcel = new ImportacionDeDatosDesdeExcel();
      deDatosDesdeExcel.AddColumnString("ID De Cliente");
      deDatosDesdeExcel.AddColumnString("RTN De Cliente");
      deDatosDesdeExcel.AddColumnString("Nombre De Cliente");
      while (deDatosDesdeExcel.ShowDialog() == DialogResult.OK)
      {
        if (!this.Guardar(deDatosDesdeExcel.TablaDeImporte, new List<int>()
        {
          0,
          1,
          2
        }, new List<string>()
        {
          "IDDeCliente",
          "RTNDeCliente",
          "NombreDeCliente"
        }, new List<string>()
        {
          "String",
          "String",
          "String"
        }, "NotAdded", true, 0, nameof (Clientes), false, (List<int>) null))
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
      if (this.ListaClientes.SelectedCells.Count <= 0)
        return;
      int num1 = this.ListaClientes.Rows.Count;
      int num2 = this.ListaClientes.Columns.Count;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < this.ListaClientes.SelectedCells.Count; ++index)
      {
        if (this.ListaClientes.SelectedCells[index].ColumnIndex < num2)
          num2 = this.ListaClientes.SelectedCells[index].ColumnIndex;
        if (this.ListaClientes.SelectedCells[index].RowIndex < num1)
          num1 = this.ListaClientes.SelectedCells[index].RowIndex;
        if (this.ListaClientes.SelectedCells[index].ColumnIndex > num4)
          num4 = this.ListaClientes.SelectedCells[index].ColumnIndex;
        if (this.ListaClientes.SelectedCells[index].RowIndex > num3)
          num3 = this.ListaClientes.SelectedCells[index].RowIndex;
      }
      string text = "";
      for (int rowIndex = num1; rowIndex <= num3; ++rowIndex)
      {
        for (int columnIndex = num2; columnIndex <= num4; ++columnIndex)
        {
          if (this.ListaClientes[columnIndex, rowIndex].Selected)
          {
            if (this.ListaClientes[columnIndex, rowIndex].Value != null)
              text += this.ListaClientes[columnIndex, rowIndex].Value.ToString();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Clientes));
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.PanelFondoBusquedaNormal = new Panel();
      this.Panel7BusquedaNormal = new Panel();
      this.BtnBuscarPor = new Button();
      this.Panel2BusquedaNormal = new Panel();
      this.CodLabelParaTexto = new Label();
      this.TxBuscarPor = new TextBox();
      this.Panel1BusquedaNormal = new Panel();
      this.label1 = new Label();
      this.ListaBuscarPor = new ComboBox();
      this.ListaClientes = new DataGridView();
      this.ColIDDeCliente = new DataGridViewTextBoxColumn();
      this.ColRTNDeCliente = new DataGridViewTextBoxColumn();
      this.ColNombreDeCliente = new DataGridViewTextBoxColumn();
      this.PanelSuperior = new Panel();
      this.StatusStrip = new Panel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.BTN_Copiar = new Button();
      this.BtnImportarDesdeExcel = new Button();
      this.BtnVolverAClientes = new Button();
      this.BtnGuardar = new Button();
      this.panel2.SuspendLayout();
      this.PanelFondoBusquedaNormal.SuspendLayout();
      this.Panel7BusquedaNormal.SuspendLayout();
      this.Panel2BusquedaNormal.SuspendLayout();
      this.Panel1BusquedaNormal.SuspendLayout();
      ((ISupportInitialize) this.ListaClientes).BeginInit();
      this.PanelSuperior.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.DimGray;
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.PanelFondoBusquedaNormal);
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 50);
      this.panel2.Margin = new Padding(4);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(0, 0, 1, 0);
      this.panel2.Size = new Size(300, 582);
      this.panel2.TabIndex = 6;
      this.panel1.BackColor = SystemColors.ScrollBar;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 138);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 444);
      this.panel1.TabIndex = 14;
      this.PanelFondoBusquedaNormal.AutoSize = true;
      this.PanelFondoBusquedaNormal.BackColor = Color.DimGray;
      this.PanelFondoBusquedaNormal.Controls.Add((Control) this.Panel7BusquedaNormal);
      this.PanelFondoBusquedaNormal.Controls.Add((Control) this.Panel2BusquedaNormal);
      this.PanelFondoBusquedaNormal.Controls.Add((Control) this.Panel1BusquedaNormal);
      this.PanelFondoBusquedaNormal.Dock = DockStyle.Top;
      this.PanelFondoBusquedaNormal.Location = new Point(0, 25);
      this.PanelFondoBusquedaNormal.Margin = new Padding(4);
      this.PanelFondoBusquedaNormal.Name = "PanelFondoBusquedaNormal";
      this.PanelFondoBusquedaNormal.Padding = new Padding(0, 1, 0, 1);
      this.PanelFondoBusquedaNormal.Size = new Size(299, 113);
      this.PanelFondoBusquedaNormal.TabIndex = 8;
      this.Panel7BusquedaNormal.BackColor = SystemColors.Control;
      this.Panel7BusquedaNormal.Controls.Add((Control) this.BtnBuscarPor);
      this.Panel7BusquedaNormal.Dock = DockStyle.Top;
      this.Panel7BusquedaNormal.Location = new Point(0, 69);
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
      this.Panel1BusquedaNormal.Controls.Add((Control) this.label1);
      this.Panel1BusquedaNormal.Controls.Add((Control) this.ListaBuscarPor);
      this.Panel1BusquedaNormal.Dock = DockStyle.Top;
      this.Panel1BusquedaNormal.Location = new Point(0, 1);
      this.Panel1BusquedaNormal.Name = "Panel1BusquedaNormal";
      this.Panel1BusquedaNormal.Size = new Size(299, 36);
      this.Panel1BusquedaNormal.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 13);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 17);
      this.label1.TabIndex = 6;
      this.label1.Text = "Buscar por:";
      this.ListaBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ListaBuscarPor.FormattingEnabled = true;
      this.ListaBuscarPor.Items.AddRange(new object[3]
      {
        (object) "ID De Cliente",
        (object) "RTN De Cliente",
        (object) "Nombre De Cliente"
      });
      this.ListaBuscarPor.Location = new Point(100, 10);
      this.ListaBuscarPor.Name = "ListaBuscarPor";
      this.ListaBuscarPor.Size = new Size(186, 24);
      this.ListaBuscarPor.TabIndex = 7;
      this.ListaBuscarPor.SelectedIndexChanged += new EventHandler(this.ListaBuscarPor_SelectedIndexChanged);
      this.ListaClientes.BackgroundColor = SystemColors.ScrollBar;
      this.ListaClientes.BorderStyle = BorderStyle.None;
      this.ListaClientes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaClientes.Columns.AddRange((DataGridViewColumn) this.ColIDDeCliente, (DataGridViewColumn) this.ColRTNDeCliente, (DataGridViewColumn) this.ColNombreDeCliente);
      this.ListaClientes.Dock = DockStyle.Fill;
      this.ListaClientes.GridColor = Color.Gray;
      this.ListaClientes.Location = new Point(300, 50);
      this.ListaClientes.Margin = new Padding(4);
      this.ListaClientes.Name = "ListaClientes";
      this.ListaClientes.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaClientes.Size = new Size(745, 582);
      this.ListaClientes.TabIndex = 9;
      this.ListaClientes.CellContentClick += new DataGridViewCellEventHandler(this.ListaClientes_CellContentClick);
      this.ColIDDeCliente.HeaderText = "ID De Cliente";
      this.ColIDDeCliente.MaxInputLength = 13;
      this.ColIDDeCliente.Name = "ColIDDeCliente";
      this.ColIDDeCliente.Width = 150;
      this.ColRTNDeCliente.HeaderText = "RTN De Cliente";
      this.ColRTNDeCliente.MaxInputLength = 14;
      this.ColRTNDeCliente.Name = "ColRTNDeCliente";
      this.ColRTNDeCliente.Width = 150;
      this.ColNombreDeCliente.HeaderText = "Nombre De Cliente";
      this.ColNombreDeCliente.MaxInputLength = 200;
      this.ColNombreDeCliente.Name = "ColNombreDeCliente";
      this.ColNombreDeCliente.Width = 300;
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BTN_Copiar);
      this.PanelSuperior.Controls.Add((Control) this.BtnImportarDesdeExcel);
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAClientes);
      this.PanelSuperior.Controls.Add((Control) this.BtnGuardar);
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
      this.panel6.BackColor = Color.Brown;
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
      this.BtnVolverAClientes.Click += new EventHandler(this.BtnVolverAClientes_Click);
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
      this.ClientSize = new Size(1045, 654);
      this.Controls.Add((Control) this.ListaClientes);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Clientes);
      this.Text = nameof (Clientes);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Clientes_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.PanelFondoBusquedaNormal.ResumeLayout(false);
      this.Panel7BusquedaNormal.ResumeLayout(false);
      this.Panel2BusquedaNormal.ResumeLayout(false);
      this.Panel2BusquedaNormal.PerformLayout();
      this.Panel1BusquedaNormal.ResumeLayout(false);
      this.Panel1BusquedaNormal.PerformLayout();
      ((ISupportInitialize) this.ListaClientes).EndInit();
      this.PanelSuperior.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
