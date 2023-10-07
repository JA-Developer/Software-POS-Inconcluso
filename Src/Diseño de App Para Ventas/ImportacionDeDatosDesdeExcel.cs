// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.ImportacionDeDatosDesdeExcel
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using ClosedXML.Excel;
using Diseño_de_App_Para_Ventas.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class ImportacionDeDatosDesdeExcel : Form
  {
    private DialogResult Resultado = DialogResult.Cancel;
    private int PruebaDeEnteros;
    private double NumeroDePrueba;
    public string FileName = "";
    public XLWorkbook Wb;
    private bool AlreadyLoaded;
    private List<string> ColumnsTypes = new List<string>();
    private List<string> ColumnsList = new List<string>();
    private List<List<string>> ListasPredefinidas = new List<List<string>>();
    private List<string> IDsListasPredefinidas = new List<string>();
    private IContainer components;
    private Panel panel1;
    private Button BtnAbrirArchivo;
    private Button BtnImportar;
    private SplitContainer splitContainer1;
    private Button BtnTransferir;
    public DataGridView TablaDeImporte;
    public DataGridView TablaHoja;
    private Panel panel2;
    private LinkLabel linkLabel1;
    private Panel panel3;
    private LinkLabel LinkLabelTablaAImportar;
    private Panel PanelBajoDeHoja;
    public NumericUpDown NumPage;
    private Label LabPg;
    private Panel panel4;

    public ImportacionDeDatosDesdeExcel()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
      this.InitializeComponent();
      typeof (DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.TablaHoja, new object[1]
      {
        (object) true
      });
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.TablaDeImporte.CellValidating += new DataGridViewCellValidatingEventHandler(this.TablaDeImporte_CellValidating);
      this.TablaDeImporte.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.TablaDeImporte_CellMouseDoubleClick);
      this.TablaDeImporte.CellValueChanged += new DataGridViewCellEventHandler(this.TablaDeImporte_CellValueChanged);
      this.TablaDeImporte.UserDeletedRow += new DataGridViewRowEventHandler(this.TablaDeImporte_UserDeletedRow);
      this.FormClosing += new FormClosingEventHandler(this.ImportacionDeDatosDesdeExcel_FormClosing);
    }

    private void TablaDeImporte_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {
      if (this.TablaHoja.Rows.Count != 0)
        return;
      this.TablaDeImporte.Rows.Add();
    }

    private void TablaDeImporte_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = SystemColors.Window;
        if (!(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() != "Intenger") || !(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() != "Double") || !(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() != "Date") || !(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() != "Time") || !(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() != "String"))
          return;
        string.Concat((object) ((DataGridViewComboBoxCell) this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex]).Items.IndexOf((object) this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
        this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = (object) ((DataGridViewComboBoxCell) this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex]).Items.IndexOf((object) this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private void TablaDeImporte_CellMouseDoubleClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      if (this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() == "Date")
      {
        SeleccionDeFecha seleccionDeFecha = new SeleccionDeFecha();
        try
        {
          System.DateTime exact = System.DateTime.Parse("01/01/1753");
          try
          {
            Clipboard.SetText(this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            exact = System.DateTime.ParseExact(this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch (Exception ex)
          {
          }
          seleccionDeFecha.ControlFecha.SelectionStart = exact;
          if (seleccionDeFecha.ShowDialog() != DialogResult.OK)
            return;
          this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) this.DateToString_ddMMyyyy(seleccionDeFecha.ControlFecha.SelectionStart);
          this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = SystemColors.Window;
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        if (!(this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString() == "Time"))
          return;
        SeleccionDeHora seleccionDeHora = new SeleccionDeHora();
        try
        {
          System.DateTime dateTime = System.DateTime.Now;
          if (this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
          {
            if (this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != (object) "")
            {
              try
              {
                dateTime = System.DateTime.ParseExact(this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "HH:mm:ss.fff", (IFormatProvider) CultureInfo.InvariantCulture);
                goto label_16;
              }
              catch (Exception ex)
              {
                goto label_16;
              }
            }
          }
          dateTime = System.DateTime.Today;
label_16:
          seleccionDeHora.ControlTiempo.Value = dateTime;
          if (seleccionDeHora.ShowDialog() != DialogResult.OK)
            return;
          this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) this.DateToString_hhmmssttt(seleccionDeHora.ControlTiempo.Value);
          this.TablaDeImporte.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = SystemColors.Window;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void TablaDeImporte_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      switch (this.TablaDeImporte.Columns[e.ColumnIndex].Tag.ToString())
      {
        case "Intenger":
          this.NumeroDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.NumeroDePrueba))
          {
            e.Cancel = true;
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          break;
        case "Double":
          this.NumeroDePrueba = 0.0;
          if (!double.TryParse(e.FormattedValue.ToString(), out this.NumeroDePrueba))
          {
            e.Cancel = true;
            int num = (int) MessageBox.Show("Esta celda solo admite caracteres numericos.", "Entrada invalida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    private XLDataType LoQueParece(string Formato)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      for (int index = 0; index < Formato.Count<char>(); ++index)
      {
        if (Formato[index] == '#')
        {
          ++num1;
          ++num2;
        }
        else if (Formato[index] == '0')
        {
          ++num1;
          ++num2;
        }
        else if (Formato[index] == 'd')
        {
          ++num1;
          ++num3;
        }
        else if (Formato[index] == 'y')
        {
          ++num1;
          ++num3;
        }
        else if (Formato[index] == 'h')
        {
          ++num1;
          ++num3;
          ++num4;
        }
        else if (Formato[index] == 'H')
        {
          ++num1;
          ++num3;
          ++num4;
        }
        else if (Formato[index] == 'm')
        {
          ++num1;
          ++num3;
          ++num4;
        }
        else if (Formato[index] == 's')
        {
          ++num1;
          ++num3;
          ++num4;
        }
        else if (Formato[index] == 't')
        {
          ++num1;
          ++num3;
          ++num4;
        }
      }
      double num5 = num2 / num1;
      double num6 = num3 / num1;
      double num7 = num4 / num1;
      return num6 >= num5 ? (num6 > num7 || num6 == num7 && !Formato.Contains("H") && !Formato.Contains("s") && !Formato.Contains("t") && !Formato.Contains("h") ? XLDataType.DateTime : XLDataType.TimeSpan) : (num6 < num5 ? XLDataType.Number : XLDataType.Text);
    }

    private object GetData(IXLCell Celda)
    {
      if (Celda.Style.NumberFormat.NumberFormatId == 0)
        return (object) Celda.GetFormattedString();
      if (Celda.Style.NumberFormat.NumberFormatId >= 1 && Celda.Style.NumberFormat.NumberFormatId <= 13)
        return (object) Celda.GetFormattedString();
      if (Celda.Style.NumberFormat.NumberFormatId >= 14 && Celda.Style.NumberFormat.NumberFormatId <= 17)
      {
        if (Celda.DataType == XLDataType.DateTime)
          return (object) (System.DateTime) Celda.Value;
        try
        {
          return (object) Celda.GetDateTime();
        }
        catch (Exception ex)
        {
          Celda.SetDataType(XLDataType.DateTime);
          return (object) (System.DateTime) Celda.Value;
        }
      }
      else
      {
        if (Celda.Style.NumberFormat.NumberFormatId >= 18 && Celda.Style.NumberFormat.NumberFormatId <= 21)
        {
          Celda.SetDataType(XLDataType.TimeSpan);
          return (object) Celda.GetTimeSpan();
        }
        if (Celda.Style.NumberFormat.NumberFormatId == 22)
        {
          if (Celda.DataType == XLDataType.DateTime)
            return (object) (System.DateTime) Celda.Value;
          try
          {
            return (object) Celda.GetDateTime();
          }
          catch (Exception ex)
          {
            Celda.SetDataType(XLDataType.DateTime);
            return (object) (System.DateTime) Celda.Value;
          }
        }
        else
        {
          if (Celda.Style.NumberFormat.NumberFormatId >= 37 && Celda.Style.NumberFormat.NumberFormatId <= 40)
            return (object) Celda.GetFormattedString();
          if (Celda.Style.NumberFormat.NumberFormatId >= 45 && Celda.Style.NumberFormat.NumberFormatId <= 47)
          {
            Celda.SetDataType(XLDataType.TimeSpan);
            return (object) Celda.GetTimeSpan();
          }
          if (Celda.Style.NumberFormat.NumberFormatId == 48)
            return (object) Celda.GetFormattedString();
          if (Celda.Style.NumberFormat.NumberFormatId == 49)
            return (object) Celda.GetFormattedString();
          if (Celda.DataType == XLDataType.DateTime)
          {
            if (Celda.Style.NumberFormat.Format == null || !Celda.Style.NumberFormat.Format.Contains("[h]:mm:ss") && !Celda.Style.NumberFormat.Format.Contains("h:mm:ss") && !Celda.Style.NumberFormat.Format.Contains("h:mm") && !Celda.Style.NumberFormat.Format.Contains("mm:ss") || Celda.Style.NumberFormat.Format.Contains("y") || Celda.Style.NumberFormat.Format.Contains("d") || Celda.Style.NumberFormat.Format.Replace(":mm", "").Replace("mm:", "").Contains("m"))
              return (object) (System.DateTime) Celda.Value;
            Celda.SetDataType(XLDataType.TimeSpan);
            return (object) (TimeSpan) Celda.Value;
          }
          if (Celda.DataType == XLDataType.TimeSpan)
            return (object) Celda.GetTimeSpan();
          if (Celda.DataType == XLDataType.Boolean)
            return (object) Celda.GetBoolean();
          if (Celda.Style.NumberFormat.Format != "")
          {
            if (this.LoQueParece(Celda.Style.NumberFormat.Format) == XLDataType.DateTime)
            {
              try
              {
                Celda.SetDataType(XLDataType.DateTime);
                return (object) (System.DateTime) Celda.Value;
              }
              catch (Exception ex)
              {
              }
            }
            else if (this.LoQueParece(Celda.Style.NumberFormat.Format) == XLDataType.TimeSpan)
            {
              try
              {
                Celda.SetDataType(XLDataType.TimeSpan);
                return (object) (TimeSpan) Celda.Value;
              }
              catch (Exception ex)
              {
                return (object) Celda.GetFormattedString();
              }
            }
            else
            {
              try
              {
                Celda.SetDataType(XLDataType.Number);
                return (object) (double) Celda.Value;
              }
              catch (Exception ex)
              {
                return (object) Celda.GetFormattedString();
              }
            }
          }
          if (Celda.DataType == XLDataType.Number)
            return (object) (double) Celda.Value;
          int dataType = (int) Celda.DataType;
          return (object) Celda.GetString();
        }
      }
    }

    private string DateToString_ddMMyyyy_hhmmss(System.DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Day >= 10 ? str1 + (object) Date.Day : str1 + "0" + (object) Date.Day) + "/";
      string str3 = (Date.Month >= 10 ? str2 + (object) Date.Month : str2 + "0" + (object) Date.Month) + "/";
      string str4 = (Date.Year >= 10 ? (Date.Year >= 100 ? (Date.Year >= 1000 ? str3 + (object) Date.Year : str3 + "0" + (object) Date.Year) : str3 + "00" + (object) Date.Year) : str3 + "000" + (object) Date.Year) + " ";
      string str5 = (Date.Hour >= 10 ? str4 + (object) Date.Hour : str4 + "0" + (object) Date.Hour) + ":";
      string str6 = (Date.Minute >= 10 ? str5 + (object) Date.Minute : str5 + "0" + (object) Date.Minute) + ":";
      return Date.Second >= 10 ? str6 + (object) Date.Second : str6 + "0" + (object) Date.Second;
    }

    private string DateToString_ddMMyyyy(System.DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Day >= 10 ? str1 + (object) Date.Day : str1 + "0" + (object) Date.Day) + "/";
      string str3 = (Date.Month >= 10 ? str2 + (object) Date.Month : str2 + "0" + (object) Date.Month) + "/";
      return Date.Year >= 10 ? (Date.Year >= 100 ? (Date.Year >= 1000 ? str3 + (object) Date.Year : str3 + "0" + (object) Date.Year) : str3 + "00" + (object) Date.Year) : str3 + "000" + (object) Date.Year;
    }

    private string DateToString_hhmmssttt(System.DateTime Date)
    {
      string str1 = "";
      string str2 = (Date.Hour >= 10 ? str1 + (object) Date.Hour : str1 + "0" + (object) Date.Hour) + ":";
      string str3 = (Date.Minute >= 10 ? str2 + (object) Date.Minute : str2 + "0" + (object) Date.Minute) + ":";
      string str4 = (Date.Second >= 10 ? str3 + (object) Date.Second : str3 + "0" + (object) Date.Second) + ".";
      return Date.Millisecond >= 10 ? (Date.Millisecond >= 100 ? str4 + (object) Date.Millisecond : str4 + "0" + (object) Date.Millisecond) : str4 + "00" + (object) Date.Millisecond;
    }

    private void BtnAbrirArchivo_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Hojas de calculo Excel (*.Xlsx; *.Xlsm)|*.Xlsx;*.Xlsm";
      openFileDialog.Multiselect = false;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.TablaHoja.Columns.Clear();
      this.TablaHoja.Rows.Clear();
      this.FileName = openFileDialog.FileName;
      int num = (int) new CargandoArchivoDeExcel()
      {
        FileName = this.FileName,
        ParentForm = this
      }.ShowDialog();
    }

    private void NumPage_ValueChanged(object sender, EventArgs e)
    {
      if (!File.Exists(this.FileName))
        return;
      this.TablaHoja.Columns.Clear();
      this.TablaHoja.Rows.Clear();
      int num = (int) new CargandoArchivoDeExcel()
      {
        FileName = this.FileName,
        ParentForm = this
      }.ShowDialog();
    }

    private bool Transferir()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (!(this.FileName != "") || !File.Exists(this.FileName) || this.Wb == null)
        return false;
      if (this.Wb.Worksheets.Count > 0)
      {
        XLWorkbook workbook = new XLWorkbook();
        try
        {
          this.Wb = new XLWorkbook(this.FileName);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "No se ha podido cargar el archivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        this.NumPage.Minimum = 1M;
        this.NumPage.Maximum = (Decimal) this.Wb.Worksheets.Count;
        IXLWorksheet xlWorksheet = (IXLWorksheet) null;
        try
        {
          this.Wb.Worksheet((int) this.NumPage.Value).CopyTo(workbook, "Sheet1");
          xlWorksheet = workbook.Worksheet(1);
          xlWorksheet.RecalculateAllFormulas();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        if (this.TablaHoja.SelectedCells.Count > 0)
        {
          if (this.TablaDeImporte.SelectedCells.Count == 1)
          {
            int num1;
            int num2;
            int rowIndex;
            int columnIndex;
            try
            {
              num1 = this.TablaHoja.Rows.Count;
              num2 = this.TablaHoja.Columns.Count;
              for (int index = 0; index < this.TablaHoja.SelectedCells.Count; ++index)
              {
                if (this.TablaHoja.SelectedCells[index].ColumnIndex < num2)
                  num2 = this.TablaHoja.SelectedCells[index].ColumnIndex;
                if (this.TablaHoja.SelectedCells[index].RowIndex < num1)
                  num1 = this.TablaHoja.SelectedCells[index].RowIndex;
              }
              rowIndex = this.TablaDeImporte.SelectedCells[0].RowIndex;
              columnIndex = this.TablaDeImporte.SelectedCells[0].ColumnIndex;
            }
            catch (Exception ex)
            {
              int num3 = (int) MessageBox.Show(ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            int num4 = xlWorksheet.RangeUsed().FirstRow().RowNumber();
            int num5 = xlWorksheet.RangeUsed().FirstColumn().ColumnNumber();
            for (int index1 = 0; index1 < this.TablaHoja.SelectedCells.Count; ++index1)
            {
              int index2 = this.TablaHoja.SelectedCells[index1].RowIndex - num1 + rowIndex;
              int index3 = this.TablaHoja.SelectedCells[index1].ColumnIndex - num2 + columnIndex;
              if (this.TablaDeImporte.Rows.Count - 1 < index2)
              {
                int num6 = index2 - (this.TablaDeImporte.Rows.Count - 1);
                for (int index4 = 1; index4 <= num6; ++index4)
                {
                  this.TablaDeImporte.Rows.Add();
                  for (int index5 = 0; index5 < this.ColumnsTypes.Count; ++index5)
                    this.TablaDeImporte.Rows[this.TablaDeImporte.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[index5].Tag = (object) 0;
                }
              }
              if (this.TablaDeImporte.Columns.Count - 1 >= index3)
              {
                IXLCell xlCell;
                try
                {
                  xlCell = xlWorksheet.Cell(num4 + this.TablaHoja.SelectedCells[index1].RowIndex, num5 + this.TablaHoja.SelectedCells[index1].ColumnIndex);
                }
                catch (Exception ex)
                {
                  return false;
                }
                this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) "";
                this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = SystemColors.Window;
                if (xlCell.HasFormula)
                  xlCell.Value = xlWorksheet.Evaluate(xlCell.FormulaA1);
                switch (this.TablaDeImporte.Columns[index3].Tag.ToString())
                {
                  case "Intenger":
                    if (xlCell.Value != null)
                    {
                      if (!(xlCell.Value.ToString() == ""))
                      {
                        try
                        {
                          double result = 0.0;
                          if (double.TryParse(xlCell.Value.ToString(), out result))
                          {
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) (int) result;
                            continue;
                          }
                          if (xlCell.DataType == XLDataType.Number)
                          {
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) (int) xlCell.Value;
                            continue;
                          }
                          try
                          {
                            xlCell.SetDataType(XLDataType.Number);
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) (int) xlCell.GetDouble();
                            continue;
                          }
                          catch (Exception ex)
                          {
                            xlCell.SetDataType(XLDataType.Number);
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) (int) xlCell.Value;
                            continue;
                          }
                        }
                        catch (Exception ex)
                        {
                          this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) 0;
                          this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = Color.IndianRed;
                          flag1 = true;
                          continue;
                        }
                      }
                    }
                    this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) 0;
                    continue;
                  case "Double":
                    if (xlCell.Value != null)
                    {
                      if (xlCell.Value != (object) "")
                      {
                        try
                        {
                          double result = 0.0;
                          if (double.TryParse(xlCell.Value.ToString(), out result))
                          {
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) result;
                            continue;
                          }
                          if (xlCell.DataType == XLDataType.Number)
                          {
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = xlCell.Value;
                            continue;
                          }
                          try
                          {
                            xlCell.SetDataType(XLDataType.Number);
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) xlCell.GetDouble();
                            continue;
                          }
                          catch (Exception ex)
                          {
                            xlCell.SetDataType(XLDataType.Number);
                            this.TablaDeImporte.Rows[index2].Cells[index3].Value = xlCell.Value;
                            continue;
                          }
                        }
                        catch (Exception ex)
                        {
                          this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) 0;
                          this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = Color.IndianRed;
                          flag1 = true;
                          continue;
                        }
                      }
                    }
                    this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) 0;
                    continue;
                  case "String":
                    string formattedString1 = xlCell.GetFormattedString();
                    if (formattedString1.Contains("'"))
                    {
                      flag2 = true;
                      continue;
                    }
                    if (formattedString1.Contains("\\"))
                    {
                      flag2 = true;
                      continue;
                    }
                    this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) xlCell.GetFormattedString();
                    continue;
                  case "Date":
                    if (xlCell == null || xlCell.Value == (object) "")
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.DateToString_ddMMyyyy(new System.DateTime(0L));
                      continue;
                    }
                    if (xlCell.DataType == XLDataType.DateTime)
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.DateToString_ddMMyyyy((System.DateTime) xlCell.Value);
                      continue;
                    }
                    try
                    {
                      xlCell.SetDataType(XLDataType.DateTime);
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.DateToString_ddMMyyyy(xlCell.GetDateTime());
                      continue;
                    }
                    catch (Exception ex)
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) "01/01/1753";
                      this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = Color.IndianRed;
                      flag1 = true;
                      continue;
                    }
                  case "Time":
                    if (xlCell.Value == null || xlCell.Value == (object) "")
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) "00:00:00.000";
                      flag1 = true;
                      continue;
                    }
                    if (xlCell.DataType == XLDataType.DateTime)
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = xlCell.Value;
                      continue;
                    }
                    if (xlCell.DataType == XLDataType.TimeSpan)
                    {
                      xlCell.SetDataType(XLDataType.DateTime);
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.DateToString_hhmmssttt((System.DateTime) xlCell.Value);
                      continue;
                    }
                    try
                    {
                      xlCell.SetDataType(XLDataType.DateTime);
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.DateToString_hhmmssttt((System.DateTime) xlCell.Value);
                      continue;
                    }
                    catch (Exception ex)
                    {
                      this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) "00:00:00.000";
                      this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = Color.IndianRed;
                      flag1 = true;
                      continue;
                    }
                  default:
                    string formattedString2 = xlCell.GetFormattedString();
                    int result1;
                    if (int.TryParse(this.TablaDeImporte.Columns[index3].Tag.ToString(), out result1))
                    {
                      Clipboard.SetText(xlCell.GetFormattedString() + "-" + this.ListasPredefinidas[result1][2]);
                      if (this.ListasPredefinidas[result1].Contains(xlCell.GetFormattedString()))
                      {
                        this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) formattedString2;
                        this.TablaDeImporte.Rows[index2].Cells[index3].Tag = (object) this.ListasPredefinidas[result1].IndexOf(formattedString2);
                        continue;
                      }
                      if (this.ListasPredefinidas[result1].Count > 0)
                      {
                        this.TablaDeImporte.Rows[index2].Cells[index3].Value = (object) this.ListasPredefinidas[result1][0];
                        this.TablaDeImporte.Rows[index2].Cells[index3].Tag = (object) 0;
                        this.TablaDeImporte.Rows[index2].Cells[index3].Style.BackColor = Color.IndianRed;
                        flag1 = true;
                        continue;
                      }
                      continue;
                    }
                    int num7 = (int) MessageBox.Show("La columna debe contener el indice de la lista.", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    continue;
                }
              }
            }
          }
        }
        xlWorksheet.Delete();
      }
      if (flag1)
      {
        int num8 = (int) MessageBox.Show("Algunas celdas se ignoraron debido a que contenían valores invalidos.", "Celdas invalidas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      if (flag2)
      {
        int num9 = (int) MessageBox.Show("No se permite el uso de comillas simples (') ni barras diagonales inversas (\\) en las celdas.", "Celdas invalidas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return true;
    }

    private void BtnTransferir_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.Transferir();
      Cursor.Current = Cursors.Arrow;
    }

    public void AddColumnIntenger(string ColumnText)
    {
      this.ColumnsList.Add(ColumnText);
      this.ColumnsTypes.Add("Intenger");
    }

    public void AddColumnDouble(string ColumnText)
    {
      this.ColumnsList.Add(ColumnText);
      this.ColumnsTypes.Add("Double");
    }

    public void AddColumnString(string ColumnText)
    {
      this.ColumnsList.Add(ColumnText);
      this.ColumnsTypes.Add("String");
    }

    public void AddColumnDate(string ColumnText)
    {
      this.ColumnsList.Add(ColumnText);
      this.ColumnsTypes.Add("Date");
    }

    public void AddColumnTime(string ColumnText)
    {
      this.ColumnsList.Add(ColumnText);
      this.ColumnsTypes.Add("Time");
    }

    public int AddColumnList(string ColumnText, List<string> Items, string Name)
    {
      if (Items == null)
      {
        int num = (int) MessageBox.Show("Error Interno: No se ha asignado ningún valor Items a la columna.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      if (!this.IDsListasPredefinidas.Contains(Name))
      {
        this.ListasPredefinidas.Add(Items);
        this.IDsListasPredefinidas.Add(Name);
        this.ColumnsTypes.Add("#List:" + Name);
        this.ColumnsList.Add(ColumnText);
        return this.ListasPredefinidas.Count - 1;
      }
      int num1 = (int) MessageBox.Show("Error Interno: Ya se ha asignado este nombre a otra columna.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return -1;
    }

    private void ImportacionDeDatosDesdeExcel_Load(object sender, EventArgs e)
    {
      this.Resultado = DialogResult.Cancel;
      if (this.AlreadyLoaded)
        return;
      try
      {
        for (int index1 = 0; index1 < this.ColumnsList.Count; ++index1)
        {
          switch (this.ColumnsTypes[index1])
          {
            case "Intenger":
              this.TablaDeImporte.Columns.Add("ColImp" + (object) index1, this.ColumnsList[index1]);
              this.TablaDeImporte.Columns[index1].Tag = (object) "Intenger";
              this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.NotSortable;
              break;
            case "Double":
              this.TablaDeImporte.Columns.Add("ColImp" + (object) index1, this.ColumnsList[index1]);
              this.TablaDeImporte.Columns[index1].Tag = (object) "Double";
              this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.NotSortable;
              break;
            case "String":
              this.TablaDeImporte.Columns.Add("ColImp" + (object) index1, this.ColumnsList[index1]);
              this.TablaDeImporte.Columns[index1].Tag = (object) "String";
              this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.NotSortable;
              break;
            case "Date":
              this.TablaDeImporte.Columns.Add("ColImp" + (object) index1, this.ColumnsList[index1]);
              this.TablaDeImporte.Columns[index1].Tag = (object) "Date";
              this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.NotSortable;
              this.TablaDeImporte.Columns[index1].ReadOnly = true;
              break;
            case "Time":
              this.TablaDeImporte.Columns.Add("ColImp" + (object) index1, this.ColumnsList[index1]);
              this.TablaDeImporte.Columns[index1].Tag = (object) "Time";
              this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.NotSortable;
              this.TablaDeImporte.Columns[index1].ReadOnly = true;
              break;
            default:
              if (this.ColumnsTypes[index1].Count<char>() >= 6 && this.ColumnsTypes[index1].Substring(0, 6) == "#List:")
              {
                int index2 = this.IDsListasPredefinidas.IndexOf(this.ColumnsTypes[index1].Substring(6));
                DataGridViewCell cellTemplate = (DataGridViewCell) new DataGridViewComboBoxCell();
                if (this.ListasPredefinidas[index2].Count > 0)
                  cellTemplate.Style.NullValue = (object) this.ListasPredefinidas[index2][0];
                for (int index3 = 0; index3 < this.ListasPredefinidas[index2].Count; ++index3)
                  ((DataGridViewComboBoxCell) cellTemplate).Items.Add((object) this.ListasPredefinidas[index2][index3]);
                ((DataGridViewComboBoxCell) cellTemplate).FlatStyle = FlatStyle.Flat;
                ((DataGridViewComboBoxCell) cellTemplate).DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                this.TablaDeImporte.Columns.Add(new DataGridViewColumn(cellTemplate));
                this.TablaDeImporte.Columns[index1].HeaderText = this.ColumnsList[index1];
                this.TablaDeImporte.Columns[index1].SortMode = DataGridViewColumnSortMode.Programmatic;
                this.TablaDeImporte.Columns[index1].Tag = (object) index2;
                break;
              }
              break;
          }
        }
        for (int index = 0; index < this.TablaDeImporte.ColumnCount; ++index)
        {
          this.TablaDeImporte.Columns[index].SortMode = DataGridViewColumnSortMode.NotSortable;
          if (this.TablaDeImporte.Columns[index].Tag == (object) "Intenger" || this.TablaDeImporte.Columns[index].Tag == (object) "Double")
            this.TablaDeImporte.Columns[index].DefaultCellStyle.NullValue = (object) "0";
          else if (this.TablaDeImporte.Columns[index].Tag == (object) "Date")
            this.TablaDeImporte.Columns[index].DefaultCellStyle.NullValue = (object) "01/01/1753";
          else if (this.TablaDeImporte.Columns[index].Tag == (object) "Time")
            this.TablaDeImporte.Columns[index].DefaultCellStyle.NullValue = (object) "00:00:00.000";
        }
        this.TablaDeImporte.Rows.Add();
        for (int index = 0; index < this.ColumnsTypes.Count; ++index)
          this.TablaDeImporte.Rows[this.TablaDeImporte.Rows.GetLastRow(DataGridViewElementStates.None)].Cells[index].Tag = (object) 0;
        this.ColumnsList.Clear();
        this.ColumnsTypes.Clear();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
      this.AlreadyLoaded = true;
    }

    private void BtnImportar_Click(object sender, EventArgs e) => this.Resultado = DialogResult.OK;

    private void ImportacionDeDatosDesdeExcel_FormClosing(object sender, FormClosingEventArgs e) => this.DialogResult = this.Resultado;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportacionDeDatosDesdeExcel));
      this.panel1 = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.TablaHoja = new DataGridView();
      this.PanelBajoDeHoja = new Panel();
      this.NumPage = new NumericUpDown();
      this.LabPg = new Label();
      this.TablaDeImporte = new DataGridView();
      this.panel4 = new Panel();
      this.panel2 = new Panel();
      this.linkLabel1 = new LinkLabel();
      this.panel3 = new Panel();
      this.LinkLabelTablaAImportar = new LinkLabel();
      this.BtnTransferir = new Button();
      this.BtnAbrirArchivo = new Button();
      this.BtnImportar = new Button();
      this.panel1.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.TablaHoja).BeginInit();
      this.PanelBajoDeHoja.SuspendLayout();
      this.NumPage.BeginInit();
      ((ISupportInitialize) this.TablaDeImporte).BeginInit();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = Color.Brown;
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.BtnTransferir);
      this.panel1.Controls.Add((Control) this.BtnAbrirArchivo);
      this.panel1.Controls.Add((Control) this.BtnImportar);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(784, 50);
      this.panel1.TabIndex = 6;
      this.splitContainer1.BackColor = Color.DimGray;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 50);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.TablaHoja);
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.PanelBajoDeHoja);
      this.splitContainer1.Panel1.Padding = new Padding(1, 0, 0, 1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.TablaDeImporte);
      this.splitContainer1.Panel2.Controls.Add((Control) this.panel4);
      this.splitContainer1.Panel2.Controls.Add((Control) this.panel3);
      this.splitContainer1.Panel2.Padding = new Padding(0, 0, 1, 1);
      this.splitContainer1.Size = new Size(784, 411);
      this.splitContainer1.SplitterDistance = 325;
      this.splitContainer1.SplitterWidth = 1;
      this.splitContainer1.TabIndex = 7;
      this.TablaHoja.AllowUserToAddRows = false;
      this.TablaHoja.AllowUserToDeleteRows = false;
      this.TablaHoja.BackgroundColor = SystemColors.ScrollBar;
      this.TablaHoja.BorderStyle = BorderStyle.None;
      this.TablaHoja.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.TablaHoja.ColumnHeadersVisible = false;
      this.TablaHoja.Dock = DockStyle.Fill;
      this.TablaHoja.GridColor = Color.Gray;
      this.TablaHoja.Location = new Point(1, 25);
      this.TablaHoja.Name = "TablaHoja";
      this.TablaHoja.ReadOnly = true;
      this.TablaHoja.RowHeadersVisible = false;
      this.TablaHoja.Size = new Size(324, 365);
      this.TablaHoja.TabIndex = 0;
      this.PanelBajoDeHoja.BackColor = Color.Brown;
      this.PanelBajoDeHoja.Controls.Add((Control) this.NumPage);
      this.PanelBajoDeHoja.Controls.Add((Control) this.LabPg);
      this.PanelBajoDeHoja.Dock = DockStyle.Bottom;
      this.PanelBajoDeHoja.Location = new Point(1, 390);
      this.PanelBajoDeHoja.Name = "PanelBajoDeHoja";
      this.PanelBajoDeHoja.Size = new Size(324, 20);
      this.PanelBajoDeHoja.TabIndex = 1;
      this.NumPage.Dock = DockStyle.Left;
      this.NumPage.Location = new Point(56, 0);
      this.NumPage.Maximum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.NumPage.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.NumPage.Name = "NumPage";
      this.NumPage.Size = new Size(50, 20);
      this.NumPage.TabIndex = 1;
      this.NumPage.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.NumPage.ValueChanged += new EventHandler(this.NumPage_ValueChanged);
      this.LabPg.AutoSize = true;
      this.LabPg.Dock = DockStyle.Left;
      this.LabPg.Font = new Font("Microsoft Sans Serif", 10f);
      this.LabPg.Location = new Point(0, 0);
      this.LabPg.Name = "LabPg";
      this.LabPg.Size = new Size(56, 17);
      this.LabPg.TabIndex = 0;
      this.LabPg.Text = "Pagina:";
      this.TablaDeImporte.AllowUserToAddRows = false;
      this.TablaDeImporte.BackgroundColor = SystemColors.ScrollBar;
      this.TablaDeImporte.BorderStyle = BorderStyle.None;
      this.TablaDeImporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.TablaDeImporte.Dock = DockStyle.Fill;
      this.TablaDeImporte.GridColor = Color.Gray;
      this.TablaDeImporte.Location = new Point(0, 25);
      this.TablaDeImporte.MultiSelect = false;
      this.TablaDeImporte.Name = "TablaDeImporte";
      this.TablaDeImporte.Size = new Size(457, 365);
      this.TablaDeImporte.TabIndex = 4;
      this.panel4.BackColor = Color.Brown;
      this.panel4.Dock = DockStyle.Bottom;
      this.panel4.Location = new Point(0, 390);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(457, 20);
      this.panel4.TabIndex = 13;
      this.panel2.BackColor = Color.LightSteelBlue;
      this.panel2.BackgroundImage = (Image) Resources.ImgIndexItem;
      this.panel2.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel2.Controls.Add((Control) this.linkLabel1);
      this.panel2.Cursor = Cursors.Hand;
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(1, 0);
      this.panel2.Margin = new Padding(0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(324, 25);
      this.panel2.TabIndex = 13;
      this.linkLabel1.ActiveLinkColor = Color.LightGray;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.BackColor = Color.Transparent;
      this.linkLabel1.Dock = DockStyle.Left;
      this.linkLabel1.Font = new Font("Microsoft Sans Serif", 12f);
      this.linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
      this.linkLabel1.LinkColor = Color.Black;
      this.linkLabel1.Location = new Point(0, 0);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(131, 20);
      this.linkLabel1.TabIndex = 6;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "Tabla A Importar:";
      this.linkLabel1.VisitedLinkColor = Color.Black;
      this.panel3.BackColor = Color.LightSteelBlue;
      this.panel3.BackgroundImage = (Image) Resources.ImgIndexItem;
      this.panel3.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel3.Controls.Add((Control) this.LinkLabelTablaAImportar);
      this.panel3.Cursor = Cursors.Hand;
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Margin = new Padding(0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(457, 25);
      this.panel3.TabIndex = 12;
      this.LinkLabelTablaAImportar.ActiveLinkColor = Color.LightGray;
      this.LinkLabelTablaAImportar.AutoSize = true;
      this.LinkLabelTablaAImportar.BackColor = Color.Transparent;
      this.LinkLabelTablaAImportar.Dock = DockStyle.Left;
      this.LinkLabelTablaAImportar.Font = new Font("Microsoft Sans Serif", 12f);
      this.LinkLabelTablaAImportar.LinkBehavior = LinkBehavior.NeverUnderline;
      this.LinkLabelTablaAImportar.LinkColor = Color.Black;
      this.LinkLabelTablaAImportar.Location = new Point(0, 0);
      this.LinkLabelTablaAImportar.Name = "LinkLabelTablaAImportar";
      this.LinkLabelTablaAImportar.Size = new Size(131, 20);
      this.LinkLabelTablaAImportar.TabIndex = 6;
      this.LinkLabelTablaAImportar.TabStop = true;
      this.LinkLabelTablaAImportar.Text = "Tabla A Importar:";
      this.LinkLabelTablaAImportar.VisitedLinkColor = Color.Black;
      this.BtnTransferir.BackgroundImage = (Image) Resources.flecha_hacia_la_izquierda;
      this.BtnTransferir.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnTransferir.FlatAppearance.BorderColor = Color.Brown;
      this.BtnTransferir.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnTransferir.FlatStyle = FlatStyle.Flat;
      this.BtnTransferir.Location = new Point(303, 3);
      this.BtnTransferir.Name = "BtnTransferir";
      this.BtnTransferir.Size = new Size(42, 42);
      this.BtnTransferir.TabIndex = 2;
      this.BtnTransferir.UseVisualStyleBackColor = true;
      this.BtnTransferir.Click += new EventHandler(this.BtnTransferir_Click);
      this.BtnAbrirArchivo.BackgroundImage = (Image) Resources.carpeta;
      this.BtnAbrirArchivo.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnAbrirArchivo.FlatAppearance.BorderColor = Color.Brown;
      this.BtnAbrirArchivo.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnAbrirArchivo.FlatStyle = FlatStyle.Flat;
      this.BtnAbrirArchivo.Location = new Point(3, 3);
      this.BtnAbrirArchivo.Name = "BtnAbrirArchivo";
      this.BtnAbrirArchivo.Size = new Size(42, 42);
      this.BtnAbrirArchivo.TabIndex = 1;
      this.BtnAbrirArchivo.UseVisualStyleBackColor = true;
      this.BtnAbrirArchivo.Click += new EventHandler(this.BtnAbrirArchivo_Click);
      this.BtnImportar.BackgroundImage = (Image) Resources.aprobar_simbolo_de_esquema;
      this.BtnImportar.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnImportar.DialogResult = DialogResult.OK;
      this.BtnImportar.FlatAppearance.BorderColor = Color.Brown;
      this.BtnImportar.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnImportar.FlatStyle = FlatStyle.Flat;
      this.BtnImportar.Location = new Point(737, 6);
      this.BtnImportar.Name = "BtnImportar";
      this.BtnImportar.Size = new Size(42, 36);
      this.BtnImportar.TabIndex = 0;
      this.BtnImportar.UseVisualStyleBackColor = true;
      this.BtnImportar.Click += new EventHandler(this.BtnImportar_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 461);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.panel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ImportacionDeDatosDesdeExcel);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (ImportacionDeDatosDesdeExcel);
      this.Load += new EventHandler(this.ImportacionDeDatosDesdeExcel_Load);
      this.panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.TablaHoja).EndInit();
      this.PanelBajoDeHoja.ResumeLayout(false);
      this.PanelBajoDeHoja.PerformLayout();
      this.NumPage.EndInit();
      ((ISupportInitialize) this.TablaDeImporte).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
