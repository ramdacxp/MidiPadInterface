using Midi;

namespace MidiPadInterface
{
    public enum Button
    {
        JogPad01 = Pitch.CNeg1,
        JogPad02 = Pitch.CSharpNeg1,
        JogPad03 = Pitch.DNeg1,
        JogPad04 = Pitch.DSharpNeg1,
        JogPad05 = Pitch.ENeg1,
        JogPad06 = Pitch.FNeg1,
        JogPad07 = Pitch.FSharpNeg1,
        JogPad08 = Pitch.GNeg1,

        JogWheel = Pitch.GSharp1,

        SmallPad01 = Pitch.E0,
        SmallPad02 = Pitch.F0,
        SmallPad03 = Pitch.FSharp0,
        SmallPad04 = Pitch.G0,
        SmallPad05 = Pitch.GSharp0,
        SmallPad06 = Pitch.A0,
        SmallPad07 = Pitch.ASharp0,
        SmallPad08 = Pitch.B0,

        LargePad01 = Pitch.C2,
        LargePad02 = Pitch.CSharp2,
        LargePad03 = Pitch.D2,
        LargePad04 = Pitch.DSharp2,
        LargePad05 = Pitch.E2,
        LargePad06 = Pitch.F2,
        LargePad07 = Pitch.FSharp2,
        LargePad08 = Pitch.G2,
        LargePad09 = Pitch.GSharp2,
        LargePad10 = Pitch.A2,
        LargePad11 = Pitch.ASharp2,
        LargePad12 = Pitch.B2,
        LargePad13 = Pitch.C3,
        LargePad14 = Pitch.CSharp3,
        LargePad15 = Pitch.D3,
        LargePad16 = Pitch.DSharp3
    }
}