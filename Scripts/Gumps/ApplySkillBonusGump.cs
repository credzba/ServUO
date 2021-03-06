using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
    public class ApplySkillBonusGump : BaseGump
    {
        public Item Item { get; set; }
        public AosSkillBonuses Bonuses { get; set; }
        public SkillName[] Skills { get; set; }
        public SkillName Selection { get; set; }
        public double Value { get; set; }
        public int Index { get; set; }

        public ApplySkillBonusGump(PlayerMobile pm, AosSkillBonuses bonuses, SkillName[] skills, double value, int index)
            : base(pm, 50, 50)
        {
            Item = bonuses.Owner;
            Bonuses = bonuses;
            Skills = skills;
            Value = value;
            Index = index;
        }

        public override void AddGumpLayout()
        {
            AddBackground(0, 0, 400, (Skills.Length * 22) + 65, 83);

            AddHtmlLocalized(15, 15, 400, 20, 1155610, 0x7FFF, false, false); // Please Chooose a Skill

            int y = 40;

            for (int i = 0; i < Skills.Length; i++)
            {
                var skill = Skills[i];

                AddButton(15, y, 4005, 4007, i + 100, GumpButtonType.Reply, 0);
                AddHtmlLocalized(50, y, 200, 20, SkillInfo.Table[(int)skill].Localization, 0x7FFF, false, false);

                y += 22;
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID >= 100)
            {
                int id = info.ButtonID - 100;

                if (id >= 0 && id < Skills.Length)
                {
                    Selection = Skills[id];
                    object text;

                    if (Item is BaseWeapon)
                    {
                        text = 1155611; // Are you sure you wish to apply the selected skill bonus to this weapon?
                    }
                    else
                    {
                        text = "Are you sure you wish to apply the selected skill bonus to this item?";
                    }

                    BaseGump.SendGump(new GenericConfirmCallbackGump<ApplySkillBonusGump>(User, string.Empty, text, this, null,
                    (m, gump) =>
                    {
                        if (gump.Item.IsChildOf(gump.User.Backpack) || gump.User.Items.Contains(gump.Item))
                        {
                            gump.User.SendLocalizedMessage(1155612); // A skill bonus has been applied to the item!
                            Bonuses.SetValues(gump.Index, gump.Selection, gump.Value);

                            gump.Item.InvalidateProperties();
                        }
                    },
                    (m, gump) =>
                    {
                        gump.Refresh();
                    }));
                }
            }
        }
    }
}
