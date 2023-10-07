// Decompiled with JetBrains decompiler
// Type: Diseño_de_App_Para_Ventas.Carrito
// Assembly: Diseño de App Para Ventas, Version=1.1.0.2, Culture=neutral, PublicKeyToken=null
// MVID: D677ECEA-E4A3-4A52-848B-C66D772C59EB
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Diseño de App Para Ventas.exe

using System.Collections.Generic;

namespace Diseño_de_App_Para_Ventas
{
  public class Carrito
  {
    public int IdDeCarrito;
    public double Descuento;
    public bool IsPercent;
    public List<string> Codigos = new List<string>();
    public List<int> Cantidades = new List<int>();
    public string IDCliente = "";
    public string NumeroDeFactura = "";
    public bool AdjuntarNumeroDeFactura;

    public Carrito(int Id)
    {
      this.IdDeCarrito = Id;
      this.Codigos = new List<string>();
      this.Cantidades = new List<int>();
    }
  }
}
