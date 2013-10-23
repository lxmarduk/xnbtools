using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xnb2png {
    public class CustomServiceProvider : IServiceProvider, IDisposable, IGraphicsDeviceService {

        Form win;
        GraphicsDevice device;

        public CustomServiceProvider() {
            win = new Form();
            PresentationParameters pp = new PresentationParameters();
            pp.DeviceWindowHandle = win.Handle;
            pp.IsFullScreen = false;
            device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, pp);
        }

        public object GetService(Type serviceType) {
            if (serviceType.Equals(typeof(IGraphicsDeviceService))) {
                return this;
            }
            return null;
        }

        public void Dispose() {
            device.Dispose();
            win.Dispose();
        }

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceReset;

        public event EventHandler<EventArgs> DeviceResetting;

        public GraphicsDevice GraphicsDevice {
            get {
                return device;
            }
        }
    }
}
