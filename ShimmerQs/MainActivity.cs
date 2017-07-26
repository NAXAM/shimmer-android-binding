using Android.App;
using Android.Widget;
using Android.OS;
using Com.Facebook.Shimmer;
using Android.Animation;

namespace ShimmerQs
{
    [Activity(Label = "ShimmerQs", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ShimmerFrameLayout mShimmerViewContainer;
        private Button[] mPresetButtons;
        private int mCurrentPreset = -1;
        private Toast mPresetToast;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            mShimmerViewContainer = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container);

            mPresetButtons = new Button[]{
                FindViewById<Button>(Resource.Id.preset_button0),
                FindViewById<Button>(Resource.Id.preset_button1),
                FindViewById<Button>(Resource.Id.preset_button2),
                FindViewById<Button>(Resource.Id.preset_button3),
                FindViewById<Button>(Resource.Id.preset_button4),
            };
            for (int i = 0; i < mPresetButtons.Length; i++)
            {
                int preset = i;
                mPresetButtons[i].Click += (s, e) =>
                {
                    SelectPreset(preset, true);
                };
            }
        }


        protected override void OnStart()
        {
            base.OnStart();
            SelectPreset(0, false);
        }


        protected override void OnResume()
        {
            base.OnResume();
            mShimmerViewContainer.StartShimmerAnimation();
        }


        protected override void OnPause()
        {
            mShimmerViewContainer.StopShimmerAnimation();
            base.OnPause();
        }

        private void SelectPreset(int preset, bool showToast)
        {
            if (mCurrentPreset == preset)
            {
                return;
            }
            if (mCurrentPreset >= 0)
            {
                mPresetButtons[mCurrentPreset].SetBackgroundResource(Resource.Color.preset_button_background);
            }
            mCurrentPreset = preset;
            mPresetButtons[mCurrentPreset].SetBackgroundResource(Resource.Color.preset_button_background_selected);

            // Save the state of the animation
            bool isPlaying = mShimmerViewContainer.IsAnimationStarted;

            // Reset all parameters of the shimmer animation
            mShimmerViewContainer.UseDefaults();

            // If a toast is already showing, hide it
            if (mPresetToast != null)
            {
                mPresetToast.Cancel();
            }

            switch (preset)
            {
                default:
                case 0:
                    // Default
                    mPresetToast = Toast.MakeText(this, "Default", ToastLength.Short);
                    break;
                case 1:
                    // Slow and reverse
                    mShimmerViewContainer.Duration = (5000);
                    mShimmerViewContainer.RepeatMode = 2;
                    mPresetToast = Toast.MakeText(this, "Slow and reverse", ToastLength.Short);
                    break;
                case 2:
                    // Thin, straight and transparent
                    mShimmerViewContainer.BaseAlpha = 0.1f;
                    mShimmerViewContainer.Dropoff = 0.1f;
                    mShimmerViewContainer.Tilt = 0;
                    mPresetToast = Toast.MakeText(this, "Thin, straight and transparent", ToastLength.Short);
                    break;
                case 3:
                    // Sweep angle 90
                    mShimmerViewContainer.Angle = ShimmerFrameLayout.MaskAngle.Cw90;
                    mPresetToast = Toast.MakeText(this, "Sweep angle 90", ToastLength.Short);
                    break;
                case 4:
                    // Spotlight
                    mShimmerViewContainer.BaseAlpha = (0);
                    mShimmerViewContainer.Duration = (2000);
                    mShimmerViewContainer.Dropoff = (0.1f);
                    mShimmerViewContainer.Intensity = (0.35f);
                    mShimmerViewContainer.SetMaskShape(ShimmerFrameLayout.MaskShape.Radial);
                    mPresetToast = Toast.MakeText(this, "Spotlight", ToastLength.Short);
                    break;
            }

            // Show toast describing the chosen preset, if necessary
            if (showToast)
            {
                mPresetToast.Show();
            }

            // Setting a value on the shimmer layout stops the animation. Restart it, if necessary.
            if (isPlaying)
            {
                mShimmerViewContainer.StartShimmerAnimation();
            }
        }
    }
}

