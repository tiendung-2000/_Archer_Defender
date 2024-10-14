using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace sunnD.Editor
{
    public class UIBaseAttribute : OdinAttributeProcessor<UIBase>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "UILayer":
                    attributes.Add(new BoxGroupAttribute("Config"));
                    attributes.Add(new EnumToggleButtonsAttribute());
                    attributes.Add(new LabelWidthAttribute(80f));
                    break;
                case "UsingSafeArea":
                    attributes.Add(new HorizontalGroupAttribute("Config/1", 0.35f));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new LabelTextAttribute("Safe Area"));
                    break;
                case "SafePanel":
                    attributes.Add(new HorizontalGroupAttribute("Config/1"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new ShowIfAttribute("UsingSafeArea"));
                    break;
                case "UsingStrechBackground":
                    attributes.Add(new HorizontalGroupAttribute("Config/2", 0.35f));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new LabelTextAttribute("Strech Back"));
                    break;
                case "BackRect":
                    attributes.Add(new HorizontalGroupAttribute("Config/2"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new ShowIfAttribute("UsingStrechBackground"));
                    break;
                case "DelayClick":
                    attributes.Add(new HorizontalGroupAttribute("Config/3", 0.35f));
                    attributes.Add(new LabelWidthAttribute(80f));
                    break;

                case "Initialize":
                    attributes.Add(new BoxGroupAttribute("Buttons"));
                    attributes.Add(new HorizontalGroupAttribute("Buttons/1", 0.33f));
                    attributes.Add(new ButtonAttribute());
                    break;
                case "Show":
                    attributes.Add(new HorizontalGroupAttribute("Buttons/1", 0.33f));
                    attributes.Add(new ButtonAttribute());
                    break;
                case "Hide":
                    attributes.Add(new HorizontalGroupAttribute("Buttons/1"));
                    attributes.Add(new ButtonAttribute());
                    break;

                default:
                    attributes.Add(new BoxGroupAttribute());
                    break;
            }
        }
    }
}
