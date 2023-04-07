﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameDatabase.DataModel;
using GameDatabase.Model;
using Session;
using Zenject;

namespace GameServices.Player
{
    public sealed class PlayerResources : GameServiceBase
    {
        [Inject]
        public PlayerResources(
            ISessionData session, 
            PlayerSkills skills, 
            SessionDataLoadedSignal dataLoadedSignal, 
            SessionCreatedSignal sessionCreatedSignal)
            : base(dataLoadedSignal, sessionCreatedSignal)
        {
            _session = session;
            _skills = skills;
        }

        public int Money
        {
            get => Cheats.GetCheatedResourcesAmount();
            set {}
        }

        public int Stars
        {
            get => Cheats.GetCheatedResourcesAmount();
            set {}
        }

        public int Tokens
        {
            get => Cheats.GetCheatedResourcesAmount();
            set {}
        }

        public int Snowflakes
        {
            get { return _session.Resources.Resources.GetQuantity((int)SpecialResources.Snowflakes); }
            set { _session.Resources.Resources[(int)SpecialResources.Snowflakes] = Clamp(value); }
        }

        public int Fuel
        {
            get => 600;
            set {}
        }

        public IEnumerable<ItemId<QuestItem>> Resources { get { return _session.Resources.Resources.Items.Select(item => new ItemId<QuestItem>(item.Key)); } }

        public int GetResource(ItemId<QuestItem> id)
        {
            return _session.Resources.Resources.GetQuantity(id.Value);
        }

        public void AddResource(ItemId<QuestItem> id, int amount)
        {
            _session.Resources.Resources.Add(id.Value, Clamp(amount));
        }

        public void RemoveResource(ItemId<QuestItem> id, int amount)
        {
            _session.Resources.Resources.Remove(id.Value, Clamp(amount));
        }

        public bool TryConsumeFuel(int amount)
        {
            if (Fuel < amount)
                return false;

            Fuel -= amount;
            return true;
        }

        protected override void OnSessionDataLoaded() {}
        protected override void OnSessionCreated() {}

        private readonly ISessionData _session;
        private readonly PlayerSkills _skills;

        private static int Clamp(int value, int max = int.MaxValue)
        {
            return value < 0 ? 0 : value > max ? max : value;
        }

        private enum SpecialResources
        {
            Snowflakes = 25,
        }
    }
}
