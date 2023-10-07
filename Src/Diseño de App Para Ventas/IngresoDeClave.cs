// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.IngresoDeClave
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class IngresoDeClave : Form
  {
    private IContainer components;
    public TextBox TxBoxClave;
    private Panel panel1;
    private Button BTN_Aceptar;
    private Button BTN_Cancelar;
    private Label EtiquetaIngrese;
    private Panel PanelSeparador;

    public IngresoDeClave() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IngresoDeClave));
      this.TxBoxClave = new TextBox();
      this.panel1 = new Panel();
      this.BTN_Aceptar = new Button();
      this.BTN_Cancelar = new Button();
      this.EtiquetaIngrese = new Label();
      this.PanelSeparador = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.TxBoxClave.BorderStyle = BorderStyle.FixedSingle;
      this.TxBoxClave.Dock = DockStyle.Top;
      this.TxBoxClave.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.TxBoxClave.Location = new Point(10, 37);
      this.TxBoxClave.Margin = new Padding(4);
      this.TxBoxClave.Name = "TxBoxClave";
      this.TxBoxClave.Size = new Size(235, 23);
      this.TxBoxClave.TabIndex = 9;
      this.TxBoxClave.UseSystemPasswordChar = true;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.BTN_Aceptar);
      this.panel1.Controls.Add((Control) this.BTN_Cancelar);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(10, 73);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(235, 28);
      this.panel1.TabIndex = 11;
      this.BTN_Aceptar.BackColor = SystemColors.ScrollBar;
      this.BTN_Aceptar.DialogResult = DialogResult.OK;
      this.BTN_Aceptar.FlatStyle = FlatStyle.Flat;
      this.BTN_Aceptar.Location = new Point(63, 0);
      this.BTN_Aceptar.Margin = new Padding(4);
      this.BTN_Aceptar.Name = "BTN_Aceptar";
      this.BTN_Aceptar.Size = new Size(82, 28);
      this.BTN_Aceptar.TabIndex = 7;
      this.BTN_Aceptar.Text = "Aceptar";
      this.BTN_Aceptar.UseVisualStyleBackColor = false;
      this.BTN_Cancelar.BackColor = Color.IndianRed;
      this.BTN_Cancelar.DialogResult = DialogResult.Cancel;
      this.BTN_Cancelar.FlatStyle = FlatStyle.Flat;
      this.BTN_Cancelar.Location = new Point(153, 0);
      this.BTN_Cancelar.Margin = new Padding(4);
      this.BTN_Cancelar.Name = "BTN_Cancelar";
      this.BTN_Cancelar.Size = new Size(82, 28);
      this.BTN_Cancelar.TabIndex = 8;
      this.BTN_Cancelar.Text = "Cancelar";
      this.BTN_Cancelar.UseVisualStyleBackColor = false;
      this.EtiquetaIngrese.AutoSize = true;
      this.EtiquetaIngrese.Dock = DockStyle.Top;
      this.EtiquetaIngrese.Location = new Point(10, 10);
      this.EtiquetaIngrese.Name = "EtiquetaIngrese";
      this.EtiquetaIngrese.Size = new Size(111, 17);
      this.EtiquetaIngrese.TabIndex = 12;
      this.EtiquetaIngrese.Text = "Ingrese la clave:";
      this.PanelSeparador.Dock = DockStyle.Top;
      this.PanelSeparador.Location = new Point(10, 27);
      this.PanelSeparador.Name = "PanelSeparador";
      this.PanelSeparador.Size = new Size(235, 10);
      this.PanelSeparador.TabIndex = 13;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Brown;
      this.ClientSize = new Size((int) byte.MaxValue, 111);
      this.Controls.Add((Control) this.TxBoxClave);
      this.Controls.Add((Control) this.PanelSeparador);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.EtiquetaIngrese);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.MaximizeBox = false;
      this.Name = nameof (IngresoDeClave);
      this.Padding = new Padding(10);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Cambiar Clave:";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
