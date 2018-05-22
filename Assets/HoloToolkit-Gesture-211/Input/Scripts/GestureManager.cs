using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    public class GestureManager : Singleton<GestureManager>
    {
        // Tap and Navigation gesture recognizer.
        public GestureRecognizer NavigationRecognizer { get; private set; }

        // Manipulation gesture recognizer.
        public GestureRecognizer ManipulationRecognizer { get; private set; }

        // Currently active gesture recognizer.
        public GestureRecognizer ActiveRecognizer { get; private set; }

        public bool IsNavigating { get; private set; }

        public Vector3 NavigationPosition { get; private set; }

        public bool IsManipulating { get; private set; }

        public Vector3 ManipulationPosition { get; private set; }

        void Awake()
        {
            /* TODO: DEVELOPER CODING EXERCISE 2.b */

            // 2.b: Instantiate the NavigationRecognizer.
            NavigationRecognizer = new GestureRecognizer();


            // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
            NavigationRecognizer.SetRecognizableGestures(
                GestureSettings.Tap |
                GestureSettings.NavigationX|
                GestureSettings.NavigationY);


            // 2.b: Register for the Tapped with the NavigationRecognizer_Tapped function.
            NavigationRecognizer.Tapped += NavigationRecognizer_Tapped;
            // 2.b: Register for the NavigationStarted with the NavigationRecognizer_NavigationStarted function.
            NavigationRecognizer.NavigationStarted += NavigationRecognizer_NavigationStarted;
            // 2.b: Register for the NavigationUpdated with the NavigationRecognizer_NavigationUpdated function.
            NavigationRecognizer.NavigationUpdated += NavigationRecognizer_NavigationUpdated;
            // 2.b: Register for the NavigationCompleted with the NavigationRecognizer_NavigationCompleted function. 
            NavigationRecognizer.NavigationCompleted += NavigationRecognizer_NavigationCompleted;
            // 2.b: Register for the NavigationCanceled with the NavigationRecognizer_NavigationCanceled function. 
            NavigationRecognizer.NavigationCanceled += NavigationRecognizer_NavigationCanceled;

            // Instantiate the ManipulationRecognizer.
            ManipulationRecognizer = new GestureRecognizer();

            // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
            ManipulationRecognizer.SetRecognizableGestures(
                GestureSettings.ManipulationTranslate);

            // Register for the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStarted += ManipulationRecognizer_ManipulationStarted;
            ManipulationRecognizer.ManipulationUpdated += ManipulationRecognizer_ManipulationUpdated;
            ManipulationRecognizer.ManipulationCompleted += ManipulationRecognizer_ManipulationCompleted;
            ManipulationRecognizer.ManipulationCanceled += ManipulationRecognizer_ManipulationCanceled;

            ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
            NavigationRecognizer.Tapped -= NavigationRecognizer_Tapped;

            NavigationRecognizer.NavigationStarted -= NavigationRecognizer_NavigationStarted;
            NavigationRecognizer.NavigationUpdated -= NavigationRecognizer_NavigationUpdated;
            NavigationRecognizer.NavigationCompleted -= NavigationRecognizer_NavigationCompleted;
            NavigationRecognizer.NavigationCanceled -= NavigationRecognizer_NavigationCanceled;

            // Unregister the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStarted -= ManipulationRecognizer_ManipulationStarted;
            ManipulationRecognizer.ManipulationUpdated -= ManipulationRecognizer_ManipulationUpdated;
            ManipulationRecognizer.ManipulationCompleted -= ManipulationRecognizer_ManipulationCompleted;
            ManipulationRecognizer.ManipulationCanceled -= ManipulationRecognizer_ManipulationCanceled;
        }

        /// <summary>
        /// Revert back to the default GestureRecognizer.
        /// </summary>
        public void ResetGestureRecognizers()
        {
            // Default to the navigation gestures.
            Transition(NavigationRecognizer);
        }

        /// <summary>
        /// Transition to a new GestureRecognizer.
        /// </summary>
        /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (ActiveRecognizer != null)
            {
                if (ActiveRecognizer == newRecognizer)
                {
                    return;
                }

                ActiveRecognizer.CancelGestures();
                ActiveRecognizer.StopCapturingGestures();
            }

            newRecognizer.StartCapturingGestures();
            ActiveRecognizer = newRecognizer;
        }

        private void NavigationRecognizer_NavigationStarted(NavigationStartedEventArgs obj)
        {
            // 2.b: Set IsNavigating to be true.
            IsNavigating = true;

            // 2.b: Set NavigationPosition to be Vector3.zero.
            NavigationPosition = Vector3.zero;
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsNavigating = true;

                NavigationPosition = Vector3.zero;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("OnNavigationStarted", NavigationPosition);
            }

        }

		private void NavigationRecognizer_NavigationUpdated(NavigationUpdatedEventArgs obj)
        {
            // 2.b: Set IsNavigating to be true.
            IsNavigating = true;

            // 2.b: Set NavigationPosition to be obj.normalizedOffset.
            NavigationPosition = obj.normalizedOffset;
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsNavigating = true;

                NavigationPosition = obj.normalizedOffset;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("OnNavigationUpdated", NavigationPosition);
            }

        }

		private void NavigationRecognizer_NavigationCompleted(NavigationCompletedEventArgs obj)
        {
            // 2.b: Set IsNavigating to be false.
            IsNavigating = false;
            
        }

        private void NavigationRecognizer_NavigationCanceled(NavigationCanceledEventArgs obj)
        {
            // 2.b: Set IsNavigating to be false.
            IsNavigating = false;
        }

        private void ManipulationRecognizer_ManipulationStarted(ManipulationStartedEventArgs obj)
        {
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsManipulating = true;

                ManipulationPosition = Vector3.zero;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", ManipulationPosition);
            }
        }

        private void ManipulationRecognizer_ManipulationUpdated(ManipulationUpdatedEventArgs obj)
        {
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsManipulating = true;

                ManipulationPosition = obj.cumulativeDelta;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", ManipulationPosition);
            }
        }

        private void ManipulationRecognizer_ManipulationCompleted(ManipulationCompletedEventArgs obj)
        {
            IsManipulating = false;
        }

        private void ManipulationRecognizer_ManipulationCanceled(ManipulationCanceledEventArgs obj)
        {
            IsManipulating = false;
        }

        private void NavigationRecognizer_Tapped(TappedEventArgs obj)
        {
            GameObject EditPanel = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel();
            GameObject TipText = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackTipText();

            if (TipText.active == true)
            {
                Debug.Log("你在截图模式中点了一下:" + DateTime.Now.ToString());
                TipText.SetActive(false);
                Sprite sprite =GameObject.Find("ObjectManager").GetComponent<ObjectManager>().ShotOnMainCam();

                EditPanel.SetActive(true);
                GameObject CropBox = GameObject.Find("CropBox");
                CropBox.GetComponent<RectTransform>().localScale= new Vector3(0.5f,0.5f,0.5f);
                CropBox.transform.localPosition = new Vector3(0f,0f, -0.06f);
                GameObject.Find("CropCam").GetComponent<Camera>().orthographicSize = (float)0.22;
                //替换编辑板的背景
                GameObject panel = GameObject.Find("CropBoard");
                panel.GetComponent<Image>().sprite = sprite;

            }
            else
            {
                Debug.Log("你在Panel模式中点了一下:" + DateTime.Now.ToString());
            }


            GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnSelect");
            }
        }
    }
}