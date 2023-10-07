// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.SeleccionDeHora
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class SeleccionDeHora : Form
  {
    private IContainer components;
    private Button BTN_Cancelar;
    private Button BTN_Aceptar;
    private Panel panel1;
    public DateTimePicker ControlTiempo;

    public SeleccionDeHora() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SeleccionDeHora));
      this.BTN_Cancelar = new Button();
      this.BTN_Aceptar = new Button();
      this.ControlTiempo = new DateTimePicker();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.BTN_Cancelar.BackColor = Color.DimGray;
      this.BTN_Cancelar.DialogResult = DialogResult.Cancel;
      this.BTN_Cancelar.FlatStyle = FlatStyle.Flat;
      this.BTN_Cancelar.Location = new Point(153, 0);
      this.BTN_Cancelar.Margin = new Padding(4);
      this.BTN_Cancelar.Name = "BTN_Cancelar";
      this.BTN_Cancelar.Size = new Size(82, 28);
      this.BTN_Cancelar.TabIndex = 8;
      this.BTN_Cancelar.Text = "Cancelar";
      this.BTN_Cancelar.UseVisualStyleBackColor = false;
      this.BTN_Aceptar.BackColor = Color.White;
      this.BTN_Aceptar.DialogResult = DialogResult.OK;
      this.BTN_Aceptar.FlatStyle = FlatStyle.Flat;
      this.BTN_Aceptar.Location = new Point(63, 0);
      this.BTN_Aceptar.Margin = new Padding(4);
      this.BTN_Aceptar.Name = "BTN_Aceptar";
      this.BTN_Aceptar.Size = new Size(82, 28);
      this.BTN_Aceptar.TabIndex = 7;
      this.BTN_Aceptar.Text = "Aceptar";
      this.BTN_Aceptar.UseVisualStyleBackColor = false;
      this.ControlTiempo.CalendarFont = new Font("Microsoft Sans Serif", 10f);
      this.ControlTiempo.CustomFormat = "HH:mm:ss";
      this.ControlTiempo.Dock = DockStyle.Top;
      this.ControlTiempo.Font = new Font("Microsoft Sans Serif", 10f);
      this.ControlTiempo.Format = DateTimePickerFormat.Custom;
      this.ControlTiempo.Location = new Point(10, 10);
      this.ControlTiempo.Margin = new Padding(10);
      this.ControlTiempo.Name = "ControlTiempo";
      this.ControlTiempo.ShowUpDown = true;
      this.ControlTiempo.Size = new Size(235, 23);
      this.ControlTiempo.TabIndex = 9;
      this.ControlTiempo.Value = new DateTime(2019, 7, 3, 0, 0, 0, 0);
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.BTN_Aceptar);
      this.panel1.Controls.Add((Control) this.BTN_Cancelar);
      this.panel1.Location = new Point(10, 44);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(235, 28);
      this.panel1.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Brown;
      this.ClientSize = new Size((int) byte.MaxValue, 82);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.ControlTiempo);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (SeleccionDeHora);
      this.Padding = new Padding(10);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Seleccionar Hora:";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
