using FlightsSimulator.Extentions;
using FlightsSimulator.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models;
using Models.Enums;
using Server.ErrorHandling;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightsSimulator.ViewModels
{
    public class SimulatorViewModel : ViewModelBase
    {
        #region Data Members
        readonly int _minTime = 1, _maxTime = 10;
        int _seconds = 5;
        bool _isSendingFlights = false;

        RelayCommand _startSendingFlightsCommand;
        RelayCommand _stopSendingFlightsCommand;

        Slider _slider;
        ListBox _flightsListBox;
        Button _sendButton;
        Button _stopButton;
        TextBlock _textBox;

        readonly HttpClient client;
        const string URL = "https://localhost:5001/api/airport/";
        Random _rnd;
        bool isDelayed;
        ObservableCollection<Flight> _sendedFlights;
        #endregion

        #region Public Properties
        public Grid GridSimulator { get; set; }
        #endregion

        public SimulatorViewModel()
        {
            isDelayed = false;
            _rnd = new Random();
            client = new HttpClient();
            _sendButton = new Button();
            _stopButton = new Button();
            _startSendingFlightsCommand = new RelayCommand(SendFlights, CanSendFlights);
            _stopSendingFlightsCommand = new RelayCommand(StopFlights, CanStopFlights);
            GridSimulator = new Grid();
            _slider = new Slider();
            _flightsListBox = new ListBox();
            _flightsListBox.DisplayMemberPath = $"Name";
            _sendedFlights = new ObservableCollection<Flight>();
            _textBox = new TextBlock();
            CreateElements();
        }

        private bool CanStopFlights()
        {
            return _isSendingFlights;
        }

        private bool CanSendFlights()
        {
            return !_isSendingFlights;
        }

        private void StopFlights()
        {
            _isSendingFlights = !_isSendingFlights;
            RaiseCanExecuteChanged();
        }

        async private void SendFlights()
        {
            _isSendingFlights = !_isSendingFlights;
            RaiseCanExecuteChanged();
            while (_isSendingFlights)
            {
                await SendNewFlight();
                await Task.Delay(_seconds * 1000);
            }
        }

        private async Task SendNewFlight()
        {
            string randomName;
            try
            {
                var res = await client.GetAsync("https://random-data-api.com/api/address/random_address");
                randomName = (await res?.Content.ReadAsAsync<DestinationHolder>()).city ?? "No_Name";
            }
            catch (Exception ex)
            {
                randomName = "No_Name";
                ex.LoggExceptionToText();
            }
            var direction = (FlightDirection)_rnd.Next(0, 2);
            var takeoffDest = direction == FlightDirection.Takeoff ? "TLV" : randomName;
            var landingDest = direction == FlightDirection.Landing ? "TLV" : randomName;
            isDelayed = true;
            Flight flight = new Flight
            {
                Date = isDelayed ? DateTime.Now + new TimeSpan(0, 0, _rnd.Next(8, 10)) : DateTime.Now ,              
                TakeoffDestination = takeoffDest,
                LandingDestination = landingDest,
                Direction = direction,

            };
            isDelayed = !isDelayed;
            _sendedFlights.Add(flight);
            await client.PostAsJsonAsync(URL + "flight", flight);
        }

        private void RaiseCanExecuteChanged()
        {
            _stopSendingFlightsCommand.RaiseCanExecuteChanged();
            _startSendingFlightsCommand.RaiseCanExecuteChanged();
        }

        private void CreateElements()
        {
            _flightsListBox.ItemsSource = _sendedFlights;
            for (int i = 0; i < 2; i++) GridSimulator.RowDefinitions.Add(new RowDefinition());
            _slider.Minimum = _minTime;
            _slider.Maximum = _maxTime;
            _slider.Value = _seconds;
            _slider.BorderThickness = new System.Windows.Thickness(10);
            _slider.Width = System.Windows.SystemParameters.PrimaryScreenWidth / 3;
            _slider.ValueChanged += (s, e) => OnValueChanged();
            _sendButton.Style = Styles.SendButton;
            _stopButton.Style = Styles.StopButton;
            _stopButton.Command = _stopSendingFlightsCommand;
            _sendButton.Command = _startSendingFlightsCommand;
            GridSimulator.SetElementToGrid(_sendButton, 1, 0);
            GridSimulator.SetElementToGrid(_stopButton, 1, 0);
            GridSimulator.SetElementToGrid(_textBox, 1, 0);
            GridSimulator.SetElementToGrid(_slider, 1, 0);
            GridSimulator.SetElementToGrid(_flightsListBox, 0, 0);
            _textBox.Text = $"Send a flight every {_seconds} seconds ";
        }

        private void OnValueChanged()
        {
            _seconds = (int)_slider.Value;
            _textBox.Text = $"Send a flight every {_seconds} seconds ";
        }
    }
}
