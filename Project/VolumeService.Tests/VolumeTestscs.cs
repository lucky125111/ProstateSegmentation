using System;
using System.Collections.Generic;
using FluentAssertions;
using VolumeService.Core.VolumeCalculator;
using Xunit;

namespace VolumeService.Tests
{
    public class VolumeTests
    {
        [Fact]
        public void EmptyList()
        {
            var l = new List<double?>();

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(0);
        }

        [Fact]
        public void ListWithFirstValue()
        {
            var l = new List<double?> {1, 0, 0, 0, 0};

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(1);
        }

        [Fact]
        public void ListWithGapValues()
        {
            var l = new List<double?> {1, 0, 0, 1, 0};

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(3);
        }

        [Fact]
        public void ListWithSingleValue()
        {
            var l = new List<double?> {0, 0, 1, 0, 0};

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(1);
        }

        [Fact]
        public void ListWithTwoValues()
        {
            var l = new List<double?> {0, 0, 1, 1, 0};

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(1);
        }

        [Fact]
        public void ListWithZeros()
        {
            var l = new List<double?> {0, 0, 0};

            var vol = VolumeCalculator.CalculateVolume(1.0, l);

            vol.Should().Be(0);
        }

        //private static double CalculateVolume(double distance, List<double> segmentsArea)
        [Fact]
        public void Null()
        {
            List<double?> l = null;

            Action action = () => VolumeCalculator.CalculateVolume(1.0, l);

            action.Should().Throw<NullReferenceException>();
        }
    }
}