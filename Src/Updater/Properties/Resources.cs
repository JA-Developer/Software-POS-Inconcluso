// Decompiled with JetBrains decompiler
// Type: Updater.Properties.Resources
// Assembly: Updater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2BEDE3E-6935-450F-ACEE-2CA0DF498A17
// Assembly location: C:\Users\User\Downloads\Software-POS-Inconcluso-main (1)\Software-POS-Inconcluso-main\Updater.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Updater.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Updater.Properties.Resources.resourceMan == null)
          Updater.Properties.Resources.resourceMan = new ResourceManager("Updater.Properties.Resources", typeof (Updater.Properties.Resources).Assembly);
        return Updater.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Updater.Properties.Resources.resourceCulture;
      set => Updater.Properties.Resources.resourceCulture = value;
    }
  }
}
