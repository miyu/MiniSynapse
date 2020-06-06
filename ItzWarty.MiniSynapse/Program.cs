using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Razer.Emily.Common;
using Razer.Emily.UI;
using Razer.Storage;

namespace ItzWarty.MiniSynapse
{
    class Program
    {
        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        // NEEDED FOR SYNAPSE 2.x+
        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        const uint SPI_GETMOUSE = 0x0003;
        const uint SPI_SETMOUSE = 0x0004;

        public enum SPIF
        {
            SPIF_NULL = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        // NEEDED FOR SYNAPSE 2.x+
        // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

        [STAThread]
        private unsafe static void Main()
        {
            // Values
            int[] MouseAccelParams = new int[3] { 0, 0, 0 };
            GCHandle PArray = GCHandle.Alloc(MouseAccelParams, GCHandleType.Pinned);
            IntPtr Pointer = PArray.AddrOfPinnedObject();

            // Store the current Windows mouse pointer settings, such as speed and acceleration status
            if (!SystemParametersInfo(SPI_GETMOUSE, 0, Pointer, 0))
                MessageBox.Show("SPI_GETMOUSE failed!", "MiniSynapse - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Log into Razer Synapse by using the last login data
            MainStorage I1 = Singleton<MainStorage>.Instance;
            I1.Initialize(Storage_Project.Emily);
            if (I1.TryLastLogin() != LoginStatus.Success)
            {
                // Something went wrong and we were unable to login
                MessageBox.Show("Cannot configure your Razer devices - you need to login through Razer Synapse first.");
                PArray.Free();

                return;
            }

            // Enumerate all the installed Razer Synapse 2.x devices
            RzDeviceManager RazerDM = new RzDeviceManager();
            RazerDM.Enumerate();

            // Initialize the settings loader
            CommonConfigLoader I2 = Singleton<CommonConfigLoader>.Instance;
            I2.StartConfig();

            // Apply the Razer Synapse settings to the device
            foreach (RzDevice RazerDevice in RazerDM.ActiveDevices.ToArray())
            {
                RazerDevice.RefreshData();
                I2.DeviceAdded(RazerDevice.VID, RazerDevice.PID);
                Console.WriteLine("Configured device: " + I2.FindDevice(RazerDevice.PID).Name);
            }

            // Restore the mouse settings back, since Synapse 2.10+ seem to reset them
            if (!SystemParametersInfo(SPI_SETMOUSE, 0, Pointer, SPIF.SPIF_SENDCHANGE))
                MessageBox.Show("SPI_SETMOUSE failed!", "MiniSynapse - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Free the GCHandle since we don't need it anymore
            PArray.Free();

            return;
        }
    }
}