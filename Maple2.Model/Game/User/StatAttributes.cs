﻿using System.Collections.Generic;
using System.Linq;
using Maple2.Model.Enum;
using Maple2.Model.Metadata;
using Maple2.PacketLib.Tools;
using Maple2.Tools;
using Maple2.Tools.Extensions;

namespace Maple2.Server.Game.Manager.Config;

public class StatAttributes : IByteSerializable {
    public readonly PointSources Sources;
    public readonly PointAllocation Allocation;

    public int TotalPoints => Sources.Count;
    public int UsedPoints => Allocation.Count;

    public StatAttributes() {
        Sources = new PointSources();
        Allocation = new PointAllocation();

        // TODO: this should be dynamic
        Sources[AttributePointSource.Trophy] = 38;
        Sources[AttributePointSource.Quest] = 12;
        Sources[AttributePointSource.Prestige] = 50;
    }

    public void WriteTo(IByteWriter writer) {
        writer.WriteClass<PointSources>(Sources);
        writer.WriteClass<PointAllocation>(Allocation);
    }

    public class PointSources : IByteSerializable {
        // MaxPoints - Trophy:38, Exploration:12, Prestige:50
        public readonly IDictionary<AttributePointSource, int> Points;

        public int Count => Points.Values.Sum();

        public int this[AttributePointSource type] {
            get => Points[type];
            set => Points[type] = value;
        }

        public PointSources() {
            Points = new Dictionary<AttributePointSource, int>();
        }

        public void WriteTo(IByteWriter writer) {
            writer.WriteInt(Points.Count);
            foreach ((AttributePointSource source, int amount) in Points) {
                writer.Write<AttributePointSource>(source);
                writer.WriteInt(amount);
            }
        }
    }

    public class PointAllocation : IByteSerializable {
        private readonly Dictionary<StatAttribute, int> points;

        public StatAttribute[] Attributes => points.Keys.ToArray();
        public int Count => points.Values.Sum();

        public int this[StatAttribute type] {
            get => points.GetValueOrDefault(type);
            set {
                if (value < 0 || value > StatLimit(type)) {
                    return;
                }
                if (value == 0) {
                    points.Remove(type);
                    return;
                }

                points[type] = value;
            }
        }

        public PointAllocation() {
            points = new Dictionary<StatAttribute, int>();
        }

        public static int StatLimit(StatAttribute type) {
            return type switch {
                StatAttribute.Strength => Constant.StatPointLimit_str,
                StatAttribute.Dexterity => Constant.StatPointLimit_dex,
                StatAttribute.Intelligence => Constant.StatPointLimit_int,
                StatAttribute.Luck => Constant.StatPointLimit_luk,
                StatAttribute.Health => Constant.StatPointLimit_hp,
                StatAttribute.CriticalRate => Constant.StatPointLimit_cap,
                _ => 0,
            };
        }

        public void WriteTo(IByteWriter writer) {
            writer.WriteInt(points.Count);
            foreach ((StatAttribute type, int value) in points) {
                writer.Write<StatAttribute>(type);
                writer.WriteInt(value);
            }
        }
    }
}