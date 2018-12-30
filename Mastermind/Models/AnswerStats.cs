﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mastermind.Models
{
    public class AnswerStats
    {
        // bull: correct value and position
        public int WhitePoints { get; private set; }
        // cow: value in incorrect position
        public int BlackPoints { get; private set; }

        public AnswerStats(int white, int black)
        {
            WhitePoints = white;
            BlackPoints = black;
        }
    }
}
