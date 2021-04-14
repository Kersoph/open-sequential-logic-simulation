using System;
using Godot.Collections;

namespace WAT
{
    public class Range: Assertion
    {
        public static Dictionary IsInRange(double value, double low, double high, string context)
        {
            var passed = $"{value} is in range {low}-{high}";
            var failed = $"{value} is not in range {low}-{high}";
            var success = value >= low && value < high;
            var result = success ? passed : failed;
            return Result(success, passed, result, context);
        }
        
        public static Dictionary IsNotInRange(double value, double low, double high, string context)
        {
            var passed = $"{value} is not in range {low}-{high}";
            var failed = $"{value} is in range {low}-{high}";
            var success = value < low || value >= high;
            var result = success ? passed : failed;
            return Result(success, passed, result, context);
        }
        
    }
}