using MidiPadInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiPadInterfaceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pad = new MidiPad())
            {
                pad.Connect();
                if (!pad.IsConnected)
                {
                    Console.WriteLine("No MIDI interface found.");
                    return;
                }

                pad.SetButton(ButtonGroup.AllButtons, ButtonIlluminationState.Off);

                pad.ButtonClick += (Button button, ButtonClickState state) =>
                {
                    Console.WriteLine("{0} -> {1}", button, state);

                    pad.SetButton(button,
                        (state == ButtonClickState.Pressed) ? ButtonIlluminationState.On : ButtonIlluminationState.Off);
                };

                pad.ControlChange += (ValueControl control, Direction direction) =>
                {
                    int value = pad.GetControlValue(control);
                    Console.WriteLine("{0} -{1}-> {2}", control, direction, value);

                    pad.ShowControlValue(control);
                };

                Console.ReadLine();
            }
        }
     
    }
}
