using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.TextField;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Countdown1223 {
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {
        TextView textViewDays;
        TextView textViewEvent;
        RadioButton rbChristmas;
        RadioButton rbHalloween;
        RadioButton rbSummer;
        RadioButton rbOther;
        TextInputEditText textInputCustomDate;
        Button btnCount;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            //ShowMessage("On Create");
            SetupReferences();
            SubscribeEventHandlers();
            dateDict = new Dictionary<RadioButton, DateTime>();
            dateDict.Add(rbHalloween, new DateTime(today.Year, 10, 31));
            dateDict.Add(rbChristmas, new DateTime(today.Year, 12, 24));
            dateDict.Add(rbSummer, new DateTime(today.Year, 6, 24));
        }

        private void SubscribeEventHandlers() {
            //rbChristmas.CheckedChange += RbChristmas_CheckedChange;
            //rbHalloween.CheckedChange += RbHalloween_CheckedChange;
            //rbSummer.CheckedChange += RbSummer_CheckedChange;
            rbOther.CheckedChange += RbOther_CheckedChange;
            btnCount.Click += BtnCount_Click;
            rbHalloween.CheckedChange += RadioButton_CheckedChange;
            rbChristmas.CheckedChange+= RadioButton_CheckedChange;
            rbSummer.CheckedChange += RadioButton_CheckedChange;
        }
        IDictionary<RadioButton, DateTime> dateDict;
        private void RadioButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e) {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) {
                DateTime dateFromDictionary = dateDict[rb];
                endDate = CountEndDate(dateFromDictionary.Day, dateFromDictionary.Month, dateFromDictionary.Year);
                textViewDays.Text = CountTheDays();
                //ViewTheCountOfDays("Summer");
            }
        }

        private void BtnCount_Click(object sender, EventArgs e) {
            //string[] dateArray = textInputCustomDate.Text.Split('.', '/', '-');
            //endDate = new DateTime(Convert.ToInt32(dateArray[2]), Convert.ToInt32(dateArray[1]), Convert.ToInt32(dateArray[0]));
            //ViewTheCountOfDays("my event");
            CultureInfo culture = CultureInfo.CreateSpecificCulture("cs-CZ");
            DateTimeStyles styles = DateTimeStyles.None;
            bool parseOk = DateTime.TryParse(textInputCustomDate.Text, culture, styles, out DateTime customDate);
            if (parseOk) {
                endDate = customDate;
                ViewTheCountOfDays("my event");
            }
            else {
                ShowMessage("error :-( ");
            }
        }

        private void RbOther_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e) {
            if (rbOther.Checked) {
                textInputCustomDate.Visibility = Android.Views.ViewStates.Visible;
                btnCount.Visibility = Android.Views.ViewStates.Visible;
            }
            else {
                textInputCustomDate.Visibility = Android.Views.ViewStates.Invisible;
                btnCount.Visibility = Android.Views.ViewStates.Invisible;
            }
        }

        private void RbSummer_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e) {
            if (rbSummer.Checked) {
                endDate = CountEndDate(24, 6, today.Year);
                ViewTheCountOfDays("Summer");
            }
        }

        private void RbHalloween_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e) {
            if (rbHalloween.Checked) {
                endDate = CountEndDate(31, 10, today.Year);
                ViewTheCountOfDays("Halloween");
            }
        }

        DateTime today = DateTime.Today;
        DateTime endDate;
        TimeSpan span;
        private void RbChristmas_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e) {
            if (rbChristmas.Checked) {
                endDate = CountEndDate(24, 12, today.Year);
                ViewTheCountOfDays("Christmas");
            }
        }

        private void ViewTheCountOfDays(string holiday) {
            textViewDays.Text = CountTheDays();
            textViewEvent.Text = $"days until {holiday}";
        }

        private string CountTheDays() {
            span = endDate - today;
            return span.Days.ToString();
        }

        private DateTime CountEndDate(int day, int month, int year) {
            endDate = new DateTime(year, month, day);
            if (today.DayOfYear > endDate.DayOfYear) {
                endDate = endDate.AddYears(1);
            }
            return new DateTime(endDate.Year, month, day);
        }

        private void SetupReferences() {
            textViewDays = FindViewById<TextView>(Resource.Id.textViewDays);
            textViewEvent = FindViewById<TextView>(Resource.Id.textViewEvent);
            rbChristmas = FindViewById<RadioButton>(Resource.Id.radioButtonTopmost);
            rbHalloween = FindViewById<RadioButton>(Resource.Id.radioButtonMiddle);
            rbSummer = FindViewById<RadioButton>(Resource.Id.radioButtonBottomMost);
            rbOther = FindViewById<RadioButton>(Resource.Id.radioButtonOther);
            textInputCustomDate = FindViewById<TextInputEditText>(Resource.Id.textInputCustomDate);
            btnCount = FindViewById<Button>(Resource.Id.buttonCount);
        }







        //protected override void OnStart() {
        //    base.OnStart();
        //    ShowMessage("On Start");
        //}
        //protected override void OnResume() {
        //    base.OnResume();
        //    ShowMessage("On Resume");
        //}
        //protected override void OnPause() {
        //    base.OnPause();
        //    ShowMessage("On Pause");
        //}
        //protected override void OnStop() {
        //    base.OnStop();
        //    ShowMessage("On Stop");
        //}
        //protected override void OnDestroy() {
        //    base.OnDestroy();
        //    ShowMessage("On Destroy");
        //}
        //protected override void OnRestart() {
        //    base.OnRestart();
        //    ShowMessage("On Restart");
        //}

        public void ShowMessage(string message) {
            System.Diagnostics.Debug.WriteLine(message);
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}