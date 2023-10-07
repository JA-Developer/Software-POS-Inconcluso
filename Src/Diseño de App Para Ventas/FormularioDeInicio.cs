// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.FormularioDeInicio
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using Diseño_de_App_Para_Ventas.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class FormularioDeInicio : Form
  {
    private string Usuario = "";
    private double Version = 1.102;
    public OleDbConnection Conn;
    public Punto_De_Ventas POS = new Punto_De_Ventas();
    private IContainer components;
    private Panel panel2;
    private Panel panel1;
    private Panel panel3;
    private Panel panel17;
    private ListView Recordatorios;
    private Panel panel14;
    private Panel panel15;
    private Panel panel16;
    private Label label2;
    private Button button2;
    private Panel panel5;
    private FlowLayoutPanel ContenedorDeMenu;
    private Button BTN_PuntoDeVentas;
    private Button BTN_Inventario;
    private Button BTN_Ventas;
    private Button BTN_Gastos;
    private Button BTN_Clientes;
    private Button BTN_Proveedores;
    private Button BTN_Usuarios;
    private Button button20;
    private Button button21;
    private Button button22;
    private Panel panel4;
    private Panel panel11;
    private Panel panel13;
    private Label label1;
    private Button button1;
    private Button BtnSalir;
    private Label LabelAgradecimientos;

    public FormularioDeInicio()
    {
      this.InitializeComponent();
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.ContenedorDeMenu.SizeChanged += new EventHandler(this.ContenedorDeMenu_SizeChanged);
      this.FormClosing += new FormClosingEventHandler(this.FormularioDeInicio_FormClosing);
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.Recordatorios.MouseDoubleClick += new MouseEventHandler(this.Recordatorios_MouseDoubleClick);
    }

    private void Recordatorios_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      ListViewItem itemAt = this.Recordatorios.GetItemAt(e.X, e.Y);
      if (itemAt == null || itemAt.Tag == null || !(itemAt.Tag.ToString() == "#Update"))
        return;
      if (System.IO.File.Exists("Updater.exe"))
      {
        new Process()
        {
          StartInfo = new ProcessStartInfo("Updater.exe")
          {
            Arguments = ("Version_" + this.Version.ToString((IFormatProvider) new CultureInfo("en-EN")).Replace(".", "_"))
          }
        }.Start();
        this.Close();
      }
      else
      {
        int num = (int) MessageBox.Show("Parece que esta característica no esta instalada en su versión del software. Porfavor, trate reinstalando el programa.", "No se encontró el asistente de actualizaciones.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void FormularioDeInicio_FormClosing(object sender, FormClosingEventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      OleDbCommand oleDbCommand = new OleDbCommand("DELETE FROM CarritosDeVentas;", this.Conn);
      oleDbCommand.ExecuteNonQuery();
      oleDbCommand.CommandText = "DELETE FROM IdsDeCarrito;";
      oleDbCommand.ExecuteNonQuery();
      Cursor.Current = Cursors.Arrow;
    }

    private void ContenedorDeMenu_SizeChanged(object sender, EventArgs e) => this.panel5.Height = this.ContenedorDeMenu.Height + 20;

    private void BTN_PuntoDeVentas_Click(object sender, EventArgs e)
    {
      this.POS.TxBox_Vendedor.Text = this.Usuario;
      this.POS.Conn = this.Conn;
      int num = (int) this.POS.ShowDialog();
    }

    private void BTN_Inventario_Click(object sender, EventArgs e)
    {
      bool flag = true;
      for (int index = 0; index < this.POS.Carritos.Count; ++index)
      {
        if (this.POS.Carritos[index].Codigos.Count > 0)
          flag = false;
      }
      Inventario inventario = new Inventario();
      DialogResult dialogResult = DialogResult.Yes;
      if (!flag)
      {
        dialogResult = MessageBox.Show("Hay una venta en proceso, por lo que los elementos de esta tabla no pueden ser modificados. ¿Desea abrirla en modo solo lectura?", "No se puede modificar esta tabla", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (dialogResult == DialogResult.Yes)
          inventario.ReadOnly = true;
      }
      if (dialogResult != DialogResult.Yes)
        return;
      inventario.Conn = this.Conn;
      int num = (int) inventario.ShowDialog();
    }

    private void FormularioDeInicio_Load(object sender, EventArgs e)
    {
      try
      {
        WebClient webClient = new WebClient();
        string tempFileName;
        string fileName = tempFileName = Path.GetTempFileName();
        string address = "https://drive.google.com/uc?id=1GmpZ8hssS0ep5KovU0biVmwHvY86XrfK&export=download&authuser=0";
        webClient.DownloadFile(address, fileName);
        List<string> list = System.IO.File.ReadLines(tempFileName).ToList<string>();
        bool flag = false;
        for (int index = 0; index < list.Count; ++index)
        {
          double result = 0.0;
          if (!(list[index] == ""))
          {
            if (double.TryParse(list[index], out result))
            {
              if (result > this.Version)
              {
                flag = true;
                break;
              }
            }
            else
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
        {
          this.Recordatorios.Items.Add("Hay una actualización nueva disponible...");
          this.Recordatorios.Items[this.Recordatorios.Items.Count - 1].Tag = (object) "#Update";
        }
      }
      catch (Exception ex)
      {
      }
      if (((IEnumerable<Process>) Process.GetProcessesByName("Diseño de App Para Ventas")).Count<Process>() > 1)
      {
        int num = (int) MessageBox.Show("Ya hay otra copia de esta aplicación ejecutandose.");
        this.Close();
      }
      else
      {
        this.Conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=InventarioAZ.mdb");
        this.Conn.Open();
        Login login = new Login();
        string str1;
        string str2;
        while (true)
        {
          login.TxBox_Contrasena.Text = "";
          if (login.ShowDialog() == DialogResult.OK)
          {
            OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT Usuarios.Usuario, Usuarios.TipoDeCuenta FROM Usuarios WHERE Usuarios.Usuario = '" + login.TxBox_Usuario.Text + "' AND Usuarios.Clave = '" + login.TxBox_Contrasena.Text + "';", this.Conn).ExecuteReader();
            str1 = "";
            str2 = "0";
            while (oleDbDataReader.Read())
            {
              str1 = oleDbDataReader.GetValue(0).ToString();
              str2 = oleDbDataReader.GetValue(1).ToString();
            }
            if (!(str1 != ""))
            {
              int num = (int) MessageBox.Show("Su Usuario y/o Contrasena son incorrectos. Por favor, revise su ortografía.", "Credenciales incorrectas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
              break;
          }
          else
            goto label_24;
        }
        this.Usuario = str1.ToString();
        Cursor.Current = Cursors.WaitCursor;
        OleDbCommand oleDbCommand = new OleDbCommand("DELETE FROM CarritosDeVentas;", this.Conn);
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DELETE FROM IdsDeCarrito;";
        oleDbCommand.ExecuteNonQuery();
        if (str2 == "0")
        {
          this.BTN_Inventario.Enabled = false;
          this.BTN_Ventas.Enabled = false;
          this.BTN_Gastos.Enabled = false;
          this.BTN_Proveedores.Enabled = false;
          this.BTN_Usuarios.Enabled = false;
        }
        else
        {
          this.BTN_Inventario.Enabled = true;
          this.BTN_Ventas.Enabled = true;
          this.BTN_Gastos.Enabled = true;
          this.BTN_Proveedores.Enabled = true;
          this.BTN_Usuarios.Enabled = true;
        }
        Cursor.Current = Cursors.Arrow;
        return;
label_24:
        this.Close();
      }
    }

    private void BTN_Proveedores_Click(object sender, EventArgs e)
    {
      int num = (int) new Proveedores() { Conn = this.Conn }.ShowDialog();
    }

    private void BTN_Clientes_Click(object sender, EventArgs e)
    {
      int num = (int) new Clientes()
      {
        ParentForm = this,
        Conn = this.Conn
      }.ShowDialog();
    }

    private void BTN_Gastos_Click(object sender, EventArgs e)
    {
      try
      {
        int num = (int) new Gastos() { Conn = this.Conn }.ShowDialog();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void BTN_Empleados_Click(object sender, EventArgs e)
    {
      bool flag = true;
      for (int index = 0; index < this.POS.Carritos.Count; ++index)
      {
        if (this.POS.Carritos[index].Codigos.Count > 0)
          flag = false;
      }
      Usuarios usuarios = new Usuarios();
      DialogResult dialogResult = DialogResult.Yes;
      if (!flag)
      {
        dialogResult = MessageBox.Show("Hay una venta en proceso, por lo que los elementos de esta tabla no pueden ser modificados. ¿Desea abrirla en modo solo lectura?", "No se puede modificar esta tabla", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (dialogResult == DialogResult.Yes)
          usuarios.ReadOnly = true;
      }
      if (dialogResult == DialogResult.Yes)
      {
        usuarios.Conn = this.Conn;
        int num = (int) usuarios.ShowDialog();
      }
      if (usuarios.ReadOnly)
        return;
      Login login = new Login();
      string str1;
      string str2;
      while (true)
      {
        login.TxBox_Contrasena.Text = "";
        if (login.ShowDialog() == DialogResult.OK)
        {
          OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT Usuarios.Usuario, Usuarios.TipoDeCuenta FROM Usuarios WHERE Usuarios.Usuario = '" + login.TxBox_Usuario.Text + "' AND Usuarios.Clave = '" + login.TxBox_Contrasena.Text + "';", this.Conn).ExecuteReader();
          str1 = "";
          str2 = "0";
          while (oleDbDataReader.Read())
          {
            str1 = oleDbDataReader.GetValue(0).ToString();
            str2 = oleDbDataReader.GetValue(1).ToString();
          }
          if (!(str1 != ""))
          {
            int num = (int) MessageBox.Show("Su Usuario y/o Contrasena son incorrectos. Por favor, revise su ortografía.", "Credenciales incorrectas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
            break;
        }
        else
          goto label_22;
      }
      this.Usuario = str1.ToString();
      Cursor.Current = Cursors.WaitCursor;
      OleDbCommand oleDbCommand = new OleDbCommand("DELETE FROM CarritosDeVentas;", this.Conn);
      oleDbCommand.ExecuteNonQuery();
      oleDbCommand.CommandText = "DELETE FROM IdsDeCarrito;";
      oleDbCommand.ExecuteNonQuery();
      if (str2 == "0")
      {
        this.BTN_Inventario.Enabled = false;
        this.BTN_Ventas.Enabled = false;
        this.BTN_Gastos.Enabled = false;
        this.BTN_Proveedores.Enabled = false;
        this.BTN_Usuarios.Enabled = false;
      }
      else
      {
        this.BTN_Inventario.Enabled = true;
        this.BTN_Ventas.Enabled = true;
        this.BTN_Gastos.Enabled = true;
        this.BTN_Proveedores.Enabled = true;
        this.BTN_Usuarios.Enabled = true;
      }
      Cursor.Current = Cursors.Arrow;
      return;
label_22:
      this.Close();
    }

    private void BTN_Ventas_Click(object sender, EventArgs e)
    {
      int num = (int) new Ventas() { Conn = this.Conn }.ShowDialog();
    }

    private void BtnSalir_Click(object sender, EventArgs e)
    {
      Login login = new Login();
      string str1;
      string str2;
      while (true)
      {
        login.TxBox_Contrasena.Text = "";
        if (login.ShowDialog() == DialogResult.OK)
        {
          OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT Usuarios.Usuario, Usuarios.TipoDeCuenta FROM Usuarios WHERE Usuarios.Usuario = '" + login.TxBox_Usuario.Text + "' AND Usuarios.Clave = '" + login.TxBox_Contrasena.Text + "';", this.Conn).ExecuteReader();
          str1 = "";
          str2 = "0";
          while (oleDbDataReader.Read())
          {
            str1 = oleDbDataReader.GetValue(0).ToString();
            str2 = oleDbDataReader.GetValue(1).ToString();
          }
          if (!(str1 != ""))
          {
            int num = (int) MessageBox.Show("Su Usuario y/o Contrasena son incorrectos. Por favor, revise su ortografía.", "Credenciales incorrectas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
            break;
        }
        else
          goto label_11;
      }
      this.Usuario = str1.ToString();
      Cursor.Current = Cursors.WaitCursor;
      OleDbCommand oleDbCommand = new OleDbCommand("DELETE FROM CarritosDeVentas;", this.Conn);
      oleDbCommand.ExecuteNonQuery();
      oleDbCommand.CommandText = "DELETE FROM IdsDeCarrito;";
      oleDbCommand.ExecuteNonQuery();
      if (str2 == "0")
      {
        this.BTN_Inventario.Enabled = false;
        this.BTN_Ventas.Enabled = false;
        this.BTN_Gastos.Enabled = false;
        this.BTN_Proveedores.Enabled = false;
        this.BTN_Usuarios.Enabled = false;
      }
      else
      {
        this.BTN_Inventario.Enabled = true;
        this.BTN_Ventas.Enabled = true;
        this.BTN_Gastos.Enabled = true;
        this.BTN_Proveedores.Enabled = true;
        this.BTN_Usuarios.Enabled = true;
      }
      Cursor.Current = Cursors.Arrow;
      return;
label_11:
      this.Close();
    }

    private void Recordatorios_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormularioDeInicio));
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.BtnSalir = new Button();
      this.panel3 = new Panel();
      this.panel17 = new Panel();
      this.Recordatorios = new ListView();
      this.panel14 = new Panel();
      this.panel15 = new Panel();
      this.panel16 = new Panel();
      this.label2 = new Label();
      this.button2 = new Button();
      this.panel5 = new Panel();
      this.ContenedorDeMenu = new FlowLayoutPanel();
      this.BTN_PuntoDeVentas = new Button();
      this.BTN_Inventario = new Button();
      this.BTN_Ventas = new Button();
      this.BTN_Gastos = new Button();
      this.BTN_Clientes = new Button();
      this.BTN_Proveedores = new Button();
      this.BTN_Usuarios = new Button();
      this.button20 = new Button();
      this.button21 = new Button();
      this.button22 = new Button();
      this.panel4 = new Panel();
      this.panel11 = new Panel();
      this.panel13 = new Panel();
      this.label1 = new Label();
      this.button1 = new Button();
      this.LabelAgradecimientos = new Label();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel17.SuspendLayout();
      this.panel14.SuspendLayout();
      this.panel15.SuspendLayout();
      this.panel16.SuspendLayout();
      this.panel5.SuspendLayout();
      this.ContenedorDeMenu.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel11.SuspendLayout();
      this.panel13.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.Brown;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.LabelAgradecimientos);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 437);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(784, 25);
      this.panel2.TabIndex = 1;
      this.panel1.BackColor = Color.Brown;
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.BtnSalir);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(784, 100);
      this.panel1.TabIndex = 0;
      this.BtnSalir.BackColor = Color.Brown;
      this.BtnSalir.BackgroundImage = (Image) Resources.salir;
      this.BtnSalir.BackgroundImageLayout = ImageLayout.Stretch;
      this.BtnSalir.FlatAppearance.BorderColor = Color.Brown;
      this.BtnSalir.FlatAppearance.MouseOverBackColor = Color.IndianRed;
      this.BtnSalir.FlatStyle = FlatStyle.Flat;
      this.BtnSalir.Location = new Point(11, 11);
      this.BtnSalir.Name = "BtnSalir";
      this.BtnSalir.Size = new Size(82, 82);
      this.BtnSalir.TabIndex = 2;
      this.BtnSalir.UseVisualStyleBackColor = false;
      this.BtnSalir.Click += new EventHandler(this.BtnSalir_Click);
      this.panel3.AutoScroll = true;
      this.panel3.BackColor = Color.Transparent;
      this.panel3.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel3.Controls.Add((Control) this.panel17);
      this.panel3.Controls.Add((Control) this.panel14);
      this.panel3.Controls.Add((Control) this.panel5);
      this.panel3.Controls.Add((Control) this.panel4);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 100);
      this.panel3.Name = "panel3";
      this.panel3.RightToLeft = RightToLeft.No;
      this.panel3.Size = new Size(784, 337);
      this.panel3.TabIndex = 2;
      this.panel17.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panel17.Controls.Add((Control) this.Recordatorios);
      this.panel17.Dock = DockStyle.Top;
      this.panel17.Location = new Point(0, 561);
      this.panel17.Name = "panel17";
      this.panel17.Padding = new Padding(50, 10, 10, 10);
      this.panel17.Size = new Size(767, 100);
      this.panel17.TabIndex = 3;
      this.Recordatorios.BorderStyle = BorderStyle.FixedSingle;
      this.Recordatorios.Cursor = Cursors.Hand;
      this.Recordatorios.Dock = DockStyle.Fill;
      this.Recordatorios.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Recordatorios.FullRowSelect = true;
      this.Recordatorios.GridLines = true;
      this.Recordatorios.Location = new Point(50, 10);
      this.Recordatorios.MultiSelect = false;
      this.Recordatorios.Name = "Recordatorios";
      this.Recordatorios.Size = new Size(707, 80);
      this.Recordatorios.TabIndex = 0;
      this.Recordatorios.UseCompatibleStateImageBehavior = false;
      this.Recordatorios.View = View.List;
      this.Recordatorios.SelectedIndexChanged += new EventHandler(this.Recordatorios_SelectedIndexChanged);
      this.panel14.Controls.Add((Control) this.panel15);
      this.panel14.Dock = DockStyle.Top;
      this.panel14.Location = new Point(0, 511);
      this.panel14.Margin = new Padding(100, 3, 3, 3);
      this.panel14.Name = "panel14";
      this.panel14.Padding = new Padding(3);
      this.panel14.Size = new Size(767, 50);
      this.panel14.TabIndex = 2;
      this.panel15.BackColor = SystemColors.ScrollBar;
      this.panel15.BorderStyle = BorderStyle.FixedSingle;
      this.panel15.Controls.Add((Control) this.panel16);
      this.panel15.Controls.Add((Control) this.button2);
      this.panel15.Dock = DockStyle.Fill;
      this.panel15.Location = new Point(3, 3);
      this.panel15.Name = "panel15";
      this.panel15.Size = new Size(761, 44);
      this.panel15.TabIndex = 0;
      this.panel16.BackgroundImage = (Image) Resources.ImgIndexItem_11;
      this.panel16.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel16.Controls.Add((Control) this.label2);
      this.panel16.Dock = DockStyle.Fill;
      this.panel16.Location = new Point(43, 0);
      this.panel16.Name = "panel16";
      this.panel16.Padding = new Padding(0, 6, 0, 0);
      this.panel16.Size = new Size(716, 42);
      this.panel16.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Dock = DockStyle.Left;
      this.label2.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(0, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(147, 26);
      this.label2.TabIndex = 2;
      this.label2.Text = "Recordatorios";
      this.button2.BackColor = Color.DimGray;
      this.button2.BackgroundImage = (Image) componentResourceManager.GetObject("button2.BackgroundImage");
      this.button2.BackgroundImageLayout = ImageLayout.Stretch;
      this.button2.Dock = DockStyle.Left;
      this.button2.FlatAppearance.BorderSize = 0;
      this.button2.FlatStyle = FlatStyle.Flat;
      this.button2.Location = new Point(0, 0);
      this.button2.Name = "button2";
      this.button2.Size = new Size(43, 42);
      this.button2.TabIndex = 2;
      this.button2.UseVisualStyleBackColor = false;
      this.panel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panel5.BackColor = Color.Transparent;
      this.panel5.Controls.Add((Control) this.ContenedorDeMenu);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 50);
      this.panel5.Name = "panel5";
      this.panel5.Padding = new Padding(50, 10, 10, 10);
      this.panel5.Size = new Size(767, 461);
      this.panel5.TabIndex = 1;
      this.ContenedorDeMenu.AutoSize = true;
      this.ContenedorDeMenu.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ContenedorDeMenu.BackColor = Color.Gray;
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_PuntoDeVentas);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Inventario);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Ventas);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Gastos);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Clientes);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Proveedores);
      this.ContenedorDeMenu.Controls.Add((Control) this.BTN_Usuarios);
      this.ContenedorDeMenu.Controls.Add((Control) this.button20);
      this.ContenedorDeMenu.Controls.Add((Control) this.button21);
      this.ContenedorDeMenu.Controls.Add((Control) this.button22);
      this.ContenedorDeMenu.Dock = DockStyle.Top;
      this.ContenedorDeMenu.Location = new Point(50, 10);
      this.ContenedorDeMenu.Name = "ContenedorDeMenu";
      this.ContenedorDeMenu.RightToLeft = RightToLeft.No;
      this.ContenedorDeMenu.Size = new Size(707, 424);
      this.ContenedorDeMenu.TabIndex = 3;
      this.BTN_PuntoDeVentas.BackColor = Color.White;
      this.BTN_PuntoDeVentas.BackgroundImage = (Image) Resources.ImgPuntoDeVentas;
      this.BTN_PuntoDeVentas.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_PuntoDeVentas.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_PuntoDeVentas.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_PuntoDeVentas.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_PuntoDeVentas.FlatStyle = FlatStyle.Flat;
      this.BTN_PuntoDeVentas.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_PuntoDeVentas.Location = new Point(3, 3);
      this.BTN_PuntoDeVentas.Name = "BTN_PuntoDeVentas";
      this.BTN_PuntoDeVentas.Size = new Size(200, 100);
      this.BTN_PuntoDeVentas.TabIndex = 1;
      this.BTN_PuntoDeVentas.Text = "\r\nPunto de ventas";
      this.BTN_PuntoDeVentas.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_PuntoDeVentas.UseVisualStyleBackColor = false;
      this.BTN_PuntoDeVentas.Click += new EventHandler(this.BTN_PuntoDeVentas_Click);
      this.BTN_Inventario.BackColor = Color.White;
      this.BTN_Inventario.BackgroundImage = (Image) Resources.ImgInventario;
      this.BTN_Inventario.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Inventario.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Inventario.FlatAppearance.MouseDownBackColor = Color.Orange;
      this.BTN_Inventario.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Inventario.FlatStyle = FlatStyle.Flat;
      this.BTN_Inventario.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Inventario.Location = new Point(209, 3);
      this.BTN_Inventario.Name = "BTN_Inventario";
      this.BTN_Inventario.Size = new Size(200, 100);
      this.BTN_Inventario.TabIndex = 2;
      this.BTN_Inventario.Text = "Inventario";
      this.BTN_Inventario.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Inventario.UseVisualStyleBackColor = false;
      this.BTN_Inventario.Click += new EventHandler(this.BTN_Inventario_Click);
      this.BTN_Ventas.BackColor = Color.White;
      this.BTN_Ventas.BackgroundImage = (Image) Resources.ImgVentas;
      this.BTN_Ventas.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Ventas.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Ventas.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_Ventas.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Ventas.FlatStyle = FlatStyle.Flat;
      this.BTN_Ventas.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Ventas.Location = new Point(415, 3);
      this.BTN_Ventas.Name = "BTN_Ventas";
      this.BTN_Ventas.Size = new Size(200, 100);
      this.BTN_Ventas.TabIndex = 3;
      this.BTN_Ventas.Text = "Ventas";
      this.BTN_Ventas.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Ventas.UseVisualStyleBackColor = false;
      this.BTN_Ventas.Click += new EventHandler(this.BTN_Ventas_Click);
      this.BTN_Gastos.BackColor = Color.White;
      this.BTN_Gastos.BackgroundImage = (Image) Resources.ImgGastos;
      this.BTN_Gastos.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Gastos.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Gastos.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_Gastos.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Gastos.FlatStyle = FlatStyle.Flat;
      this.BTN_Gastos.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Gastos.Location = new Point(3, 109);
      this.BTN_Gastos.Name = "BTN_Gastos";
      this.BTN_Gastos.Size = new Size(200, 100);
      this.BTN_Gastos.TabIndex = 4;
      this.BTN_Gastos.Text = "Inversiones y Gastos";
      this.BTN_Gastos.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Gastos.UseVisualStyleBackColor = false;
      this.BTN_Gastos.Click += new EventHandler(this.BTN_Gastos_Click);
      this.BTN_Clientes.BackColor = Color.White;
      this.BTN_Clientes.BackgroundImage = (Image) Resources.ImgClientes;
      this.BTN_Clientes.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Clientes.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Clientes.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_Clientes.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Clientes.FlatStyle = FlatStyle.Flat;
      this.BTN_Clientes.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Clientes.Location = new Point(209, 109);
      this.BTN_Clientes.Name = "BTN_Clientes";
      this.BTN_Clientes.Size = new Size(200, 100);
      this.BTN_Clientes.TabIndex = 5;
      this.BTN_Clientes.Text = "Clientes";
      this.BTN_Clientes.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Clientes.UseVisualStyleBackColor = false;
      this.BTN_Clientes.Click += new EventHandler(this.BTN_Clientes_Click);
      this.BTN_Proveedores.BackColor = Color.White;
      this.BTN_Proveedores.BackgroundImage = (Image) Resources.ImgProveedores;
      this.BTN_Proveedores.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Proveedores.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Proveedores.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_Proveedores.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Proveedores.FlatStyle = FlatStyle.Flat;
      this.BTN_Proveedores.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Proveedores.Location = new Point(415, 109);
      this.BTN_Proveedores.Name = "BTN_Proveedores";
      this.BTN_Proveedores.Size = new Size(200, 100);
      this.BTN_Proveedores.TabIndex = 6;
      this.BTN_Proveedores.Text = "Proveedores";
      this.BTN_Proveedores.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Proveedores.UseVisualStyleBackColor = false;
      this.BTN_Proveedores.Click += new EventHandler(this.BTN_Proveedores_Click);
      this.BTN_Usuarios.BackColor = Color.White;
      this.BTN_Usuarios.BackgroundImage = (Image) Resources.ImgEmpleados;
      this.BTN_Usuarios.BackgroundImageLayout = ImageLayout.Stretch;
      this.BTN_Usuarios.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.BTN_Usuarios.FlatAppearance.MouseDownBackColor = Color.White;
      this.BTN_Usuarios.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.BTN_Usuarios.FlatStyle = FlatStyle.Flat;
      this.BTN_Usuarios.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.BTN_Usuarios.Location = new Point(3, 215);
      this.BTN_Usuarios.Name = "BTN_Usuarios";
      this.BTN_Usuarios.Size = new Size(200, 100);
      this.BTN_Usuarios.TabIndex = 7;
      this.BTN_Usuarios.Text = "Usuarios";
      this.BTN_Usuarios.TextAlign = ContentAlignment.BottomCenter;
      this.BTN_Usuarios.UseVisualStyleBackColor = false;
      this.BTN_Usuarios.Click += new EventHandler(this.BTN_Empleados_Click);
      this.button20.BackColor = Color.White;
      this.button20.BackgroundImage = (Image) Resources.ImgEstadísticas;
      this.button20.BackgroundImageLayout = ImageLayout.Stretch;
      this.button20.Enabled = false;
      this.button20.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.button20.FlatAppearance.MouseDownBackColor = Color.White;
      this.button20.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.button20.FlatStyle = FlatStyle.Flat;
      this.button20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.button20.Location = new Point(209, 215);
      this.button20.Name = "button20";
      this.button20.Size = new Size(200, 100);
      this.button20.TabIndex = 8;
      this.button20.Text = "Estadísticas (Proximamente)";
      this.button20.TextAlign = ContentAlignment.BottomCenter;
      this.button20.UseVisualStyleBackColor = false;
      this.button21.BackColor = Color.White;
      this.button21.BackgroundImage = (Image) Resources.ImgAjustes;
      this.button21.BackgroundImageLayout = ImageLayout.Stretch;
      this.button21.Enabled = false;
      this.button21.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.button21.FlatAppearance.MouseDownBackColor = Color.White;
      this.button21.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.button21.FlatStyle = FlatStyle.Flat;
      this.button21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.button21.Location = new Point(415, 215);
      this.button21.Name = "button21";
      this.button21.Size = new Size(200, 100);
      this.button21.TabIndex = 9;
      this.button21.Text = "Ajustes (Proximamente)";
      this.button21.TextAlign = ContentAlignment.BottomCenter;
      this.button21.UseVisualStyleBackColor = false;
      this.button22.BackColor = Color.White;
      this.button22.BackgroundImage = (Image) Resources.ImgAyuda;
      this.button22.BackgroundImageLayout = ImageLayout.Stretch;
      this.button22.Enabled = false;
      this.button22.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
      this.button22.FlatAppearance.MouseDownBackColor = Color.White;
      this.button22.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
      this.button22.FlatStyle = FlatStyle.Flat;
      this.button22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.button22.Location = new Point(3, 321);
      this.button22.Name = "button22";
      this.button22.Size = new Size(200, 100);
      this.button22.TabIndex = 10;
      this.button22.Text = "Ayuda (Proximamente)";
      this.button22.TextAlign = ContentAlignment.BottomCenter;
      this.button22.UseVisualStyleBackColor = false;
      this.panel4.Controls.Add((Control) this.panel11);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Margin = new Padding(100, 3, 3, 3);
      this.panel4.Name = "panel4";
      this.panel4.Padding = new Padding(3);
      this.panel4.Size = new Size(767, 50);
      this.panel4.TabIndex = 0;
      this.panel11.BackColor = SystemColors.ScrollBar;
      this.panel11.BorderStyle = BorderStyle.FixedSingle;
      this.panel11.Controls.Add((Control) this.panel13);
      this.panel11.Controls.Add((Control) this.button1);
      this.panel11.Dock = DockStyle.Fill;
      this.panel11.Location = new Point(3, 3);
      this.panel11.Name = "panel11";
      this.panel11.Size = new Size(761, 44);
      this.panel11.TabIndex = 0;
      this.panel13.BackgroundImage = (Image) Resources.ImgIndexItem_11;
      this.panel13.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel13.Controls.Add((Control) this.label1);
      this.panel13.Dock = DockStyle.Fill;
      this.panel13.Location = new Point(43, 0);
      this.panel13.Name = "panel13";
      this.panel13.Padding = new Padding(0, 6, 0, 0);
      this.panel13.Size = new Size(716, 42);
      this.panel13.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Dock = DockStyle.Left;
      this.label1.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(0, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(169, 26);
      this.label1.TabIndex = 2;
      this.label1.Text = "Panel de control";
      this.button1.BackColor = Color.DimGray;
      this.button1.BackgroundImage = (Image) componentResourceManager.GetObject("button1.BackgroundImage");
      this.button1.BackgroundImageLayout = ImageLayout.Stretch;
      this.button1.Dock = DockStyle.Left;
      this.button1.FlatAppearance.BorderSize = 0;
      this.button1.FlatStyle = FlatStyle.Flat;
      this.button1.Location = new Point(0, 0);
      this.button1.Name = "button1";
      this.button1.Size = new Size(43, 42);
      this.button1.TabIndex = 2;
      this.button1.UseVisualStyleBackColor = false;
      this.LabelAgradecimientos.AutoSize = true;
      this.LabelAgradecimientos.Dock = DockStyle.Right;
      this.LabelAgradecimientos.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.LabelAgradecimientos.ForeColor = Color.White;
      this.LabelAgradecimientos.Location = new Point(-37, 0);
      this.LabelAgradecimientos.Name = "LabelAgradecimientos";
      this.LabelAgradecimientos.Size = new Size(819, 20);
      this.LabelAgradecimientos.TabIndex = 1;
      this.LabelAgradecimientos.Text = "Desarrollado por AyJ Company - Dirigido por Jesús Sanchez Palma, Anthomy Vega Fuentes y Oscar Josue Benitez";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Gray;
      this.ClientSize = new Size(784, 462);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.DoubleBuffered = true;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (FormularioDeInicio);
      this.Text = "Inicio";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.FormularioDeInicio_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel17.ResumeLayout(false);
      this.panel14.ResumeLayout(false);
      this.panel15.ResumeLayout(false);
      this.panel16.ResumeLayout(false);
      this.panel16.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.ContenedorDeMenu.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel11.ResumeLayout(false);
      this.panel13.ResumeLayout(false);
      this.panel13.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
