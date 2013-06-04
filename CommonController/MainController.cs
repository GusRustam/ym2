using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonController {
    public class ShutdownController {
        [DllImport("ole32.dll")]
        private extern void CoUninitialize();

        public event Action ShutdownNow;
        private static ShutdownController _instance; 


        public static ShutdownController Instance  {
            get {
                if (_instance == null) _instance = new ShutdownController();
                return _instance;
            }
        }

        private ShutdownController() {
        }

        public void Shutdown() {
            ShutdownNow();
            CoUninitialize();
        }
    }
}
