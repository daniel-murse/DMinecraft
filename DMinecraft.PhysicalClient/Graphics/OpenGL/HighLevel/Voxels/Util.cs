using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Voxels
{
    internal class Util
    {
        public Model BuildModelFromImage(byte[] rgbaPixels, int width, int height)
        {
            bool IsPixel(int x, int y)
            {
                return x >= 0 && x < width && y >= 0 && y < height;
            }

            Color4 GetPixel(int x, int y)
            {
                return new Color4(rgbaPixels[width * y + x], rgbaPixels[width * y + x + 1], rgbaPixels[width * y + x + 2], rgbaPixels[width * y + x + 3]);
            }


            //for row i
            for (int i = 0; i < height; i++)
            {
                //for col j
                for (int j = 0; j < width; j++)
                {
                    //check above
                    if(IsPixel(i, j - 1))
                    {
                        GetPixel(i, j);
                    }
                }
            }

            return null;
        }
    }
}
