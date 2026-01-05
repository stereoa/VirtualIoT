using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualIoT.Models.Virtual
{
    public class VirtualHumiditySensor : IVirtualSensor
    {
        private readonly Random _rand = new();

        public string Name => "humidity";

        public object ReadValue()
            => Math.Round(40 + _rand.NextDouble() * 20, 2);
    }
}
