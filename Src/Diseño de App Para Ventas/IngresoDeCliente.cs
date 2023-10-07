// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.IngresoDeCliente
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  public class IngresoDeCliente : Form
  {
    public OleDbConnection Conn;
    private IContainer components;
    private Panel panel1;
    private Button BTN_Aceptar;
    private Button BTN_Cancelar;
    private Label LabelIngresoDeCliente;
    private TextBox TBX_Buscar;
    private Button BotonBuscar;
    private DataGridViewTextBoxColumn ColNombre;
    private DataGridViewTextBoxColumn ColID;
    public DataGridView ListaVerClientes;

    public IngresoDeCliente()
    {
      this.InitializeComponent();
      this.ListaVerClientes.CellDoubleClick += new DataGridViewCellEventHandler(this.ListaVerClientes_CellDoubleClick);
    }

    private void ListaVerClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void BuscarEnClientes(string Filtro)
    {
      this.ListaVerClientes.Rows.Clear();
      OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT NombreDeCliente, IDDeCliente FROM Clientes WHERE NombreDeCliente LIKE '%" + this.TBX_Buscar.Text + "%' OR IDDeCliente = '" + this.TBX_Buscar.Text + "';", this.Conn).ExecuteReader();
      while (oleDbDataReader.Read())
        this.ListaVerClientes.Rows.Add((object) oleDbDataReader.GetValue(0).ToString(), (object) oleDbDataReader.GetValue(1).ToString());
      if (this.ListaVerClientes.Rows.Count > 0)
        this.ListaVerClientes.Rows[0].Selected = true;
      this.ListaVerClientes.Rows.Add((object) "Ninguno", (object) "Ninguno");
    }

    private void IngresoDeCliente_Load(object sender, EventArgs e) => this.BuscarEnClientes("");

    private void BotonBuscar_Click(object sender, EventArgs e) => this.BuscarEnClientes(this.TBX_Buscar.Text);

    private void TBX_Buscar_TextChanged(object sender, EventArgs e)
    {
      if (((IEnumerable<string>) this.TBX_Buscar.Lines).Count<string>() <= 1)
        return;
      this.TBX_Buscar.Text = this.TBX_Buscar.Text.Replace(Environment.NewLine, "");
      this.BuscarEnClientes(this.TBX_Buscar.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IngresoDeCliente));
      this.panel1 = new Panel();
      this.BTN_Aceptar = new Button();
      this.BTN_Cancelar = new Button();
      this.LabelIngresoDeCliente = new Label();
      this.TBX_Buscar = new TextBox();
      this.ListaVerClientes = new DataGridView();
      this.ColNombre = new DataGridViewTextBoxColumn();
      this.ColID = new DataGridViewTextBoxColumn();
      this.BotonBuscar = new Button();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.ListaVerClientes).BeginInit();
      this.SuspendLayout();
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.BTN_Aceptar);
      this.panel1.Controls.Add((Control) this.BTN_Cancelar);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(10, 213);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(235, 28);
      this.panel1.TabIndex = 11;
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
      this.LabelIngresoDeCliente.AutoSize = true;
      this.LabelIngresoDeCliente.Dock = DockStyle.Top;
      this.LabelIngresoDeCliente.Location = new Point(10, 10);
      this.LabelIngresoDeCliente.Name = "LabelIngresoDeCliente";
      this.LabelIngresoDeCliente.Size = new Size(223, 17);
      this.LabelIngresoDeCliente.TabIndex = 12;
      this.LabelIngresoDeCliente.Text = "Ingrese el nombre o ID del cliente:";
      this.TBX_Buscar.Font = new Font("Microsoft Sans Serif", 12f);
      this.TBX_Buscar.Location = new Point(13, 32);
      this.TBX_Buscar.Name = "TBX_Buscar";
      this.TBX_Buscar.Size = new Size(143, 26);
      this.TBX_Buscar.TabIndex = 13;
      this.TBX_Buscar.TextChanged += new EventHandler(this.TBX_Buscar_TextChanged);
      this.ListaVerClientes.AllowUserToAddRows = false;
      this.ListaVerClientes.AllowUserToDeleteRows = false;
      this.ListaVerClientes.AllowUserToResizeRows = false;
      this.ListaVerClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ListaVerClientes.Columns.AddRange((DataGridViewColumn) this.ColNombre, (DataGridViewColumn) this.ColID);
      this.ListaVerClientes.Location = new Point(13, 67);
      this.ListaVerClientes.MultiSelect = false;
      this.ListaVerClientes.Name = "ListaVerClientes";
      this.ListaVerClientes.ReadOnly = true;
      this.ListaVerClientes.RowHeadersVisible = false;
      this.ListaVerClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.ListaVerClientes.Size = new Size(232, 139);
      this.ListaVerClientes.TabIndex = 15;
      this.ColNombre.HeaderText = "Nombre";
      this.ColNombre.Name = "ColNombre";
      this.ColNombre.ReadOnly = true;
      this.ColNombre.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.ColNombre.Width = 128;
      this.ColID.HeaderText = "ID";
      this.ColID.Name = "ColID";
      this.ColID.ReadOnly = true;
      this.ColID.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.BotonBuscar.BackColor = Color.White;
      this.BotonBuscar.FlatStyle = FlatStyle.Flat;
      this.BotonBuscar.Location = new Point(163, 32);
      this.BotonBuscar.Margin = new Padding(4);
      this.BotonBuscar.Name = "BotonBuscar";
      this.BotonBuscar.Size = new Size(82, 28);
      this.BotonBuscar.TabIndex = 16;
      this.BotonBuscar.Text = "Buscar";
      this.BotonBuscar.UseVisualStyleBackColor = false;
      this.BotonBuscar.Click += new EventHandler(this.BotonBuscar_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Brown;
      this.ClientSize = new Size((int) byte.MaxValue, 251);
      this.Controls.Add((Control) this.BotonBuscar);
      this.Controls.Add((Control) this.ListaVerClientes);
      this.Controls.Add((Control) this.TBX_Buscar);
      this.Controls.Add((Control) this.LabelIngresoDeCliente);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.MaximizeBox = false;
      this.Name = nameof (IngresoDeCliente);
      this.Padding = new Padding(10);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Ingresar Cliente:";
      this.Load += new EventHandler(this.IngresoDeCliente_Load);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.ListaVerClientes).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
