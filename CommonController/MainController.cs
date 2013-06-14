using System;
using Uitls;

namespace CommonController {
    public class ShutdownController {
        public event Action ShutdownNow;
        private static ShutdownController _instance; 

        public static ShutdownController Instance  {
            get { return _instance ?? (_instance = new ShutdownController()); }
        }

        private ShutdownController() {
        }

        public void Shutdown() {
            ShutdownNow();
            DllFunctions.CoUninitialize();
        }
    }
}
