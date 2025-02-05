﻿/* Copyright (c) 2019 Rick (rick 'at' gibbed 'dot' us)
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gibbed.BorderlandsOz.GameInfo
{
    public static class Experience
    {
        private const float _Multiplier = 60.0f;
        private const float _Power = 2.8f;
        private const float _Offset = 7.33f;

        private static int Compute(int level)
        {
            return (int)Math.Floor((Math.Pow(level, _Power) + _Offset) * _Multiplier);
        }

        private static readonly int _MinimumLevel;
        private static readonly int _MaximumLevel;
        private static readonly int _Reduction;
        private static readonly KeyValuePair<int, int>[] _LevelsAndTheirRequiredPoints;

        static Experience()
        {
            _MinimumLevel = 1;
            _MaximumLevel = 50 + 10 + 10; // TODO: put this in a resource generated by datamining

            _Reduction = Compute(_MinimumLevel);
            _LevelsAndTheirRequiredPoints = Enumerable
                .Range(1, _MaximumLevel)
                .Select(l => new KeyValuePair<int, int>(l, GetPointsForLevel(l)))
                .ToArray();
        }

        public static int GetPointsForLevel(int level)
        {
            if (level <= _MinimumLevel)
            {
                return 0;
            }

            return Compute(level) - _Reduction;
        }

        public static int GetLevelForPoints(int points)
        {
            if (points < 0)
            {
                return _MinimumLevel;
            }

            if (points >= _LevelsAndTheirRequiredPoints.Last().Value)
            {
                return _MaximumLevel;
            }

            return _LevelsAndTheirRequiredPoints
                       .First(lp => points < lp.Value)
                       .Key - 1;
        }
    }
}
