using System;
using System.Windows.Forms;
using Razer.Emily.Common;
using Razer.Emily.UI;
using Razer.Storage;

namespace ItzWarty.MiniSynapse {
   class Program {
      [STAThread]
      static void Main() {
         var mainStorage = Singleton<MainStorage>.Instance;
         mainStorage.Initialize(Storage_Project.Emily);

         var isLoggedIn = mainStorage.TryLastLogin();
         if (isLoggedIn != LoginStatus.Success) {
            MessageBox.Show("Cannot configure your Razer devices - you need to login through Razer Synapse first.");
         } else {
            var deviceManager = new RzDeviceManager();
            deviceManager.Enumerate();

            var commonConfigLoader = Singleton<CommonConfigLoader>.Instance;
            commonConfigLoader.StartConfig();

            var devices = deviceManager.ActiveDevices.ToArray();
            foreach (var device in devices) {
               device.RefreshData();
               commonConfigLoader.DeviceAdded(device.VID, device.PID);

               // The following code is unnecessary for the program's functioning: 
               var pluginDevice = commonConfigLoader.FindDevice(device.PID);
               Console.WriteLine("Configured device: " + pluginDevice.Name);
            }

            MessageBox.Show("Razer Devices Loaded.\t\t","Mini Synapse", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }
   }
}
