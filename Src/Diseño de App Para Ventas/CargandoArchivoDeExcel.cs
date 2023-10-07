// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.CargandoArchivoDeExcel
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class CargandoArchivoDeExcel : Form
  {
    public ImportacionDeDatosDesdeExcel ParentForm;
    public string FileName;
    private IXLWorksheet Ws;
    private int i = 1;
    private IContainer components;
    private Label label1;
    private ProgressBar BarraDeProgreso;
    private System.Windows.Forms.Timer Cargador;

    public CargandoArchivoDeExcel()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.CargandoArchivoDeExcel_FormClosing);
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

    private string DateToString(System.DateTime Date) => Date.Second == 0 && Date.Minute == 0 && Date.Hour == 0 ? this.DateToString_ddMMyyyy(Date) : this.DateToString_ddMMyyyy_hhmmss(Date);

    private void CargandoArchivoDeExcel_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.BarraDeProgreso.Value == 100)
        return;
      e.Cancel = true;
    }

    private Color GetCellColor(IXLCell Celda)
    {
      try
      {
        switch (Celda.Style.Fill.BackgroundColor.ColorType)
        {
          case XLColorType.Color:
            return Celda.Style.Fill.BackgroundColor.Color;
          case XLColorType.Theme:
            switch (Celda.Style.Fill.BackgroundColor.ThemeColor)
            {
              case XLThemeColor.Background1:
                return this.ParentForm.Wb.Theme.Background1.Color;
              case XLThemeColor.Background2:
                return this.ParentForm.Wb.Theme.Background2.Color;
              case XLThemeColor.Accent1:
                return this.ParentForm.Wb.Theme.Accent1.Color;
              case XLThemeColor.Accent2:
                return this.ParentForm.Wb.Theme.Accent2.Color;
              case XLThemeColor.Accent3:
                return this.ParentForm.Wb.Theme.Accent3.Color;
              case XLThemeColor.Accent4:
                return this.ParentForm.Wb.Theme.Accent4.Color;
              case XLThemeColor.Accent5:
                return this.ParentForm.Wb.Theme.Accent5.Color;
              case XLThemeColor.Accent6:
                return this.ParentForm.Wb.Theme.Accent6.Color;
              default:
                return SystemColors.Window;
            }
          case XLColorType.Indexed:
            return XLColor.FromIndex(Celda.Style.Fill.BackgroundColor.Indexed).Color;
          default:
            return SystemColors.Window;
        }
      }
      catch (Exception ex)
      {
        return SystemColors.Window;
      }
    }

    private Color GetCellFontColor(IXLCell Celda)
    {
      try
      {
        switch (Celda.Style.Font.FontColor.ColorType)
        {
          case XLColorType.Color:
            return Celda.Style.Font.FontColor.Color;
          case XLColorType.Theme:
            switch (Celda.Style.Font.FontColor.ThemeColor)
            {
              case XLThemeColor.Background1:
                return this.ParentForm.Wb.Theme.Background1.Color;
              case XLThemeColor.Background2:
                return this.ParentForm.Wb.Theme.Background2.Color;
              case XLThemeColor.Accent1:
                return this.ParentForm.Wb.Theme.Accent1.Color;
              case XLThemeColor.Accent2:
                return this.ParentForm.Wb.Theme.Accent2.Color;
              case XLThemeColor.Accent3:
                return this.ParentForm.Wb.Theme.Accent3.Color;
              case XLThemeColor.Accent4:
                return this.ParentForm.Wb.Theme.Accent4.Color;
              case XLThemeColor.Accent5:
                return this.ParentForm.Wb.Theme.Accent5.Color;
              case XLThemeColor.Accent6:
                return this.ParentForm.Wb.Theme.Accent6.Color;
              default:
                return Color.Black;
            }
          case XLColorType.Indexed:
            return XLColor.FromIndex(Celda.Style.Font.FontColor.Indexed).Color;
          default:
            return Color.Black;
        }
      }
      catch (Exception ex)
      {
        return SystemColors.Window;
      }
    }

    private void Cargador_Tick(object sender, EventArgs e)
    {
      if (this.i <= this.Ws.RangeUsed().RowCount())
      {
        List<object> objectList = new List<object>();
        for (int index = 1; index <= this.Ws.RangeUsed().ColumnCount(); ++index)
        {
          try
          {
            IXLCell xlCell = this.Ws.Cell(this.Ws.RangeUsed().FirstRow().RowNumber() + this.i - 1, this.Ws.RangeUsed().FirstColumn().ColumnNumber() + index - 1);
            string formattedString = xlCell.GetFormattedString();
            try
            {
              if (xlCell.Style.NumberFormat.NumberFormatId >= 14 && xlCell.Style.NumberFormat.NumberFormatId <= 16 || xlCell.Style.NumberFormat.NumberFormatId == 22 || xlCell.Style.NumberFormat.Format.Contains("d") && xlCell.Style.NumberFormat.Format.Contains("m") && xlCell.Style.NumberFormat.Format.Contains("y"))
              {
                if (xlCell.HasFormula)
                {
                  object Date = this.Ws.Evaluate(xlCell.FormulaA1);
                  if (Date != null && Date.GetType() == typeof (System.DateTime))
                  {
                    objectList.Add((object) this.DateToString((System.DateTime) Date));
                  }
                  else
                  {
                    xlCell.SetDataType(XLDataType.DateTime);
                    objectList.Add((object) this.DateToString(xlCell.GetDateTime()));
                  }
                }
                else
                {
                  xlCell.SetDataType(XLDataType.DateTime);
                  objectList.Add((object) this.DateToString(xlCell.GetDateTime()));
                }
              }
              else
                objectList.Add((object) formattedString);
            }
            catch (Exception ex)
            {
              objectList.Add((object) formattedString);
            }
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.Message, "No se ha podido abrir el archivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.BarraDeProgreso.Value = 100;
            this.Close();
          }
        }
        this.ParentForm.TablaHoja.Rows.Add(objectList.ToArray());
        for (int index = 1; index <= this.Ws.RangeUsed().ColumnCount(); ++index)
        {
          IXLCell Celda = (IXLCell) null;
          try
          {
            Celda = this.Ws.Cell(this.Ws.RangeUsed().FirstRow().RowNumber() + this.i - 1, this.Ws.RangeUsed().FirstColumn().ColumnNumber() + index - 1);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.Message, "No se ha podido abrir el archivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.BarraDeProgreso.Value = 100;
            this.Close();
          }
          try
          {
            if (Celda.Style.Fill.BackgroundColor != (XLColor) null)
            {
              Color cellColor = this.GetCellColor(Celda);
              this.ParentForm.TablaHoja.Rows[this.ParentForm.TablaHoja.Rows.Count - 1].Cells[index - 1].Style.BackColor = Color.FromArgb((int) cellColor.R, (int) cellColor.G, (int) cellColor.B);
            }
          }
          catch (Exception ex)
          {
          }
          try
          {
            if (Celda.Style.Font.FontColor != (XLColor) null)
              this.ParentForm.TablaHoja.Rows[this.ParentForm.TablaHoja.Rows.Count - 1].Cells[index - 1].Style.ForeColor = this.GetCellFontColor(Celda);
          }
          catch (Exception ex)
          {
          }
          FontStyle style = FontStyle.Regular;
          if (Celda.Style.Font.Italic)
            style |= FontStyle.Italic;
          if (Celda.Style.Font.Bold)
            style |= FontStyle.Bold;
          if (Celda.Style.Font.Underline != XLFontUnderlineValues.None)
            style |= FontStyle.Underline;
          if (Celda.Style.Font.Strikethrough)
            style |= FontStyle.Strikeout;
          try
          {
            this.ParentForm.TablaHoja.Rows[this.ParentForm.TablaHoja.Rows.Count - 1].Cells[index - 1].Style.Font = new Font(Celda.Style.Font.FontName, (float) Celda.Style.Font.FontSize, style);
          }
          catch (Exception ex1)
          {
            try
            {
              this.ParentForm.TablaHoja.Rows[this.ParentForm.TablaHoja.Rows.Count - 1].Cells[index - 1].Style.Font = new Font("Arial", (float) Celda.Style.Font.FontSize, style);
            }
            catch (Exception ex2)
            {
              int num = (int) MessageBox.Show(ex2.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
            }
          }
        }
        this.BarraDeProgreso.Value = (int) ((double) this.i / (double) this.Ws.RangeUsed().RowCount() * 100.0);
        ++this.i;
      }
      else
      {
        this.BarraDeProgreso.Value = 100;
        this.Cargador.Stop();
        this.Close();
      }
    }

    private void CargandoArchivoDeExcel_Load(object sender, EventArgs e)
    {
      try
      {
        this.ParentForm.Wb = new XLWorkbook(this.FileName);
        if (this.ParentForm.Wb.Worksheets.Count <= 0)
          return;
        this.ParentForm.NumPage.Minimum = 1M;
        this.ParentForm.NumPage.Maximum = (Decimal) this.ParentForm.Wb.Worksheets.Count;
        this.Ws = (IXLWorksheet) null;
        this.Ws = this.ParentForm.Wb.Worksheet((int) this.ParentForm.NumPage.Value);
        for (int index = 0; index < this.Ws.RangeUsed().ColumnCount(); ++index)
          this.ParentForm.TablaHoja.Columns.Add("Col" + (object) index, string.Concat((object) index));
        this.Cargador.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "No se ha podido cargar el archivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.BarraDeProgreso.Value = 100;
        this.ParentForm.Wb = (XLWorkbook) null;
        this.Ws = (IXLWorksheet) null;
        this.ParentForm.FileName = "";
        this.ParentForm.NumPage.Value = 1M;
        this.ParentForm.NumPage.Minimum = 1M;
        this.ParentForm.NumPage.Maximum = 1M;
        this.Close();
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
      this.label1 = new Label();
      this.BarraDeProgreso = new ProgressBar();
      this.Cargador = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(252, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Porfavor, espere mientras se carga el archivo Excel:";
      this.BarraDeProgreso.Location = new Point(15, 28);
      this.BarraDeProgreso.Name = "BarraDeProgreso";
      this.BarraDeProgreso.Size = new Size(356, 23);
      this.BarraDeProgreso.TabIndex = 1;
      this.Cargador.Interval = 1;
      this.Cargador.Tick += new EventHandler(this.Cargador_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Brown;
      this.ClientSize = new Size(383, 60);
      this.Controls.Add((Control) this.BarraDeProgreso);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CargandoArchivoDeExcel);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Abriendo archivo:";
      this.Load += new EventHandler(this.CargandoArchivoDeExcel_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
