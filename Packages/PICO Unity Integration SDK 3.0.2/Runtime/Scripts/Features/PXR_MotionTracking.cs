﻿/*******************************************************************************
Copyright © 2015-2022 PICO Technology Co., Ltd.All rights reserved.  

NOTICE：All information contained herein is, and remains the property of 
PICO Technology Co., Ltd. The intellectual and technical concepts 
contained herein are proprietary to PICO Technology Co., Ltd. and may be 
covered by patents, patents in process, and are protected by trade secret or 
copyright law. Dissemination of this information or reproduction of this 
material is strictly forbidden unless prior written permission is obtained from
PICO Technology Co., Ltd. 
*******************************************************************************/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.XR.PXR
{
    /// <summary>
    /// The codes that indicates the state of motion tracking features.
    /// </summary>
    public enum TrackingStateCode
    {
        /// <summary>
        /// Request succeeded.
        /// </summary>
        PXR_MT_SUCCESS = 0,
        /// <summary>
        /// Request failed.
        /// </summary>
        PXR_MT_FAILURE = -1,
        /// <summary>
        /// Invalid mode.
        /// </summary>
        PXR_MT_MODE_NONE = -2,
        /// <summary>
        /// The current device does not support this feature.
        /// </summary>
        PXR_MT_DEVICE_NOT_SUPPORT = -3,
        /// <summary>
        /// This feature is not started.
        /// </summary>
        PXR_MT_SERVICE_NEED_START = -4,
        /// <summary>
        /// Eye tracking permission denied.
        /// </summary>
        PXR_MT_ET_PERMISSION_DENIED = -5,
        /// <summary>
        /// Face tracking permission denied.
        /// </summary>
        PXR_MT_FT_PERMISSION_DENIED = -6,
        /// <summary>
        /// Microphone permission denied.
        /// </summary>
        PXR_MT_MIC_PERMISSION_DENIED = -7,
        /// <summary>
        /// (Reserved)
        /// </summary>
        PXR_MT_SYSTEM_DENIED = -8,
        /// <summary>
        /// Unknown error.
        /// </summary>
        PXR_MT_UNKNOW_ERROR = -9
    }

    #region Eye Tracking
    /// <summary>
    /// Eye tracking modes.
    /// </summary>
    public enum EyeTrackingMode
    {
        /// <summary>
        /// To disable eye tracking. 
        /// </summary>
        PXR_ETM_NONE = -1,
        /// <summary>
        /// To enable eye tracking.
        /// </summary>
        PXR_ETM_BOTH = 0,
        /// <summary>
        /// (Reserved)
        /// </summary>
        PXR_ETM_COUNT = 1
    }

    public enum PerEyeUsage
    {
        LeftEye = 0,
        RightEye = 1,
        Combined = 2,
        EyeCount = 3
    }

    /// <summary>
    /// Eye tracking data flags.
    /// </summary>
    public enum EyeTrackingDataGetFlags : long
    {
        /// <summary>
        /// Do not return any data.
        /// </summary>
        PXR_EYE_DEFAULT = 0,
        /// <summary>
        /// To return the positions of both eyes.
        /// </summary>
        PXR_EYE_POSITION = 1 << 0,
        /// <summary>
        /// To return the orientations of both eyes.
        /// </summary>
        PXR_EYE_ORIENTATION = 1 << 1
    }

    /// <summary>
    /// The information to pass for starting eye tracking.
    /// </summary>
    public struct EyeTrackingStartInfo
    {
        private int apiVersion;
        /// <summary>
        /// Whether the app needs eye tracking calibration.
        /// * `0`: needs
        /// * `1`: does not need
        /// </summary>
        public byte needCalibration;
        /// <summary>
        /// Select an eye tracking mode for the app. Refer to the `EyeTrackingMode` enum for details.
        /// </summary>
        public EyeTrackingMode mode;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, needCalibration:{1}, mode:{2}", apiVersion, needCalibration, mode);
        }

    }

    public struct EyeTrackingStopInfo
    {
        private int apiVersion;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}", apiVersion);
        }
    }

    /// <summary>
    /// Information about the state of eye tracking.
    /// </summary>
    public struct EyeTrackingState
    {
        private int apiVersion;
        /// <summary>
        /// Eye tracking mode. Refer to the `EyeTrackingMode` enum for details.
        /// </summary>
        public EyeTrackingMode currentTrackingMode;
        /// <summary>
        /// The state code of eye tracking. Refer to the `TrackingStateCode` enum for details.
        /// </summary>
        public TrackingStateCode code;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, currentTrackingMode:{1}, code:{2}", apiVersion, currentTrackingMode, code);
        }
    }

    /// <summary>
    /// The information to pass for getting eye tracking data.
    /// </summary>
    public struct EyeTrackingDataGetInfo
    {
        private int apiVersion;
        /// <summary>
        /// Reserved. Pass `0`.
        /// </summary>
        public long displayTime;
        /// <summary>
        /// Specifies what eye tracking data to return. Refer to the `EyeTrackingDataGetFlags` enum for details.
        /// </summary>
        public EyeTrackingDataGetFlags flags;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, displayTime:{1}, flags:{2}", apiVersion, displayTime, flags);
        }
    }

    public struct PxrPose
    {
        public PxrVector4f orientation;
        public PxrVector3f position;
    
        public override string ToString()
        {
            return string.Format("orientation :({0},{1},{2},{3}) position:({4},{5},{6})",
                orientation.x.ToString("F6"), orientation.y.ToString("F6"), orientation.z.ToString("F6"), orientation.w.ToString("F6"),
                position.x.ToString("F6"), position.y.ToString("F6"), position.z.ToString("F6"));
        }
    };

    public struct PerEyeData
    {
        private int apiVersion;
        public PxrPose pose;
        public byte isPoseValid;
        public float openness;
        public byte isOpennessValid;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, pose:{1}, isPoseValid:{2}, openness:{3}, isOpennessValid:{4}", apiVersion, pose, isPoseValid, openness, isOpennessValid);
        }
    }

    public struct EyeTrackingData
    {
        private int apiVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)PerEyeUsage.EyeCount)]
        public PerEyeData[] eyeDatas;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0},\n eyeDatas[0]:{1},\n eyeDatas[1]:{2},\n eyeDatas[2]:{3}", apiVersion, eyeDatas[0], eyeDatas[1], eyeDatas[2]);
        }
    }

    /// <summary>
    /// The information about the pupils of both eyes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct EyePupilInfo
    {
        /// <summary>
        /// The diameter (unit: millimeters) of the left eye's pupil.
        /// </summary>
        public float leftEyePupilDiameter;
        /// <summary>
        /// The diameter (unit: millimeters) of the right eye's pupil.
        /// </summary>
        public float rightEyePupilDiameter;
        /// <summary>
        /// The position of the left eye's pupil.
        /// </summary>
        public fixed float leftEyePupilPosition[2];
        /// <summary>
        /// The position of the right eye's pupil.
        /// </summary>
        public fixed float rightEyePupilPosition[2];
        public override string ToString()
        {
            string str = string.Format("leftEyePupilDiameter :{0}, rightEyePupilDiameter:{1}", leftEyePupilDiameter.ToString("F6"), rightEyePupilDiameter.ToString("F6"));
            for (int i = 0; i < 2; i++)
            {
                str += string.Format("\nleftEyePupilPosition[{0}] :{1}", i, leftEyePupilPosition[i].ToString("F6"));
                str += string.Format(" rightEyePupilPosition[{0}] :{1}", i, rightEyePupilPosition[i].ToString("F6"));
            }
            return str;
        }
    }
    #endregion

    #region Face Tracking
    /// <summary>
    /// Face tracking modes.
    /// </summary>
    public enum FaceTrackingMode
    {
        /// <summary>
        /// No face tracking.
        /// </summary>
        [InspectorName("None")]
        PXR_FTM_NONE = -1,
        /// <summary>
        /// Face tracking only (without lipsync).
        /// </summary>
        [InspectorName("Face Only")]
        PXR_FTM_FACE = 0,
        /// <summary>
        /// Lipsync only.
        /// </summary>
        [InspectorName("Lipsync Only")]
        PXR_FTM_LIPS = 1,
        /// <summary>
        /// Hybrid mode. Enable both face tracking and lipsync. The lip data's output format is viseme.
        /// </summary>
        [InspectorName("Hybrid Viseme")]
        PXR_FTM_FACE_LIPS_VIS = 2,
        /// <summary>
        /// Hybrid mode. Enable both face tracking and lipsync. The lip data's output format is blendshape.
        /// </summary>
        [InspectorName("Hybrid BlendShape")]
        PXR_FTM_FACE_LIPS_BS = 3
    }

    enum BlendShapeIndex
    {
        EyeLookDown_L = 0,
        NoseSneer_L = 1,
        EyeLookIn_L = 2,
        BrowInnerUp = 3,
        BrowDown_R = 4,
        MouthClose = 5,
        MouthLowerDown_R = 6,
        JawOpen = 7,
        MouthUpperUp_R = 8,
        MouthShrugUpper = 9,
        MouthFunnel = 10,
        EyeLookIn_R = 11,
        EyeLookDown_R = 12,
        NoseSneer_R = 13,
        MouthRollUpper = 14,
        JawRight = 15,
        BrowDown_L = 16,
        MouthShrugLower = 17,
        MouthRollLower = 18,
        MouthSmile_L = 19,
        MouthPress_L = 20,
        MouthSmile_R = 21,
        MouthPress_R = 22,
        MouthDimple_R = 23,
        MouthLeft = 24,
        JawForward = 25,
        EyeSquint_L = 26,
        MouthFrown_L = 27,
        EyeBlink_L = 28,
        CheekSquint_L = 29,
        BrowOuterUp_L = 30,
        EyeLookUp_L = 31,
        JawLeft = 32,
        MouthStretch_L = 33,
        MouthPucker = 34,
        EyeLookUp_R = 35,
        BrowOuterUp_R = 36,
        CheekSquint_R = 37,
        EyeBlink_R = 38,
        MouthUpperUp_L = 39,
        MouthFrown_R = 40,
        EyeSquint_R = 41,
        MouthStretch_R = 42,
        CheekPuff = 43,
        EyeLookOut_L = 44,
        EyeLookOut_R = 45,
        EyeWide_R = 46,
        EyeWide_L = 47,
        MouthRight = 48,
        MouthDimple_L = 49,
        MouthLowerDown_L = 50,
        TongueOut = 51,
        PP = 52,
        CH = 53,
        o = 54,
        O = 55,
        I = 56,
        u = 57,
        RR = 58,
        XX = 59,
        aa = 60,
        i = 61,
        FF = 62,
        U = 63,
        TH = 64,
        kk = 65,
        SS = 66,
        e = 67,
        DD = 68,
        E = 69,
        nn = 70,
        sil = 71
    };


    /// <summary>
    /// Specifies the face tracking data to return.
    /// </summary>
    public enum FaceTrackingDataGetFlags : long
    {
        /// <summary>
        /// To return all types of face tracking data.
        /// </summary>
        PXR_FACE_DEFAULT = 0,
    }

    /// <summary>
    /// The information to pass for starting face tracking.
    /// </summary>
    public struct FaceTrackingStartInfo
    {
        private int apiVersion;
        /// <summary>
        /// The face tracking mode to enable. Refer to the `FaceTrackingMode` enum for details.
        /// </summary>
        public FaceTrackingMode mode;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, mode:{1}", apiVersion, mode);
        }
    }

    /// <summary>
    /// The information to pass for stopping face tracking.
    /// </summary>
    public struct FaceTrackingStopInfo
    {
        private int apiVersion;
        /// <summary>
        /// Determines whether to pause face tracking.
        /// * `0`: pause
        /// * `1`: do not pause
        /// </summary>
        public byte pause;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, pause:{1}", apiVersion, pause);
        }
    }

    /// <summary>
    /// Information about the state of face tracking.
    /// </summary>
    public struct FaceTrackingState
    {
        private int apiVersion;
        /// <summary>
        /// The face tracking mode of the app. Refer to the `FaceTrackingMode` enum for details.
        /// </summary>
        public FaceTrackingMode currentTrackingMode;
        /// <summary>
        /// Face tracking state code. Refer to the `TrackingStateCode` enum for details.
        /// </summary>
        public TrackingStateCode code;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, currentTrackingMode:{1}, code:{2}", apiVersion, currentTrackingMode, code);
        }
    }

    /// <summary>
    /// The information to pass for getting face tracking data.
    /// </summary>
    public struct FaceTrackingDataGetInfo
    {
        private int apiVersion;
        /// <summary>
        /// Reserved. Pass `0`.
        /// </summary>
        public long displayTime;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public FaceTrackingDataGetFlags flags;
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, displayTime:{1}, flags:{2}", apiVersion, displayTime, flags);
        }
    }

    /// <summary>
    /// Face tracking data.
    /// </summary>
    public unsafe struct FaceTrackingData
    {
        private int apiVersion;
        /// <summary>
        /// A float* value, the length must be 72. Refer to `BlendShapeIndex` for the definition of each value.
        /// </summary>
        public float* blendShapeWeight;
        /// <summary>
        /// The timestamp for the current data.
        /// </summary>
        public long timestamp;
        /// <summary>
        /// The laughing prob is a float ranging from `0` to `1`.
        /// </summary>
        public float laughingProb;
        /// <summary>
        /// Whether the data of the eye area is valid.
        /// </summary>
        public byte eyeValid;
        /// <summary>
        /// Whether the data of the face area is valid.
        /// </summary>
        public byte faceValid;
        public void SetVersion(int version)
        {
            apiVersion = version;
        }
        public override string ToString()
        {
            string str = string.Format("apiVersion :{0}, timestamp:{1}, laughingProb:{2}, eyeValid:{3}, faceValid:{4}\n", apiVersion, timestamp, laughingProb, eyeValid, faceValid);
            for (int i = 0; i < 72; i++)
            {
                str += string.Format(" blendShapeWeight[{0}]:{1}", i, blendShapeWeight[i].ToString("F6"));
            }

            return str;
        }
    }
    #endregion

    #region Body Tracking

    public enum BodyTrackingMode
    {
        BTM_FULL_BODY_LOW = 0,
        BTM_FULL_BODY_HIGH = 1,
    }

    public struct BodyTrackingStartInfo
    {
        private int apiVersion;
        private int withMotionTracker; // use 1
        public BodyTrackingMode mode;
        public BodyTrackingBoneLength BoneLength;
    }
    public struct BodyTrackingStopInfo
    {
        private int apiVersion;
    }
    public enum BodyTrackingStatusCode
    {
        BT_INVALID = 0,
        BT_VALID = 1,
        BT_LIMITED = 2
    }
    public enum BodyTrackingErrorCode
    {
        BT_ERROR_INNER_EXCEPTION = 0,
        BT_ERROR_TRACKER_NOT_CALIBRATED = 1,
        BT_ERROR_TRACKER_NUM_NOT_ENOUGH = 2,
        BT_ERROR_TRACKER_STATE_NOT_SATISFIED = 3,
        BT_ERROR_TRACKER_PERSISTENT_INVISIBILITY = 4,
        BT_ERROR_TRACKER_DATA_ERROR = 5,
        BT_ERROR_USER_CHANGE = 6,
        BT_ERROR_TRACKING_POSE_ERROR = 7
    }
    public unsafe struct BodyTrackingState
    {
        private int apiVersion;
        private int currentTrackingMode;
        public TrackingStateCode code;
        public BodyTrackingStatusCode stateCode;
        public BodyTrackingErrorCode errorCode;
        public byte connectedBandCount;
        public fixed byte motionTracker[12];
        public override string ToString()
        {
            string str = string.Format("apiVersion :{0}, currentTrackingMode:{1}, code:{2}, stateCode:{3},errorCode:{4}, connectedBandCount:{5}\n", apiVersion, currentTrackingMode, code, stateCode, errorCode, connectedBandCount);
            for (int i = 0; i < 12; i++)
            {
                str += string.Format(" motionTracker[{0}]:{1}", i, motionTracker[i].ToString());
            }

            return str;
        }
    }
    public enum BodyTrackingGetDataFlags
    {
        PXR_BODY_NONE = 0,
        PXR_BODY_POSE = 1 << 0,
        PXR_BODY_ACTION = 1 << 1,
        PXR_BODY_VELO_ACC = 1 << 2,
        PXR_BODY_MAX_ENUM = 0x7FFFFFFF
    }
    public struct BodyTrackingGetDataInfo
    {
        private int apiVersion;
        public long displayTime;
        public BodyTrackingGetDataFlags flags;
        public override string ToString()
        {
            return string.Format("apiVersion :{0}, displayTime:{1}, flags:{2}", apiVersion, displayTime, flags);
        }
    }

    public unsafe struct BodyTrackingRoleData
    {
        private int apiVersion;
        public BodyTrackerRole role;
        public BodyActionList bodyAction;
        public BodyTrackerTransPose localPose;
        public BodyTrackerTransPose globalPose;
        public fixed double velo[3];
        public fixed double acce[3];
        public fixed double wvelo[3];
        public fixed double wacce[3];
        public override string ToString()
        {
            string str = string.Format("apiVersion :{0}, role:{1}, bodyAction:{2}, localPose:{3}, globalPose:{4}\n", apiVersion, role, bodyAction, localPose, globalPose);
            for (int i = 0; i < 3; i++)
            {
                str += string.Format(" velo[{0}]:{1}", i, velo[i].ToString("F6"));
                str += string.Format(" acce[{0}]:{1}", i, acce[i].ToString("F6"));
                str += string.Format(" wvelo[{0}]:{1}", i, wvelo[i].ToString("F6"));
                str += string.Format(" wacce[{0}]:{1}", i, wacce[i].ToString("F6"));
                str += "\n";
            }
            return str;
        }
    }

    public struct BodyTrackingData
    {
        private int apiVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)BodyTrackerRole.NONE_ROLE)]
        public BodyTrackingRoleData[] roleDatas;
        public override string ToString()
        {
            string str = string.Format("apiVersion :{0}\n", apiVersion);
            for (int i = 0; i < (int)BodyTrackerRole.NONE_ROLE; i++)
            {
                str += string.Format(" roleData[{0}]:{1}", i, roleDatas[i].ToString());
            }

            return str;
        }
    }
    #endregion

    #region Motion Tracker
    public enum MotionTrackerType
    {
        MT_1 = 1,
        MT_2
    }

    public enum MotionTrackerMode
    {
        BodyTracking,
        MotionTracking
    }

    public enum MotionTrackerNum
    {
        NONE = 0,
        ONE,
        TWO,
        THREE
    }
    
    public enum MotionTrackerConfidence
    {
        PXR_STATIC_ACCURATE = 0,
        PXR_6DOF_ACCURATE,
        PXR_3DOF_NOT_ACCURATE,
        PXR_6DOF_NOT_ACCURATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MotionTrackerConnectState
    {
        public int trackerSum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public TrackerSN[] trackersSN;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackerSN
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string value;
    }

    public unsafe struct MotionTrackerLocation
    {
        public Posef pose;
        public fixed float angularVelocity[3];
        public fixed float linearVelocity[3];
        public fixed float angularAcceleration[3];
        public fixed float linearAcceleration[3];
        public override string ToString()
        {
            string str = string.Format("pose:{0}\n", pose);
            for (int i = 0; i < 3; i++)
            {
                str += string.Format(" angularVelocity[{0}]:{1}", i, angularVelocity[i].ToString("F6"));
                str += string.Format(" linearVelocity[{0}]:{1}", i, linearVelocity[i].ToString("F6"));
                str += string.Format(" angularAcceleration[{0}]:{1}", i, angularAcceleration[i].ToString("F6"));
                str += string.Format(" linearAcceleration[{0}]:{1}", i, linearAcceleration[i].ToString("F6"));
                str += "\n";
            }

            return str;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MotionTrackerLocations
    {
        public TrackerSN trackerSN;
        public MotionTrackerLocation localLocation; // use this
        public MotionTrackerLocation globalLocation;

        public override string ToString()
        {
            string str = string.Format("trackerSN :{0}\n pose:{1}\n globalPose:{2}\n", trackerSN, localLocation, globalLocation);
            return str;
        }
    }


    #endregion

    #region Motion Tracker For External Device
    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerInfo
    {
        public TrackerSN trackerSN;
        public byte chargerStatus;
        public byte batteryVolume;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerConnectState
    {
        public int extNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public ExtDevTrackerInfo[] info;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerMotorVibrate
    {
        public TrackerSN trackerSN;
        public int level;
        public int frequency;
        public int duration;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerPassData
    {
        public TrackerSN trackerSN;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] passData;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerPassDataArray
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public ExtDevTrackerPassData[] passDatas;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerKey
    {
        public int home;
        public int app;
        public int a_x;
        public int b_y;
        public int grip;
        public int rocker;
        public int trigger;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerTouch
    {
        public int a_x;
        public int b_y;
        public int rocker;
        public int trigger;
        public int thumbrest;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevTrackerKeyData
    {
        public int extDevID; //
        public ExtDevTrackerKey key;
        public ExtDevTrackerTouch touch;
        public byte trigger;
        public byte grip;
        public byte rocker_x;
        public byte rocker_y;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct MotionTrackerEventData
    {
        public TrackerSN trackerSN;
        public int code; // Key value, converted to Android standard key value. (Power key: 26)
        public int action; // up:1 and down:0. now only action 0 is supported.
        public int repeat; // Currently, it appears to be 1.
        public bool shortPress; // Is it a short press? Currently, short press is supported, so it is all true.
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevConnectEventData
    {
        public TrackerSN trackerSN;
        public int state;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ExtDevBatteryEventData
    {
        public TrackerSN trackerSN;
        public int battery;
        public int charger;
    };


    #endregion


    public class PXR_MotionTracking
    {
        #region Eye Tracking
        //Eye Tracking
        public const int PXR_EYE_TRACKING_API_VERSION = 1;

        /// <summary>
        /// Wants eye tracking service for the current app.
        /// </summary>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int WantEyeTrackingService()
        {
            return PXR_Plugin.MotionTracking.UPxr_WantEyeTrackingService();
        }

        /// <summary>
        /// Gets whether the current device supports eye tracking.
        /// </summary>
        /// <param name="supported">
        /// Returns a bool indicating whether eye tracking is supported:
        /// * `true`: supported
        /// * `false`: not supported
        /// </param>
        /// <param name="supportedModesCount">
        /// Returns the number of eye tracking modes supported by the current device.
        /// </param>
        /// <param name="supportedModes">
        /// Returns the eye tracking modes supported by the current device.
        /// </param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetEyeTrackingSupported(ref bool supported, ref int supportedModesCount, ref EyeTrackingMode[] supportedModes)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetEyeTrackingSupported(ref supported, ref supportedModesCount, ref supportedModes);
        }

        /// <summary>
        /// Starts eye tracking.
        /// @note Supported by PICO Neo3 Pro Eye, PICO 4 Pro, and PICO 4 Enterprise.
        /// </summary>
        /// <param name="startInfo">Passes the information for starting eye tracking.
        /// </param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int StartEyeTracking(ref EyeTrackingStartInfo startInfo)
        {
            startInfo.SetVersion(PXR_EYE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_StartEyeTracking1(ref startInfo);
        }

        /// <summary>
        /// Stops eye tracking.
        /// @note Supported by PICO Neo3 Pro Eye, PICO 4 Pro, and PICO 4 Enterprise.
        /// </summary>
        /// <param name="stopInfo">Passes the information for stopping eye tracking. Currently, you do not need to pass anything.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int StopEyeTracking(ref EyeTrackingStopInfo stopInfo)
        {
            stopInfo.SetVersion(PXR_EYE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_StopEyeTracking1(ref stopInfo);
        }

        /// <summary>
        /// Gets the state of eye tracking.
        /// @note Supported by PICO Neo3 Pro Eye, PICO 4 Pro, and PICO 4 Enterprise.
        /// </summary>
        /// <param name="isTracking">Returns a bool that indicates whether eye tracking is working:
        /// * `true`: eye tracking is working
        /// * `false`: eye tracking has been stopped
        /// </param>
        /// <param name="state">Returns the eye tracking state information, including the eye tracking mode and eye tracking state code.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetEyeTrackingState(ref bool isTracking, ref EyeTrackingState state)
        {
            state.SetVersion(PXR_EYE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_GetEyeTrackingState(ref isTracking, ref state);
        }

        /// <summary>
        /// Gets eye tracking data.
        /// @note Supported by PICO Neo3 Pro Eye, PICO 4 Pro, and PICO 4 Enterprise.
        /// </summary>
        /// <param name="getInfo">Specifies the eye tracking data you want.</param>
        /// <param name="data">Returns the desired eye tracking data.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetEyeTrackingData(ref EyeTrackingDataGetInfo getInfo, ref EyeTrackingData data)
        {
            getInfo.SetVersion(PXR_EYE_TRACKING_API_VERSION);
            data.SetVersion(PXR_EYE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_GetEyeTrackingData1(ref getInfo, ref data);
        }

        //PICO4E
        /// <summary>
        /// Gets the opennesses of the left and right eyes.
        /// @note Supported by PICO 4 Enterprise.
        /// </summary>
        /// <param name="leftEyeOpenness">The openness of the left eye, which is a float value ranges from `0.0` to `1.0`. `0.0` indicates completely closed, `1.0` indicates completely open.</param>
        /// <param name="rightEyeOpenness">The openness of the right eye, which is a float value ranges from `0.0` to `1.0`. `0.0` indicates completely closed, `1.0` indicates completely open.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetEyeOpenness(ref float leftEyeOpenness, ref float rightEyeOpenness)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetEyeOpenness(ref leftEyeOpenness, ref rightEyeOpenness);
        }

        /// <summary>
        /// Gets the information about the pupils of both eyes.
        /// @note Supported by PICO 4 Enterprise.
        /// </summary>
        /// <param name="eyePupilPosition">Returns the diameters and positions of both pupils.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetEyePupilInfo(ref EyePupilInfo eyePupilPosition)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetEyePupilInfo(ref eyePupilPosition);
        }

        public static int GetPerEyePose(ref long timestamp, ref Posef leftEyePose, ref Posef rightPose)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetPerEyePose(ref timestamp, ref leftEyePose, ref rightPose);
        }

        public static int GetEyeBlink(ref long timestamp, ref bool isLeftBlink, ref bool isRightBlink)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetEyeBlink(ref timestamp, ref isLeftBlink, ref isRightBlink);
        }

        #endregion

        #region Face Tracking
        //Face Tracking
        public const int PXR_FACE_TRACKING_API_VERSION = 1;

        /// <summary>
        /// Wants face tracking service for the current app.
        /// </summary>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int WantFaceTrackingService()
        {
            return PXR_Plugin.MotionTracking.UPxr_WantFaceTrackingService();
        }

        /// <summary>
        /// Gets whether the current device supports face tracking.
        /// </summary>
        /// <param name="supported">Indicates whether the device supports face tracking:
        /// * `true`: support
        /// * `false`: not support
        /// </param>
        /// <param name="supportedModesCount">Returns the total number of face tracking modes supported by the device.</param>
        /// <param name="supportedModes">Returns the specific face tracking modes supported by the device.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static unsafe int GetFaceTrackingSupported(ref bool supported, ref int supportedModesCount, ref FaceTrackingMode[] supportedModes)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetFaceTrackingSupported(ref supported, ref supportedModesCount, ref supportedModes);
        }

        /// <summary>
        /// Starts face tracking.
        /// @note Supported by PICO 4 Pro and PICO 4 Enterprise.
        /// </summary>
        /// <param name="startInfo">Passes the information for starting face tracking.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int StartFaceTracking(ref FaceTrackingStartInfo startInfo)
        {
            startInfo.SetVersion(PXR_FACE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_StartFaceTracking(ref startInfo);
        }

        /// <summary>
        /// Stops face tracking.
        /// @note Supported by PICO 4 Pro and PICO 4 Enterprise.
        /// </summary>
        /// <param name="stopInfo">Passes the information for stopping face tracking.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int StopFaceTracking(ref FaceTrackingStopInfo stopInfo)
        {
            stopInfo.SetVersion(PXR_FACE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_StopFaceTracking(ref stopInfo);
        }

        /// <summary>
        /// Gets the state of face tracking.
        /// @note Supported by PICO 4 Pro and PICO 4 Enterprise.
        /// </summary>
        /// <param name="isTracking">Returns a bool indicating whether face tracking is working:
        /// * `true`: face tracking is working
        /// * `false`: face tracking has been stopped
        /// </param>
        /// <param name="state">Returns the state of face tracking, including the face tracking mode and face tracking state code.
        /// </param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetFaceTrackingState(ref bool isTracking, ref FaceTrackingState state)
        {
            state.SetVersion(PXR_FACE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_GetFaceTrackingState(ref isTracking, ref state);
        }

        /// <summary>
        /// Gets face tracking data.
        /// @note Supported by PICO 4 Pro and PICO 4 Enterprise.
        /// </summary>
        /// <param name="getInfo">Specifies the face tracking data you want.</param>
        /// <param name="data">Returns the desired face tracking data.</param>
        /// <returns>Returns `0` for success and other values for failure.</returns>
        public static int GetFaceTrackingData(ref FaceTrackingDataGetInfo getInfo, ref FaceTrackingData data)
        {
            getInfo.SetVersion(PXR_FACE_TRACKING_API_VERSION);
            data.SetVersion(PXR_FACE_TRACKING_API_VERSION);
            return PXR_Plugin.MotionTracking.UPxr_GetFaceTrackingData1(ref getInfo, ref data);
        }
        #endregion

        #region Body Tracking
        //Body Tracking
        public const int PXR_BODY_TRACKING_API_VERSION = 1;
        /// <summary>
        /// A callback function that notifies calibration exceptions.
        /// The user then needs to recalibrate with PICO Motion Tracker.
        /// </summary>
        public static Action<int, int> BodyTrackingAbnormalCalibrationData;
        public static Action<BodyTrackingStatusCode, BodyTrackingErrorCode> BodyTrackingStateError;
        public static Action<int, BodyActionList> BodyTrackingAction;

        public static int StartMotionTrackerCalibApp()
        {
            return PXR_Plugin.MotionTracking.UPxr_StartMotionTrackerCalibApp();
        }

        public static int GetBodyTrackingSupported(ref bool supported)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetBodyTrackingSupported(ref supported); 
        }

        public static int StartBodyTracking(BodyTrackingMode mode, BodyTrackingBoneLength boneLength)
        {
            return PXR_Plugin.MotionTracking.UPxr_StartBodyTracking(mode, boneLength);
        }

        public static int StopBodyTracking()
        {
            return PXR_Plugin.MotionTracking.UPxr_StopBodyTracking();
        }

        public static int GetBodyTrackingState(ref bool isTracking, ref BodyTrackingState state)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetBodyTrackingState(ref isTracking, ref state);
        }

        public static int GetBodyTrackingData(ref BodyTrackingGetDataInfo getInfo, ref BodyTrackingData data)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetBodyTrackingData(ref getInfo, ref data);
        }
        #endregion

        #region Motion Tracker
        //Motion Tracker

        /// <summary>
        /// A callback function that notifies the current connection status of PICO Motion Tracker and the number of motion trackers connected.
        /// For connection status, `0` indicates "disconnected" and `1` indicates "connected".
        /// </summary>
        public static Action<int, int> MotionTrackerNumberOfConnections;
        /// <summary>
        /// A callback function that notifies the battery of PICO Motion Traker.(int trackerID, int battery)
        /// Value range: [0,5]. `0` indicates a low battery, which can affect the tracking accuracy.
        /// </summary>
        public static Action<int, int> MotionTrackerBatteryLevel;
        /// <summary>
        /// A callback function that notifies button presses.
        /// The user then needs to recalibrate with PICO Motion Tracker.
        /// </summary>
        public static Action<MotionTrackerEventData> MotionTrackerKeyAction;

        /// <summary>
        /// A callback function that notifies mode changed.
        /// 0 bodytracking;1 motiontracking.
        /// </summary>
        public static Action<MotionTrackerMode> MotionTrackingModeChangedAction;

        public static int GetMotionTrackerConnectStateWithSN(ref MotionTrackerConnectState connectState)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetMotionTrackerConnectStateWithSN(ref connectState);
        }

        public static MotionTrackerType GetMotionTrackerDeviceType()
        {
            return PXR_Plugin.MotionTracking.UPxr_GetMotionTrackerDeviceType();
        }

        public static int CheckMotionTrackerModeAndNumber(MotionTrackerMode mode, MotionTrackerNum number = MotionTrackerNum.NONE)
        {
            return PXR_Plugin.MotionTracking.UPxr_CheckMotionTrackerModeAndNumber(mode, (int)number);
        }

        public static MotionTrackerMode GetMotionTrackerMode()
        {
            return PXR_Plugin.MotionTracking.UPxr_GetMotionTrackerMode();
        }

        public static int GetMotionTrackerLocations(TrackerSN trackerSN, ref MotionTrackerLocations locations, ref MotionTrackerConfidence confidence, double predictTime = 0)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetMotionTrackerLocations(predictTime, trackerSN, ref locations, ref confidence);
        }

        #endregion

        #region Motion Tracker For External Device
        public static Action<ExtDevConnectEventData> ExtDevConnectAction;
        public static Action<ExtDevBatteryEventData> ExtDevBatteryAction;
        /// <summary>
        /// Developers need to listen for this event to call the GetExtDevTrackerByPassData interface:
        /// When 1, it is necessary to call the PXR_GetExtDevTrackerByPassData interface to obtain transparent data.
        /// When 0, stop obtaining transparent data.This should be something that developers need to be reminded to do.
        /// </summary>
        public static Action<int> ExtDevPassDataAction;

        public static int GetExtDevTrackerConnectState(ref ExtDevTrackerConnectState connectState)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetExtDevTrackerConnectState(ref connectState);
        }

        public static int SetExtDevTrackerMotorVibrate(ref ExtDevTrackerMotorVibrate motorVibrate)
        {
            return PXR_Plugin.MotionTracking.UPxr_SetExtDevTrackerMotorVibrate(ref motorVibrate);
        }
        
        public static int SetExtDevTrackerPassDataState(bool state)
        {
            return PXR_Plugin.MotionTracking.UPxr_SetExtDevTrackerPassDataState(state);
        }

        public static int SetExtDevTrackerByPassData(ref ExtDevTrackerPassData passData)
        {
            return PXR_Plugin.MotionTracking.UPxr_SetExtDevTrackerByPassData(ref passData);
        }

        public static int GetExtDevTrackerByPassData(ref ExtDevTrackerPassDataArray passData, ref int realLength)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetExtDevTrackerByPassData(ref passData, ref realLength);
        }

        public static int GetExtDevTrackerBattery(ref TrackerSN trackerSN, ref int battery, ref int charger)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetExtDevTrackerBattery(ref trackerSN, ref battery, ref charger);
        }

        public static int GetExtDevTrackerKeyData(ref TrackerSN trackerSN, ref ExtDevTrackerKeyData keyData)
        {
            return PXR_Plugin.MotionTracking.UPxr_GetExtDevTrackerKeyData(ref trackerSN, ref keyData);
        }

        #endregion
    }
}