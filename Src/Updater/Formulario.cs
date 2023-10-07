// Decompiled with JetBrains decompiler
// Type: Updater.Formulario
// Assembly: Updater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2BEDE3E-6935-450F-ACEE-2CA0DF498A17
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Updater.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Updater
{
  public class Formulario : Form
  {
    private double Version = 999999999.0;
    private bool MustExit;
    private IContainer components;
    private ProgressBar BarraDeProgreso;
    private BackgroundWorker BackGroundWorkerForProgressBar;
    public Label EtiquetaEstado;

    public Formulario(double _Version)
    {
      this.Version = _Version;
      this.InitializeComponent();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.BackGroundWorkerForProgressBar.ProgressChanged += new ProgressChangedEventHandler(this.BackGroundWorkerForProgressBar_ProgressChanged);
      this.BackGroundWorkerForProgressBar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BackGroundWorkerForProgressBar_RunWorkerCompleted);
      this.FormClosing += new FormClosingEventHandler(this.Formulario_FormClosing);
    }

    private void BackGroundWorkerForProgressBar_RunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      this.Close();
    }

    private void BackGroundWorkerForProgressBar_ProgressChanged(
      object sender,
      ProgressChangedEventArgs e)
    {
      if (e.ProgressPercentage != -1)
        this.BarraDeProgreso.Value = e.ProgressPercentage;
      if (e.UserState == null)
        return;
      string s = e.UserState.ToString();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 140165252:
          if (!(s == "DownloadingError_R2"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de descargar un archivo, la operación sera reintentada en 2 segundos...";
          break;
        case 156942871:
          if (!(s == "DownloadingError_R3"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de descargar un archivo, la operación sera reintentada en 3 segundos...";
          break;
        case 190498109:
          if (!(s == "DownloadingError_R1"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de descargar un archivo, la operación sera reintentada en 1 segundos...";
          break;
        case 1715199460:
          if (!(s == "Closing"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Black;
          this.EtiquetaEstado.Text = "Terminando...";
          break;
        case 1903485527:
          if (!(s == "ConnectionError_R1"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de contactar con el servidor, la operación sera reintentada en 1 segundos...";
          break;
        case 1920263146:
          if (!(s == "ConnectionError_R2"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de contactar con el servidor, la operación sera reintentada en 2 segundos...";
          break;
        case 1937040765:
          if (!(s == "ConnectionError_R3"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Red;
          this.EtiquetaEstado.Text = "Ocurrió un error al tratar de contactar con el servidor, la operación sera reintentada en 3 segundos...";
          break;
        case 2365995591:
          if (!(s == "Downloading"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Black;
          this.EtiquetaEstado.Text = "Descargando paquetes...";
          break;
        case 3427671234:
          if (!(s == "Restoring"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Black;
          this.EtiquetaEstado.Text = "Restaurando archivos antiguos...";
          break;
        case 3480512692:
          if (!(s == "Getting_Info"))
            break;
          this.EtiquetaEstado.ForeColor = Color.Black;
          this.EtiquetaEstado.Text = "Obteniendo información necesaria sobre los paquetes...";
          break;
      }
    }

    private void Formulario_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.BackGroundWorkerForProgressBar.IsBusy)
        return;
      if (MessageBox.Show("¿Esta seguro de que desea cancelar la actualización?", "¿Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.MustExit = true;
      e.Cancel = true;
    }

    private bool SubProcesoDeRestaurarFolder(string Path)
    {
      try
      {
        List<string> list1 = Directory.EnumerateFiles(Path).ToList<string>();
        List<string> list2 = Directory.EnumerateDirectories(Path).ToList<string>();
        for (int index = 0; index < list1.Count; ++index)
        {
          if (list1[index].Count<char>() > 16 && list1[index].StartsWith("Temp\\OldFiles 1"))
          {
            string path = list1[index].Substring(16);
            if (Path.GetDirectoryName(path) != "" && !Directory.Exists(Path.GetDirectoryName(path)))
              Directory.CreateDirectory(Path.GetDirectoryName(path));
            Thread.Sleep(100);
            System.IO.File.WriteAllBytes(path, System.IO.File.ReadAllBytes(list1[index]));
          }
        }
        for (int index = 0; index < list2.Count; ++index)
        {
          if (list2[index].Count<char>() > 16 && list2[index].StartsWith("Temp\\OldFiles 1"))
            this.SubProcesoDeRestaurarFolder(list2[index]);
        }
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("La restauración falló: " + (object) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand));
        return false;
      }
    }

    private bool RestaurarFolder()
    {
      if (Directory.Exists("Temp"))
      {
        bool flag = !Directory.Exists("Temp\\OldFiles 1") || this.SubProcesoDeRestaurarFolder("Temp\\OldFiles 1");
        Directory.Delete("Temp", true);
        return flag;
      }
      int num = (int) MessageBox.Show("No se pudieron restaurar los archivos, la carpeta de respaldo fue borrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void Formulario_Load(object sender, EventArgs e)
    {
      bool flag = false;
      for (int index = 0; index < 10; ++index)
      {
        if (((IEnumerable<Process>) Process.GetProcessesByName("Diseño de App Para Ventas")).Count<Process>() >= 1)
        {
          Thread.Sleep(5000);
        }
        else
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        this.BackGroundWorkerForProgressBar.RunWorkerAsync();
      }
      else
      {
        int num = (int) MessageBox.Show("No se puede ejecutar la actualización porque hay una copia de el programa abierta.", "No se pudo completar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Close();
      }
    }

    private void BackGroundWorkerForProgressBar_DoWork(object sender, DoWorkEventArgs e)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      this.BackGroundWorkerForProgressBar.ReportProgress(0, (object) "Getting_Info");
      WebClient webClient;
      try
      {
        webClient = new WebClient();
      }
      catch (Exception ex)
      {
        goto label_108;
      }
      if (!this.MustExit && !e.Cancel)
      {
        string str1 = (string) null;
        bool flag1 = false;
        for (int index = 0; index < 4; ++index)
        {
          if (!this.MustExit)
          {
            if (!e.Cancel)
            {
              try
              {
                this.BackGroundWorkerForProgressBar.ReportProgress(25, (object) "Getting_Info");
                str1 = Path.GetTempFileName();
                webClient.DownloadFile("https://drive.google.com/uc?id=1GmpZ8hssS0ep5KovU0biVmwHvY86XrfK&export=download&authuser=0", str1);
                flag1 = true;
                break;
              }
              catch (Exception ex)
              {
                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R3");
                Thread.Sleep(1000);
                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R2");
                Thread.Sleep(1000);
                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R1");
                Thread.Sleep(1000);
              }
            }
            else
              goto label_108;
          }
          else
            goto label_108;
        }
        if (!flag1)
        {
          int num1 = (int) MessageBox.Show("No se pudo completar la actualización debido a un error en la conexión.", "Actualización fallida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (!this.MustExit && !e.Cancel)
        {
          List<string> list1 = System.IO.File.ReadLines(str1).ToList<string>();
          List<double> doubleList = new List<double>();
          this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "Getting_Info");
          for (int index = 0; index < list1.Count; ++index)
          {
            if (!this.MustExit && !e.Cancel)
            {
              double result = -1.0;
              if (!(list1[index] == ""))
              {
                if (double.TryParse(list1[index], out result))
                {
                  doubleList.Add(result);
                }
                else
                {
                  int num2 = (int) MessageBox.Show("El servidor respondió de forma inesperada.", "Error de servidor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  goto label_108;
                }
              }
            }
            else
              goto label_108;
          }
          doubleList.Sort();
          if (!this.MustExit && !e.Cancel)
          {
            string str2 = (string) null;
            bool flag2 = false;
            for (int index = 0; index < 4; ++index)
            {
              if (!this.MustExit)
              {
                if (!e.Cancel)
                {
                  try
                  {
                    this.BackGroundWorkerForProgressBar.ReportProgress(75, (object) "Getting_Info");
                    str2 = Path.GetTempFileName();
                    webClient.DownloadFile("https://drive.google.com/uc?id=1_FmtABOn1llB0oY9Fnc1acDZORWQUlN-&export=download&authuser=0", str2);
                    flag2 = true;
                    break;
                  }
                  catch (Exception ex)
                  {
                    this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R3");
                    Thread.Sleep(1000);
                    this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R2");
                    Thread.Sleep(1000);
                    this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R1");
                    Thread.Sleep(1000);
                  }
                }
                else
                  goto label_108;
              }
              else
                goto label_108;
            }
            if (!flag2)
            {
              int num3 = (int) MessageBox.Show("No se pudo completar la actualización debido a un error en la conexión.", "Actualización fallida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              this.BackGroundWorkerForProgressBar.ReportProgress(100, (object) "Getting_Info");
              if (!this.MustExit && !e.Cancel)
              {
                List<string> list2 = System.IO.File.ReadLines(str2).ToList<string>();
                for (int index = doubleList.Count - 1; index >= 0; --index)
                {
                  if (!this.MustExit && !e.Cancel)
                  {
                    if (doubleList[index] <= this.Version)
                      doubleList.RemoveAt(index);
                  }
                  else
                    goto label_108;
                }
                int num4 = 100 / doubleList.Count;
                if (Directory.Exists("Temp"))
                {
                  if (System.IO.File.Exists("Temp\\Finished"))
                  {
                    if (System.IO.File.ReadAllText("Temp\\Finished") == "Yes")
                    {
                      try
                      {
                        Directory.Delete("Temp", true);
                      }
                      catch (Exception ex)
                      {
                        int num5 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        goto label_108;
                      }
                    }
                    else
                    {
                      int num6 = (int) MessageBox.Show("Hemos detectado que hubo un error en la última actualización. Por lo que trataremos de restaurar los archivos anteriores a dicha actualización.", "Restauración de archivos anteriores", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                      goto label_107;
                    }
                  }
                  else
                  {
                    try
                    {
                      Directory.Delete("Temp", true);
                    }
                    catch (Exception ex)
                    {
                      int num7 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      goto label_108;
                    }
                  }
                }
                if (!this.MustExit && !e.Cancel)
                {
                  Directory.CreateDirectory("Temp");
                  new DirectoryInfo("Temp").Attributes = FileAttributes.Hidden;
                  try
                  {
                    System.IO.File.WriteAllText("Temp\\Finished", "No");
                  }
                  catch (Exception ex)
                  {
                    int num8 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    goto label_107;
                  }
                  int num9 = 1;
                  this.BackGroundWorkerForProgressBar.ReportProgress(0, (object) "Downloading");
                  for (int index1 = 0; index1 < doubleList.Count; ++index1)
                  {
                    if (!this.MustExit && !e.Cancel)
                    {
                      for (int index2 = 0; index2 < list2.Count; ++index2)
                      {
                        if (!this.MustExit && !e.Cancel)
                        {
                          if (list2[index2].Contains(" ? "))
                          {
                            string s = list2[index2].Substring(0, list2[index2].IndexOf(" ? "));
                            double num10 = -1.0;
                            ref double local = ref num10;
                            if (double.TryParse(s, out local))
                            {
                              if (num10 == doubleList[index1])
                              {
                                try
                                {
                                  Directory.CreateDirectory("Temp\\OldFiles " + (object) num9);
                                }
                                catch (Exception ex)
                                {
                                  int num11 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                  goto label_107;
                                }
                                string str3 = list2[index2].Substring(list2[index2].IndexOf(" ? ") + 3);
                                string str4 = (string) null;
                                bool flag3 = false;
                                for (int index3 = 0; index3 < 4; ++index3)
                                {
                                  if (!this.MustExit)
                                  {
                                    if (!e.Cancel)
                                    {
                                      try
                                      {
                                        this.BackGroundWorkerForProgressBar.ReportProgress(-1, (object) "Downloading");
                                        str4 = Path.GetTempFileName();
                                        webClient.DownloadFile("https://drive.google.com/uc?id=" + str3 + "&export=download&authuser=0", str4);
                                        flag3 = true;
                                        break;
                                      }
                                      catch (Exception ex)
                                      {
                                        this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R3");
                                        Thread.Sleep(1000);
                                        this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R2");
                                        Thread.Sleep(1000);
                                        this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "ConnectionError_R1");
                                        Thread.Sleep(1000);
                                      }
                                    }
                                    else
                                      goto label_107;
                                  }
                                  else
                                    goto label_107;
                                }
                                if (!flag3)
                                {
                                  int num12 = (int) MessageBox.Show("No se pudo completar la actualización debido a un error en la conexión.", "Actualización fallida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                  goto label_107;
                                }
                                else
                                {
                                  List<string> list3 = ((IEnumerable<string>) System.IO.File.ReadAllLines(str4, Encoding.UTF8)).ToList<string>();
                                  for (int index4 = 0; index4 < list3.Count; ++index4)
                                  {
                                    if (!this.MustExit && !e.Cancel)
                                    {
                                      if (list3[index4].Contains(" ? "))
                                      {
                                        string str5 = list3[index4].Substring(0, list3[index4].IndexOf(" ? "));
                                        string str6 = list3[index4].Substring(list3[index4].IndexOf(" ? ") + 3);
                                        try
                                        {
                                          if (System.IO.File.Exists(str5))
                                          {
                                            string path = Path.Combine("Temp\\OldFiles " + (object) num9, Path.GetDirectoryName(str5));
                                            if (!Directory.Exists(path))
                                              Directory.CreateDirectory(path);
                                            System.IO.File.Move(str5, Path.Combine("Temp\\OldFiles " + (object) num9, str5));
                                          }
                                          else if (Path.GetDirectoryName(str5) != "")
                                          {
                                            if (!Directory.Exists(Path.GetDirectoryName(str5)))
                                              Directory.CreateDirectory(Path.GetDirectoryName(str5));
                                          }
                                        }
                                        catch (Exception ex)
                                        {
                                          int num13 = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                          goto label_107;
                                        }
                                        bool flag4 = false;
                                        for (int index5 = 0; index5 < 4; ++index5)
                                        {
                                          if (!this.MustExit)
                                          {
                                            if (!e.Cancel)
                                            {
                                              try
                                              {
                                                this.BackGroundWorkerForProgressBar.ReportProgress(-1, (object) "Downloading");
                                                webClient.DownloadFile("https://drive.google.com/uc?id=" + str6 + "&export=download&authuser=0", str5);
                                                flag4 = true;
                                                break;
                                              }
                                              catch (Exception ex)
                                              {
                                                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "DownloadingError_R3");
                                                Thread.Sleep(1000);
                                                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "DownloadingError_R2");
                                                Thread.Sleep(1000);
                                                this.BackGroundWorkerForProgressBar.ReportProgress(50, (object) "DownloadingError_R1");
                                                Thread.Sleep(1000);
                                              }
                                            }
                                            else
                                              goto label_107;
                                          }
                                          else
                                            goto label_107;
                                        }
                                        if (!flag4)
                                        {
                                          int num14 = (int) MessageBox.Show("No se pudo completar la actualización debido a un error en la conexión.", "Actualización fallida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                          goto label_107;
                                        }
                                        else if (!this.MustExit && !e.Cancel)
                                          this.BackGroundWorkerForProgressBar.ReportProgress(num4 * index1 + num4 / list3.Count * index4, (object) "Downloading");
                                        else
                                          goto label_107;
                                      }
                                      else
                                      {
                                        int num15 = (int) MessageBox.Show("El servidor respondió de forma inesperada.", "Error de servidor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                        goto label_107;
                                      }
                                    }
                                    else
                                      goto label_107;
                                  }
                                  ++num9;
                                }
                              }
                            }
                            else
                            {
                              int num16 = (int) MessageBox.Show("El servidor respondió de forma inesperada.", "Error de servidor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                              goto label_107;
                            }
                          }
                        }
                        else
                          goto label_107;
                      }
                      this.BackGroundWorkerForProgressBar.ReportProgress(num4 * (index1 + 1), (object) "Downloading");
                    }
                    else
                      goto label_107;
                  }
                  if (!this.MustExit)
                  {
                    if (!e.Cancel)
                    {
                      try
                      {
                        System.IO.File.WriteAllText("Temp\\Finished", "Yes");
                      }
                      catch (Exception ex)
                      {
                      }
                      try
                      {
                        Directory.Delete("Temp", true);
                        goto label_108;
                      }
                      catch (Exception ex)
                      {
                        goto label_108;
                      }
                    }
                  }
                }
                else
                  goto label_108;
label_107:
                this.BackGroundWorkerForProgressBar.ReportProgress(100, (object) "Restoring");
              }
            }
          }
        }
      }
label_108:
      this.BackGroundWorkerForProgressBar.ReportProgress(100, (object) "Closing");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Formulario));
      this.BarraDeProgreso = new ProgressBar();
      this.EtiquetaEstado = new Label();
      this.BackGroundWorkerForProgressBar = new BackgroundWorker();
      this.SuspendLayout();
      this.BarraDeProgreso.Location = new Point(15, 25);
      this.BarraDeProgreso.Name = "BarraDeProgreso";
      this.BarraDeProgreso.Size = new Size(557, 23);
      this.BarraDeProgreso.TabIndex = 0;
      this.EtiquetaEstado.AutoSize = true;
      this.EtiquetaEstado.Location = new Point(12, 9);
      this.EtiquetaEstado.Name = "EtiquetaEstado";
      this.EtiquetaEstado.Size = new Size(116, 13);
      this.EtiquetaEstado.TabIndex = 1;
      this.EtiquetaEstado.Text = "Descargando paquete:";
      this.BackGroundWorkerForProgressBar.WorkerReportsProgress = true;
      this.BackGroundWorkerForProgressBar.WorkerSupportsCancellation = true;
      this.BackGroundWorkerForProgressBar.DoWork += new DoWorkEventHandler(this.BackGroundWorkerForProgressBar_DoWork);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlLightLight;
      this.ClientSize = new Size(584, 71);
      this.Controls.Add((Control) this.EtiquetaEstado);
      this.Controls.Add((Control) this.BarraDeProgreso);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (Formulario);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Espere mientras se completa la actualización:";
      this.Load += new EventHandler(this.Formulario_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
