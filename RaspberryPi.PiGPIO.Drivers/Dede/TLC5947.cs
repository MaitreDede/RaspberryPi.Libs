﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public sealed class TLC5947 : BaseDriver, ITLC5947
    {
        private readonly int m_numdrivers;
        private readonly int m_gpioClock;
        private readonly int m_gpioData;
        private readonly int m_gpioLatch;
        private readonly int m_gpioOutputEnabled;
        private readonly ushort[] m_pwmbuffer;
        private bool m_outputEnabled = false;

        public IPiGPIO PiGPIO => this.m_gpio;
        public bool OutputEnabled => this.m_outputEnabled;

        public TLC5947(IPiGPIO pigpio, int numdrivers, int gpioClock, int gpioData, int gpioLatch, int gpioOutputEnabled = int.MinValue)
            : base(pigpio)
        {
            if (numdrivers <= 0)
                throw new ArgumentOutOfRangeException(nameof(numdrivers));

            this.m_pwmbuffer = new ushort[24 * numdrivers];
            this.m_numdrivers = numdrivers;
            this.m_gpioClock = gpioClock;
            this.m_gpioData = gpioData;
            this.m_gpioLatch = gpioLatch;
            this.m_gpioOutputEnabled = gpioOutputEnabled;
        }

        protected override void DefineUsedPins()
        {
            this.UseOutput(this.m_gpioClock, false);
            this.UseOutput(this.m_gpioData, false);
            this.UseOutput(this.m_gpioLatch, false);
            if (this.m_gpioOutputEnabled != int.MinValue)
                this.UseOutput(this.m_gpioOutputEnabled, false);
        }

        public void SetOutputEnabled(bool enabled)
        {
            this.m_outputEnabled = enabled;
            if (this.m_gpioOutputEnabled != int.MinValue)
                this.m_gpio.Write(this.m_gpioOutputEnabled, !this.m_outputEnabled);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte Reverse(byte b)
        {
            int revB = 0;
            revB += (b & 0b0000_0001) << 7;
            revB += (b & 0b0000_0010) << 5;
            revB += (b & 0b0000_0100) << 3;
            revB += (b & 0b0000_1000) << 1;
            revB += (b & 0b0001_0000) >> 1;
            revB += (b & 0b0010_0000) >> 3;
            revB += (b & 0b0100_0000) >> 5;
            revB += (b & 0b1000_0000) >> 7;
            return (byte)revB;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Clamp(ref int value, int minInclusive, int maxInclusive)
        {
            if (value < minInclusive)
                value = minInclusive;
            if (value > maxInclusive)
                value = maxInclusive;
        }

        /// <inheritDoc />
        public void Write()
        {
            int len = this.m_pwmbuffer.Length * 3 / 2;
            byte[] buffer = new byte[len];
            for (int i = 0; i < 24 * m_numdrivers; i += 2)
            {
                ushort pwm0 = this.m_pwmbuffer[i + 0];
                ushort pwm1 = this.m_pwmbuffer[i + 1];

                byte b0 = (byte)((pwm0 & 0x0FF0) >> 4);
                byte b1 = (byte)((pwm0 & 0x000F) << 4 | ((pwm1 & 0x0F00) >> 8));
                byte b2 = (byte)(pwm1 & 0x00FF);

                //buffer[i * 3 + 0] = b0;
                //buffer[i * 3 + 1] = b1;
                //buffer[i * 3 + 2] = b2;
                buffer[len - 1 - (i * 3 / 2 + 0)] = Reverse(b0);
                buffer[len - 1 - (i * 3 / 2 + 1)] = Reverse(b1);
                buffer[len - 1 - (i * 3 / 2 + 2)] = Reverse(b2);
            }
            this.m_gpio.BitBangSend(this.m_gpioData, this.m_gpioClock, buffer);
            this.m_gpio.Write(this.m_gpioLatch, true);
            this.m_gpio.Write(this.m_gpioLatch, false);
        }

        /// <inheritDoc />
        public void SetPWM(int chan, int pwm)
        {
            Clamp(ref pwm, 0, 4095);
            m_pwmbuffer[chan] = (ushort)pwm;
        }
    }
}
