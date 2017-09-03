using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3D_KitKat_Touch_Press
{
    class HotKey
    {
        private int fsModifier;
        private int key;
        private int x_location;
        private int y_location;

        public int FsModifier { get => fsModifier; set => fsModifier = value; }
        public int Key { get => key; set => key = value; }
        public int X_location { get => x_location; set => x_location = value; }
        public int Y_location { get => y_location; set => y_location = value; }

        public HotKey()
        {
            fsModifier = 0;
            key = 0;
            x_location = 0;
            y_location = 0;
        }

        public HotKey(int fsModifier, int key, int x_location, int y_location)
        {
            this.fsModifier = fsModifier;
            this.key = key;
            this.x_location = x_location;
            this.y_location = y_location;
        }
    }
}
