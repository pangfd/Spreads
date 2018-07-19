﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Spreads.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Spreads.DataTypes
{
    /// <summary>
    /// A Timestamp stored as nanos since Unix epoch as Int64.
    /// 2^63: 9,223,372,036,854,780,000
    /// Nanos per day: 86,400,000,000,000 (2^47)
    /// Nanos per year: 31,557,600,000,000,000 (2^55)
    /// 292 years of nanos in 2^63 is ought to be enough for everyone (except JavaScript).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    [Serialization(BlittableSize = 8)]
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public readonly struct Timestamp : IComparable<Timestamp>, IEquatable<Timestamp>
    {
        private static readonly long UnixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
        private readonly long _nanos;

        public Timestamp(long nanos)
        {
            _nanos = nanos;
        }

        public DateTime DateTime => this;

        public long Nanos
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _nanos; }
        }

        public TimeSpan TimeSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new TimeSpan(((DateTime)this).Ticks); }
        }

        public static implicit operator DateTime(Timestamp timestamp)
        {
            return new DateTime(UnixEpochTicks + timestamp._nanos / 100, DateTimeKind.Utc);
        }

        public static implicit operator Timestamp(DateTime dateTime)
        {
            var value = (dateTime.ToUniversalTime().Ticks - UnixEpochTicks) * 100;
            return new Timestamp(value);
        }

        public static explicit operator long(Timestamp timestamp)
        {
            return timestamp._nanos;
        }

        public static explicit operator Timestamp(long nanos)
        {
            return new Timestamp(nanos);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Timestamp other)
        {
            return _nanos.CompareTo(other._nanos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Timestamp other)
        {
            return _nanos == other._nanos;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Timestamp timestamp && Equals(timestamp);
        }

        public override int GetHashCode()
        {
            return _nanos.GetHashCode();
        }

        public override string ToString()
        {
            return ((DateTime)this).ToString("O");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Timestamp operator -(Timestamp x)
        {
            return new Timestamp(-x._nanos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Timestamp operator -(Timestamp x, Timestamp y)
        {
            return new Timestamp(checked(x._nanos - y._nanos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Timestamp operator +(Timestamp x, Timestamp y)
        {
            return new Timestamp(checked(x._nanos + y._nanos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Timestamp x, Timestamp y)
        {
            return x.Equals(y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Timestamp x, Timestamp y)
        {
            return !x.Equals(y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Timestamp x, Timestamp y)
        {
            return x.CompareTo(y) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Timestamp x, Timestamp y)
        {
            return x.CompareTo(y) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Timestamp x, Timestamp y)
        {
            return x.CompareTo(y) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Timestamp x, Timestamp y)
        {
            return x.CompareTo(y) <= 0;
        }
    }
}