using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util
{
    internal class Transform
    {
        private Matrix4 matrix;
        private Vector3 position;
        private Vector3 rotationRad;
        private Vector3 scale;
        private Vector3 origin;

        private bool isDirty;

        public Transform()
        {
            position = Vector3.Zero;
            rotationRad = Vector3.Zero;
            scale = Vector3.One;
            origin = Vector3.Zero;
            matrix = Matrix4.Identity;
            isDirty = false;
        }

        public Vector3 Position
        {
            get => position; set
            {
                isDirty = true;
                position = value;
            }
        }

        public Vector3 RotationRad
        {
            get => rotationRad; set
            {
                isDirty = true;
                rotationRad = value;
            }
        }

        public Vector3 Scale
        {
            get => scale; set
            {
                isDirty = true;
                scale = value;
            }
        }

        public Vector3 Origin
        {
            get => origin; set
            {
                isDirty = true;
                origin = value;
            }
        }

        public Matrix4 Matrix
        {
            get
            {
                if (isDirty)
                {
                    ComputeMatrix(out matrix);
                    isDirty = false;
                }
                return matrix;
            }

            set
            {
                isDirty = true;
                matrix = value;
            }
        }

        public ref readonly Matrix4 RefMatrix
        {
            get
            {
                if (isDirty)
                {
                    ComputeMatrix(out matrix);
                    isDirty = false;
                }
                return ref matrix;
            }
        }

        public void ComputeMatrix(out Matrix4 result)
        {
            result =
                Matrix4.CreateTranslation(-Origin) * //first place on the origin
                Matrix4.CreateScale(Scale) *
                Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(RotationRad)) *
                Matrix4.CreateTranslation(Position);
        }
    }
}
