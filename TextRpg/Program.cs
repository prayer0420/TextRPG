﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace TextRpg
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game.GetInstance().Start(); ;
            }

        }
    }
}
