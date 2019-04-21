using System;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class BlazeOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Blaze; } }

        [Constructable]
        public BlazeOre()
            : this(1)
        {
        }

        [Constructable]
        public BlazeOre(int amount)
            : base(CraftResource.Blaze, amount)
        {
        }

        public BlazeOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new BlazeIngot();
        }
    }
    public class IceOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Ice; } }

        [Constructable]
        public IceOre()
            : this(1)
        {
        }

        [Constructable]
        public IceOre(int amount)
            : base(CraftResource.Ice, amount)
        {
        }

        public IceOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new IceIngot();
        }
    }
    public class ToxicOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Toxic; } }

        [Constructable]
        public ToxicOre()
            : this(1)
        {
        }

        [Constructable]
        public ToxicOre(int amount)
            : base(CraftResource.Toxic, amount)
        {
        }

        public ToxicOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new ToxicIngot();
        }
    }
    public class ElectrumOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Electrum; } }

        [Constructable]
        public ElectrumOre()
            : this(1)
        {
        }

        [Constructable]
        public ElectrumOre(int amount)
            : base(CraftResource.Electrum, amount)
        {
        }

        public ElectrumOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new ElectrumIngot();
        }
    }
    public class MoonstoneOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Moonstone; } }

        [Constructable]
        public MoonstoneOre()
            : this(1)
        {
        }

        [Constructable]
        public MoonstoneOre(int amount)
            : base(CraftResource.Moonstone, amount)
        {
        }

        public MoonstoneOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new MoonstoneIngot();
        }
    }
    public class BloodstoneOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Bloodstone; } }

        [Constructable]
        public BloodstoneOre()
            : this(1)
        {
        }

        [Constructable]
        public BloodstoneOre(int amount)
            : base(CraftResource.Bloodstone, amount)
        {
        }

        public BloodstoneOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new BloodstoneIngot();
        }
    }
    public class PlatinumOre : BaseOre
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Platinum; } }

        [Constructable]
        public PlatinumOre()
            : this(1)
        {
        }

        [Constructable]
        public PlatinumOre(int amount)
            : base(CraftResource.Platinum, amount)
        {
        }

        public PlatinumOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new PlatinumIngot();
        }
    }

}