// Decompiled with JetBrains decompiler
// Type: Updater.Program
// Assembly: Updater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2BEDE3E-6935-450F-ACEE-2CA0DF498A17
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Updater.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Updater
{
  internal static class Program
  {
    private static bool SubProcesoDeRestaurarFolder(string Path)
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
            File.WriteAllBytes(path, File.ReadAllBytes(list1[index]));
          }
        }
        for (int index = 0; index < list2.Count; ++index)
        {
          if (list2[index].Count<char>() > 16 && list2[index].StartsWith("Temp\\OldFiles 1"))
            Program.SubProcesoDeRestaurarFolder(list2[index]);
        }
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("La restauración falló: " + (object) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand));
        return false;
      }
    }

    private static bool RestaurarFolder()
    {
      if (Directory.Exists("Temp"))
      {
        bool flag = !Directory.Exists("Temp\\OldFiles 1") || Program.SubProcesoDeRestaurarFolder("Temp\\OldFiles 1");
        Directory.Delete("Temp", true);
        return flag;
      }
      int num = (int) MessageBox.Show("No se pudieron restaurar los archivos, la carpeta de respaldo fue borrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    [STAThread]
    private static int Main(string[] args)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
      if (Directory.Exists("Temp") && File.Exists("Temp\\Finished") && File.ReadAllText("Temp\\Finished") != "Yes")
      {
        int num1 = (int) MessageBox.Show("Hemos detectado que hubo un error en la última actualización. Por lo que trataremos de restaurar los archivos anteriores a dicha actualización.", "Restauración de archivos anteriores", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        if (Program.RestaurarFolder())
        {
          if (MessageBox.Show("La restauración fue exitosa, ¿Desea continuar con la actualización?", "Restauración exitosa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            return 0;
        }
        else
        {
          int num2 = (int) MessageBox.Show("No se han podido restaurar los archivos anteriores. Como última instancia, y si no han habido perdidas de información en la base de datos, haga una copia de seguridad de la base de datos y cambie la aplicación a otra carpeta o reinstale el programa. Si ha tenido perdidas de información, porfavor, comuníquese con soporte tecnico antes de hacer cualquier otra cosa.");
          return 0;
        }
      }
      for (int index = 0; index < ((IEnumerable<string>) args).Count<string>(); ++index)
      {
        if (args[index].Count<char>() > 8 && args[index].Substring(0, 8) == "Version_")
        {
          double result = 999999999.0;
          if (double.TryParse(args[index].Substring(8).Replace("_", "."), out result))
          {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((Form) new Formulario(result));
          }
        }
      }
      return 0;
    }
  }
}
