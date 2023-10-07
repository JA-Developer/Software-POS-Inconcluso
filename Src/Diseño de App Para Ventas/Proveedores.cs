// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Proveedores
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
  public class Proveedores : Form
  {
    public OleDbConnection Conn;
    private Point Origen = new Point(0, 0);
    private IContainer components;
    private Panel panel2;
    private Panel PanelBusquedaAvanzada;
    private Button BtnBuscarPorNombre;
    private Label CodLabel;
    private TextBox TxBuscarPorNombre;
    private Panel panel6;
    private LinkLabel linkLabel2;
    private DataGridView ListaProveedores;
    private DataGridViewTextBoxColumn ColCodigo;
    private Panel PanelSuperior;
    private Button BTN_Copiar;
    private Button BtnImportarDesdeExcel;
    private Button BtnVolverAProveedores;
    private Button BtnGuardar;
    private Panel panel1;
    private Panel StatusStrip;

    public Proveedores()
    {
      this.InitializeComponent();
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.ListaProveedores, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.ListaProveedores.CellValueChanged += new DataGridViewCellEventHandler(this.ListaProveedores_CellValueChanged);
      this.ListaProveedores.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.ListaProveedores_UserDeletingRow);
      this.ListaProveedores.UserAddedRow += new DataGridViewRowEventHandler(this.ListaProveedores_UserAddedRow);
      this.FormClosing += new FormClosingEventHandler(this.Proveedores_FormClosing);
      this.ListaProveedores.CellValidating += new DataGridViewCellValidatingEventHandler(this.ListaProveedores_CellValidating);
    }

    private void ListaProveedores_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      for (int index = 0; index < this.ListaProveedores.Rows[this.ListaProveedores.Rows.Count - 1].Cells.Count; ++index)
        this.ListaProveedores.Rows[this.ListaProveedores.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    private void Proveedores_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.VerificarElementosSinGuardar())
        return;
      e.Cancel = true;
    }

    private void ListaProveedores_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (MessageBox.Show("La celda será eliminada permanentemente y se borrará su registro de todas las otras tablas, ¿Está seguro de eliminar la celda?", "¿Está seguro?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        if (e.Row.Tag != (object) "NotAdded")
          return;
        OleDbCommand oleDbCommand = new OleDbCommand();
        oleDbCommand.Connection = this.Conn;
        oleDbCommand.Transaction = this.Conn.BeginTransaction();
        try
        {
          oleDbCommand.CommandText = "UPDATE Inventario SET Proveedor = Null WHERE Proveedor = '" + e.Row.Cells[0].Value.ToString() + "';";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "DELETE FROM Proveedores WHERE Nombre = '" + e.Row.Cells[0].Tag + "';";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.Transaction.Commit();
        }
        catch (Exception ex)
        {
          oleDbCommand.Transaction.Rollback();
          int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
        e.Cancel = true;
    }

    private bool VerificarSiExiste(string Nombre)
    {
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Proveedores WHERE Nombre = '" + Nombre + "';", this.Conn).ExecuteReader();
      if (oleDbDataReader.Read())
        return true;
      oleDbDataReader.Close();
      return false;
    }

    private void ListaProveedores_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ListaProveedores.Rows[e.RowIndex].Tag != (object) "NotAdded")
        return;
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = this.Conn;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      bool flag = true;
      if (e.ColumnIndex == 0)
      {
        if (this.ListaProveedores.Rows[e.RowIndex].Cells[0].Value != null)
        {
          if (this.ListaProveedores.Rows[e.RowIndex].Cells[0].Value != (object) "")
          {
            if (!this.VerificarSiExiste(this.ListaProveedores.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
            {
              str1 = str1 + "INSERT INTO Proveedores VALUES('" + this.ListaProveedores.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "');";
              str2 = "UPDATE Inventario SET Proveedor = '" + this.ListaProveedores.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "' WHERE Proveedor = '" + this.ListaProveedores.Rows[e.RowIndex].Cells[0].Tag + "';";
              str3 = str3 + "DELETE FROM Proveedores WHERE Nombre = '" + this.ListaProveedores.Rows[e.RowIndex].Cells[0].Tag + "';";
            }
            else
            {
              int num = (int) MessageBox.Show("Ya existe un proveedor registrado con este nombre. Porfavor, ingrese uno diferente.", "Nombre duplicado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              flag = false;
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Debe asignar un nombre al proveedor antes de registrarlo.", "Proveedor sin nombre", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            flag = false;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Debe asignar un nombre al proveedor antes de registrarlo.", "Proveedor sin nombre", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = false;
        }
      }
      if (!flag)
        return;
      oleDbCommand.Transaction = this.Conn.BeginTransaction();
      try
      {
        oleDbCommand.CommandText = str1;
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = str2;
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = str3;
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.Transaction.Commit();
      }
      catch (Exception ex)
      {
        oleDbCommand.Transaction.Rollback();
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void ActualizarProveedores()
    {
      this.ListaProveedores.Rows.Clear();
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Proveedores;", this.Conn).ExecuteReader();
      while (oleDbDataReader.Read())
        this.ListaProveedores.Rows.Add(new object[1]
        {
          oleDbDataReader.GetValue(0)
        });
      for (int index = 0; index < this.ListaProveedores.Rows.Count - 1; ++index)
      {
        this.ListaProveedores.Rows[index].Tag = (object) "NotAdded";
        this.ListaProveedores.Rows[index].Cells[0].Tag = this.ListaProveedores.Rows[index].Cells[0].Value;
      }
      oleDbDataReader.Close();
      for (int index = 0; index < this.ListaProveedores.Rows[this.ListaProveedores.Rows.Count - 1].Cells.Count; ++index)
        this.ListaProveedores.Rows[this.ListaProveedores.Rows.Count - 1].Cells[index].Style.BackColor = Color.LightGray;
    }

    private void Proveedores_Load(object sender, EventArgs e) => this.ActualizarProveedores();

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

    private bool Guardar() => this.Guardar(this.ListaProveedores, new List<int>()
    {
      0
    }, new List<string>() { "Nombre" }, new List<string>()
    {
      "String"
    }, "NotAdded", true, 0, nameof (Proveedores), true, (List<int>) null, "");

    private bool VerificarElementosSinGuardar()
    {
      bool flag = false;
      for (int index = 0; index < this.ListaProveedores.Rows.Count - 1; ++index)
      {
        if (this.ListaProveedores.Rows[index].Tag != (object) "NotAdded")
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

    private void BuscarPorNombre()
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaProveedores.Rows.Clear();
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM Proveedores WHERE Nombre LIKE '%" + this.TxBuscarPorNombre.Text + "%';", this.Conn).ExecuteReader();
      while (oleDbDataReader.Read())
        this.ListaProveedores.Rows.Add(new object[1]
        {
          oleDbDataReader.GetValue(0)
        });
      for (int index = 0; index < this.ListaProveedores.Rows.Count - 1; ++index)
      {
        this.ListaProveedores.Rows[index].Tag = (object) "NotAdded";
        this.ListaProveedores.Rows[index].Cells[0].Tag = this.ListaProveedores.Rows[index].Cells[0].Value;
      }
      oleDbDataReader.Close();
    }

    private void BtnVolverAProveedores_Click(object sender, EventArgs e)
    {
      if (!this.VerificarElementosSinGuardar())
        return;
      this.ListaProveedores.Rows.Clear();
      this.ActualizarProveedores();
    }

    private void TxBuscarPorNombre_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TxBuscarPorNombre.Lines).Count<string>() <= 1)
        return;
      this.TxBuscarPorNombre.Text = this.TxBuscarPorNombre.Text.Replace(Environment.NewLine, "");
      this.BuscarPorNombre();
    }

    private void BtnBuscarPorNombre_Click(object sender, EventArgs e) => this.BuscarPorNombre();

    private void ListaProveedores_CellValidating(
      object sender,
      DataGridViewCellValidatingEventArgs e)
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
      deDatosDesdeExcel.AddColumnString("Nombre");
      while (deDatosDesdeExcel.ShowDialog() == DialogResult.OK)
      {
        if (!this.Guardar(deDatosDesdeExcel.TablaDeImporte, new List<int>()
        {
          0
        }, new List<string>() { "Nombre" }, new List<string>()
        {
          "String"
        }, "NotAdded", true, 0, nameof (Proveedores), false, (List<int>) null, ""))
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
      this.ActualizarProveedores();
    }

    private void BTN_Copiar_Click(object sender, EventArgs e)
    {
      if (this.ListaProveedores.SelectedCells.Count <= 0)
        return;
      int num1 = this.ListaProveedores.Rows.Count;
      int num2 = this.ListaProveedores.Columns.Count;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < this.ListaProveedores.SelectedCells.Count; ++index)
      {
        if (this.ListaProveedores.SelectedCells[index].ColumnIndex < num2)
          num2 = this.ListaProveedores.SelectedCells[index].ColumnIndex;
        if (this.ListaProveedores.SelectedCells[index].RowIndex < num1)
          num1 = this.ListaProveedores.SelectedCells[index].RowIndex;
        if (this.ListaProveedores.SelectedCells[index].ColumnIndex > num4)
          num4 = this.ListaProveedores.SelectedCells[index].ColumnIndex;
        if (this.ListaProveedores.SelectedCells[index].RowIndex > num3)
          num3 = this.ListaProveedores.SelectedCells[index].RowIndex;
      }
      string text = "";
      for (int rowIndex = num1; rowIndex <= num3; ++rowIndex)
      {
        for (int columnIndex = num2; columnIndex <= num4; ++columnIndex)
        {
          if (this.ListaProveedores[columnIndex, rowIndex].Selected)
          {
            if (this.ListaProveedores[columnIndex, rowIndex].Value != null)
              text += this.ListaProveedores[columnIndex, rowIndex].Value.ToString();
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

    private void PanelBusquedaAvanzada_Paint(object sender, PaintEventArgs e)
    {
      this.PanelBusquedaAvanzada.CreateGraphics().DrawLine(Pens.DimGray, this.Origen, new Point(this.PanelBusquedaAvanzada.Width, 0));
      this.PanelBusquedaAvanzada.CreateGraphics().DrawLine(Pens.DimGray, new Point(0, this.PanelBusquedaAvanzada.Height - 1), new Point(this.PanelBusquedaAvanzada.Width, this.PanelBusquedaAvanzada.Height - 1));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Proveedores));
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.PanelBusquedaAvanzada = new Panel();
      this.BtnBuscarPorNombre = new Button();
      this.CodLabel = new Label();
      this.TxBuscarPorNombre = new TextBox();
      this.ListaProveedores = new DataGridView();
      this.ColCodigo = new DataGridViewTextBoxColumn();
      this.PanelSuperior = new Panel();
      this.StatusStrip = new Panel();
      this.panel6 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.BTN_Copiar = new Button();
      this.BtnImportarDesdeExcel = new Button();
      this.BtnVolverAProveedores = new Button();
      this.BtnGuardar = new Button();
      this.panel2.SuspendLayout();
      this.PanelBusquedaAvanzada.SuspendLayout();
      ((ISupportInitialize) this.ListaProveedores).BeginInit();
      this.PanelSuperior.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.DimGray;
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.PanelBusquedaAvanzada);
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
      this.panel1.Location = new Point(0, 110);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 472);
      this.panel1.TabIndex = 14;
      this.PanelBusquedaAvanzada.BackColor = SystemColors.Control;
      this.PanelBusquedaAvanzada.Controls.Add((Control) this.BtnBuscarPorNombre);
      this.PanelBusquedaAvanzada.Controls.Add((Control) this.CodLabel);
      this.PanelBusquedaAvanzada.Controls.Add((Control) this.TxBuscarPorNombre);
      this.PanelBusquedaAvanzada.Dock = DockStyle.Top;
      this.PanelBusquedaAvanzada.Location = new Point(0, 25);
      this.PanelBusquedaAvanzada.Margin = new Padding(4);
      this.PanelBusquedaAvanzada.Name = "PanelBusquedaAvanzada";
      this.PanelBusquedaAvanzada.Size = new Size(299, 85);
      this.PanelBusquedaAvanzada.TabIndex = 8;
      this.PanelBusquedaAvanzada.Paint += new PaintEventHandler(this.PanelBusquedaAvanzada_Paint);
      this.BtnBuscarPorNombre.BackColor = Color.DimGray;
      this.BtnBuscarPorNombre.FlatStyle = FlatStyle.Flat;
      this.BtnBuscarPorNombre.Location = new Point(10, 42);
      this.BtnBuscarPorNombre.Margin = new Padding(4);
      this.BtnBuscarPorNombre.Name = "BtnBuscarPorNombre";
      this.BtnBuscarPorNombre.Size = new Size(275, 28);
      this.BtnBuscarPorNombre.TabIndex = 5;
      this.BtnBuscarPorNombre.Text = "Buscar";
      this.BtnBuscarPorNombre.UseVisualStyleBackColor = false;
      this.BtnBuscarPorNombre.Click += new EventHandler(this.BtnBuscarPorNombre_Click);
      this.CodLabel.AutoSize = true;
      this.CodLabel.Location = new Point(11, 13);
      this.CodLabel.Margin = new Padding(4, 0, 4, 0);
      this.CodLabel.Name = "CodLabel";
      this.CodLabel.Size = new Size(62, 17);
      this.CodLabel.TabIndex = 5;
      this.CodLabel.Text = "Nombre:";
      this.TxBuscarPorNombre.BorderStyle = BorderStyle.FixedSingle;
      this.TxBuscarPorNombre.Location = new Point(81, 11);
      this.TxBuscarPorNombre.Margin = new Padding(4);
      this.TxBuscarPorNombre.Multiline = true;
      this.TxBuscarPorNombre.Name = "TxBuscarPorNombre";
      this.TxBuscarPorNombre.Size = new Size(204, 23);
      this.TxBuscarPorNombre.TabIndex = 5;
      this.TxBuscarPorNombre.TextChanged += new EventHandler(this.TxBuscarPorNombre_TextChanged);
      this.ListaProveedores.BackgroundColor = SystemColors.ScrollBar;
      this.ListaProveedores.BorderStyle = BorderStyle.None;
      this.ListaProveedores.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaProveedores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaProveedores.Columns.AddRange((DataGridViewColumn) this.ColCodigo);
      this.ListaProveedores.Dock = DockStyle.Fill;
      this.ListaProveedores.GridColor = Color.Gray;
      this.ListaProveedores.Location = new Point(300, 50);
      this.ListaProveedores.Margin = new Padding(4);
      this.ListaProveedores.Name = "ListaProveedores";
      this.ListaProveedores.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ListaProveedores.Size = new Size(745, 582);
      this.ListaProveedores.TabIndex = 9;
      this.ColCodigo.HeaderText = "Nombre";
      this.ColCodigo.MaxInputLength = (int) byte.MaxValue;
      this.ColCodigo.Name = "ColCodigo";
      this.ColCodigo.Width = 400;
      this.PanelSuperior.BackColor = Color.Brown;
      this.PanelSuperior.Controls.Add((Control) this.BTN_Copiar);
      this.PanelSuperior.Controls.Add((Control) this.BtnImportarDesdeExcel);
      this.PanelSuperior.Controls.Add((Control) this.BtnVolverAProveedores);
      this.PanelSuperior.Controls.Add((Control) this.BtnGuardar);
      this.PanelSuperior.Dock = DockStyle.Top;
      this.PanelSuperior.Location = new Point(0, 0);
      this.PanelSuperior.Margin = new Padding(4);
      this.PanelSuperior.Name = "PanelSuperior";
      this.PanelSuperior.Size = new Size(1045, 50);
      this.PanelSuperior.TabIndex = 12;
      this.StatusStrip.BackColor = Color.Brown;
      this.StatusStrip.Dock = DockStyle.Bottom;
      this.StatusStrip.Location = new Point(0, 632);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new Size(1045, 22);
      this.StatusStrip.TabIndex = 14;
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
      this.linkLabel2.Size = new Size(171, 20);
      this.linkLabel2.TabIndex = 6;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "Búsqueda por nombre:";
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
      this.BtnVolverAProveedores.BackColor = Color.Brown;
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
      this.BtnVolverAProveedores.Click += new EventHandler(this.BtnVolverAProveedores_Click);
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
      this.Controls.Add((Control) this.ListaProveedores);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.PanelSuperior);
      this.Controls.Add((Control) this.StatusStrip);
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Proveedores);
      this.Text = nameof (Proveedores);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.Proveedores_Load);
      this.panel2.ResumeLayout(false);
      this.PanelBusquedaAvanzada.ResumeLayout(false);
      this.PanelBusquedaAvanzada.PerformLayout();
      ((ISupportInitialize) this.ListaProveedores).EndInit();
      this.PanelSuperior.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
