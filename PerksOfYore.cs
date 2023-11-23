using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

using MelonLoader;
using UnityEngine;

// https://melonwiki.xyz/#/modders/quickstart

/*
desired mod features:
- noclip
- checkpoint / savestate (likely hard)
*/

namespace perksofyore
{
    public class PerksOfYore : MelonMod
    {
        GUIStyle heightStyle = new GUIStyle();
        public override void OnInitializeMelon()
        {
            heightStyle.fontSize = 35;
            heightStyle.normal.textColor = new Color(255.0f/255.0f, 95.0f/255.0f, 31.0f/255.0f);
            MelonEvents.OnGUI.Subscribe(DrawHeight);
        }
        public override void OnUpdate()
        {
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            //LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
            SetupHeight();
        }

        private float currentMetresUp;
        private float summitHeight;
        private float groundY;
        private Transform playerTransform;
        private void SetupHeight()
        {
            //if (!isMenu && !isDeathScene && !isEnding && !isCabin && !isCabin4)
            {
                var po = GameObject.FindGameObjectWithTag("Player");
                var so = GameObject.FindGameObjectWithTag("SummitBox");
                if (!po || !so)
                {
                    currentMetresUp = 0;
                    summitHeight = 0;
                    return;
                }
                playerTransform = po.GetComponent<Transform>();
                var peakSummit = so.GetComponent<StamperPeakSummit>();
                if (!playerTransform || !peakSummit)
                {
                    currentMetresUp = 0;
                    summitHeight = 0;
                    return;
                }
                groundY = playerTransform.position.y;
                summitHeight = peakSummit.transform.position.y - groundY + 0.75f;
            }
        }
        private void CalculateHeights()
        {
            if (!playerTransform)
            {
                currentMetresUp = 0;
                summitHeight = 0;
                return;
            }
            currentMetresUp = playerTransform.position.y - groundY;
        }
        private void DrawHeight()
        {
            CalculateHeights();
            if (currentMetresUp > 0 && summitHeight > 0)
            {
                //string s = String.Format("Height: {0:F2}m (%{1:F0}) {2:F2}", currentMetresUp, currentMetresUp / summitHeight * 100.0f, summitHeight);
                string s = String.Format("Height: {0:F1}m (%{1:F0})", currentMetresUp, currentMetresUp / summitHeight * 100.0f);
                GUI.Box(new Rect(6, 300, 100, 100), s, heightStyle);
            }
        }
    }
}
