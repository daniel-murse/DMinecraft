using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Voxels.Loading
{
    /// <summary>
    /// If a pixel should be culled.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    internal delegate bool PixelCullPredicate(in Color4 color);

    internal static class Util
    {
        public static Model BuildModel(RgbaImage2D image)
        {
            bool IsPixelTransparent(in Color4 color)
            {
                return color.A > 0.001f;
            }
            return BuildModel(image, IsPixelTransparent);
        }

        

        /// <summary>
        /// Build a model like mc builds item models from textures.
        /// Takes an "image" which just gives access to pixels over dimensions.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cullTest"></param>
        /// <returns></returns>
        public static Model BuildModel(RgbaImage2D image, PixelCullPredicate cullTest)
        {
            ModelBuilder builder = new ModelBuilder(true, 1);

            //for row i
            for (int i = 0; i < image.Height; i++)
            {
                //for col j
                for (int j = 0; j < image.Width; j++)
                {
                    if (cullTest(image.GetPixel(i, j)))
                        continue;
                    //check above
                    if (image.IsPixel(i, j - 1))
                    {
                        Color4 pixel = image.GetPixel(i, j - 1);
                        if (cullTest(in pixel))
                        {
                            //do
                        }
                    }
                    if(image.IsPixel(i, j + 1))
                    {
                        //if (cullTest(in pixel))
                        {
                            //do
                        }
                    }
                }
            }

            return null;
        }

        private static int GetMaxVertices(int width, int height)
        {
            //top and bottom all covered + sides todo
            return (width + 1) * (1 + height) * 2 + (width + 1) * (1 + height);
        }
    }
}
