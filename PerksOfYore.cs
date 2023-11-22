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
        private GameObject playerGameobject;
        private Transform playerTransformStartPosition;
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
                playerGameobject = new GameObject("PlayerStartPosition");
                playerGameobject.transform.position = playerTransform.position;
                playerTransformStartPosition = playerGameobject.transform;
                playerTransformStartPosition.position = playerTransform.position;
                var ground = new Vector3(peakSummit.transform.position.x, playerTransformStartPosition.position.y, peakSummit.transform.position.z);
                summitHeight = Vector3.Distance(ground, peakSummit.transform.position) + 0.75f /*playerheight*/;
            }
        }
        private void CalculateHeights()
        {
            if (!playerTransform || !playerTransformStartPosition)
            {
                currentMetresUp = 0;
                summitHeight = 0;
                return;
            }

            currentMetresUp = Vector3.Distance(new Vector3(playerTransform.position.x, playerTransformStartPosition.position.y, playerTransform.position.z), playerTransform.position);
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
