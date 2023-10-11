using System.Diagnostics;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace CGCloseX;

public partial class Form1 : Form
{
    // Win32 API constants
    private const int MF_BYCOMMAND = 0x00000000;
    private const int MF_GRAYED = 0x1;
    private const int SC_CLOSE = 0xF060;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);

    private List<IntPtr> closed = new List<IntPtr>();
    
    public void CloseAllCGProcess()
    {
        var process = Process.GetProcessesByName("cghk");

        foreach (var process1 in process)
        {
            RemoveCloseButton(process1);
        }
    }


    private void RemoveCloseButton(Process p)
    {
        IntPtr hWnd = p.MainWindowHandle;
        if (closed.Contains(hWnd))
        {
            return;
        }
        if (hWnd != IntPtr.Zero)
        {
            IntPtr hMenu = GetSystemMenu(hWnd, false);
            if (hMenu != IntPtr.Zero)
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
            closed.Add(hWnd);
        }
    }

    
    public Form1()
    {
        InitializeComponent();
        this.Text = "魔力寶貝關X";
        var lbl = new Label();
        Width = 300;
        Height = 200;
        lbl.Width = 200;
        lbl.Text = "開著 15 秒掃一次魔力視窗關X，已經跑過的不會跑";
        this.Controls.Add(lbl);

        Timer t = new Timer();

        t.Enabled = true;
        t.Interval = 15000;
        t.Tick += (sender, args) =>
        {
            CloseAllCGProcess();
        };

    }
}