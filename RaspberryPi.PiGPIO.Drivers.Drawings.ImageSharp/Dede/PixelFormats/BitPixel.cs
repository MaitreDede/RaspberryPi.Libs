﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats
{
    public struct BitPixel : IPixel<BitPixel>
    {
        private static readonly Vector4 v4Off = new Vector4(0, 0, 0, 1);

        public bool Value { get; set; }

        public BitPixel(bool value)
        {
            this.Value = value;
        }

        public PixelOperations<BitPixel> CreatePixelOperations()
        {
            return new PixelOperations<BitPixel>();
        }

        public bool Equals(BitPixel other)
        {
            return this.Value == other.Value;
        }

        public void PackFromRgba32(Rgba32 source)
        {
            throw new NotImplementedException();
        }

        public void PackFromVector4(Vector4 vector)
        {
            this.Value = (((vector.X + vector.Y + vector.Z + vector.W) / 4) > 0.5);
        }

        public void ToBgr24(ref Bgr24 dest)
        {
            throw new NotImplementedException();
        }

        public void ToBgra32(ref Bgra32 dest)
        {
            throw new NotImplementedException();
        }

        public void ToRgb24(ref Rgb24 dest)
        {
            throw new NotImplementedException();
        }

        public void ToRgba32(ref Rgba32 dest)
        {
            dest = this.Value ? Rgba32.White : Rgba32.Black;
        }

        public Vector4 ToVector4()
        {
            return this.Value ? Vector4.One : v4Off;
        }

        public static readonly BitPixel On = new BitPixel(true);
        public static readonly BitPixel Off = new BitPixel(false);
    }
}
