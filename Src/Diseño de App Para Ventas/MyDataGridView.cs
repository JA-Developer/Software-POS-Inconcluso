// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.MyDataGridView
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System.Windows.Forms;

namespace Diseño_de_App_Para_Ventas
{
  internal class MyDataGridView : DataGridView
  {
    public MyDataGridView()
    {
      if (SystemInformation.TerminalServerSession)
        return;
      this.DoubleBuffered = true;
    }
  }
}
