using System;
using System.Text;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Utils;
using VRageMath;

// ReSharper disable ImplicitlyCapturedClosure

namespace Sisk.Utils.TerminalControls {

    public static class Extensions {

        public static IMyTerminalAction CreateButtonAction<TBlock>(this IMyTerminalControlButton button) where TBlock : IMyTerminalBlock {
            return CreateButtonAction<TBlock>(button, TerminalActionIcons.TOGGLE);
        }

        public static IMyTerminalAction CreateButtonAction<TBlock>(this IMyTerminalControlButton button, string iconPath) where TBlock : IMyTerminalBlock {
            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>(button.Id);
            action.Name = MyTexts.Get(button.Title);
            action.Action = button.Action;
            action.Icon = iconPath;
            action.Enabled = button.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateDecreaseAction<TBlock>(this IMyTerminalControlSlider slider, float step, Func<IMyTerminalBlock, float> min, Func<IMyTerminalBlock, float> max) where TBlock : IMyTerminalBlock {
            return CreateDecreaseAction<TBlock>(slider, step, min, max, TerminalActionIcons.DECREASE);
        }

        public static IMyTerminalAction CreateDecreaseAction<TBlock>(this IMyTerminalControlSlider slider, float step, Func<IMyTerminalBlock, float> min, Func<IMyTerminalBlock, float> max, string iconPath) where TBlock : IMyTerminalBlock {
            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>("Decrease" + ((IMyTerminalControl)slider).Id);
            action.Name = Combine(MySpaceTexts.ToolbarAction_Decrease, slider.Title);
            action.Action = block => slider.Setter(block, MathHelper.Clamp(slider.Getter(block) - max(block) * step, min(block), max(block)));
            action.Writer = slider.Writer;
            action.Icon = iconPath;
            action.Enabled = slider.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateIncreaseAction<TBlock>(this IMyTerminalControlSlider slider, float step, Func<IMyTerminalBlock, float> min, Func<IMyTerminalBlock, float> max) where TBlock : IMyTerminalBlock {
            return CreateIncreaseAction<TBlock>(slider, step, min, max, TerminalActionIcons.INCREASE);
        }

        public static IMyTerminalAction CreateIncreaseAction<TBlock>(this IMyTerminalControlSlider slider, float step, Func<IMyTerminalBlock, float> min, Func<IMyTerminalBlock, float> max, string iconPath) where TBlock : IMyTerminalBlock {
            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>("Increase" + ((IMyTerminalControl)slider).Id);
            action.Name = Combine(MySpaceTexts.ToolbarAction_Increase, slider.Title);
            action.Action = block => slider.Setter(block, MathHelper.Clamp(slider.Getter(block) + max(block) * step, min(block), max(block)));
            action.Writer = slider.Writer;
            action.Icon = iconPath;
            action.Enabled = slider.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateOffAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch) where TBlock : IMyTerminalBlock {
            return @switch.CreateOffAction<TBlock>(TerminalActionIcons.SWITCH_OFF);
        }

        public static IMyTerminalAction CreateOffAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch, string iconPath) where TBlock : IMyTerminalBlock {
            var onText = MyTexts.Get(@switch.OnText);
            var offText = MyTexts.Get(@switch.OffText);
            var name = GetTitle(@switch.Title).Append(" ").Append(offText);

            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>(((IMyTerminalControl)@switch).Id + "_Off");
            action.Name = name;

            action.Action = block => @switch.Setter(block, false);
            action.Writer = (block, sb) => sb.Append(@switch.Getter(block) ? onText : offText);
            action.Icon = iconPath;
            action.Enabled = @switch.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateOnAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch) where TBlock : IMyTerminalBlock {
            return @switch.CreateOnAction<TBlock>(TerminalActionIcons.SWITCH_ON);
        }

        public static IMyTerminalAction CreateOnAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch, string iconPath) where TBlock : IMyTerminalBlock {
            var onText = MyTexts.Get(@switch.OnText);
            var offText = MyTexts.Get(@switch.OffText);
            var name = GetTitle(@switch.Title).Append(" ").Append(onText);

            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>(((IMyTerminalControl)@switch).Id + "_On");
            action.Name = name;

            action.Action = block => @switch.Setter(block, true);
            action.Writer = (block, sb) => sb.Append(@switch.Getter(block) ? onText : offText);
            action.Icon = iconPath;
            action.Enabled = @switch.Enabled;

            return action;
        }

        public static IMyTerminalControlProperty<bool> CreateProperty<TBlock>(this IMyTerminalControlOnOffSwitch @switch) where TBlock : IMyTerminalBlock {
            return @switch.CreateProperty<TBlock, bool>();
        }

        public static IMyTerminalControlProperty<bool> CreateProperty<TBlock>(this IMyTerminalControlCheckbox checkbox) where TBlock : IMyTerminalBlock {
            return checkbox.CreateProperty<TBlock, bool>();
        }

        public static IMyTerminalControlProperty<float> CreateProperty<TBlock>(this IMyTerminalControlSlider slider) where TBlock : IMyTerminalBlock {
            return slider.CreateProperty<TBlock, float>();
        }

        public static IMyTerminalAction CreateResetAction<TBlock>(this IMyTerminalControlSlider slider, Func<IMyTerminalBlock, float> defaultValue) where TBlock : IMyTerminalBlock {
            return CreateResetAction<TBlock>(slider, defaultValue, TerminalActionIcons.RESET);
        }

        public static IMyTerminalAction CreateResetAction<TBlock>(this IMyTerminalControlSlider slider, Func<IMyTerminalBlock, float> defaultValue, string iconPath) where TBlock : IMyTerminalBlock {
            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>("Reset" + ((IMyTerminalControl)slider).Id);
            action.Name = Combine(MySpaceTexts.ToolbarAction_Reset, slider.Title);
            action.Action = block => slider.Setter(block, defaultValue(block));
            action.Writer = slider.Writer;
            action.Icon = iconPath;
            action.Enabled = slider.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateToggleAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch) where TBlock : IMyTerminalBlock {
            return @switch.CreateToggleAction<TBlock>(TerminalActionIcons.TOGGLE);
        }

        public static IMyTerminalAction CreateToggleAction<TBlock>(this IMyTerminalControlCheckbox checkbox) where TBlock : IMyTerminalBlock {
            return checkbox.CreateToggleAction<TBlock>(TerminalActionIcons.TOGGLE);
        }

        public static IMyTerminalAction CreateToggleAction<TBlock>(this IMyTerminalControlOnOffSwitch @switch, string iconPath) where TBlock : IMyTerminalBlock {
            var onText = MyTexts.Get(@switch.OnText);
            var offText = MyTexts.Get(@switch.OffText);
            var name = GetTitle(@switch.Title).Append(" ").Append(onText).Append("/").Append(offText);

            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>(((IMyTerminalControl)@switch).Id);
            action.Name = name;

            action.Action = block => @switch.Setter(block, !@switch.Getter(block));
            action.Writer = (block, sb) => sb.Append(@switch.Getter(block) ? onText : offText);
            action.Icon = iconPath;
            action.Enabled = @switch.Enabled;

            return action;
        }

        public static IMyTerminalAction CreateToggleAction<TBlock>(this IMyTerminalControlCheckbox checkbox, string iconPath) where TBlock : IMyTerminalBlock {
            var onText = MyTexts.Get(checkbox.OnText);
            var offText = MyTexts.Get(checkbox.OffText);
            var name = GetTitle(checkbox.Title).Append(" ").Append(onText).Append("/").Append(offText);

            var action = MyAPIGateway.TerminalControls.CreateAction<TBlock>(((IMyTerminalControl)checkbox).Id);
            action.Name = name;

            action.Action = block => checkbox.Setter(block, !checkbox.Getter(block));
            action.Writer = (block, sb) => sb.Append(checkbox.Getter(block) ? onText : offText);
            action.Icon = iconPath;
            action.Enabled = checkbox.Enabled;

            return action;
        }

        private static StringBuilder Combine(MyStringId prefix, MyStringId title) {
            var sb = new StringBuilder();
            var original = MyTexts.Get(prefix);
            if (original.Length > 0) {
                sb.Append(original).Append(" ");
            }

            return sb.Append(MyTexts.GetString(title)).TrimTrailingWhitespace();
        }

        private static IMyTerminalControlProperty<TValue> CreateProperty<TBlock, TValue>(this IMyTerminalValueControl<TValue> control) where TBlock : IMyTerminalBlock {
            var property = MyAPIGateway.TerminalControls.CreateProperty<TValue, TBlock>(((IMyTerminalControl)control).Id);
            property.Getter = control.Getter;
            property.Setter = control.Setter;
            property.SupportsMultipleBlocks = false;
            property.Enabled = ((IMyTerminalControl)control).Enabled;

            return property;
        }

        private static StringBuilder GetTitle(MyStringId title) {
            var stringBuilder = new StringBuilder();
            var str = MyTexts.GetString(title);
            if (str.Length > 0) {
                stringBuilder.Append(str);
            }

            return stringBuilder;
        }

        private static StringBuilder TrimTrailingWhitespace(this StringBuilder sb) {
            var length = sb.Length;
            while (length > 0 && (sb[length - 1] == ' ' || sb[length - 1] == '\r' || sb[length - 1] == '\n')) {
                --length;
            }

            sb.Length = length;
            return sb;
        }
    }
}