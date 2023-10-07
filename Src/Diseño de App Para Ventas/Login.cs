// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Login
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using Diseño_de_App_Para_Ventas.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class Login : Form
  {
    private IContainer components;
    private PictureBox pictureBox1;
    private Label label1;
    private Label label2;
    private Button BTN_Aceptar;
    private Button BTN_Cancelar;
    public TextBox TxBox_Usuario;
    public TextBox TxBox_Contrasena;

    public Login() => this.InitializeComponent();

    private void BTN_Aceptar_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Login));
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.TxBox_Usuario = new TextBox();
      this.label2 = new Label();
      this.TxBox_Contrasena = new TextBox();
      this.BTN_Aceptar = new Button();
      this.BTN_Cancelar = new Button();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.pictureBox1.BackColor = Color.White;
      this.pictureBox1.BackgroundImage = (Image) componentResourceManager.GetObject("pictureBox1.BackgroundImage");
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      this.pictureBox1.Location = new Point(13, 13);
      this.pictureBox1.Margin = new Padding(4);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(128, 128);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(149, 13);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(133, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Nombre de usuario:";
      this.TxBox_Usuario.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_Usuario.Location = new Point(152, 34);
      this.TxBox_Usuario.Margin = new Padding(4);
      this.TxBox_Usuario.Name = "TxBox_Usuario";
      this.TxBox_Usuario.Size = new Size(311, 23);
      this.TxBox_Usuario.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(149, 61);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(85, 17);
      this.label2.TabIndex = 3;
      this.label2.Text = "Contraseña:";
      this.TxBox_Contrasena.BorderStyle = BorderStyle.FixedSingle;
      this.TxBox_Contrasena.Location = new Point(152, 82);
      this.TxBox_Contrasena.Margin = new Padding(4);
      this.TxBox_Contrasena.Name = "TxBox_Contrasena";
      this.TxBox_Contrasena.Size = new Size(311, 23);
      this.TxBox_Contrasena.TabIndex = 4;
      this.TxBox_Contrasena.UseSystemPasswordChar = true;
      this.BTN_Aceptar.BackColor = Color.White;
      this.BTN_Aceptar.FlatStyle = FlatStyle.Flat;
      this.BTN_Aceptar.Location = new Point(291, 113);
      this.BTN_Aceptar.Margin = new Padding(4);
      this.BTN_Aceptar.Name = "BTN_Aceptar";
      this.BTN_Aceptar.Size = new Size(82, 28);
      this.BTN_Aceptar.TabIndex = 5;
      this.BTN_Aceptar.Text = "Aceptar";
      this.BTN_Aceptar.UseVisualStyleBackColor = false;
      this.BTN_Aceptar.Click += new EventHandler(this.BTN_Aceptar_Click);
      this.BTN_Cancelar.BackColor = Color.DimGray;
      this.BTN_Cancelar.DialogResult = DialogResult.Cancel;
      this.BTN_Cancelar.FlatStyle = FlatStyle.Flat;
      this.BTN_Cancelar.Location = new Point(381, 113);
      this.BTN_Cancelar.Margin = new Padding(4);
      this.BTN_Cancelar.Name = "BTN_Cancelar";
      this.BTN_Cancelar.Size = new Size(82, 28);
      this.BTN_Cancelar.TabIndex = 6;
      this.BTN_Cancelar.Text = "Cancelar";
      this.BTN_Cancelar.UseVisualStyleBackColor = false;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources.FondoDeAplicacionDeVentas;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.CancelButton = (IButtonControl) this.BTN_Cancelar;
      this.ClientSize = new Size(477, 153);
      this.Controls.Add((Control) this.BTN_Cancelar);
      this.Controls.Add((Control) this.BTN_Aceptar);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.TxBox_Contrasena);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.TxBox_Usuario);
      this.Controls.Add((Control) this.pictureBox1);
      this.DoubleBuffered = true;
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = nameof (Login);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Iniciar Sesión";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
