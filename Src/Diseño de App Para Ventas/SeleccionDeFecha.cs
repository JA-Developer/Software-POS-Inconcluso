// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.SeleccionDeFecha
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class SeleccionDeFecha : Form
  {
    private IContainer components;
    private Button BTN_Cancelar;
    private Button BTN_Aceptar;
    public MonthCalendar ControlFecha;

    public SeleccionDeFecha() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SeleccionDeFecha));
      this.ControlFecha = new MonthCalendar();
      this.BTN_Cancelar = new Button();
      this.BTN_Aceptar = new Button();
      this.SuspendLayout();
      this.ControlFecha.Dock = DockStyle.Top;
      this.ControlFecha.Location = new Point(0, 0);
      this.ControlFecha.MaxSelectionCount = 1;
      this.ControlFecha.Name = "ControlFecha";
      this.ControlFecha.TabIndex = 0;
      this.BTN_Cancelar.BackColor = Color.DimGray;
      this.BTN_Cancelar.DialogResult = DialogResult.Cancel;
      this.BTN_Cancelar.FlatStyle = FlatStyle.Flat;
      this.BTN_Cancelar.Location = new Point(100, 170);
      this.BTN_Cancelar.Margin = new Padding(4);
      this.BTN_Cancelar.Name = "BTN_Cancelar";
      this.BTN_Cancelar.Size = new Size(82, 28);
      this.BTN_Cancelar.TabIndex = 8;
      this.BTN_Cancelar.Text = "Cancelar";
      this.BTN_Cancelar.UseVisualStyleBackColor = false;
      this.BTN_Aceptar.BackColor = Color.White;
      this.BTN_Aceptar.DialogResult = DialogResult.OK;
      this.BTN_Aceptar.FlatStyle = FlatStyle.Flat;
      this.BTN_Aceptar.Location = new Point(10, 170);
      this.BTN_Aceptar.Margin = new Padding(4);
      this.BTN_Aceptar.Name = "BTN_Aceptar";
      this.BTN_Aceptar.Size = new Size(82, 28);
      this.BTN_Aceptar.TabIndex = 7;
      this.BTN_Aceptar.Text = "Aceptar";
      this.BTN_Aceptar.UseVisualStyleBackColor = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Brown;
      this.ClientSize = new Size(192, 207);
      this.Controls.Add((Control) this.BTN_Cancelar);
      this.Controls.Add((Control) this.BTN_Aceptar);
      this.Controls.Add((Control) this.ControlFecha);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (SeleccionDeFecha);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Seleccionar Fecha:";
      this.ResumeLayout(false);
    }
  }
}
