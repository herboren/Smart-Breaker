using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    class SmartBreaker : Program
    {
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10;
        }
        public void Main(string argument, UpdateType updateSource)
        {
            // Find respected blocks for role in 4 hours timer until battery power shut off.
            // Sensor block checks to see if the player has left the base, resets, upon rentry to base.
            IMyTimerBlock tblock = (IMyTimerBlock)GridTerminalSystem.GetBlockWithName("SmartBreaker");
            IMySensorBlock sblock = (IMySensorBlock)GridTerminalSystem.GetBlockWithName("MarcoPolo");

            // Find surface for button panel
            IMyTextSurfaceProvider buttonLcd = (IMyTextSurfaceProvider)GridTerminalSystem.GetBlockWithName("Departure Panel");
            IMyTextSurface buttonpanel = buttonLcd.GetSurface(0);

            // Configure a few properties on the button panel
            // This will tell the player if the status of their
            // departure is ready.
            //
            // Button is just for show, but not really needed
            buttonpanel.ContentType = ContentType.TEXT_AND_IMAGE;
            buttonpanel.Alignment = TextAlignment.CENTER;
            buttonpanel.TextPadding = 20.0f;
            buttonpanel.FontSize = 3.5f;
            buttonpanel.BackgroundColor = Color.White;

            // Covers 50 meter radius around sensor
            // will detect owner of base.
            sblock.LeftExtend = 50;
            sblock.RightExtend = 50;
            sblock.TopExtend = 50;
            sblock.BottomExtend = 50;
            sblock.FrontExtend = 50;
            sblock.BackExtend = 250;

            tblock.TriggerDelay = 10800; // 3 Hours (14400 seconds)

            // Check if sensor was enabled through the button.
            if (sblock.Enabled)
            {
                buttonpanel.FontColor = Color.Green;
                buttonpanel.WriteText("Wakeup\nEnabled");

                if (sblock.LastDetectedEntity.IsEmpty())
                {
                    if (!tblock.IsCountingDown)
                        // Owner went to space or somewhere else
                        tblock.StartCountdown();
                    
                    if (tblock.DetailedInfo.Contains("Time to trigger: 00:00:00"))
                    {
                        // Find funcitonal blocks onluy
                        List<IMyFunctionalBlock> everything = new List<IMyFunctionalBlock>();
                        GridTerminalSystem.GetBlocksOfType(everything);

                        // Checks if functional blocks on base or disabled
                        foreach (var b in everything)
                        {
                            if (!b.Enabled)
                                b.Enabled = true;
                        }
                    }                       
                }
                else
                    // Owners back at base.
                    tblock.StopCountdown();
            }
            else
            {
                buttonpanel.FontColor = Color.Red;
                buttonpanel.WriteText("Wakeup\nDisabled");
                
                // Disable Timer by hand
                tblock.StopCountdown();
            }
        }
    }
}
