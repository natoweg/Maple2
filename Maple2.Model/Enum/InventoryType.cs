﻿namespace Maple2.Model.Enum; 

public enum InventoryType : byte {
    // Inventory
    Gear = 0,
    Outfit = 1,
    Mount = 2,
    Catalyst = 3,
    FishingMusic = 4,
    Quest = 5,
    Gemstone = 6,
    Misc = 7,
    LifeSkill = 9,
    Pets = 10,
    Consumable = 11,
    Currency = 12,
    Badge = 13,
    //Mushtopia = 14,
    Lapenshard = 15,
    Fragment = 16,
    // Equip
    GearEquip = 20,
    OutfitEquip = 21,
    BadgeEquip = 22,
    LapenshardEquip = 23,
    // Storage
    Trade = 30,
    Storage = 31,
    PetStorage = 32,
    FurnishingStorage = 33,
}