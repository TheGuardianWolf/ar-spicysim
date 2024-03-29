﻿//  
// Copyright (c) 2017 Vulcan, Inc. All rights reserved.  
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
//

using UnityEngine;


using System;

using HoloLensCameraStream;
using System.Threading.Tasks;
using System.Collections.Generic;
using Breadboard;
using Breadboard.AI;
using VirtualComponent;
using Assets.VirtualComponent.Display;

//#if WINDOWS_UWP
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Graphics.Imaging;
//using Windows.Storage.Streams;
//using Windows.Storage;
//using Windows.Media;
//#endif

/// <summary>
/// This example gets the video frames at 30 fps and displays them on a Unity texture,
/// which is locked the User's gaze.
/// </summary>
public class BreadboardCaptureTool : ComponentTool
{
    private readonly string ML_MODEL_PATH = "ms-appx:///Data/StreamingAssets/resistor_classifier.onnx";

    private DebugServer debugServer;
    private ComponentToolkit componentToolkit;
    private DisplayManager displayManager;
    private AutoPlacement autoPlacement;
    public TextMesh scannerStatus;

    private HoloLensCameraStream.Resolution _resolution;
    private VideoCapture videoCapture;
    private IntPtr spatialCoordinateSystemPtr;
    private BreadboardScanner breadboardScanner;
    private BreadboardScannerML breadboardScannerML;

    private byte[] lastDetectionBytes;
    private int frameNumber = 0;
    private List<Item> breadboardItemList = new List<Item>();
    private ScannerState scannerState = ScannerState.IDLE;

    private enum ScannerState
    {
        IDLE,
        BUSY,
        DONE
    }

    void Start()
    {
        debugServer = FindObjectOfType<DebugServer>();
        componentToolkit = FindObjectOfType<ComponentToolkit>();
        displayManager = FindObjectOfType<DisplayManager>();
        autoPlacement = FindObjectOfType<AutoPlacement>();

        breadboardScanner = new BreadboardScanner(
            new int[4] { 0, 1, 2, 3 },
            "",
            new int[2] { 63, 10 },
            new int[2] { 660, 220 },
            new int[2] { 2, 5 }, 11
        );

        breadboardScannerML = new BreadboardScannerML(
            new int[4] { 0, 1, 2, 3 },
            "",
            new int[2] { 63, 10 },
            new int[2] { 660, 220 },
            new int[2] { 2, 5 }, 11
        );

        //Fetch a pointer to Unity's spatial coordinate system if you need pixel mapping
        spatialCoordinateSystemPtr = UnityEngine.XR.WSA.WorldManager.GetNativeISpatialCoordinateSystemPtr();

        scannerStatus.text = "Tool is initialising";

#if WINDOWS_UWP
        //Task.Run(async () =>
        //{
        //    var mlFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ML_MODEL_PATH));
        //    IModel mlModel = await Breadboard.AI.ResistorClassifier.ResistorClassifierModel.CreateResistorClassifierModel(mlFile);
        //    breadboardScannerML.SetMLModel(mlModel);

            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                scannerStatus.text = "Tool is idle";
            }, false);
        //}).Wait();
#endif
    }

    override public void ComponentToolSelect()
    {
        if (scannerState == ScannerState.IDLE && componentToolkit.ActiveTool != this)
        {
            scannerState = ScannerState.BUSY;
            componentToolkit.ActiveTool = this;
            scannerStatus.text = "Tool is busy";

            // Disable the 3D UI here
            displayManager.HudTooltip.text = "Place breadboard in view or tap to exit";
            displayManager.HudTooltip.color = new Color(255, 255, 255);
            CameraStreamHelper.Instance.GetVideoCaptureAsync(OnVideoCaptureCreated);
        }
        else if (scannerState == ScannerState.DONE && componentToolkit.ActiveTool != this)
        {
            componentToolkit.ActiveTool = this;
            // Change cursor to circuit model
        }
    }

    override public void ComponentToolPlace()
    {
        componentToolkit.ActiveTool = null;
        if (scannerState == ScannerState.BUSY)
        {
            StopFrameCapture();

            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                displayManager.HudTooltip.text = "";
                displayManager.HudTooltip.color = Color.white;
            }, false);

            if (lastDetectionBytes != null)
            {
                //Task.Run(async () =>
                //{
                //    await breadboardScannerML.RunScannerAsync(lastDetectionBytes, _resolution.width, _resolution.height);
                //    breadboardScannerML.GetList(breadboardItemList);
                //    scannerState = ScannerState.DONE;
                //    scannerStatus.text = "Tap to place generated circuit";

                //    // Generate the circuit object
                //    this.transform.parent.GetComponentInChildren<AutoPlacement>().RealComponents = breadboardItemList;
                //    this.transform.parent.GetComponentInChildren<AutoPlacement>().onGenerate();
                //});
                breadboardScanner.RunScanner(lastDetectionBytes, _resolution.width, _resolution.height);
                breadboardScanner.GetList(breadboardItemList);
                scannerState = ScannerState.DONE;

                scannerStatus.text = "Tap to place generated circuit";
                breadboardItemList.ForEach((Item item) =>
                {
                    scannerStatus.text += $"\nx: {item.pos.x.ToString()}, y: {item.pos.y.ToString()}";
                });
            }
            else
            {
                scannerState = ScannerState.IDLE;
                scannerStatus.text = "Tool is idle";
            }
        }
        else if (scannerState == ScannerState.DONE)
        {
            // Place the circuit object
            autoPlacement.RealComponents = breadboardItemList;
            autoPlacement.onGenerate();

            scannerState = ScannerState.IDLE;
            scannerStatus.text = "Tool is idle";
            lastDetectionBytes = null;
            breadboardItemList.Clear();
        }

    }

    private void StopFrameCapture()
    {
        if (videoCapture != null)
        {
            videoCapture.FrameSampleAcquired -= OnFrameSampleAcquired;
            videoCapture.StopVideoModeAsync((VideoCaptureResult result) => { });
        }
    }

    private void OnDestroy()
    {
        StopFrameCapture();
    }

    private void OnVideoCaptureCreated(VideoCapture videoCapture)
    {
        if (videoCapture == null)
        {
            Debug.LogError("Did not find a video capture object. You may not be using the HoloLens.");
            return;
        }

        this.videoCapture = videoCapture;

        //Request the spatial coordinate ptr if you want fetch the camera and set it if you need to 
        CameraStreamHelper.Instance.SetNativeISpatialCoordinateSystemPtr(spatialCoordinateSystemPtr);

        _resolution = new HoloLensCameraStream.Resolution(1408, 792);
        float frameRate = 20;
        videoCapture.FrameSampleAcquired += OnFrameSampleAcquired;

        //You don't need to set all of these params.
        //I'm just adding them to show you that they exist.
        CameraParameters cameraParams = new CameraParameters();
        cameraParams.cameraResolutionHeight = _resolution.height;
        cameraParams.cameraResolutionWidth = _resolution.width;
        cameraParams.frameRate = Mathf.RoundToInt(frameRate);
        cameraParams.pixelFormat = CapturePixelFormat.BGRA32;
        cameraParams.rotateImage180Degrees = false; //If your image is upside down, remove this line.
        cameraParams.enableHolograms = false;

        videoCapture.StartVideoModeAsync(cameraParams, OnVideoModeStarted);
    }

    private void OnVideoModeStarted(VideoCaptureResult result)
    {
        if (result.success == false)
        {
            Debug.LogWarning("Could not start video mode.");
            return;
        }

        Debug.Log("Video capture started.");
    }

    private void OnFrameSampleAcquired(VideoCaptureSample sample)
    {
        // Limit Frames used to one per 20
        if (frameNumber % 20 == 0)
        {
            var latestImageBytes = new byte[sample.dataLength];

            sample.CopyRawImageDataIntoBuffer(latestImageBytes);

            var hasBreadboard = breadboardScannerML.HasBreadboard(latestImageBytes, _resolution.width, _resolution.height);

            if (hasBreadboard)
            {
                if (lastDetectionBytes == null)
                {
                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        displayManager.HudTooltip.text = "Breadboard captured, tap to exit or continue to refresh";
                        displayManager.HudTooltip.color = Color.green;
                    }, false);
                }
                lastDetectionBytes = latestImageBytes;
            }
        }

        sample.Dispose();
        frameNumber++;
    }
}
