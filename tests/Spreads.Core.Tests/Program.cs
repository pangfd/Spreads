﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using Spreads.Tests.Buffers;

namespace Spreads.Tests
{
    internal class Program
    {
        private static void Main1(string[] args)
        {
            using (Process p = Process.GetCurrentProcess())
            {
                p.PriorityClass = ProcessPriorityClass.High;
            }

            //Benchmark.ForceSilence = true;

            for (int i = 0; i < 20; i++)
            {
                //new ArithmeticTests().CouldUseStructSeries();
                //new ZipCursorTests().CouldAddTwoSeriesWithSameKeysBenchmark();
                //new SCMTests().EnumerateScmSpeed();
                //new VariantTests().CouldCreateAndReadInlinedVariantInALoop();
                //new StatTests().Stat2StDevBenchmark();
                //new FastDictionaryTests().CompareSCGAndFastDictionaryWithInts();
                new RecyclableMemoryStreamTests().CouldUseSafeWriteReadArray();
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}