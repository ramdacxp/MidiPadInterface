using Midi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPadInterface
{
    public class MidiPad : IDisposable
    {
        private const string _deviceName = "CMD DC-1";
        private const Channel _deviceChannel = Channel.Channel6;

        private InputDevice _inputDevice;
        private OutputDevice _outputDevice;

        private Dictionary<ValueControl, int> _controlValues;

        public MidiPad()
        {
            _controlValues = new Dictionary<ValueControl, int>();
        }

        public bool IsConnected
        {
            get { return (_inputDevice != null) && (_outputDevice != null); }
        }

        public void Connect()
        {
            if (_inputDevice == null)
            {
                _inputDevice = (from device in InputDevice.InstalledDevices
                                where device.Name == _deviceName
                                select device).FirstOrDefault();
                if (_inputDevice != null)
                {
                    _inputDevice.Open();

                    _inputDevice.NoteOn += OnInputDeviceNoteOn;
                    _inputDevice.NoteOff += OnInputDeviceNoteOff;
                    _inputDevice.ControlChange += OnInputDeviewControlChange;

                    _inputDevice.StartReceiving(null);
                }
            }

            if (_outputDevice == null)
            {
                _outputDevice = (from device in OutputDevice.InstalledDevices
                                 where device.Name == _deviceName
                                 select device).FirstOrDefault();
                if (_outputDevice != null)
                {
                    _outputDevice.Open();
                    _outputDevice.SilenceAllNotes();
                }
            }
        }

     
        public void DisConnect()
        {
            if (_inputDevice != null)
            {
                _inputDevice.StopReceiving();
                _inputDevice.Close();
                _inputDevice.RemoveAllEventHandlers();

                _inputDevice = null;
            }

            if (_outputDevice != null)
            {
                _outputDevice.Close();
                _outputDevice = null;
            }
        }

        public void Dispose()
        {
            DisConnect();
        }

        public delegate void ButtonClickEventHandler(Button button, ButtonClickState state);

        public event ButtonClickEventHandler ButtonClick;

        protected void OnButtonClick(Button button, ButtonClickState state)
        {
            if (ButtonClick != null)
            {
                ButtonClick(button, state);
            }
        }

        private void OnInputDeviceNoteOn(NoteOnMessage msg)
        {
            OnButtonClick((Button)msg.Pitch, ButtonClickState.Pressed);
        }

        private void OnInputDeviceNoteOff(NoteOffMessage msg)
        {
            OnButtonClick((Button)msg.Pitch, ButtonClickState.Released);
        }

        public void SetButton(Button button, ButtonIlluminationState state)
        {
            if (_outputDevice != null)
            {
                _outputDevice.SendNoteOn(_deviceChannel, (Pitch)button, (int)state);
            }
        }

        public void SetButton(ButtonGroup group, ButtonIlluminationState state)
        {
            if (_outputDevice != null)
            {
                if (group == ButtonGroup.JogButtons || group == ButtonGroup.AllButtons)
                {
                    for (int i = (int)Button.JogPad01; i <= (int)Button.JogPad08; i++)
                    {
                        _outputDevice.SendNoteOn(Channel.Channel6, (Pitch)i, (int)state);
                    }
                }

                if (group == ButtonGroup.SmallPadButtons || group == ButtonGroup.AllButtons)
                {
                    for (int i = (int)Button.SmallPad01; i <= (int)Button.SmallPad08; i++)
                    {
                        _outputDevice.SendNoteOn(Channel.Channel6, (Pitch)i, (int)state);
                    }
                }

                if (group == ButtonGroup.LargePadButtons || group == ButtonGroup.AllButtons)
                {
                    for (int i = (int)Button.LargePad01; i <= (int)Button.LargePad16; i++)
                    {
                        _outputDevice.SendNoteOn(Channel.Channel6, (Pitch)i, (int)state);
                    }
                }
            }
        }

        public delegate void ControlChangeEventHandler(ValueControl control, Direction direction);

        public event ControlChangeEventHandler ControlChange;

        protected void OnControlChange(ValueControl control, Direction direction)
        {
            if (ControlChange != null)
            {
                ControlChange(control, direction);
            }
        }

        private void OnInputDeviewControlChange(ControlChangeMessage msg)
        {
            var control = (ValueControl)msg.Control;
            var direction = msg.Value == 63 ? Direction.Down : Direction.Up;

            // update madded values
            var value = GetControlValue(control);
            value += (int) direction;
            SetControlValue(control, value);

            // send event
            OnControlChange(control, direction);
        }

        public int GetControlValue(ValueControl control)
        {
            if ( _controlValues.ContainsKey(control))
            {
                return _controlValues[control];
            }
            else
            {
                return 0;
            }
        }

        public void SetControlValue(ValueControl control, int value)
        {
            if (value < 0) value = 0;
            if (value > 15) value = 15;
            _controlValues[control] = value;
        }

        public void ShowControlValue(ValueControl control)
        {
            if (_outputDevice != null)
            {
                var value = GetControlValue(control);
                _outputDevice.SendControlChange(_deviceChannel, (Control) control, value);
            }
        }



    }
}
