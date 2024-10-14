using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace sunnD.Editor
{
    public class UIControllerAttribute : OdinAttributeProcessor<UIController>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "StartScreen":
                    attributes.Add(new BoxGroupAttribute("Config"));
                    attributes.Add(new HorizontalGroupAttribute("Config/1"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    break;
                case "UIPath":
                    attributes.Add(new HorizontalGroupAttribute("Config/1"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    break;
                case "QuitInMenu":
                    attributes.Add(new BoxGroupAttribute("Config"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    break;
                case "QuitCountBack":
                    attributes.Add(new HorizontalGroupAttribute("Config/2"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new ShowIfAttribute("QuitInMenu"));
                    attributes.Add(new LabelTextAttribute("Count Back"));
                    break;
                case "DelayQuitBack":
                    attributes.Add(new HorizontalGroupAttribute("Config/2"));
                    attributes.Add(new LabelWidthAttribute(80f));
                    attributes.Add(new ShowIfAttribute("QuitInMenu"));
                    attributes.Add(new LabelTextAttribute("Delay Back"));
                    break;
            }
        }
    }
}
